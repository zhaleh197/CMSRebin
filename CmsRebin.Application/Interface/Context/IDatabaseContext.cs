using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Common;
using CmsRebin.Domain.Entities;
using CmsRebin.Domain.Entities.Collections;
using CmsRebin.Domain.Entities.Commons;
using CmsRebin.Domain.Entities.Database;
using CmsRebin.Domain.Entities.Field;
//using CmsRebin.Domain.Entities.Log;
using CmsRebin.Domain.Entities.Persons;
using CmsRebin.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CmsRebin.Application.Interface.Context
{
    public interface IDatabaseContext
    {
        public DbSet<Tables> Tables { get; set; }
        public DbSet<FieldsofTables> FieldsofTable { get; set; }
        public DbSet<RelationsofTables> RelationsofTable { get; set; }
        public DbSet<TypeofReleation> TypeofReleation { get; set; }
        public DbSet<PermitionstoActivities> PermitionstoActivitie { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Tokens> Tokens { get; set; }
        public DbSet<ModelContainer> ModelContainer { get; set; }

        public DbSet<TypeofField> TypeofField { get; set; }

        public DbSet<DatabaseList> DatabaseLists { get; set; }
        //public DbSet<Loggs> Loggs { get; set; }


        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        int Creatmodel(string Tname,string DbName);
        int Creatmodel2(string DTname, string DbName, string table2, string forignkey);
        int inserfiled( FiledDto t, string DbName);
        int CreatBD(string DBname);
        int EditBD(string DBname, string DBnameNew);
        int EditTable(string DBname, string Tname, string TnameNew);
        /////////
        List<object> GetItems(string DTname, string DbName);
        List<object> GetItemsS(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName);
        List<object> GetItemsSnmRel(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName);
        
        List<object> GetItemsSandidrelfild(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName);
        List<object> GetColumname(string DTname, string DbName);
        
        public int InsertItem(List<string> S, List<string> I, string DTname, string DbName);
        /// <summary>//14001128
        /// 
        public int InsertItemtoTokenLevel2(List<string> S, List<string> I, string DTname, string DbName);
        public int DeleteItem(string DbName, string DTname, string iditem);

        public int EditItem(ItemdDto req);
        public int EditoneItem(ItemdDto req);
        ///////////////////1401-01-17
        public List<object> GetNestedSelectonefilter(Equation filters, string DTname, string DbName);
        public List<object> GetNestedSelectonefilterbyListofValue(Equation filters, string DTname, string DbName);
        ///////////////////
        public List<object> GetItembyFilterallfild(Equation filters, string DTname, string DbName);

        public List<object> GetItembyFilteronefile(string fildgeted, Equation filters, string DTname, string DbName);

        public GetoptionUplodedDb uploadDB_GetTables_GetRelation(string DbName);
        public int DeleteDb(string DBname);
        public int DownloadDB(string DBname, string nameFile);
        public int DeleteTable(string DbName, string DTname);
        public int DeleteFiled(string DbName, string DTname,string FiledName);



        public int Getiditem(string DbName, string tableName);

    }
}
