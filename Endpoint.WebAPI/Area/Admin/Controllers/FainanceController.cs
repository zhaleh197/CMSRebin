using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Common.Fainances.Commands.AddRequestPay;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Common.SMS;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using static CmsRebin.Application.Service.Common.SMS.SMSSender;

namespace Endpoint.WebAPI.Area.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FainanceController : ControllerBase
    {

        private readonly ILogger<DBController> _logger;
        private readonly ICreatTable _creatTable;
        private readonly ICreateFiled _createFiled;
        private readonly ISMSSender _iSMSSender;

        private readonly ICreateItem _createItem;


        private readonly IGetEverythings _getEverythings;
        public FainanceController(
            ILogger<DBController> logger,
            ICreatTable creatTable, ICreateFiled createFiled, ICreateItem createItem, ISMSSender iSMSSender, IGetEverythings getEverythings)
        {

            _logger = logger;
            _creatTable = creatTable;
            _createFiled = createFiled;
            _createItem = createItem;
            _iSMSSender=iSMSSender;
            _getEverythings = getEverythings;
    }

        [Area("Admin")]
        [HttpPost]
        //[Route("/CreatDB")]
        public IActionResult CreateRequestPayTable([FromBody]  RequestCreatPayDto req)
        {

            var result1 = _creatTable.Execute(new RequestCreateTableDto { collection = "RequsetPay", DbName = req.DBName, note = "", singleton = false });
            var result2 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Guid", Nullable = true, Relation = null, table2 = null, tablename = "RequsetPay", type = "string", Uniqe = true });
            var result3 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = "id", name = "IdUser", Nullable = true, Relation ="1-n", table2 = "Users", tablename = "RequsetPay", type = "int", Uniqe = false });
            var result4 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Amount", Nullable = true, Relation =null, table2 = null, tablename = "RequsetPay", type = "int", Uniqe = false });
            var result5 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "IsPay", Nullable = true, Relation = null, table2 = null, tablename = "RequsetPay", type = "bit", Uniqe = false });
            var result6= _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "PayDate", Nullable = true, Relation = null, table2 = null, tablename = "RequsetPay", type = "datetime", Uniqe = false });
            var result7 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "RefId", Nullable = true, Relation = null, table2 = null, tablename = "RequsetPay", type = "int", Uniqe = false });
      
            _logger.LogInformation("Creat RequsetPay {0} by Owner {0}", req.DBName, req.IdUserOwner);
            return Ok(result1.Data.TableId);
        }


        [Area("Admin")]
        [HttpPost]

        //[Route("/CreatDB")]
        public IActionResult SubmitRequestPayKoli([FromBody] RequestPayDto req)
        {
            req.Guid = Guid.NewGuid().ToString();
            req.PayDate = DateTime.Now;
            req.RefId = 0;
            req.IsPay = false;
            var result1 = _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { req.Guid, req.IdUser.ToString(), req.Amount.ToString(), req.IsPay.ToString(), req.PayDate.ToString(), req.RefId.ToString() }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid", "IdUser", "Amount", "IsPay", "PayDate", "RefId" }, TableName = "RequsetPay" });
            var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });
            var result2 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.Guid.ToString() }, }, nametable = "RequsetPay", DbName = req.DBName });

            //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });
   
            for (int i = 5; i < result2.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(result2.ITM[0].fieldnamelist[i]);
                result.ITM[0].valuefiledlistList.Add(result2.ITM[0].valuefiledlistList[i]);
            }

            _logger.LogInformation("Creat RequsetPay {0} in {1}", req.IdUser
                , req.DBName);
            return Ok(result);
        }


        [Area("Admin")]
        //[HttpGet]
        [HttpGet("{Guid}")]
        //[Route("/CreatDB")]
        public IActionResult GetuserAndPaybyGuidKoli([FromRoute] string Guid, string DB, string OrderTable)
        {
            var payment = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { Guid }, }, nametable = "RequsetPay", DbName = DB });
            string Userid = payment.ITM[0].valuefiledlistList[6][0];
            var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { Userid }, }, nametable = "Users", DbName = DB });
            string username = result.ITM[0].valuefiledlistList[5][0];
            //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });
            string RequestPayId = payment.ITM[0].valuefiledlistList[0][0];



            var result3 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "idRequestPay" }, Compare = new List<string>() { "=" }, Value = new List<string>() { RequestPayId } }, nametable = OrderTable, DbName = DB });
            var operator1 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { result3.ITM[0].valuefiledlistList[5][0] } }, nametable = "Users", DbName = DB });

            for (int i = 5; i < payment.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(payment.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(payment.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(payment.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(payment.ITM[0].valuefiledlistList[i]);
            }
            for (int i = 5; i < operator1.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(operator1.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(operator1.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(operator1.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(operator1.ITM[0].valuefiledlistList[i]);
            }
       
            //var user = _getEverythings.Execute2(new RequestGetDto);
            return Ok(result);
        }


        /// <summary>
        /// ///////////////////// *** THIS IS FOR EPAY PROJECT - 1400-12-10 ****
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>

        [Area("Admin")]
        [HttpPost]

        //[Route("/CreatDB")]
        public IActionResult SubmitRequestPay([FromBody] RequestPayDto req)
        {
            req.Guid = Guid.NewGuid().ToString();
            req.PayDate = DateTime.Now;
            req.RefId = 0;
            req.IsPay = false;
            var result1= _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { req.Guid,req.IdUser.ToString(),req.Amount.ToString(),req.IsPay.ToString(),req.PayDate.ToString(),req.RefId.ToString()  }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid" , "IdUser", "Amount", "IsPay", "PayDate", "RefId" }, TableName = "RequsetPay" });
            var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname=new List<string>() { "id" }, Compare=new List<string>() { "=" },Value=new List<string>() { req.IdUser.ToString() },}, nametable = "Users", DbName = req.DBName });
            var result2= _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.Guid.ToString() }, }, nametable = "RequsetPay", DbName = req.DBName });

            //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });


            string username = result.ITM[0].valuefiledlistList[5][0];

            string SmsText = username+ " عزیز فاکتور شما در لینک زیر قابل مشاده است لطفه جهت پرداخت کلیک کنید" +"\n"+ "https://localhost:7057/Home/ViewUser/?Guid=" + req.Guid;//https://localhost:44332/ViewRepository/ViewData/

            _iSMSSender.SMS(new SMSSendRequest {to= req.Mobile,txt=SmsText});
            for (int i = 5; i < result2.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(result2.ITM[0].fieldnamelist[i]);
                result.ITM[0].valuefiledlistList.Add(result2.ITM[0].valuefiledlistList[i]);
            }

            _logger.LogInformation("Creat RequsetPay {0} in {1}",req.IdUser
                , req.DBName);
            return Ok(result);
        }

        [Area("Admin")]
        //[HttpGet]
        [HttpGet("{Guid}")]
        //[Route("/CreatDB")]
        public IActionResult GetuserbyGuid([FromRoute] string Guid)
        {


            var payment = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() {  Guid }, }, nametable = "RequsetPay", DbName = "EPay" });
            string Userid= payment.ITM[0].valuefiledlistList[6][0];
            var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { Userid }, }, nametable = "Users", DbName = "EPay" });
            
            string username = result.ITM[0].valuefiledlistList[5][0];
            //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });
            string RequestPayId = payment.ITM[0].valuefiledlistList[0][0];
            


            var result3 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "idRequestPay" }, Compare = new List<string>() { "=" }, Value = new List<string>() { RequestPayId } }, nametable = "AvaresList", DbName = "EPay" });
            var operator1 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { result3.ITM[0].valuefiledlistList[5][0] } }, nametable = "Users", DbName = "EPay" }) ;
            //var operator = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "UserName" }, Compare = new List<string>() { "=" }, Value = new List<string>() { result3.ITM[0].valuefiledlistList[0][0] } }, nametable = "Users", DbName = "EPay" }); 

            var zirsazman= _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { result3.ITM[0].valuefiledlistList[7][0] } }, nametable = "ZirSazman", DbName = "EPay" }) ;
            var typeavarez = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { result3.ITM[0].valuefiledlistList[8][0] } }, nametable = "AvarezType", DbName = "EPay" }) ;

            for (int i = 5; i < payment.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(payment.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(payment.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(payment.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(payment.ITM[0].valuefiledlistList[i]);
            }
            for (int i = 5; i < operator1.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(operator1.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(operator1.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(operator1.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(operator1.ITM[0].valuefiledlistList[i]);
            }
            for (int i = 5; i < zirsazman.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(zirsazman.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(zirsazman.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(zirsazman.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(zirsazman.ITM[0].valuefiledlistList[i]);
            }
            for (int i = 5; i < typeavarez.ITM[0].fieldnamelist.Count; i++)
            {
                result.ITM[0].fieldnamelist.Add(typeavarez.ITM[0].fieldnamelist[i]);
                //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
                result.ITM[0].releationfiled.Add(typeavarez.ITM[0].releationfiled[i]);
                result.ITM[0].table2id.Add(typeavarez.ITM[0].table2id[i]);
                result.ITM[0].valuefiledlistList.Add(typeavarez.ITM[0].valuefiledlistList[i]);
            }
            //var user = _getEverythings.Execute2(new RequestGetDto);
            return Ok(result);
        }


        [Area("Admin")]
        //[HttpGet]
        [HttpGet("{Guid}")]
        //[Route("/CreatDB")]
        public IActionResult GetuserbyGuidInsta([FromRoute] string Guid)
        {

            //payment
            var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { Guid }, }, nametable = "RequsetPay", DbName = "Instagram" });
            string RequestPayId = result.ITM[0].valuefiledlistList[0][0];
           
            var orders = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "idRequestPay" }, Compare = new List<string>() { "=" }, Value = new List<string>() { RequestPayId }, }, nametable = "Orders", DbName = "Instagram" });
            string productid = orders.ITM[0].valuefiledlistList[5][0];
            
            string username = orders.ITM[0].valuefiledlistList[8][0];
           
            var Product = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { productid }, }, nametable = "Product", DbName ="Instagram" });
            
            string Productname= Product.ITM[0].valuefiledlistList[6][0];
            string Price= Product.ITM[0].valuefiledlistList[8][0];

            var Seller = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { Product.ITM[0].valuefiledlistList[5][0] } }, nametable = "Users", DbName = "Instagram" });
            
            string Storename=Seller.ITM[0].valuefiledlistList[10][0];
            List<string> res = new List<string>() { username, Productname , Price, Storename };
            return Ok(res);
            // ///////////////////////
            // for (int i = 5; i < orders.ITM[0].fieldnamelist.Count; i++)
            // {
            //     result.ITM[0].fieldnamelist.Add(orders.ITM[0].fieldnamelist[i]);
            //     //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
            //     result.ITM[0].releationfiled.Add(orders.ITM[0].releationfiled[i]);
            //     result.ITM[0].table2id.Add(orders.ITM[0].table2id[i]);
            //     result.ITM[0].valuefiledlistList.Add(orders.ITM[0].valuefiledlistList[i]);
            // }
            // for (int i = 5; i < Product.ITM[0].fieldnamelist.Count; i++)
            // {
            //     result.ITM[0].fieldnamelist.Add(Product.ITM[0].fieldnamelist[i]);
            //     //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
            //     result.ITM[0].releationfiled.Add(Product.ITM[0].releationfiled[i]);
            //     result.ITM[0].table2id.Add(Product.ITM[0].table2id[i]);
            //     result.ITM[0].valuefiledlistList.Add(Product.ITM[0].valuefiledlistList[i]);
            // }
            // for (int i = 5; i < Seller.ITM[0].fieldnamelist.Count; i++)
            // {
            //     result.ITM[0].fieldnamelist.Add(Seller.ITM[0].fieldnamelist[i]);
            //     //result.ITM[0].valuefiledlist.Add(result2.ITM[0].valuefiledlist[i]);
            //     result.ITM[0].releationfiled.Add(Seller.ITM[0].releationfiled[i]);
            //     result.ITM[0].table2id.Add(Seller.ITM[0].table2id[i]);
            //     result.ITM[0].valuefiledlistList.Add(Seller.ITM[0].valuefiledlistList[i]);
            // } 
            // return Ok(result);
            ////////////////////////////
            /////


        }




    }
}
