using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Models.DBs
{
   public interface IDBRepository
    {
        List<GetDBDto> GetallDBs(string token);
        List<GetDBDto> GetallDBsbyUserid(string token);
        //GetDBDto GetDBsbyname(int id, string token);
        List<GetDBDto> GetDBbyid2(int id, string token);
        GetDBDto GetDBbyid(int id, string token);
        void AddDB(RequestCreateDBDto db, string token);

        void UploadDB(UploadDBDto db, string token);
        void EditDB(RequestEditDBDto db, string token);//just name edited
        void EditDBAdmin(RequestEditDBAdminDto db, string token);//Edit name and remove or notRemove
        void DeletDB(int id, string token);// just isRemove is fale.
        void DeletDBAdmin(int id, string token);// Remove totally from DB
        void DownloadDB(int id, string token);
        //void DownloadDB(GetDBDto req, string token); 



    }
    public class DBRepository : IDBRepository
    {
        private string ApiUrl = "https://localhost:44332/api/DB";
        //private string ApiUrl = "https://localhost:44332/api/DB";
        private HttpClient _client;
        private readonly IUserLoginService _userLoginService;
        public DBRepository(IUserLoginService userLoginService)
        {
            _client = new HttpClient();
            _userLoginService = userLoginService;
        }

        


        public List<GetDBDto> GetallDBs(string token)
        {

            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBs").Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBs").Result;
            List <GetDBDto> DbsList = JsonConvert.DeserializeObject<List<GetDBDto>>(result);
            return DbsList;
        }
        /// /////////////////////////////////////////////
        public List<GetDBDto> GetallDBsbyUserid(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            var userid = _userLoginService.GetUsertbyToken(token);


            //var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBsbyUserid/" + userid.id).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBsbyUserid/" + userid.id).Result;

            List<GetDBDto> DbsList = JsonConvert.DeserializeObject<List<GetDBDto>>(result);
            return DbsList;

        }
        public List<GetDBDto> getdbbyid(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///


            //var result = _client.GetStringAsync("https://localhost:44332/api/DB/SelectDB/" + id).Result;

            var result = _client.GetStringAsync("https://localhost:44332/api/DB/SelectDB/" + id).Result;

            List<GetDBDto> DbsList = JsonConvert.DeserializeObject<List<GetDBDto>>(result);
            return DbsList;

        }


        /// /////////////////////////////////////////////


        public void AddDB(RequestCreateDBDto db, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //////////////////// 
            string jasonuser = JsonConvert.SerializeObject(db);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            var usernew = _client.PostAsync("https://localhost:44332/api/DB/CreatDB", content).Result;

            //var usernew = _client.PostAsync("https://localhost:44332/api/DB/CreatDB", content).Result;

        }


        public void UploadDB(UploadDBDto db, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //_client.DefaultRequestHeaders.



            //////////////////// 
            string jasonuser = JsonConvert.SerializeObject(db);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var usernew = _client.PostAsync("https://localhost:44332/api/DB/UploadDB", content).Result;

            var usernew = _client.PostAsync("https://localhost:44332/api/DB/UploadDB", content).Result;


        }

        public List<GetDBDto> GetDBbyid2(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var result = _client.GetStringAsync("https://localhost:44332/api/DB/SelectDB/" + id).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/DB/SelectDB/" + id).Result;
            //result = result.Replace("[", "").Replace("]", "");
            //GetDBDto DB = JsonConvert.DeserializeObject<GetDBDto>(result);
            List <GetDBDto> DB = JsonConvert.DeserializeObject<List<GetDBDto>>(result);

            return DB;
        }
        public GetDBDto GetDBbyid(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///

            var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBExist/" + id).Result;
            //var result = _client.GetStringAsync("https://localhost:44332/api/DB/DBExist/" + id).Result;
            result = result.Replace("[", "").Replace("]", "");
            //GetDBDto DB = JsonConvert.DeserializeObject<GetDBDto>(result);
            GetDBDto DB = JsonConvert.DeserializeObject<GetDBDto>(result);

            return DB;
        }

        public void EditDB(RequestEditDBDto db, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////  
           
            string jasonuser = JsonConvert.SerializeObject(db);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var DBnew = _client.PutAsync("https://localhost:44332/api/DB/EditDB", content).Result;

            var DBnew = _client.PutAsync("https://localhost:44332/api/DB/EditDB", content).Result;
            //var usernew = _client.PutAsync(requestUri: ApiUrl + "/EditUser", content: content).Result;
        }
        public void DeletDB(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            var DBnew = _client.DeleteAsync("https://localhost:44332/api/DB/DeleteDB/" + id).Result;
            //var DBnew = _client.DeleteAsync("https://localhost:44332/api/DB/DeleteDB/" + id).Result;

        }
        public void DownloadDB(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            var Backup = _client.GetAsync("https://localhost:44332/api/DB/DownloadDB/" + id).Result;

        }
        //public void DownloadDB(GetDBDto req, string token)
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    ////////////////////  

        //    string jasonuser = JsonConvert.SerializeObject(req);
        //    StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json"); 

        //    var DBnew = _client.PostAsync("https://localhost:44332/api/DB/DownloadDB", content).Result; 

        //}
        /// ////////////
        /// 
        public void EditDBAdmin(RequestEditDBAdminDto db, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////  

            string jasonuser = JsonConvert.SerializeObject(db);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var DBnew = _client.PutAsync("https://localhost:44332/api/DB/EditDB", content).Result;

            var DBnew = _client.PutAsync("https://localhost:44332/api/DB/EditDBAdmin", content).Result;
        }

        public void DeletDBAdmin(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            var DBnew = _client.DeleteAsync("https://localhost:44332/api/DB/DeleteDBAdmin/" + id).Result;
            //var DBnew = _client.DeleteAsync("https://localhost:44332/api/DB/DeleteDBAdmin/" + id).Result;
        }
    }
}
