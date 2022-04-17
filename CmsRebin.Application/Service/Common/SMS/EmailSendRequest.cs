using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.SMS
{
    public class EmailSendRequest
    {
        public NetworkCredential from { get; set; }
        public string titleFrom { get; set; }
        public string to { get; set; }
        public string titleTo { get; set; }
        public string subject { get; set; }
        public string message1 { get; set; }
    }
}
