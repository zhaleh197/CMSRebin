using CmsRebin.Application.Interface.Context;
using CmsRebin.Domain.Entities.Collections;
using CmsRebin.Domain.Entities.Database;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Database.Queris.UploadDB
{
    public interface IUploadBD
    {
        void Execute(UploadDBDto request);
    }

    public class UploadDBService : IUploadBD
    {
         
        /////////////////////////////////
         
        private readonly IDatabaseContext _context;
        public UploadDBService(IDatabaseContext context)
        {
            _context = context;
        }

        ////////////////////////////////

        public void Execute(UploadDBDto request)
        {

           //Phase 1:
           //get Iformation from DB
           var res= _context.uploadDB_GetTables_GetRelation(request.fileDBName);

            //Note:
            //migration at first of all things. 
            //so tou have DB. and when upload your dataset, you have this db too.
            //just save DBname and use this name in all Actions.

            //set data in our Tables

            List<string> rree = new List<string>();

            for (int i = 0; i < res.getRelationDb.Count; i++)
            {
                JObject json = JObject.FromObject(res.getRelationDb[i]);
                foreach (JProperty property in json.Properties())
                {
                    rree.Add(property.Value.ToString());
                }

            }



            //for (int i = 0; i < res.getRelationDb.Count; i++)
            //{
            //    //1. set relations in RelationTables
            //    _context.RelationsofTable.Add(
            //        new RelationsofTables
            //        {
            //            one_collection = rree[1]
            //        }
            //        ) ;
            ////2. set tables in Tables
            ////3. set filesds of tables in FilesdsTable
            ////4. set Items in Items.

            //}
            //Phase 2:
            //save this BD as file in a path 
            //1. creat copy of this DB in aspatioal path
            //2. creat a model for files.(once time ): 
            //this propertis:
            //1. id 
            //2. StorageAdrees
            //3. File-name-disk
            //4. File-name-downloded
            //5. Title
            //6. type
            //7. uploadedBy(userId)
            //3. save in Files Table.

            //phase3: save thisDB in ListDB
            _context.DatabaseLists.Add(
                     new DatabaseList
                     {
                         DBName = request.fileDBName,
                         User = _context.Users.FirstOrDefault(i => i.id.Equals(request.UserId)),
                         InsertTime = DateTime.Now,
                         IsRemoved = false
                     }
                    //new DatabaseList{ DBName = request.DBName, IdUser = request.IdUserOwner,InsertTime=DateTime.Now,IsRemoved=false }
                    );
            _context.SaveChanges();

        }
    }
    public class UploadDBDto
    {
        public long UserId { get; set; }
        [Display(Name = "فایل DB")]
        //public List<IFormFile> fileDB { get; set; }
        //public List<string> fileDBName { get; set; }
        public IFormFile fileDB { get; set; }
        public string fileDBName { get; set; }



    }

}
