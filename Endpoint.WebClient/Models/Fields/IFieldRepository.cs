using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Models.Fields
{
    public interface IFieldRepository
    {
        List<GetFiledsDto> GetallFieldbyDTId(int id, string token);
        public List<GetFiledsDto> GetallFieldinallDB(string token);


        public ReslutGetFiledDto GetFieldbyid(int Fid, string token);
        public void AddField(RequestCreateFieldDto tb, string token);
        public void UpdateField(FiledDto tb, string token);
        public void DeleteField(int FId, string token);
        public void DeleteFieldAdmin(int FId, string token);
        public List<string> GetTypeRelations(string token);
        public List<T_FDto> GetTables2andfilds(string token);
        public List<string> GetTypeField(string token);


        //List<PostTableDto> GetallFileds(string token);
        ////List<PostTableDto> GetallCollectionbydbid(RequestDto db,string token);
        //List<PostTableDto> GetFiledsbyCollectionid(int dbid, string token);
        //PostTableDto GetCollectionbyid(int id, string token);
        //void AddCollection(RequestCreateTableDto user, string token);
        ////void Updateuser(RequestEdituserDto user, string token);
        //void UpdateCollection(RequestEdittableDto user, string token);
        //void DeleteCollection(int id, string token);

    }
    public class FieldRepository : IFieldRepository
    {
        //private string ApiUrl = "https://localhost:44332/api/Collection";
        private string ApiUrl = "https://localhost:44332/api/Collection";
        private HttpClient _client;
        public FieldRepository()
        {
            _client = new HttpClient();
        }



        public List<GetFiledsDto> GetallFieldbyDTId(int id, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetFiledsIdDT/" + id).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetFiledsIdDT/" + id).Result;

            List<GetFiledsDto> Filedlist = JsonConvert.DeserializeObject<List<GetFiledsDto>>(result);
            return Filedlist;

        }


        public List<GetFiledsDto> GetallFieldinallDB(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            /// 
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetFiledsInallDB").Result;

            List<GetFiledsDto> Filedlist = JsonConvert.DeserializeObject<List<GetFiledsDto>>(result);
            return Filedlist;

        }



        public ReslutGetFiledDto GetFieldbyid(int Fid, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetFiledbyid/" + Fid).Result;

            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/GetFiledbyid/" + Fid).Result;
            //result = result.Replace("[", "").Replace("]", "");
            ReslutGetFiledDto collectin = JsonConvert.DeserializeObject<ReslutGetFiledDto>(result);
            return collectin;
        }

        public void AddField(RequestCreateFieldDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //////////////////// 
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            //var fildnes = _client.PostAsync("https://localhost:44332/api/Collection/addFiled", content).Result;
            var fildnes = _client.PostAsync("https://localhost:44332/api/Collection/addFiled", content).Result;



            //ResultDto<RequestCreateFieldDto> user = JsonConvert.DeserializeObject<ResultDto<RequestCreateFieldDto>>(fildnes);
            //return user;
        }
        public void UpdateField(FiledDto tb, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////
            ///
            string jasonuser = JsonConvert.SerializeObject(tb);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
            var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditFiled", content).Result;

            //var usernew = _client.PutAsync("https://localhost:44332/api/Collection/EditFiled", content).Result;
            //var usernew = _client.PutAsync(requestUri: ApiUrl + "/EditUser", content: content).Result;
        }
        public void DeleteField(int FId, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;

            var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteField/" + FId).Result;

            //var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteField/" + FId).Result;

        }

        public void DeleteFieldAdmin(int FId, string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            var usernew = _client.DeleteAsync("https://localhost:44332/api/Collection/DeleteFieldAdmin/" + FId).Result;

        }


        public List<string> GetTypeField(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Typefileds/").Result;
            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Typefileds/").Result;
            List<string> Typefilds = JsonConvert.DeserializeObject<List<string>>(result);
            return Typefilds;
        }
        public List<string> GetTypeRelations(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;

            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Typerelation/").Result;
            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/Typerelation/").Result;
            List<string> Typefilds = JsonConvert.DeserializeObject<List<string>>(result);
            return Typefilds;
        }
        public List<T_FDto> GetTables2andfilds(string token)
        {
            //////////////////// Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////

            //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;

            //var result = _client.GetStringAsync("https://localhost:44332/api/Collection/ListTable/").Result;
            var result = _client.GetStringAsync("https://localhost:44332/api/Collection/ListTable/").Result;
            List<T_FDto> Typefilds = JsonConvert.DeserializeObject<List<T_FDto>>(result);
            return Typefilds;
        }


    }


}
