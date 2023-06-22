using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WPFSerialport.Common.Models;
using Parity = WPFSerialport.Common.Models.Parity;
using StopBits = WPFSerialport.Common.Models.StopBits;

namespace WPFSerialport.ViewModels
{
    public class SerialPortPageModel : BindableBase
    {
        public static SerialPort Sp { get; set; } = new SerialPort();
        private int num;
        private List<int> arrX = new List<int>();
        private List<double> arrY = new List<double>();
        private int receive_count = 0;
        private int error = 0;
        private int showPointCount =100;

        #region 串口设置
        private int baudRate = 9600;
        private int dataBits = 8;
        private System.IO.Ports.Parity parity = System.IO.Ports.Parity.None;
        private System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.One;
        #endregion
        private SerialPortInfo serialPortInfo;
        public SerialPortInfo SerialPortInfo
        {
            get { return serialPortInfo; }
            set { serialPortInfo = value; RaisePropertyChanged(); }
        }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        private string _mess="";
        public string Mess
        {
            get { return _mess; }
            set { _mess = value; RaisePropertyChanged(); }
        }
        private PlotModel _ChartModel;
        /// <summary>
        /// 图表Model的mvvm属性，可通知UI更新
        /// </summary>
        public PlotModel ChartModel
        {
            get { return _ChartModel; }
            set { SetProperty(ref _ChartModel, value); }
        }

        public SerialPortPageModel()
        {
            SerialPortInfo = new SerialPortInfo();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            DataInitialization();
            Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived);
        }
        public void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            num++;
            int tmp = Sp.BytesToRead;
            byte[] received_buf = new byte[tmp];
            receive_count += tmp;
            Sp.Read(received_buf, 0, tmp);
            //检查头
            if (CheckDatalegal(received_buf))
            {
                UpdateChart(received_buf);
            }
            else
            {
                error++;
            }
        }
        /// <summary>
        /// 数据合法性校验
        /// </summary>
        /// <param name="received_buf"></param>
        /// <returns></returns>
        private bool CheckDatalegal(byte[] received_buf)
        {
            bool re = true;
            if (received_buf.Length < 32)
            {
                re = false;
                return re;
            }
            //头
            if (received_buf[0] == 0x42 && received_buf[1] == 0x4D)
            {
                re = true;
            }
            else
            {
                re = false;
            }
            //尾
            int sum = 0x00;
            for (int i = 0; i < received_buf.Length - 2; i++)
            {
                sum += received_buf[i];
            }
            int Csum = sum;
            string tail = "0x" + Convert.ToString(received_buf[received_buf.Length - 2], 16).PadLeft(2, '0').ToUpper()
                        + Convert.ToString(received_buf[received_buf.Length - 1], 16).PadLeft(2, '0').ToUpper();
            int intTail = Convert.ToInt32(tail, 16);
            if (Csum == intTail)
            {
                re = true;
            }
            else
            {
                re = false;
            }
            return re;
        }
        private void UpdateChart(byte[] received_buf)
        {
            try
            {
                List<string> strList = ReadAngleFromByteArray(received_buf);
                int i = Convert.ToInt32(strList[0], 16);
                double d = Convert.ToDouble(i);
                arrX.Add(num);
                arrY.Add(d);
                while (arrX.Count > showPointCount)
                {
                    arrX.RemoveAt(0);
                    arrY.RemoveAt(0);
                }
                SetData(arrX, arrY);
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < strList.Count; j++)
                {
                    int tmp = Convert.ToInt32(strList[j], 16);
                    sb.Append($"Data[{j}]:{tmp.ToString()}\r\n");
                }
                Mess = sb.ToString();
                Console.WriteLine(Mess);
            }
            catch
            {

            }
        }
        
        private PlotModel CreateChartModel1(List<int> arrX, List<double> arrY)
        {
            var model = new PlotModel() { Title = "折线" };
            var series1 = new LineSeries { Title = "折线", MarkerType = MarkerType.Circle };
            for (int i = 0; i < arrX.Count; i++)
            {
                series1.Points.Add(new DataPoint(arrX[i], arrY[i]));
            }
            model.Series.Add(series1);
            return model;
        }
        public void SetData(List<int> arrX, List<double> arrY)
        {
            ChartModel = CreateChartModel1(arrX, arrY);
        }
        private List<string> ReadAngleFromByteArray(byte[] ReceiveBuffer)
        {
            List<string> re = new List<string>();
            for (int i = 0; i < 13; i++)
            {
                int index = 4 + i * 2;
                string tmp = "0x" + Convert.ToString(ReceiveBuffer[index], 16).PadLeft(2, '0').ToUpper()
                        + Convert.ToString(ReceiveBuffer[index + 1], 16).PadLeft(2, '0').ToUpper();
                re.Add(tmp);
            }
            return re;
        }
        void DataInitialization()
        {
            GetPortName();
            SerialPortInfo.PortOpenButton = new string[4] { "打开串口", "#676767", "TRUE", "FALSE" };
            SerialPortInfo.ReceiveSet = new ReceiveSet { ReceiveIsAscii = false, ReceiveIsHex = true, AutoLineFeed = true, DisplaySend = true, DisplayTime = true };
            SerialPortInfo.SendSet = new SendSet { SendIsAscii = false, SendIsHex = true, Circulate = false, CirculateTime = 2000 };
            SerialPortInfo.BaudRate = new List<BaudRate>
            {
                new BaudRate{Description="300",SelectedModel=300},
                new BaudRate{Description="600",SelectedModel=600},
                new BaudRate{Description="1200",SelectedModel=1200},
                new BaudRate{Description="2400",SelectedModel=2400},
                new BaudRate{Description="4800",SelectedModel=4800},
                new BaudRate{Description="9600",SelectedModel=9600},
                new BaudRate{Description="14400",SelectedModel=14400},
                new BaudRate{Description="19200",SelectedModel=19200},
                new BaudRate{Description="38400",SelectedModel=38400},
                new BaudRate{Description="56000",SelectedModel=56000},
                new BaudRate{Description="57600",SelectedModel=57600},
                new BaudRate{Description="115200",SelectedModel=115200},
                new BaudRate{Description="128000",SelectedModel=128000},
                new BaudRate{Description="256000",SelectedModel=256000},
                new BaudRate{Description="460800",SelectedModel=460800},
                new BaudRate{Description="512000",SelectedModel=512000},
                new BaudRate{Description="750000",SelectedModel=750000},
                new BaudRate{Description="921600",SelectedModel=921600},
                new BaudRate{Description="1500000",SelectedModel=1500000},
            };
            SerialPortInfo.SelectedBaudRate = SerialPortInfo.BaudRate[5].SelectedModel;

            SerialPortInfo.DataBits = new List<DataBits>
            {
                new DataBits{Description="5",SelectedModel=5},
                new DataBits{Description="6",SelectedModel=6},
                new DataBits{Description="7",SelectedModel=7},
                new DataBits{Description="8",SelectedModel=8},
            };
            SerialPortInfo.SelectedDataBits = SerialPortInfo.DataBits[3].SelectedModel;

            SerialPortInfo.StopBits = new List<StopBits>
            {
                new StopBits { Description = "One", SelectedModel = System.IO.Ports.StopBits.One },
                new StopBits { Description = "OnePointFive", SelectedModel = System.IO.Ports.StopBits.OnePointFive },
                new StopBits { Description = "Two", SelectedModel = System.IO.Ports.StopBits.Two },

            };
            SerialPortInfo.SelectedStopBits = SerialPortInfo.StopBits[0].SelectedModel;

            SerialPortInfo.Parity = new List<Parity>
            {
                new Parity { Description = "None", SelectedModel = System.IO.Ports.Parity.None },
                new Parity { Description = "Odd", SelectedModel = System.IO.Ports.Parity.Odd },
                new Parity { Description = "Even", SelectedModel = System.IO.Ports.Parity.Even },
                new Parity { Description = "Mark", SelectedModel = System.IO.Ports.Parity.Mark },

            };
            SerialPortInfo.SelectedParity = SerialPortInfo.Parity[0].SelectedModel;
        }
        void GetPortName()
        {
            SerialPortInfo.SerialPortName = new List<string>();
            string[] SerialPortName = SerialPort.GetPortNames();
            SerialPortInfo.SerialPortName = SerialPortName.ToList();
            if (SerialPort.GetPortNames().Any())
            {
                SerialPortInfo.SelectedSerialPortName = serialPortInfo.SerialPortName[0];
            }
        }
        private void Execute(string obj)
        {
            switch (obj)
            {
                case "OpenPort": OpenPort(); break;
                //case "SendData": SendText(); break;
                //case "CleanDisplay": CleanDisplay(); break;
                //case "Circulate": CirculateSend(); break;
            }
        }
        public void OpenPort()
        {
            if (!SerialPort.GetPortNames().Any() || string.IsNullOrWhiteSpace(SerialPortInfo.SelectedSerialPortName))
            {
                MessageBox.Show("该设备没有发现端口(COM和LPT)", "警告");
                GetPortName();
                return;
            }
            switch (Sp.IsOpen)
            {
                case true:
                    this.Mess = "";
                    try
                    {
                        Sp.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        return;
                    }
                    SerialPortInfo.PortOpenButton = new string[4] { "打开串口", "#676767", "TRUE", "FALSE" };
                    break;
                case false:
                    Sp.PortName = SerialPortInfo.SelectedSerialPortName;
                    Sp.BaudRate = SerialPortInfo.SelectedBaudRate;
                    Sp.DataBits = SerialPortInfo.SelectedDataBits;
                    Sp.StopBits = SerialPortInfo.SelectedStopBits;
                    Sp.Parity = serialPortInfo.SelectedParity;
                    try
                    {
                        Sp.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        return;
                    }
                    SerialPortInfo.PortOpenButton = new string[4] { "关闭串口", "#7d4dcd", "FALSE", "TRUE" };
                    break;
            }
        }
    }

    public class ChartData
    {
        public DateTime Date { get; set; }

        public double Total { get; set; }

        public double PassRate { get; set; }
    }
}
