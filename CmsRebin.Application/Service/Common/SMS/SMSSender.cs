using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Infrastructure.Enum;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Kavenegar;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using ZarinPal.Class;

namespace CmsRebin.Application.Service.Common.SMS
{

    public class SMSSender : ISMSSender
    {
        /// 
         //for payment Zarinpal
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;




        private readonly IDatabaseContext _context;
        private readonly IGetEverythings _getEverythings;
        public SMSSender(IDatabaseContext context, IGetEverythings getEverythings)
        {
            _context = context;
            _getEverythings = getEverythings;

        }
        //  private IAdmin _admin;

        // AuthorizationFilterContext context ;
        // 

        public void SMS(SMSSendRequest req)
        {// _admin = (IAdmin)context.HttpContext.RequestServices.GetService(typeof(IAdmin));

            // Setting setting = _admin.GetSetting();
            // var sender = setting.SMSSender;
            //var sender = "1000596446";//10000900900300//1008663
            req.sender = "10000900900300";//10000900900300//1008663
            var resiver = req.to;
            var txte = req.txt;
            req.apikey = "6F35654138502B574E563439634A782B6177333770766356546B564B706B736D4A76686F392B4C736F70773D";
            var api = new KavenegarApi(req.apikey);
            //var api = new KavenegarApi(setting.SMSApi);

            api.Send(req.sender, resiver, txte);

        }
        public void SMSF(SMSSendRequest2 req)
        {  //"from=10009424&to=9188716505&text=تست سامانه&password=nim@123&username=kurdet";
            string URI = "http://87.107.121.52/post/sendsms.ashx";
            string myParameters = "from=50002237171&to=";
            myParameters += req.to + "&text=" + req.txt + "&password=nim@123@nim&username=westco";
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string HtmlResult = wc.UploadString(URI, myParameters);


        }

        public async Task SendEmailAsync(EmailSendRequest req)
        {
            req.from.UserName = "Swan.kurdestan@gmail.com";
            req.from.Password = "Swan11193";

            req.titleFrom = "سیستم مدیریت ریبین";
            req.titleTo = "کاربرگرامی";
            using (var Client = new SmtpClient())
            {
                var Credential = new NetworkCredential
                {
                    UserName = req.from.UserName,
                    Password =req. from.Password

                };
                Client.Credentials = Credential;
                Client.Host = "smtp.gmail.com";
                Client.Port = 587; // or 25  -- 587 -- 465 For Send Email
                Client.EnableSsl = true;
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(req.to, req.titleTo));
                    message.From = new MailAddress(req.from.UserName,req. titleFrom);

                    message.Subject = req.subject;
                    message.IsBodyHtml = true;
                    message.Body = req.message1;

                    Client.Send(message);
                };
                await Task.CompletedTask;
            }
            /////////////////////////////////////////////////////////////////
        }

       

        public async Task Report(ReportRequest req)
        {
            StiPdfExportService pdfexport = new StiPdfExportService();
            StiReport report = new StiReport();
            var data = _getEverythings.Execute2(new RequestGetDto { DbName = req.db, nametable =req.table, filters = new Equation { Filname = new List<string>() { req.fildname }, Tablename = req.table, DBname = req.db, Compare = new List<string>() { "=" }, Value = new List<string>() { req.valuefildname }, Addcon = null } });
            req.TypetosavefileREport = ".pdf";
            //req.nametosavefileREport=req.fildname
            req.TablebameRecomamnded = "DataSource";

            string s = System.IO.Directory.GetCurrentDirectory();
            report.Load(System.IO.Directory.GetCurrentDirectory() + "/Report.mrt");

            //coustomise kardan data . motanaseb ba dt tarahi shode.
            report.RegData("dt", data);
            report.Render(); 
            pdfexport.ExportPdf(report, req.nametosavefileREport.Replace(".pdf", "") + ".pdf");
            await Task.CompletedTask;
        }

        
        /// <summary>
        /// 1401-01-28
        /// 
        public void Notification(NotificationSendRequest req)
        {
            // FirebaseAdmin.Instance.Notification(req);
            FirebaseApp.Create(new AppOptions() { Credential=GoogleCredential.FromFile("private_key.json") });
            var registrationToken = "ADD_TOKEN";//get DiviceToken
            var message = new Message()
            {
                Data=new Dictionary<string, string>() { {"myDate","1337" }}
                , 
                Token=registrationToken,
                Notification=new Notification()
                {
                    Title="hi ",
                    Body=" this is boby1"
                }
            };

            string responce = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
        
        }


        ///// <summary>
        ///// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ
        ///// </summary>
        ///// <returns></returns>
        //public async Task Request()
        //{
        //    var result = await _payment.Request(new DtoRequest()
        //    {
        //        Mobile = _user.GetUserphone(1027),
        //        // Mobile = "09121234563",
        //        CallbackUrl = "https://localhost:44351/Payment/Validate",
        //        // CallbackUrl = "https://localhost:44351/Panel/Dashboard",
        //        Description = "پرداخت فاکتوز شماره",
        //        Email = _user.GetUsermail(1027),
        //        Amount = 1000000,
        //        MerchantId = "140003100074",
        //    }, ZarinPal.Class.Payment.Mode.zarinpal); ;
        //    return Redirect($"https://zarinpal.com/pg/StartPay/{result.Authority}");
        //    //return Redirect($"https://zarinp.al/swan");

        //    //return RedirectToAction("Account", "Login");
        //    //return View();
        //}

        ///// <summary>
        ///// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ ﺑﺎ ﺗﺴﻮﻳﻪ ﺍﺷﺘﺮﺍﻛﻲ 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> RequestWithExtra()
        //{
        //    var result = await _payment.Request(new DtoRequestWithExtra()
        //    {
        //        Mobile = "09121112222",
        //        //CallbackUrl = "https://localhost:44310/home/validate",
        //        CallbackUrl = "https://localhost:44351/home/validate",
        //        Description = "توضیحات",
        //        Email = "farazmaan@outlook.com",
        //        Amount = 1000000,
        //        MerchantId = "140003100074",
        //        ////MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
        //        AdditionalData = "{\"Wages\":{\"zp.1.1\":{\"Amount\":120,\"Description\":\" ﺗﻘﺴﻴﻢ \"}, \" ﺳﻮﺩ ﺗﺮﺍﻛﻨﺶ zp.2.5\":{\"Amount\":60,\"Description\":\" ﻭﺍﺭﻳﺰ \"}}} "
        //    }, ZarinPal.Class.Payment.Mode.zarinpal);
        //    return Redirect($"https://zarinpal.com/pg/StartPay/{result.Authority}");
        //}
        ///// <summary>
        ///// اعتبار سنجی خرید
        ///// </summary>
        ///// <param name="authority"></param>

        ///// <param name="status"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> Validate(string authority, string status)
        //{
        //    var verification = await _payment.Verification(new DtoVerification
        //    {
        //        Amount = 1000000,
        //        MerchantId = "140003100074",
        //        Authority = authority
        //    }, Payment.Mode.zarinpal);
        //    if (verification.Status == 100)
        //    {
        //        return RedirectToAction("Dashboard", "Panel");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }


        //    //return View();
        //}

        ///// <summary>
        ///// ﺩﺭ ﺭﻭﺵ ﺍﻳﺠﺎﺩ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﺑﺎ ﻃﻮﻝ ﻋﻤﺮ ﺑﺎﻻ ﻣﻤﻜﻦ ﺍﺳﺖ ﺣﺎﻟﺘﻲ ﭘﻴﺶ ﺁﻳﺪ ﻛﻪ ﺷﻤﺎ ﺑﻪ ﺗﻤﺪﻳﺪ ﺑﻴﺸﺘﺮ ﻃﻮﻝ ﻋﻤﺮ ﻳﻚ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ
        ///// ﺩﺭ ﺍﻳﻦ ﺻﻮﺭﺕ ﻣﻲ ﺗﻮﺍﻧﻴﺪ ﺍﺯ ﻣﺘﺪ زیر ﺍﺳﺘﻔﺎﺩﻩ ﻧﻤﺎﻳﻴﺪ 
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IActionResult> RefreshAuthority()
        //{
        //    var refresh = await _authority.Refresh(new DtoRefreshAuthority
        //    {
        //        Authority = "",
        //        ExpireIn = 1,
        //        MerchantId = "140003100074"
        //    }, Payment.Mode.zarinpal);
        //    return View();
        //}

        ///// <summary>
        ///// ﻣﻤﻜﻦ ﺍﺳﺖ ﺷﻤﺎ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ ﻛﻪ ﻣﺘﻮﺟﻪ ﺷﻮﻳﺪ ﭼﻪ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﺗﻮﺳﻂ ﻭﺏ ﺳﺮﻭﻳﺲ ﺷﻤﺎ ﺑﻪ ﺩﺭﺳﺘﻲ ﺍﻧﺠﺎﻡ ﺷﺪﻩ ﺍﻣﺎ ﻣﺘﺪ  ﺭﻭﻱ ﺁﻧﻬﺎ ﺍﻋﻤﺎﻝ ﻧﺸﺪﻩ
        ///// ، ﺑﻪ ﻋﺒﺎﺭﺕ ﺩﻳﮕﺮ ﺍﻳﻦ ﻣﺘﺪ ﻟﻴﺴﺖ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﻣﻮﻓﻘﻲ ﻛﻪ ﺷﻤﺎ ﺁﻧﻬﺎ ﺭﺍ ﺗﺼﺪﻳﻖ ﻧﻜﺮﺩﻩ ﺍﻳﺪ ﺭﺍ ﺑﻪ PaymentVerification ﺷﻤﺎ ﻧﻤﺎﻳﺶ ﻣﻲ ﺩﻫﺪ.
        ///// </summary>
        ///// <returns></returns>

        //public async Task<IActionResult> Unverified()
        //{
        //    var refresh = await _transactions.GetUnverified(new DtoMerchant
        //    {
        //        MerchantId = "140003100074"
        //    }, Payment.Mode.zarinpal);
        //    return View();
        //}




        //public class SMSSendRequest
        //{
        //    public string sender { get; set; }
        //    public string to { get; set; }
        //    public string txt { get; set; }
        //    public string apikey { get; set; }
        //}
        //public class EmailSendRequest
        //{
        //    public NetworkCredential from { get; set; }
        //    public string titleFrom { get; set; }
        //    public string to { get; set; }
        //    public string titleTo { get; set; }
        //    public string subject { get; set; }
        //    public string message1 { get; set; }

        //}
        public class ReportRequest
        {

            public string fildname { get; set; }
            public string table { get; set; }
            public string db { get; set; }
            public string valuefildname { get; set; }
            public string mrtAdress { get; set; }
            public string TablebameRecomamnded { get; set; }
            public string TypetosavefileREport { get; set; }
            public string nametosavefileREport { get; set; }
        }
        public class PayRequest
        {
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string CallbackUrl { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public string MerchantId { get; set; }
            public string RedirectURL { get; set; }
            public int RequwstPayId { get; set; }
            public Guid guid { get; set; }
        }
        public class PayVerification
        {
            public string authority { get; set; }

            public int Price { get; set; }
            public string MerchantId { get; set; }
            public string status { get; set; }
        }
    }
}
