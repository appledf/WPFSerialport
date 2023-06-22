using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSerialport.Common.Models
{
    public class BaudRate
    {
        public string Description { get; set; }

        public Int32 SelectedModel { get; set; }
    }

    public class StopBits
    {
        public string Description { get; set; }

        public System.IO.Ports.StopBits SelectedModel { get; set; }
    }

    public class Parity
    {
        public string Description { get; set; }

        public System.IO.Ports.Parity SelectedModel { get; set; }
    }

    public class DataBits
    {
        public string Description { get; set; }

        public Int32 SelectedModel { get; set; }
    }

    public class ReceiveSet
    {
        public bool ReceiveIsAscii { get; set; }

        public bool ReceiveIsHex { get; set; }

        public bool AutoLineFeed { get; set; }

        public bool DisplaySend { get; set; }

        public bool DisplayTime { get; set; }

    }

    public class SendSet
    {
        public bool SendIsAscii { get; set; }

        public bool SendIsHex { get; set; }

        public bool Circulate { get; set; }

        public Int16 CirculateTime { get; set; }

    }
}
