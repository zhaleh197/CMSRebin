using CmsRebin.Common;
using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Domain.Entities;
using CmsRebin.Domain.Entities.Collections;
using CmsRebin.Domain.Entities.Commons;
using CmsRebin.Domain.Entities.Field;
using CmsRebin.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Dapper;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Infrastructure.Enum;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Domain.Entities.Database;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using Microsoft.VisualStudio.Services.Common;
//using CmsRebin.Domain.Entities.Log;

namespace CmsRebin.Persistance.Context
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        private const string V = "'";
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("A FALLBACK CONNECTION STRING");
            }
        }
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

        /// <summary>
        /// seeddd 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tables>()
              .HasData(
               new Tables { id = 1, collection = "Role", note = "", singleton = false, IdDBase = 0 },
               new Tables { id = 2, collection = "Users", note = "", singleton = false, IdDBase = 0 }
               );



            //TypeofReleation
            modelBuilder.Entity<TypeofField>()
                        .HasData(
                         new TypeofField { id = 1, typefield = "int" },
                         new TypeofField { id = 2, typefield = "bigint" },
                         new TypeofField { id = 3, typefield = "nvarchar(50)" },
                         new TypeofField { id = 4, typefield = "binary(50)" },
                         new TypeofField { id = 5, typefield = "text" },
                         new TypeofField { id = 6, typefield = "bit" },
                         new TypeofField { id = 7, typefield = "char(10)" },
                         new TypeofField { id = 8, typefield = "date" },
                         new TypeofField { id = 9, typefield = "datetime" },
                         new TypeofField { id = 10, typefield = "datetime2(7)" },
                         new TypeofField { id = 11, typefield = "datetimeoffset(7)" },
                         new TypeofField { id = 12, typefield = "decimal(18,0)" },
                         new TypeofField { id = 13, typefield = "float" },
                         new TypeofField { id = 14, typefield = "geography" },
                         new TypeofField { id = 15, typefield = "geometry" },
                         new TypeofField { id = 16, typefield = "hierarchyid" },
                         new TypeofField { id = 17, typefield = "image" },
                         new TypeofField { id = 18, typefield = "money" },
                         new TypeofField { id = 19, typefield = "nchar(10)" },
                         new TypeofField { id = 20, typefield = "ntext" },
                         new TypeofField { id = 21, typefield = "numeric(8,0)" },
                         new TypeofField { id = 22, typefield = "nvarchar(MAX)" },
                         new TypeofField { id = 23, typefield = "real" },
                         new TypeofField { id = 24, typefield = "time(7)" },
                         new TypeofField { id = 25, typefield = "xml" },
                         new TypeofField { id = 26, typefield = "multifile" }
                         );

            modelBuilder.Entity<TypeofReleation>()
                     .HasData(
                      new TypeofReleation { id = 1, typerelation = "1-1" },
                      new TypeofReleation { id = 2, typerelation = "1-n" },
                      new TypeofReleation { id = 3, typerelation = "m-n" });

            //
            modelBuilder.Entity<Role>()
                        .HasData(
                         new Role {
                             id = new PasswordHasher().HashPassword("admin"),
                             rolename = "admin"
                         },
                         new Role { id = new PasswordHasher().HashPassword("operator"), rolename = "operator" },
                         new Role { id = new PasswordHasher().HashPassword("user"), rolename = "user" });



            //14001107 - creat admin user-defult
            //modelBuilder.Entity<Users>().HasData(
            //          new Users
            //          {
            //              id = 1,
            //              Email = "Rebin@gmail.com",
            //              first_name = "Admin",
            //              last_name = "Admin",
            //              Role = this.Role.Where(r => r.rolename == "admin").FirstOrDefault(),
            //              InsertTime = DateTime.Now,
            //              IsActive = true,
            //              IsRemoved = false,
            //              Password = "Rebin@gmail.com",
            //              location = "jkvghyg",
            //              RemoveTime = null,
            //              UpdateTime = null,
            //              DatabaseLists = null
            //          });

            //// modelBuilder.Entity<DatabaseList>()
            ////.HasData(
            ////    new DatabaseList { id = 0, DBName = "swa", User = this.Users.Where(r => r.Email == "Rebin@gmail.com").FirstOrDefault(), }
            //// );


            //// اعمال ایندکس بر روی فیلد ایمیل
            // اعمال عدم تکراری بودن ایمیل
            modelBuilder.Entity<Users>().HasIndex(u => u.Email).IsUnique();

            //-- عدم نمایش اطلاعات حذف شده
            ApplyQueryFilter(modelBuilder);

        }
        private void ApplyQueryFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasQueryFilter(p => !p.IsRemoved);
            modelBuilder.Entity<Role>().HasQueryFilter(p => !p.IsRemoved);

        }
        //public IDbConnection Connection => new SqlConnection("Data Source= .; Initial Catalog= CRMREBINFinal0710 ; Integrated Security=true ; MultipleActiveResultSets=True;");
        public GetoptionUplodedDb uploadDB_GetTables_GetRelation(string DbName)
        {
            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);

            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            IDbConnection Connection = new SqlConnection(connectionString);

            using (var con1 = Connection)
            {
                con1.Open();

                string selestDatabase = "USE " + DbName.ToString();
                string getTablesQuery = "select * from sys.tables";
                string getrelationQuery = "SELECT fk.name 'FK Name', tp.name 'TableP', cp.name 'KeyP', cp.column_id, tr.name 'TableR',cr.name 'KeyR', cr.column_id " +
"FROM sys.foreign_keys fk INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id " +
"INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id " +
"INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id " +
"INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id " +
"INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id " +
"ORDER BY  tp.name, cp.column_id ";


                GetoptionUplodedDb result = new GetoptionUplodedDb();
                var re = con1.Query<object>(selestDatabase).ToList();
                result.getTablesDB = con1.Query<object>(getTablesQuery).ToList();
                result.getRelationDb = con1.Query<object>(getrelationQuery).ToList();
                return result;


            }
        }

        public int DeleteFiled(string DbName, string DTname, string FiledName)
        {
        //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

        IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
             string query ="ALTER TABLE "+ DTname + " DROP COLUMN "+ FiledName + ";";
                 
                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                }
                catch (Exception e)
{
    Console.WriteLine(e.Message);
    return 0;
}

            }
        }

        public int DeleteTable( string DbName, string DTname)
        {
            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
                string query = "DROP TABLE " + DTname + " ;";

                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        public int Creatmodel2(string DTname, string DbName,string forignkey,string table2)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            string query = "";
            using (var con1 = Connection)
            {
                con1.Open();
                if (forignkey != null)
                    query = "create table " + DTname + " ( id int NOT NULL " + "FOREIGN KEY REFERENCES " + table2 + "(" + forignkey + ") " + ", InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime);";
                else
                    query = "create table " + DTname + " ( id int NOT NULL IDENTITY(1,1) PRIMARY KEY, InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime);";
                ///////Creat forinkey in SQL = 1400-11-11.

                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        public int Creatmodel(string DTname, string DbName )
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            string query = "";
            using (var con1 = Connection)
            {
                con1.Open();
                query = "create table " + DTname + " ( id int NOT NULL IDENTITY(1,1) PRIMARY KEY, InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime);";
                ///////Creat forinkey in SQL = 1400-11-11.
                
                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }


           

                //// using (var context = new CmsDBContext())
                //using (var scommand = this.Database.GetDbConnection().CreateCommand())
                //{
                //    scommand.CommandText = "create table " + DTname + " ( id int NOT NULL IDENTITY(1,1) PRIMARY KEY, InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime);";

                //    //scommand.CommandText = "create table " + DTname + " ( id int NOT NULL IDENTITY(1,1) , InsertTime  DateTime , UpdateTime  DateTime , IsRemoved  bit , RemoveTime  DateTime , PRIMARY KEY (id));";
                //   // AUTO_INCREMENT
                //    //scommand.CommandText = "create table " + DTname + " (ID int NOT NULL PRIMARY KEY , InsertTime  DateTime = " + DateTime.Now +" , UpdateTime DateTime = null , IsRemoved bool = false  , RemoveTime DateTime = null );";
                //    //scommand.CommandText = "create ta5ble " + DTname + " (ID int NOT NULL PRIMARY KEY, InsertTime  DateTime, UpdateTime DateTime,IsRemoved bool,RemoveTime DateTime,PRIMARY KEY (ID));";
                //    /*  scommand.CommandText = "create table " + DTname + " (id long NOT NULL PRIMARY, FOREIGN KEY (id) REFERENCES BaseEntity (id));";*/// "create table " + txt_table.Text;
                //                                                                                                                                        //  scommand.CommandText = "create table "+ DTname + " ( ID int NOT NULL PRIMARY KEY , snam int );";
                //    this.Database.OpenConnection();
                //    try
                //    {
                //        using (var result = scommand.ExecuteReader())
                //        {
                //            // do something with result
                //            try
                //            {
                //                return 1;
                //                //MessageBox.Show("Table is created successfully with EFCore.", "MyProgram",
                //                //                MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            }
                //            catch
                //            {
                //                return 0;
                //                //MessageBox.Show(ex.ToString());
                //            }
                //        }



                //    }
                //    catch (Exception ex)
                //    {
                //        return -1;
                //        //MessageBox.Show("Table is exist already.");
                //    }

                //    // this.Database.CloseConnection();
                //}

                //

                //FieldsofTables.Add(new FieldsofTables { fieldname = "id", tablename = DTname, relation = null });

            }
        public List<object> GetColumname(string DTname, string DbName)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            List<string> cln = new List<string>();
            using (var con1 = Connection)
            {
                con1.Open();
                string query = "select column_name from information_schema.columns where table_name =N'" + DTname + "'";
                // var resullt = con1.Query<object>(query).ToList();
                var result2 = con1.Query(query).ToList();

                return result2;
            }
        }

        public List<object> GetItemsSnmRel(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName)
        {
            if (DatabaseLists.Where(b => b.DBName.Equals(DbName)).Count() < 1)
            {
                return null;
            }

            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            //List<object> FildsofFilds = new List<object>();
            string q = "";
            string T = DTname;
            for (int i = 0; i < fildsofthisTable.Count; i++)
            {
                if (fildsofthisTable[i].typeReleaton.Equals("1-1") || fildsofthisTable[i].typeReleaton.Equals("m-1") || fildsofthisTable[i].typeReleaton.Equals("m-n"))
                {

                    //var RR = this.RelationsofTable.Where(r => r.one_collection.Equals(fildsofthisTable[i].tablename) && r.one_field.Equals(fildsofthisTable[i].fieldname)).FirstOrDefault();

                    ///edit in 14001012
                    long idDb = DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
                    long idtablethisfilf = Tables.Where(t => t.collection.Equals(fildsofthisTable[i].tablename) && t.IdDBase.Equals(idDb) && t.IsRemoved == false).FirstOrDefault().id;
                    long idrelation = TypeofReleation.Where(r => r.typerelation.Equals(fildsofthisTable[i].typeReleaton)).FirstOrDefault().id;
                    long idfildnew = FieldsofTable.Where(t => t.fieldname.Equals(fildsofthisTable[i].fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                    var RR = RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                    ////
                    ///
                    //T += " , " + Tables.Where(t => t.id.Equals(RR.id)).Select(t => t.collection).FirstOrDefault().ToString();
                    //edited 14001012
                    T += " , " + Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();

                    var ff = FieldsofTable.Where(r => r.IdTable.Equals(RR.many_collection));
                    //FildsofFilds.Add(ff);
                    //for (int j = 0; j < ff.Count(); j++)
                    //{

                    int last = ff.Count();
                    int jj = 0;
                    foreach (var fildd in ff)
                    {
                        jj++;
                        q += fildd.fieldname;
                        if (jj < last)
                            q += " , ";
                        //else
                        //    q += " , ";
                    }
                }
                else
                {
                    q += fildsofthisTable[i].fieldname;
                    if (i != fildsofthisTable.Count - 1)
                        q += " , ";
                }

            }

            using (var con1 = Connection)
            {
                con1.Open();
                string query = "select " + q + " from " + T;
                var resullt = con1.Query<object>(query).ToList();
                return resullt;
            }
             


        }
        public List<object> GetItemsS(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName)
        {
            if (DatabaseLists.Where(b => b.DBName.Equals(DbName)).Count() < 1)
            {
                return null;
            }

            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            //List<object> FildsofFilds = new List<object>();
            string q = "";
            string T = DTname;
            for (int i = 0; i < fildsofthisTable.Count; i++)
            {
                if (fildsofthisTable[i].typeReleaton.Equals("1-1") || fildsofthisTable[i].typeReleaton.Equals("m-1") || fildsofthisTable[i].typeReleaton.Equals("m-n"))
                {

                    //var RR = this.RelationsofTable.Where(r => r.one_collection.Equals(fildsofthisTable[i].tablename) && r.one_field.Equals(fildsofthisTable[i].fieldname)).FirstOrDefault();

                    ///edit in 14001012
                    long idDb = DatabaseLists.Where(d=>d.DBName.Equals(DbName) && d.IsRemoved==false).FirstOrDefault().id;
                    long idtablethisfilf = Tables.Where(t => t.collection.Equals(fildsofthisTable[i].tablename) && t.IdDBase.Equals(idDb) && t.IsRemoved == false).FirstOrDefault().id;
                    long idrelation =  TypeofReleation.Where(r => r.typerelation.Equals(fildsofthisTable[i].typeReleaton)).FirstOrDefault().id;
                    long idfildnew =  FieldsofTable.Where(t => t.fieldname.Equals(fildsofthisTable[i].fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                    var RR =  RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                    ////
                    ///
                    //T += " , " + Tables.Where(t => t.id.Equals(RR.id)).Select(t => t.collection).FirstOrDefault().ToString();
                    //edited 14001012
                    T += " , " + Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();

                    var ff = FieldsofTable.Where(r => r.IdTable.Equals(RR.many_collection));
                    //FildsofFilds.Add(ff);
                    //for (int j = 0; j < ff.Count(); j++)
                    //{

                    int last = ff.Count();
                    int jj = 0;
                    foreach (var fildd in ff)
                    {
                        jj++;
                        q += fildd.fieldname;
                        if (jj < last)
                            q += " , ";
                        //else
                        //    q += " , ";
                    }
                }
                else
                {
                    q += fildsofthisTable[i].fieldname;
                    if (i != fildsofthisTable.Count - 1)
                        q += " , ";     
                }

            }

            using (var con1 = Connection)
            {
                con1.Open();
                string query = "select " + q + " from " + T;
                var resullt = con1.Query<object>(query).ToList();
                return resullt;
            }

            //DataTable list = new DataTable();
            // using (var scommand = this.Database.GetDbConnection().CreateCommand())
            // {
            //     try
            //     {
            //         scommand.CommandText = "SELECT * FROM " + DTname;
            //         this.Database.OpenConnection();

            //         var resultt = scommand.ExecuteReader();
            //         //list.Load(resultt);
            //         while (resultt.Read())
            //         {
            //             Console.WriteLine("\t{0}\t{1}\t{2}",
            //                 resultt[0], resultt[1], resultt[2]);
            //         }
            //         return resultt;

            //         //using (var resultt = scommand.ExecuteReader())
            //         //{
            //         //    list.Load(resultt);
            //         //    return list;
            //         //}



            //     }
            //     catch (Exception ex)
            //     {
            //         return null;
            //     }

            // }

            //string contectionstring = "Data Source= .; Initial Catalog= CRMREBINFinal3 ; Integrated Security=true ; MultipleActiveResultSets=True;";

            //string queryString =
            //    "SELECT * FROM " + DTname;

            //using (SqlConnection connection =
            //           new SqlConnection(contectionstring))
            //{
            //    SqlCommand command =
            //        new SqlCommand(queryString, connection);
            //    connection.Open();

            //    SqlDataReader reader = command.ExecuteReader();
            //        return(reader.AsQueryable());
            //}



            //var result = new DataSet();
            //var scommand = this.Database.GetDbConnection().CreateCommand();

            //    scommand.CommandText = "SELECT * FROM " + DTname;
            //    this.Database.OpenConnection();
            //    var reader = scommand.ExecuteReader();
            //do
            //{
            //    // loads the DataTable (schema will be fetch automatically)
            //    var tb = new DataTable();
            //    tb.Load(reader);
            //    result.Tables.Add(tb);

            //} while (!reader.IsClosed);

            //return result;


        }
        public List<object> GetItemsSandidrelfild(string DTname, List<GetFiledsDto> fildsofthisTable, string DbName)
        {
            if (DatabaseLists.Where(b => b.DBName.Equals(DbName)).Count() < 1)
            {
                return null;
            }
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            //List<object> FildsofFilds = new List<object>();
            string q = "";
            string T = DTname;
            for (int i = 0; i < fildsofthisTable.Count; i++)
            {
                if (fildsofthisTable[i].typeReleaton.Equals("1-1") || fildsofthisTable[i].typeReleaton.Equals("m-1") || fildsofthisTable[i].typeReleaton.Equals("m-n"))
                {
                    var RR = this.RelationsofTable.Where(r => r.one_collection.Equals(fildsofthisTable[i].tablename) && r.one_field.Equals(fildsofthisTable[i].fieldname)).FirstOrDefault();
                    T += " , " + Tables.Where(t => t.id.Equals(RR.id)).Select(t => t.collection).FirstOrDefault().ToString();
                    var ff = FieldsofTable.Where(r => r.IdTable.Equals(RR.many_collection));

                    int last = ff.Count();
                    int jj = 0;
                    foreach (var fildd in ff)
                    {
                        jj++;
                        q += fildd.fieldname;
                        if (jj < last)
                            q += " , ";
                        //else
                        //    q += " , ";
                    }
                }
                else
                {
                    q += fildsofthisTable[i].fieldname;
                    if (i != fildsofthisTable.Count - 1)
                        q += " , ";
                }

            }

            using (var con1 = Connection)
            {
                con1.Open();
                string query = "select " + q + " from " + T;
                var resullt = con1.Query<object>(query).ToList();
                return resullt;
            }


        }
        //////////// backup 
        public List<object> GetItems(string DTname, string DbName)
        {

            if (DatabaseLists.Where(b => b.DBName.Equals(DbName)).Count() < 1)
            {
                return null;

            }
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            using (var con1 = Connection)
            {
                con1.Open();
                string query = "select * from " + DTname;
                var resullt = con1.Query<object>(query).ToList();
                return resullt;
            }

            //DataTable list = new DataTable();
            // using (var scommand = this.Database.GetDbConnection().CreateCommand())
            // {
            //     try
            //     {
            //         scommand.CommandText = "SELECT * FROM " + DTname;
            //         this.Database.OpenConnection();

            //         var resultt = scommand.ExecuteReader();
            //         //list.Load(resultt);
            //         while (resultt.Read())
            //         {
            //             Console.WriteLine("\t{0}\t{1}\t{2}",
            //                 resultt[0], resultt[1], resultt[2]);
            //         }
            //         return resultt;

            //         //using (var resultt = scommand.ExecuteReader())
            //         //{
            //         //    list.Load(resultt);
            //         //    return list;
            //         //}



            //     }
            //     catch (Exception ex)
            //     {
            //         return null;
            //     }

            // }

            //string contectionstring = "Data Source= .; Initial Catalog= CRMREBINFinal3 ; Integrated Security=true ; MultipleActiveResultSets=True;";

            //string queryString =
            //    "SELECT * FROM " + DTname;

            //using (SqlConnection connection =
            //           new SqlConnection(contectionstring))
            //{
            //    SqlCommand command =
            //        new SqlCommand(queryString, connection);
            //    connection.Open();

            //    SqlDataReader reader = command.ExecuteReader();
            //        return(reader.AsQueryable());
            //}



            //var result = new DataSet();
            //var scommand = this.Database.GetDbConnection().CreateCommand();

            //    scommand.CommandText = "SELECT * FROM " + DTname;
            //    this.Database.OpenConnection();
            //    var reader = scommand.ExecuteReader();
            //do
            //{
            //    // loads the DataTable (schema will be fetch automatically)
            //    var tb = new DataTable();
            //    tb.Load(reader);
            //    result.Tables.Add(tb);

            //} while (!reader.IsClosed);

            //return result;


        }
         
        //////1401-1-7
         
        public int EditItem(ItemdDto req)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", req.DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            ///////1401-1-07
            List<string> S2 = new List<string>();
            if (req.S.Count == req.I.Count) //if (S.Count! > I.Count)
            {
                S2.Add("InsertTime");
                S2.Add("UpdateTime");
                S2.Add("IsRemoved");
                S2.Add("RemoveTime");
            }
            foreach (var t in req.S)
                S2.Add(t);

            ///
            string C1 = "";C1 += "UpdateTime='"+ DateTime.Now+"' ";
            for (int i = 0; i < req.I.Count; i++)
            {
                C1 +=", "+ S2[i + 4]+"="+ "'"+req.I[i]+"' ";
            }
            using (var con1 = Connection)
            {
                con1.Open();

                string query = "UPDATE "+req.TableName+" SET "+C1+ " WHERE id="+req.ItemId+"; ";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }

            }

        //////1401-1-7

        public int EditoneItem(ItemdDto req)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", req.DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            string C1 = ""; C1 += "UpdateTime='" + DateTime.Now + "' ";
           
            C1 += ", " + req.S[0] + "=" + "'" + req.I[0] + "' ";
           
            using (var con1 = Connection)
            {
                con1.Open();

                string query = "UPDATE " + req.TableName + " SET " + C1 + " WHERE id=" + req.ItemId + "; ";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }

        }
        public int InsertItem(List<string> S, List<string> I, string DTname, string DbName)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            List<string> S2 = new List<string>();
            if (S.Count==I.Count) //if (S.Count! > I.Count)
            {
                S2.Add("InsertTime");
                S2.Add("UpdateTime");
                S2.Add("IsRemoved");
                S2.Add("RemoveTime");
            }
            foreach(var t in S)
                S2.Add(t);

            //2020-04-04 00:00:00.000
            string C1 = " (", C2 = " (";
            //InsertTime  DateTime , UpdateTime  DateTime , IsRemoved  bit , RemoveTime  DateTime 
            C1 += "InsertTime,UpdateTime,IsRemoved,RemoveTime,";
            C2 += V + DateTime.Now + V + "," + V + DateTime.Now + V + "," + V + "False" + V + "," + "NULL" + ",";
            //C2 += "NULL" + "," + "NULL" + "," + "False" + "," + "NULL" + ",";
            if (S2[0] == "id")
            {
                for (int i = 0; i < I.Count; i++)
                {
                    C1 += S2[i + 5];
                    if (I[i] == "" || I[i] == null)
                        C2 += "'NULL'";
                    else C2 += V + I[i] + V;
                    if (i < I.Count - 1) { C1 += ","; C2 += ","; }
                }
            }
            else
            {
                for (int i = 0; i < I.Count; i++)
                {
                    C1 += S2[i + 4];
                    if (I[i] == "" || I[i] == null)
                        C2 += "'NULL'";
                    else C2 += V + I[i] + V;
                    if (i < I.Count - 1) { C1 += ","; C2 += ","; }
                }
            }
            C1 += ")";
            C2 += ")";
            using (var con1 = Connection)
            {
                con1.Open();
                string query = "INSERT INTO " + DTname + C1 + " VALUES " + C2;
                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }

        }
        public int InsertItemtoTokenLevel2(List<string> S, List<string> I, string DTname, string DbName)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            //2020-04-04 00:00:00.000
            string C1 = " (", C2 = " (";
            //InsertTime  DateTime , UpdateTime  DateTime , IsRemoved  bit , RemoveTime  DateTime 
            C1 += "id,InsertTime,UpdateTime,IsRemoved,RemoveTime,"+ S[5];
            C2 += V + I[0] + V + ","+ V + DateTime.Now + V + "," + V + DateTime.Now + V + "," + V + "False" + V + "," + "NULL" + ","+ V + I[1] + V;
            //C2 += "NULL" + "," + "NULL" + "," + "False" + "," + "NULL" + ",";
            //for (int i = 0; i < I.Count; i++)
            //{
            //    C1 += S[i + 5];
            //    if (I[i] == "" || I[i] == null)
            //        C2 += "'NULL'";
            //    else C2 += V + I[i+1] + V;
            //    if (i < I.Count - 1) { C1 += ","; C2 += ","; }
            //}
            C1 += ")";
            C2 += ")";
            using (var con1 = Connection)
            {
                con1.Open();
                string query = "INSERT INTO " + DTname + C1 + " VALUES " + C2;
                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }

        }
        public int DeleteItem(string DbName, string DTname, string iditem)
        {
            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
                string query = "DELETE FROM " + DTname + " WHERE " + "id = '"+ iditem+ "';";

                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return 1;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        //public int InsertItem(List<string> S, List<string> I, string DTname, string DbName)
        //{
        //    string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
        //    IDbConnection Connection = new SqlConnection(connectionString);

        //    //2020-04-04 00:00:00.000
        //    string C1 = " (", C2 = " (";
        //    //InsertTime  DateTime , UpdateTime  DateTime , IsRemoved  bit , RemoveTime  DateTime 
        //    C1 += "InsertTime,UpdateTime,IsRemoved,RemoveTime,";
        //    C2 += V + DateTime.Now + V + "," + V + DateTime.Now + V + "," + V + "False" + V + "," + "NULL" + ",";
        //    //C2 += "NULL" + "," + "NULL" + "," + "False" + "," + "NULL" + ",";
        //    for (int i = 5; i < I.Count; i++)
        //    {
        //        C1 += I[i];
        //        if (S[i] == "")
        //            C2 += "'NULL'";
        //        else C2 += V + S[i] + V;
        //        if (i < I.Count - 1) { C1 += ","; C2 += ","; }
        //    }
        //    C1 += ")";
        //    C2 += ")";
        //    using (var con1 = Connection)
        //    {
        //        con1.Open();
        //        string query = "INSERT INTO " + DTname + C1 + " VALUES " + C2;
        //        try
        //        {
        //            var resullt = con1.Query<object>(query).ToList();
        //            return 1;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //            return 0;
        //        }

        //    }

        //}
        
        
        
        //1401-01-07
        public List<object> GetNestedSelectonefilter(Equation filters, string DTname, string DbName)
        {
            var result = new List<object>();
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);
            if (filters.Compare == null)
            {
                filters.Compare = new List<string>();
                filters.Compare.Add("=");
            }
            if (filters.Compare.Count == 0)
            {
                filters.Compare.Add("=");
            }

            ///1. if this fild name isnt in this table
            ///
            var iddb = DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault().id;
            var idthist = Tables.Where(c => c.collection == DTname && c.IdDBase == iddb).FirstOrDefault().id;
            var ffildTistable = FieldsofTable.Where(t => t.IdTable == idthist);
            //if (ffild.Find(filters.Filname[0]).Count < 0){}
            if (Array.IndexOf(ffildTistable.Select(t => t.fieldname).ToArray(), filters.Filname[0]) < 0)
            {
                var R = RelationsofTable.Where(t => t.one_collection == (idthist));
                var Ronefilds = R.Select(t => t.one_field).ToArray();

                for (int d = 0; d < Ronefilds.Length; d++)
                {
                    int ind = Array.IndexOf(ffildTistable.Select(t => t.id).ToArray(), Ronefilds[d]);//idprouduct
                    if (ind >= 0)//
                    {
                        string fildTotable2 = ffildTistable.ToArray()[ind].fieldname;//idProduct for exanmple

                        var idt2 = R.ToArray()[d].many_collection;//.Select(t => t.many_collection).ToArray()[d];
                        var ffildstable2 = FieldsofTable.Where(t => t.IdTable == idt2); // all filds in Product
                        int ind2 = Array.IndexOf(ffildstable2.Select(t => t.id).ToArray(), R.ToArray()[d].many_field);//

                        string fildIntable2 = ffildstable2.ToArray()[ind2].fieldname;//id for exanmple
                         string TableRel = Tables.Where(g => g.id == idt2 && g.IsRemoved == false).FirstOrDefault().collection;//Product for exaple
                                                                                                                                                                         //////
                                                                                                                                                                         //IDbConnection Connection = new SqlConnection(connectionString);
                        using (var con1 = Connection)
                        {
                            con1.Open();
                            string query = "Select * From " + DTname + " Where  " + fildTotable2 + " IN (select " + fildIntable2 + " from " + TableRel + " where " + filters.Filname[0] + " = " + filters.Value[0] + " );";
                            try
                            {
                                var resullt = con1.Query<object>(query).ToList();
                                result = resullt;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                return null;
                            }
                        }
                        break;
                    }
                }
                return result;
            }
            else
            {

                string C1 = " ";
                for (int i = 0; i < filters.Filname.Count; i++)
                {
                    if (filters.Addcon != null)
                        if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                            C1 += filters.Addcon[i - 1] + " ";
                    C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                   

                }
                using (var con1 = Connection)
                {
                    string query = "";
                    con1.Open();
                    if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                        query = "Select * From " + DTname;
                    else query = "Select * From " + DTname + " Where  " + C1;


                    try
                    {
                        var resullt = con1.Query<object>(query).ToList();
                        result = resullt;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }



                }
                return result;
            }

        }

        //1401-01-17
        public List<object> GetNestedSelectonefilterbyListofValue(Equation filters, string DTname, string DbName)
        {
            
            var result = new List<object>();
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);
            IDbConnection Connection = null;// = new SqlConnection(connectionString);
            if (filters.Compare == null)
            {
                filters.Compare = new List<string>();
                filters.Compare.Add("=");
            }
            if (filters.Compare.Count == 0)
            {
                filters.Compare.Add("=");
            }
            ///for Search
            if (filters.Filname == null)
            {
                filters.Filname = new List<string>();
                var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                for (int ii = 5; ii < ffild.Count; ii++)
                {
                    filters.Filname.Add(ffild[ii]);
                    string C1 = " ";
                    for (int i = 0; i < filters.Filname.Count; i++)
                    {
                        if (filters.Addcon != null)
                            if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                                C1 += filters.Addcon[i - 1] + " ";
                        C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                        //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                        //    C1 += filters.Addcon[0]+" ";
                        //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                        //    C1 += filters.Addcon[i-1] + " ";

                    }
                    Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        con1.Open();
                        if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                            query = "Select * From " + DTname;
                        else query = "Select * From " + DTname + " Where  " + C1;


                        try
                        {
                            var resullt = con1.Query<object>(query);
                            foreach (var r in resullt)
                                result.Add(r);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return null;
                        }

                    }
                    filters.Filname.Remove(ffild[ii]);
                }
                return result;
            }
            else if (filters.Filname.Count == 0)
            {
                var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                for (int ii = 5; ii < ffild.Count; ii++)
                {
                    filters.Filname.Add(ffild[ii]);
                    string C1 = " ";
                    for (int i = 0; i < filters.Filname.Count; i++)
                    {
                        if (filters.Addcon != null)
                            if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                                C1 += filters.Addcon[i - 1] + " ";
                        C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                        //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                        //    C1 += filters.Addcon[0]+" ";
                        //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                        //    C1 += filters.Addcon[i-1] + " ";

                    }
                    Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        con1.Open();
                        if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                            query = "Select * From " + DTname;
                        else query = "Select * From " + DTname + " Where  " + C1;


                        try
                        {
                            var resullt = con1.Query<object>(query);
                            foreach (var r in resullt)
                                result.Add(r);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            // return null;
                        }

                    }
                    filters.Filname.Remove(ffild[ii]);
                }
                return result;
                //filters.Filname.Add(FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList()[5]);
            }
            //////////////////////////////
            ///
            //1401-01-17
            if (filters.Value.Count > filters.Filname.Count)//search list in this(own) table
            {
                //if (filters.Addcon == null)
                //    filters.Addcon = new List<string>();
                //for(int v=0;v<(filters.Value.Count-1);v++)
                //{
                //    filters.Filname.Add(filters.Filname[0]);
                //    filters.Addcon.Add("or");
                //    filters.Compare.Add("=");
                //}


                for (int ii =0; ii < filters.Value.Count; ii++)
                {
                    string C1 = " ";
                    C1 += filters.Filname[0] + " " + filters.Compare[0] + " " + "'" + filters.Value[ii] + "'" + " ";
                    
                    Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        
                        query = "Select * From " + DTname + " Where  " + C1;
                        try
                        {
                            con1.Open();
                            var resullt = con1.Query<object>(query);
                            foreach (var r in resullt)
                                result.Add(r);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            // return null;
                        }

                    }
                }
                 return result;

            }

            ///1. if this fild name isnt in this table
            ///
            var iddb = DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault().id;
            var idthist = Tables.Where(c => c.collection == DTname && c.IdDBase == iddb).FirstOrDefault().id;
            var ffildTistable = FieldsofTable.Where(t => t.IdTable == idthist);
            //if (ffild.Find(filters.Filname[0]).Count < 0){}
            if (Array.IndexOf(ffildTistable.Select(t => t.fieldname).ToArray(), filters.Filname[0]) < 0)
            {
                var R = RelationsofTable.Where(t => t.one_collection == (idthist));
                var Ronefilds = R.Select(t => t.one_field).ToArray();

                for (int d = 0; d < Ronefilds.Length; d++)
                {
                    int ind = Array.IndexOf(ffildTistable.Select(t => t.id).ToArray(), Ronefilds[d]);//idprouduct
                    if (ind >= 0)//
                    {
                        string fildTotable2 = ffildTistable.ToArray()[ind].fieldname;//idProduct for exanmple

                        var idt2 = R.ToArray()[d].many_collection;//.Select(t => t.many_collection).ToArray()[d];
                        var ffildstable2 = FieldsofTable.Where(t => t.IdTable == idt2); // all filds in Product
                        int ind2 = Array.IndexOf(ffildstable2.Select(t => t.id).ToArray(), R.ToArray()[d].many_field);//

                        string fildIntable2 = ffildstable2.ToArray()[ind2].fieldname;//id for exanmple
                        string TableRel = Tables.Where(g => g.id == idt2 && g.IsRemoved == false).FirstOrDefault().collection;//Product for exaple
                                                                                                                              //////
                                                                                                                              //IDbConnection Connection = new SqlConnection(connectionString);
                        using (var con1 = Connection)
                        {
                            con1.Open();
                            string query = "Select * From " + DTname + " Where  " + fildTotable2 + " IN (select " + fildIntable2 + " from " + TableRel + " where " + filters.Filname[0] + " = " + filters.Value[0] + " );";
                            try
                            {
                                var resullt = con1.Query<object>(query).ToList();
                                result = resullt;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                result= null;
                            }
                        }
                        break;
                    }
                }
            //    if(result == null)
            //    {
            //        R = RelationsofTable.Where(t => t.many_collection == (idthist));
            //        Ronefilds = R.Select(t => t.many_field).ToArray();

            //        for (int d = 0; d < Ronefilds.Length; d++)
            //        {
            //            int ind = Array.IndexOf(ffildTistable.Select(t => t.id).ToArray(), Ronefilds[d]);//idprouduct
            //            if (ind >= 0)//
            //            {
            //                string fildTotable2 = ffildTistable.ToArray()[ind].fieldname;//idProduct for exanmple

            //                var idt2 = R.ToArray()[d].one_collection;//.Select(t => t.many_collection).ToArray()[d];
            //                var ffildstable2 = FieldsofTable.Where(t => t.IdTable == idt2); // all filds in Product
            //                int ind2 = Array.IndexOf(ffildstable2.Select(t => t.id).ToArray(), R.ToArray()[d].one_field);//

            //                string fildIntable2 = ffildstable2.ToArray()[ind2].fieldname;//id for exanmple
            //                string TableRel = Tables.Where(g => g.id == idt2 && g.IsRemoved == false).FirstOrDefault().collection;//Product for exaple
            //                                                                                                                      //////
            //                                                                                                                      //IDbConnection Connection = new SqlConnection(connectionString);
            //                using (var con1 = Connection)
            //                {
            //                    con1.Open();
            //                    string query = "Select * From " + DTname + " Where  " + fildTotable2 + " IN (select " + fildIntable2 + " from " + TableRel + " where " + filters.Filname[0] + " = " + filters.Value[0] + " );";
            //                    try
            //                    {
            //                        var resullt = con1.Query<object>(query).ToList();
            //                        result = resullt;
            //                    }
            //                    catch (Exception e)
            //                    {
            //                        Console.WriteLine(e.Message);
            //                        return null;
            //                    }
            //                }
            //                break;
            //            }
            //        }
            //    }
               return result;
            }
            
            else
            {


                /////for Search
                //if (filters.Filname == null)
                //{
                //    filters.Filname = new List<string>();
                //    var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                //    for (int ii = 5; ii < ffild.Count; ii++)
                //    {
                //        filters.Filname.Add(ffild[ii]);
                //        string C1 = " ";
                //        for (int i = 0; i < filters.Filname.Count; i++)
                //        {
                //            if (filters.Addcon != null)
                //                if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                //                    C1 += filters.Addcon[i - 1] + " ";
                //            C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                //            //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                //            //    C1 += filters.Addcon[0]+" ";
                //            //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                //            //    C1 += filters.Addcon[i-1] + " ";

                //        }
                //        Connection = new SqlConnection(connectionString);
                //        using (var con1 = Connection)
                //        {
                //            string query = "";
                //            con1.Open();
                //            if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                //                query = "Select * From " + DTname;
                //            else query = "Select * From " + DTname + " Where  " + C1;


                //            try
                //            {
                //                var resullt = con1.Query<object>(query);
                //                foreach (var r in resullt)
                //                    result.Add(r);
                //            }
                //            catch (Exception e)
                //            {
                //                Console.WriteLine(e.Message);
                //                return null;
                //            }

                //        }
                //        filters.Filname.Remove(ffild[ii]);
                //    }
                //}
                //else if (filters.Filname.Count == 0)
                //{
                //    var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                //    for (int ii = 5; ii < ffild.Count; ii++)
                //    {
                //        filters.Filname.Add(ffild[ii]);
                //        string C1 = " ";
                //        for (int i = 0; i < filters.Filname.Count; i++)
                //        {
                //            if (filters.Addcon != null)
                //                if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                //                    C1 += filters.Addcon[i - 1] + " ";
                //            C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                //            //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                //            //    C1 += filters.Addcon[0]+" ";
                //            //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                //            //    C1 += filters.Addcon[i-1] + " ";

                //        }
                //        Connection = new SqlConnection(connectionString);
                //        using (var con1 = Connection)
                //        {
                //            string query = "";
                //            con1.Open();
                //            if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                //                query = "Select * From " + DTname;
                //            else query = "Select * From " + DTname + " Where  " + C1;


                //            try
                //            {
                //                var resullt = con1.Query<object>(query);
                //                foreach (var r in resullt)
                //                    result.Add(r);
                //            }
                //            catch (Exception e)
                //            {
                //                Console.WriteLine(e.Message);
                //                // return null;
                //            }

                //        }
                //        filters.Filname.Remove(ffild[ii]);
                //    }

                //    //filters.Filname.Add(FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList()[5]);
                //}
                ////////////////////////////////
                /////
                //else
                //{
                    string C1 = "( ";
                    for (int i = 0; i < filters.Filname.Count; i++)
                    {
                        if (filters.Addcon != null)
                            if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                                C1 += filters.Addcon[i - 1] + " ";
                        C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";


                    }
                    Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        con1.Open();
                        if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                            query = "Select * From " + DTname;
                        else query = "Select * From " + DTname + " Where  " + C1 + " ) ;";


                        try
                        {
                            var resullt = con1.Query<object>(query).ToList();
                            result = resullt;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return null;
                        }
                    }
                //}
                return result;
            }

        }

        public List<object> GetItembyFilterallfild(Equation filters, string DTname, string DbName)
        {
           var result = new List<object>() ;
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            


            //////////////////////////////
            if (filters.Compare == null )
            {
                filters.Compare = new List<string>();
                filters.Compare.Add( "=");
            }
            if( filters.Compare.Count == 0)
            {
                filters.Compare.Add("=");
            }
            //////////////////////////////
            //string C1 = " ";
            ///for Search
            if (filters.Filname == null)
            {
                filters.Filname = new List<string>();
                var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                for (int ii = 5; ii < ffild.Count; ii++)
                {
                    filters.Filname.Add(ffild[ii]);
                    string C1 = " ";
                    for (int i = 0; i < filters.Filname.Count; i++)
                    {
                        if (filters.Addcon != null)
                            if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                                C1 += filters.Addcon[i - 1] + " ";
                        C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                        //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                        //    C1 += filters.Addcon[0]+" ";
                        //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                        //    C1 += filters.Addcon[i-1] + " ";

                    }
                    IDbConnection Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        con1.Open();
                        if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                            query = "Select * From " + DTname;
                        else query = "Select * From " + DTname + " Where  " + C1;


                        try
                        {
                            var resullt = con1.Query<object>(query);
                            foreach(var r in resullt)
                                result.Add(r);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            //return null;
                        }

                    }
                    filters.Filname.Remove(ffild[ii ]);
                }
            }
            else if (filters.Filname.Count == 0)
            {
                var ffild = FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList();
                for (int ii = 5; ii < ffild.Count; ii++)
                {
                    filters.Filname.Add(ffild[ii]);
                    string C1 = " ";
                    for (int i = 0; i < filters.Filname.Count; i++)
                    {
                        if (filters.Addcon != null)
                            if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                                C1 += filters.Addcon[i - 1] + " ";
                        C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                        //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                        //    C1 += filters.Addcon[0]+" ";
                        //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                        //    C1 += filters.Addcon[i-1] + " ";

                    }
                    IDbConnection Connection = new SqlConnection(connectionString);
                    using (var con1 = Connection)
                    {
                        string query = "";
                        con1.Open();
                        if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                            query = "Select * From " + DTname;
                        else query = "Select * From " + DTname + " Where  " + C1;


                        try
                        {
                            var resullt = con1.Query<object>(query);
                            foreach (var r in resullt)
                                result.Add(r);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                           // return null;
                        }

                    }
                    filters.Filname.Remove(ffild[ii]);
                }

                //filters.Filname.Add(FieldsofTable.Where(t => t.IdTable == (Tables.Where(c => c.collection == DTname && c.IdDBase == (DatabaseLists.Where(d => d.DBName == DbName && d.IsRemoved == false).FirstOrDefault()).id).FirstOrDefault().id)).Select(s => s.fieldname).ToList()[5]);
            }
            //////////////////////////////
            else
            {
                string C1 = " ";
                for (int i = 0; i < filters.Filname.Count; i++)
                {
                    if (filters.Addcon != null)
                        if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                            C1 += filters.Addcon[i - 1] + " ";
                    C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                    //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                    //    C1 += filters.Addcon[0]+" ";
                    //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                    //    C1 += filters.Addcon[i-1] + " ";

                }
                IDbConnection Connection = new SqlConnection(connectionString);
                using (var con1 = Connection)
                {
                    string query = "";
                    con1.Open();
                    if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                        query = "Select * From " + DTname;
                    else query = "Select * From " + DTname + " Where  " + C1;


                    try
                    {
                        var resullt = con1.Query<object>(query).ToList();
                        result = resullt;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }

                }

            }
            return result;

        }
        public int Getiditem(string DBname, string Tname)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DBname);
            IDbConnection Connection = new SqlConnection(connectionString);
            int id = 0;
            using (var con1 = Connection)
            {
                con1.Open();
                string query ="SELECT MAX(id) AS LastID FROM "+Tname;
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    var resultSet = new List<SerializableDictionary<string, object>>();
                    foreach (IDictionary<string, object> row in resullt)
                    {
                        var dict = new SerializableDictionary<string, object>();
                        foreach (var pair in row)
                        {
                            dict.Add(pair.Key, pair.Value);
                        }
                        resultSet.Add(dict);
                    }

                    
                    //resullt.Select(x => (IDictionary<string, object>)x).ToList();
                    resultSet[0].TryGetValue("LastID", out id);
                   
                    return id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        public List<object> GetItembyFilteronefile(string fildgeted , Equation filters, string DTname, string DbName)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);

            string C1 = " ";

            for (int i = 0; i < filters.Filname.Count; i++)
            {
                if (filters.Addcon!=null)
                if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i > 0)
                    C1 += filters.Addcon[i - 1] + " ";
                C1 += filters.Filname[i] + " " + filters.Compare[i] + " " + "'" + filters.Value[i] + "'" + " ";
                //if(filters.Addcon.Count>0 && filters.Filname.Count>1&&i==0)
                //    C1 += filters.Addcon[0]+" ";
                //if (filters.Addcon.Count > 0 && filters.Filname.Count > 1 && i >0)
                //    C1 += filters.Addcon[i-1] + " ";

            }

            using (var con1 = Connection)
            {
                string query = "";
                con1.Open();
                if (filters.Compare.Count == 0 || filters.Value.Count == 0)
                    query = "Select "+fildgeted+"  From " + DTname;
                else query = "Select " + fildgeted + "  From " + DTname + " Where  " + C1;


                try
                {
                    var resullt = con1.Query<object>(query).ToList();
                    return resullt;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

            }



        }
       
        public int EditBD(string DBname, string DBnameNew)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DBname);
            IDbConnection Connection = new SqlConnection(connectionString);

            using (var con1 = Connection)
            {
                con1.Open();

                string query = "ALTER DATABASE " + DBname + " MODIFY NAME = "+ DBnameNew;
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        public int EditTable(string DBname,string Tname, string TnameNew)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DBname);
            IDbConnection Connection = new SqlConnection(connectionString);

            using (var con1 = Connection)
            {
                con1.Open();
                string query = "EXEC sp_rename '"+Tname + "' , '" + TnameNew + "'";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
        }
        public int CreatBD(string DBname)
        {
           // string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            string connectionString = "Data Source=.;Integrated Security=True ; MultipleActiveResultSets=True;";
            IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
                ///1400-11-20 =collate Arabic_100_CI_AS_KS_WS_SC_UTF8  
                string query = "create database " + DBname + " collate Arabic_100_CI_AS_KS_WS_SC_UTF8";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }
            //try
            //{
            //    Creatmodel("Users", DBname);
            //    Creatmodel("Tokens", DBname);
            //    return 1;
            //}
            //    catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return 0;
            //}
           

        }
        public int DeleteDb(string DBname)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            //string connectionString = "Data Source=.;Integrated Security=True ; MultipleActiveResultSets=True;";
            IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
                string query = "DROP DATABASE " + DBname + ";";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }

        }
        public int inserfiled(FiledDto f, string DbName)
        {
            string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DbName);

            //string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True;", DbName);
            IDbConnection Connection = new SqlConnection(connectionString);


            using (var con1 = Connection)
            {
                con1.Open();
                string query = "";
                string typ = "";
                if (f.type == "string")
                    typ = "nvarchar(50)";
                else if (f.type == "long")
                    typ = "bigint";
                else if (f.type == "bool")
                    typ = "bit";
                else if (f.type == "DateTime")
                    typ = "datetime";
                else if (f.type == "image"||f.type=="file")
                {
                    typ = "nvarchar(MAX)";
                }

                else
                    typ = f.type;

                if (!f.Nullable)
                    query = string.Format("alter  table {0} add {1} {2} NOT NULL ", f.tablename, f.name, typ);
                else
                    query = string.Format("alter  table {0} add {1} {2} ", f.tablename, f.name, typ);

                ///////Creat forinkey in SQL = 1400-11-11.
                //if (f.forignkey != null)
                //    query += "FOREIGN KEY REFERENCES " + f.table2 + "(" + f.forignkey + ")";

                ///Remove foring key From Queri for Resolve Loop in several ForingKeys- 1400-12-05

                query +=";";
                //////////
                try
                {
                    var resullt = con1.Query(query).ToList();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }

            //using (var scommand = this.Database.GetDbConnection().CreateCommand())
            //{
            //    if (f.Nullable)
            //        scommand.CommandText = string.Format("alter  table {0} add {1} {2} NOT NULL;", f.tablename, f.name, f.type);
            //    else
            //        scommand.CommandText = string.Format("alter  table {0} add {1} {2};", f.tablename, f.name, f.type);

            //    //scommand.CommandText = string.Format("create table {0}(ID int NOT NULL PRIMARY KEY);", txt_table.Text);
            //    this.Database.OpenConnection();
            //    try
            //    {
            //        using (var result = scommand.ExecuteReader())
            //        {
            //            // do something with result

            //            try
            //            {
            //                return 1;
            //                //MessageBox.Show("Database is created successfully with EFCore.", "MyProgram",
            //                //                MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //            catch (Exception ex)
            //            {
            //                return -1;
            //                //MessageBox.Show(ex.ToString());
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        return 0;
            //        // MessageBox.Show("database is exsist already.");
            //    }

            //}
        }

        public int DownloadDB(string DBname,string nameFile)
        {
            // string connectionString = string.Format("Data Source=.;Initial Catalog={0};Integrated Security=True ; MultipleActiveResultSets=True; ", DBname);

            string connectionString = "Data Source=.;Integrated Security=True ; MultipleActiveResultSets=True;";
            IDbConnection Connection = new SqlConnection(connectionString);
             
            string backupDIR = "E:\\DatabaseBackup";
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            using (var con1 = Connection)
            {
                con1.Open();
                string query = "backup database "+ DBname + " to disk='" + backupDIR + "\\" + nameFile + ".Bak'";
                try
                {
                    var resullt = con1.Query<object>(query).ToList();

                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }

            }

        }

        //public int inserToListTable(Tables t)
        //{
        //    // using (var context = new CmsDBContext())
        //    using (var scommand = this.Database.GetDbConnection().CreateCommand())
        //    {
        //        scommand.CommandText = "INSERT INTO Tables (collection,note,singleton) VALUES ('" + t.collection + "','" + t.note + "'," +Convert.ToInt32( t.singleton) +");";
        //        this.Database.OpenConnection();
        //        try
        //        {
        //            using (var result = scommand.ExecuteReader())
        //            {
        //                // do something with result
        //                try
        //                {
        //                    return 1;
        //                    //MessageBox.Show("Table is created successfully with EFCore.", "MyProgram",
        //                    //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                catch (Exception ex)
        //                {
        //                    return 0;
        //                    //MessageBox.Show(ex.ToString());
        //                }
        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //            return -1;
        //            //MessageBox.Show("Table is exist already.");
        //        }


        //    }

        //}
    }
}
