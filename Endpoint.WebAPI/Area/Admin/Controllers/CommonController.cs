using CmsRebin.Application.Service.Common.SMS;
using Dto.Other;
using Dto.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Threading.Tasks;
using ZarinPal.Class;


using static CmsRebin.Application.Service.Common.SMS.SMSSender;

namespace Endpoint.WebAPI.Area.Admin.Controllers
{
    [Route("api /[controller] /[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        //for payment Zarinpal
         
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;


        private readonly ISMSSender _iSMSSender;
        private readonly ILogger<DBController> _logger;


        public CommonController(ISMSSender iSMSSender,  ILogger<DBController> logger)
        {
            _logger = logger;
            _iSMSSender = iSMSSender;


            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
        }
         
        [Area("Admin")]
        [HttpPost]
        public IActionResult SendSMS([FromBody] SMSSendRequest request)
        {


            _iSMSSender.SMS(request);


            _logger.LogInformation("send sms to {0}", request.to);
            return Ok();
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult SendSMSF([FromBody] SMSSendRequest2 request)
        {
            _iSMSSender.SMSF(request);


            _logger.LogInformation("send sms to {0}", request.to);
            return Ok();
        }


        [Area("Admin")]
        [HttpPost]
        public IActionResult SendEmail([FromBody] EmailSendRequest request)
        {

            _iSMSSender.SendEmailAsync(request);


            _logger.LogInformation("send Email to {0}", request.to);
            return Ok();
        }




        [Area("Admin")]
        [HttpPost]

        public IActionResult Report([FromBody] ReportRequest req)
        {
            _iSMSSender.Report(req);


            _logger.LogInformation("REport of  {0} {1}",req.table,req.valuefildname);
            return Ok();

        }

        //[Area("Admin")]
        //[HttpPost]
        //public IActionResult Pardakht( )
        //{
             
        //    _logger.LogInformation("pardakht ");
        //    return Ok();

        //}
        ///////////////////////////////////////////////////////////////////////


        /// <summary>
        /// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ
        /// </summary>
        /// <returns></returns>
        /// 
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Request1([FromBody] PayRequest req)
        {
            ////dargah vagheiii
            //req.RedirectURL = "https://zarinpal.com/pg/StartPay/";
            //req.MerchantId = "140003100074";
            //req.CallbackUrl= "https://localhost:44332/Common/RefreshAuthority";
            //var R = new DtoRequest()
            //{
            //    Mobile = req.Mobile,//_user.GetUserphone(1027),
            //    // Mobile = "09121234563",
            //    CallbackUrl = req.CallbackUrl,//"https://localhost:44332",
            //    // CallbackUrl = "https://localhost:44351/Panel/Dashboard",
            //    Description = req.Description,
            //    Email = req.Email,// _user.GetUsermail(1027),
            //    Amount = req.Price,
            //    MerchantId = req.MerchantId,
            //};
            //var result = await _payment.Request(R, Payment.Mode.zarinpal); ///mode is 2 value=1. Mode.zarinpal for real pay. 2. Mode.standbox for test.
            //return Redirect("https://zarinpal.com/pg/StartPay/" + $"{result.Authority}");



            /////dargahtest
            req.RedirectURL = "https://sandbox.zarinpal.com/pg/StartPay/";
            req.MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX";// "140003100074";
            req.CallbackUrl = "https://localhost:44332/Common/Verify";
            var R = new DtoRequest()
            {
                Mobile = req.Mobile,//_user.GetUserphone(1027),
                // Mobile = "09121234563",
                CallbackUrl = req.CallbackUrl+$"?guid={req.guid}",//"https://localhost:44332",
                // CallbackUrl = "https://localhost:44351/Panel/Dashboard",
                Description = req.Description+ req.RequwstPayId,
                Email = req.Email,// _user.GetUsermail(1027),
                Amount = req.Price,
                MerchantId = req.MerchantId,
            };
            var result = await _payment.Request(R, Payment.Mode.sandbox); ///mode is 2 value=1. Mode.zarinpal for real pay. 2. Mode.standbox for test.
            //Redirect(req.RedirectURL + $"{result.Authority}");
            string rerirectpath = req.RedirectURL + $"{result.Authority}";

            return Ok(rerirectpath);

            //return Redirect($"https://zarinp.al/swan");

            //return RedirectToAction("Account", "Login");
            //return View();
        }



        //public async Task<IActionResult> VerifyAsync([FromBody] Guid guid, string authority, string status)
        //{

              
        //    var verification = await _payment.Verification(new DtoVerification
        //    {
        //        Amount =  2323,//getprice frome
        //        MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
        //        Authority = authority,
        //    }, Payment.Mode.sandbox);
        //    if (verification.Status == 100)
        //    {
        //        return RedirectToAction("Dashboard", "Panel");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }


        //    return Ok();
        //}



        /// <summary>
        /// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ ﺑﺎ ﺗﺴﻮﻳﻪ ﺍﺷﺘﺮﺍﻛﻲ 
        /// </summary>
        /// <returns></returns>
        /// 

        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> RequestWithExtra([FromBody] PayRequest req)
        {

            ////dargah vagheii
            //req.RedirectURL = "https://zarinpal.com/pg/StartPay/";
            //req.MerchantId = "140003100074";
            //var result = await _payment.Request(new DtoRequestWithExtra()
            //{
            //    Mobile = req.Mobile,
            //    //CallbackUrl = "https://localhost:44310/home/validate",
            //    CallbackUrl = "https://localhost:44351/home/validate",//req.CallbackUrl
            //    Description = req.Description,
            //    Email = req.Email,
            //    Amount = req.Price,
            //    MerchantId = req.MerchantId,

            //    AdditionalData = "{\"Wages\":{\"zp.1.1\":{\"Amount\":120,\"Description\":\" ﺗﻘﺴﻴﻢ \"}, \" ﺳﻮﺩ ﺗﺮﺍﻛﻨﺶ zp.2.5\":{\"Amount\":60,\"Description\":\" ﻭﺍﺭﻳﺰ \"}}} "
            //}, ZarinPal.Class.Payment.Mode.zarinpal);
            ////return Redirect(s.Payment.Mode.zarinpal);
            //return Redirect( req.RedirectURL + $"{result.Authority}");

            ///dargahe test
            ///
            req.RedirectURL = "https://sandbox.zarinpal.com/pg/StartPay/";
            req.MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX";

            var result = await _payment.Request(new DtoRequestWithExtra()
            {
                Mobile = req.Mobile,
                //CallbackUrl = "https://localhost:44310/home/validate",
                CallbackUrl = "https://localhost:44351/home/validate",//req.CallbackUrl+ req.CallbackUrl+$"?guid{req.guid}"
                Description = req.Description +req.RequwstPayId,
                Email = req.Email,
                Amount = req.Price,
                MerchantId = req.MerchantId,

                AdditionalData = "{\"Wages\":{\"zp.1.1\":{\"Amount\":120,\"Description\":\" ﺗﻘﺴﻴﻢ \"}, \" ﺳﻮﺩ ﺗﺮﺍﻛﻨﺶ zp.2.5\":{\"Amount\":60,\"Description\":\" ﻭﺍﺭﻳﺰ \"}}} "
            }, ZarinPal.Class.Payment.Mode.sandbox);
            //return Redirect(s.Payment.Mode.zarinpal);
            return Redirect(req.RedirectURL + $"{result.Authority}");




        }
        /// <summary>
        /// اعتبار سنجی خرید
        /// </summary>
        /// <param name="authority"></param>

        /// <param name="status"></param>
        /// <returns></returns>
        /// 
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] PayVerification req)//string authority, string status
        {
            req.MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX";// "140003100074";
            var verification = await _payment.Verification(new DtoVerification
            {
                Amount = req.Price,
                MerchantId = req.MerchantId,
                Authority = req.authority
            }, Payment.Mode.sandbox);
            if (verification.Status == 100)
            {
                return RedirectToAction("Dashboard", "Panel");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


            //return View();
        }

        /// <summary>
        /// ﺩﺭ ﺭﻭﺵ ﺍﻳﺠﺎﺩ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﺑﺎ ﻃﻮﻝ ﻋﻤﺮ ﺑﺎﻻ ﻣﻤﻜﻦ ﺍﺳﺖ ﺣﺎﻟﺘﻲ ﭘﻴﺶ ﺁﻳﺪ ﻛﻪ ﺷﻤﺎ ﺑﻪ ﺗﻤﺪﻳﺪ ﺑﻴﺸﺘﺮ ﻃﻮﻝ ﻋﻤﺮ ﻳﻚ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ
        /// ﺩﺭ ﺍﻳﻦ ﺻﻮﺭﺕ ﻣﻲ ﺗﻮﺍﻧﻴﺪ ﺍﺯ ﻣﺘﺪ زیر ﺍﺳﺘﻔﺎﺩﻩ ﻧﻤﺎﻳﻴﺪ 
        /// </summary>
        /// <returns></returns>
        /// 
        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> RefreshAuthority()
        {
            var refresh = await _authority.Refresh(new DtoRefreshAuthority
            {
                Authority = "",
                ExpireIn = 1,
                MerchantId = "140003100074"
            }, Payment.Mode.zarinpal);
            return Ok();
        }

        /// <summary>
        /// ﻣﻤﻜﻦ ﺍﺳﺖ ﺷﻤﺎ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ ﻛﻪ ﻣﺘﻮﺟﻪ ﺷﻮﻳﺪ ﭼﻪ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﺗﻮﺳﻂ ﻭﺏ ﺳﺮﻭﻳﺲ ﺷﻤﺎ ﺑﻪ ﺩﺭﺳﺘﻲ ﺍﻧﺠﺎﻡ ﺷﺪﻩ ﺍﻣﺎ ﻣﺘﺪ  ﺭﻭﻱ ﺁﻧﻬﺎ ﺍﻋﻤﺎﻝ ﻧﺸﺪﻩ
        /// ، ﺑﻪ ﻋﺒﺎﺭﺕ ﺩﻳﮕﺮ ﺍﻳﻦ ﻣﺘﺪ ﻟﻴﺴﺖ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﻣﻮﻓﻘﻲ ﻛﻪ ﺷﻤﺎ ﺁﻧﻬﺎ ﺭﺍ ﺗﺼﺪﻳﻖ ﻧﻜﺮﺩﻩ ﺍﻳﺪ ﺭﺍ ﺑﻪ PaymentVerification ﺷﻤﺎ ﻧﻤﺎﻳﺶ ﻣﻲ ﺩﻫﺪ.
        /// </summary>
        /// <returns></returns>
        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Unverified()
        {
            var refresh = await _transactions.GetUnverified(new DtoMerchant
            {
                MerchantId = "140003100074"
            }, Payment.Mode.zarinpal);
            return Ok();
        }

/////////////////////////////////////////////////////////////////////////





    }
}
