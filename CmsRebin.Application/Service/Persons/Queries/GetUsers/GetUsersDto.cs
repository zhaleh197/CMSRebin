namespace CmsRebin.Application.Service.Persons.Queries.GetUsers
{
    public class GetUsersDto
    {
        public long Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        //public bool IsRemoved { get; set; }
        public bool IsActive { get; set; }
        public string role { get; set; }

    }
}
