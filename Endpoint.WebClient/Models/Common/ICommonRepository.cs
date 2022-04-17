using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Infrastructure.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Models.Common
{
    public interface ICommonRepository
    {
        //List<PostTableDto> Search(string TBNANE, string DBNAME, string key, string token);
        Task<ReslutGetItemsdDto> Search(string TBNANE, string DBNAME, string key, string token);

    }
    public class CommonRepository : ICommonRepository
    {

        private string ApiUrl = "https://localhost:44332/api/GetEverythingsController";
        private HttpClient _client;
        public CommonRepository()
        {
            _client = new HttpClient();
        }



        public async Task<ReslutGetItemsdDto> Search(string DBNAME, string TBNANE, string key, string token)
        {

            /////////////////////   Get token from cooki by this line.
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////////////////////   

            Equation Filtrs = new Equation { Tablename = TBNANE, DBname = DBNAME, Value = new List<string> { key } };

            string jasonuser = JsonConvert.SerializeObject(Filtrs);
            StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");

            var result = _client.PostAsync("https://localhost:44332/api/GetEverythings/GetEverythingsa2/", content).Result;

            var dereult = await result.Content.ReadAsStringAsync();

            var Itemlist1 = JsonConvert.DeserializeObject<ReslutGetItemsdDto>(dereult);
            //List<GetItemslistDto> IT = new List<GetItemslistDto>();
            //foreach (var i in Itemlist1.Items)
            //    IT.Add((GetItemslistDto)i);
            //ReslutGetItemsdDto Itemlist2 = new ReslutGetItemsdDto {  ITM = IT, Rows = Itemlist1.Rows };

            return Itemlist1;





        }

     


    }
}
