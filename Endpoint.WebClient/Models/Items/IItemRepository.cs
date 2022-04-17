using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.RemoveItem;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.SMS;
using CmsRebin.Common.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static CmsRebin.Application.Service.Common.SMS.SMSSender;

namespace Endpoint.WebClient.Models.Items
{
   public interface IItemRepository
    {
        ReslutGetItemsdDto GetallItemsbyDTId(int id, string token);
        //public ReslutGetItemsdDto GetItemdbyid(int Iid, string token);
        //public void AddItem(RequestCreateItemdDto tb, string token);
        public  Task<ResultCreateItemdDto> AddItem(RequestCreateItemdDto tb, string token);
        public void UpdateItem(ItemdDto tb, string token);
        //public void DeleteItem(int IId, string token);
        public void DeleteItem(itemDto req, string token);
        public void SendSMS(SMSSendRequest req, string token);
        public void SendMail(EmailSendRequest req, string token);

    }
    public class ItemRepository : IItemRepository
    {
        //private string ApiUrl = "https://localhost:44332/api/Collection";
        private string ApiUrl = "https://localhost:44332/api/Collection";
        private HttpClient _client;
        public ItemRepository()
        {
            _client = new HttpClient();
        }

        //public async Task<ReslutGetItemsdDto> GetallItemsbyDTIdSearchkey(RequestDtoIDs request, string token)
        //{

        //    //////////////////// Get token from cooki by this line.
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    ///
        //    string jasonuser = JsonConvert.SerializeObject(request);
        //    StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

        //    var result = _client.PostAsync("https://localhost:44332/api/Collection/GetItemsbyIDTSearchkey", content).Result;

        //    var user = (await result.Content.ReadAsStringAsync()).ToString();
        //    ReslutGetItemsdDto Itemlist = JsonConvert.DeserializeObject<ReslutGetItemsdDto>(result.Content.ReadAsStreamAsync().ToString());
        //    return Itemlist;

        //}

        public ReslutGetItemsdDto GetallItemsbyDTId(int id, string token)
        {
            
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///

            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetItemsbyIDT/" + id).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetItemsbyIDT/" + id).Result;
            ReslutGetItemsdDto Itemlist = JsonConvert.DeserializeObject<ReslutGetItemsdDto>(result);
            return Itemlist; 

        }

        //public ReslutGetItemsdDto GetItemdbyid(int Iid, string token)
        //{
        //    //////////////////// Get token from cooki by this line.
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    ////////////////////

        //    var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetItembyid/" + Iid).Result;
        //    //result = result.Replace("[", "").Replace("]", "");
        //    ReslutGetItemsdDto Item = JsonConvert.DeserializeObject<ReslutGetItemsdDto>(result);
        //    return Item;
        //}
        
        public async Task<ResultCreateItemdDto> AddItem(RequestCreateItemdDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //////////////////// 
            ///
            var v = new StringLsetModel { DataItems=tb.I,FildsNamae=tb.S,DBname=tb.DbName,Tname=tb.TableName};
            string jasonuser = JsonConvert.SerializeObject(v);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var result = _client.PostAsync("https://localhost:44332/api/Collection/InsertItemFinal", content).Result;


            var result = _client.PostAsync("https://localhost:44332/api/Collection/InsertItemFinal", content).Result;
            var dereult = await result.Content.ReadAsStringAsync();
            var Item= JsonConvert.DeserializeObject<ResultCreateItemdDto>(dereult);
            return Item;



            //ResultDto<RequestCreateFieldDto> user = JsonConvert.DeserializeObject<ResultDto<RequestCreateFieldDto>>(fildnes);
            //return user;
        }
        public void UpdateItem(ItemdDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditItem", content).Result;

            var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditItem", content).Result;
            //var usernew = _client.PutAsync(requestUri: ApiUrl + "/EditUser", content: content).Result;
        }
        public void DeleteItem(itemDto req, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            //var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteItem/" + IId).Result;


            string jasonuser = JsonConvert.SerializeObject(req);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
            var result = _client.PostAsync("https://localhost:44332/api/Collection/DeleteItem/", content).Result;
           

        }
        public void SendMail(EmailSendRequest req, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            string jasonuser = JsonConvert.SerializeObject(req);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
            var result = _client.PostAsync("https://localhost:44332/api/Common/SendEmail/", content).Result;


        }
        public void SendSMS(SMSSendRequest req, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///

            string jasonuser = JsonConvert.SerializeObject(req);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
            var result = _client.PostAsync("https://localhost:44332/api/Common/SendSMS/", content).Result;

        }




    }


}
