using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CmsRebin.Application.Service.Common.SMS.SMSSender;

namespace CmsRebin.Application.Service.Common.SMS
{
    public interface ISMSSender
    {
        public void SMS(SMSSendRequest req);
        public void SMSF(SMSSendRequest2 req);
        public Task SendEmailAsync(EmailSendRequest req);
        public Task Report(ReportRequest req);
        public void Notification(NotificationSendRequest req);

    }
}
