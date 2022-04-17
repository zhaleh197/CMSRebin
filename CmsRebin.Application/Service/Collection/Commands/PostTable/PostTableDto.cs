namespace CmsRebin.Application.Service.Collection.Commands.PostTable
{
    public class PostTableDto
    {
        public long Id { get; set; }
        public string collection { get; set; }
        public string note { get; set; }
        public bool IsRemoved { get; set; }
        public string DbName { get; set; }

    }
}
