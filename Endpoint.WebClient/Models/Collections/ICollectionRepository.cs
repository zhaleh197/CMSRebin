using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Common.Fainances.Commands.AddRequestPay;
using CmsRebin.Common.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Models.Collections
{
   public interface ICollectionRepository
    {

        List<PostTableDto> GetallCollection(string token);
        //List<PostTableDto> GetallCollectionbydbid(RequestDto db,string token);
        List<PostTableDto> GetallCollectionbyDBid(int dbid, string token);
        PostTableDto GetCollectionbyid(int id, string token);
        Task<string> AddCollectionAsync(RequestCreateTableDto user, string token);
        //void Updateuser(RequestEdituserDto user, string token);
        void UpdateCollection(RequestEdittableDto user, string token);
        public void UpdateCollectionAdmin(RequestEdittableAdminDto tb, string token);
        public void DeleteCollection(int id, string token);
        public void DeleteCollectionAdmin(int TableId, string token);
        public Task<string> Payment(RequestCreatPayDto py, string token);
    }
    public class CollectionRepository: ICollectionRepository
    {

        private string ApiUrl = "https://localhost:44332/api/Collection";
        //private string ApiUrl = "https://localhost:44332/api/Collection";
        private HttpClient _client; 
        public CollectionRepository()
        {
               _client = new HttpClient();
        }

        public List<PostTableDto> GetallCollection(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            //var result = _client.GetStringAsync("https://localhost:44332/Tables/").Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetAllCollection").Result;
            List <PostTableDto> colectionlist = JsonConvert.DeserializeObject<List<PostTableDto>>(result);
            return colectionlist;
        }


        /// ///////////////////////////////////////////////////////////////////////////////
        public List<PostTableDto> GetallCollectionbyDBid(int dbid, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///

            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Collections3/" + dbid).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Collections3/" + dbid).Result;
            List<PostTableDto> colectionlist = JsonConvert.DeserializeObject<List<PostTableDto>>(result);
            return colectionlist;

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////



        //public List<PostTableDto> GetallCollectionbydb(RequestDto db,string token)
        //{
        //    //////////////////// Get token from cooki by this line.
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    ////////////////////
        //    ///   //////////////////// 
        //    string jasonuser = JsonConvert.SerializeObject(db);
        //    StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
        //    var usernew = _client.PostAsync("https://localhost:44332/UploadDB", content).Result;

        //    var result = _client.GetStringAsync("https://localhost:44332/Tables/"+ db.DbName).Result;
        //    List<PostTableDto> colectionlist = JsonConvert.DeserializeObject<List<PostTableDto>>(result);
        //    return colectionlist;

        //}





        public PostTableDto GetCollectionbyid(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////


            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/SelectTable/" + id).Result;
            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/SelectTable/" + id).Result;
            //result = result.Replace("[", "").Replace("]", "");
            PostTableDto collectin = JsonConvert.DeserializeObject<PostTableDto>(result);
            return collectin;
        }

        public async Task<string> AddCollectionAsync(RequestCreateTableDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //////////////////// 
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var usernew = _client.PostAsync("https://localhost:44332/api/Collection/CreatTable", content).Result;

            var usernewss = _client.PostAsync("https://localhost:44332/api/Collection/CreatTable", content);
            //var contect= usernew.Content;

            //usernewss.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.

          var user=  (await usernewss.Result.Content.ReadAsStringAsync()).ToString();
            //string user = JsonConvert.DeserializeObject<string>(usernewss.Content.ToString());

            return user;
        }
        public void UpdateCollection(RequestEdittableDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditTable", content).Result;
            //var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditTable", content).Result;
            //var usernew = _client.PutAsync(requestUri: ApiUrl + "/EditUser", content: content).Result;
        }
        public void UpdateCollectionAdmin(RequestEdittableAdminDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditTableAdmin", content).Result;
        }

        public void DeleteCollection(int TableId, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;

            var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteTable/" + TableId).Result;
            //var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteTable/" + TableId).Result;

        }

        
        public void DeleteCollectionAdmin(int TableId, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;

            var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteTableAdmin/" + TableId).Result; 
        }


        public async Task<string> Payment(RequestCreatPayDto py, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           

            string jasonuser = JsonConvert.SerializeObject(py);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
             
            var paymenttable = _client.PostAsync("https://localhost:44332/api/Fainance/CreateRequestPayTable", content);

            var paymenttablet = (await paymenttable.Result.Content.ReadAsStringAsync()).ToString();
          
            return paymenttablet;
        }



    }


}
