using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.SMS
{
    public class SMSSendRequest
    {
        public string sender { get; set; }
        public string to { get; set; }
        public string txt { get; set; }
        public string apikey { get; set; }
    }
    public class SMSSendRequest2
    {
        public string to { get; set; }
        public string txt { get; set; }
    }
}
