using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public interface IUserRepository
{
    List<GetUsersDto> Getalluser(string token);
    GetUsersDto Getuserbyid(int id, string token);
    void Adduser(RequestRegisterUserDto user, string token);
    //void Updateuser(RequestEdituserDto user, string token);
    void Updateuser(GetUsersDto user, string token);
    void DeleteUser(int id, string token);
    void UserSatusChange(GetUsersDto user, string token);
    List<RolesDto> GetRoles(string token);
}

public class UserRepository : IUserRepository
{
    //private string ApiUrl = "https://localhost:44332/api/User";
    private string ApiUrl = "https://localhost:44332/api/User";
    private HttpClient _client;

    public UserRepository()
    {
        _client = new HttpClient();
    }
    public List<GetUsersDto> Getalluser(string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////
        var result = _client.GetStringAsync(ApiUrl+ "/GetAllUser").Result;
        List<GetUsersDto> usersList = JsonConvert.DeserializeObject<List<GetUsersDto>>(result);
        return usersList;
    }
    public List<RolesDto> GetRoles(string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////

        //var result = _client.GetStringAsync("https://localhost:44332/Role").Result;
        var result = _client.GetStringAsync("https://localhost:44332/Role").Result;
        List<RolesDto> usersList = JsonConvert.DeserializeObject<List<RolesDto>>(result);
        return usersList;
    }

    public GetUsersDto Getuserbyid(int id, string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////
        ///

        //var result = _client.GetStringAsync("https://localhost:44332/api/User/" + id).Result;
        var result = _client.GetStringAsync("https://localhost:44332/api/User/GetUser/" + id).Result;
        List<GetUsersDto> usersList = JsonConvert.DeserializeObject<List<GetUsersDto>>(result);
        //var result = _client.GetStringAsync("https://localhost:44332/api/User/" + id).Result.Replace("[","").Replace("]","");


        //GetUsersDto user = JsonConvert.DeserializeObject<GetUsersDto>(result);///
        //List<GetUsersDto> user = JsonConvert.DeserializeObject<List<GetUsersDto>>(result);
        //return user[0];
        return usersList[0];
    }

    public void Adduser(RequestRegisterUserDto user, string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //////////////////// 
        string jasonuser = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
        var usernew = _client.PostAsync(ApiUrl+ "/InserUser", content).Result;
    }


    public void Updateuser(GetUsersDto user, string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////
        ///
        string jasonuser = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
        var usernew = _client.PutAsync(ApiUrl+ "/UpdateUser", content).Result;
        //var usernew = _client.PutAsync(requestUri: ApiUrl + "/EditUser", content: content).Result;
    }
    public void DeleteUser(int id, string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////

        //var usernew = _client.DeleteAsync(ApiUrl + "/DeleteUser/" + id).Result;
        var usernew = _client.DeleteAsync("https://localhost:44332/api/User/DeleteUser/" + id).Result;
        //var usernew = _client.DeleteAsync("https://localhost:44332/api/User/" + id).Result;

    }
    public void UserSatusChange(GetUsersDto user, string token)
    {
        //////////////////// Get token from cooki by this line.
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        ////////////////////
        /// 
        string jasonuser = JsonConvert.SerializeObject(user);
        StringContent content = new StringContent(jasonuser, Encoding.UTF8, "application/json");
        //var usernew = _client.PutAsync(requestUri: "https://localhost:44332/api/User/" + user.Id, content: content).Result;
        var usernew = _client.PutAsync(requestUri: "https://localhost:44332/api/User/UserSatusChange/" + user.Id, content: content).Result;

    }



}


