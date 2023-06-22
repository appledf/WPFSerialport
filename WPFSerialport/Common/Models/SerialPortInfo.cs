using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSerialport.Common.Models
{
    public class SerialPortInfo : BaseDto
    {
        private List<string> serialportname;

        /// <summary>
        /// 串口号
        /// </summary>
        public List<string> SerialPortName
        {
            get { return serialportname; }
            set { serialportname = value; OnPropertyChanged(); }
        }

        private string selectedSerialPortName;
        public string SelectedSerialPortName
        {
            get { return selectedSerialPortName; }
            set { selectedSerialPortName = value; OnPropertyChanged(); }
        }

        private List<BaudRate> baudRate;

        /// <summary>
        /// 波特率
        /// </summary>
        public List<BaudRate> BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; OnPropertyChanged(); }
        }

        private Int32 selectedBaudRate;
        public Int32 SelectedBaudRate
        {
            get { return selectedBaudRate; }
            set { selectedBaudRate = value; OnPropertyChanged(); }
        }

        private List<StopBits> stopbits;

        /// <summary>
        /// 停止位
        /// </summary>
        public List<StopBits> StopBits
        {
            get { return stopbits; }
            set { stopbits = value; OnPropertyChanged(); }
        }

        private System.IO.Ports.StopBits selectedStopBits;
        public System.IO.Ports.StopBits SelectedStopBits
        {
            get { return selectedStopBits; }
            set { selectedStopBits = value; OnPropertyChanged(); }
        }

        private List<Parity> parity;

        /// <summary>
        /// 校验
        /// </summary>
        public List<Parity> Parity
        {
            get { return parity; }
            set { parity = value; OnPropertyChanged(); }
        }

        private System.IO.Ports.Parity selectedParity;
        public System.IO.Ports.Parity SelectedParity
        {
            get { return selectedParity; }
            set { selectedParity = value; OnPropertyChanged(); }
        }

        private List<DataBits> dataBits;

        /// <summary>
        /// 数据位
        /// </summary>
        public List<DataBits> DataBits
        {
            get { return dataBits; }
            set { dataBits = value; OnPropertyChanged(); }
        }

        private Int32 selectedDataBits;
        public Int32 SelectedDataBits
        {
            get { return selectedDataBits; }
            set { selectedDataBits = value; OnPropertyChanged(); }
        }

        private string[] portOpenButton;

        /// <summary>
        /// 打开串口按钮设置
        /// </summary>
        public string[] PortOpenButton
        {
            get { return portOpenButton; }
            set { portOpenButton = value; OnPropertyChanged(); }
        }

        private string sendText;

        /// <summary>
        /// 写数据
        /// </summary>
        public string SendText
        {
            get { return sendText; }
            set { sendText = value; OnPropertyChanged(); }
        }

        private string receiveText;

        /// <summary>
        /// <summary>发送记录显示
        /// </summary>
        public string ReceiveText
        {
            get { return receiveText; }
            set { receiveText = value; OnPropertyChanged(); }
        }

        private ReceiveSet receiveSet;

        /// <summary>
        /// 接收区设置
        /// </summary>
        public ReceiveSet ReceiveSet
        {
            get { return receiveSet; }
            set { receiveSet = value; OnPropertyChanged(); }
        }

        private SendSet sendSet;

        /// <summary>
        /// 发送区设置
        /// </summary>
        public SendSet SendSet
        {
            get { return sendSet; }
            set { sendSet = value; OnPropertyChanged(); }
        }

    }
}
