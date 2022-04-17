using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.CreateItem
{
    public interface ICreateItem
    {
        ResultDto<ResultCreateItemdDto> Execute(RequestCreateItemdDto request);
        ResultDto<ResultCreateItemdDto> Execute2token(RequestCreateItemdDto request);
    }


    public class CreateItemServie : ICreateItem
    {
        private readonly IDatabaseContext _context;
        public CreateItemServie(IDatabaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultCreateItemdDto> Execute(RequestCreateItemdDto request)
        {
            //RequestCreateItemdDto request
            try
            {
                if (string.IsNullOrWhiteSpace(request.DbName))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام Db را وارد نمایید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.TableName))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را وارد نمایید"
                    };
                }
                if (!(request.S.Count>0))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "فیلدها را وارد نمایید"
                    };
                }
                if (!(request.I.Count > 0))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "مقادیر فیلدها را وارد نمایید"
                    };
                }
                //////Chech the RElations on filds
                ///


                //search in filesdoftables(tablename);
                //for (filsd in resultsearch)
                //if(resultsearch[i].relation = null)
                ///.....high
                ///
                /////if(resultsearch[i].relation = 1-1 )
                ///.....insert this filad and [virtual that role on this table and viversa]
                ///
                /////if(resultsearch[i].relation = 1-n )
                ///.....
                ///
                ////////if(resultsearch[i].relation = m-n )
                ///.....
                ///


                ///////// 
                ///
                ItemdDto Item = new ItemdDto()
                {
                   TableName = request.TableName,
                    I = request.I,
                    S = request.S,
                    DbName=request.DbName
                };

                //FieldsofTables fildintable = new FieldsofTables { fieldname = filed.name, tablename = filed.tablename, relation = filed.Relation };
                ////evey time add file, insert this table,
                //_context.FieldsofTable.Add(fildintable);
                /////
                //// insert evry where else m-n. but i insert it even m-n rel.
                ////_context.inserfiled(filed);
                /////

                //if (request.Relation == "-")
                //{
                //    _context.inserfiled(filed);
                //}
                //if (filed.Relation == "1-1")
                //{
                //    _context.inserfiled(filed);
                //    //insert into relation table
                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });

                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });
                //}
                //if (filed.Relation == "m-1")
                //{
                //    _context.inserfiled(filed);
                //    //insert into relation table
                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });
                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });
                //}
                //if (filed.Relation == "m-n")
                //{

                //    //insert into interfacetable- 3 record is added.

                //    //1 is inert above
                //    //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name, tablename = filed.tablename, relation = filed.Relation });
                //    //2
                //    fildintable.fieldname = filed.name + "_id";
                //    fildintable.tablename = filed.tablename + "_" + filed.table2;
                //    fildintable.relation = null;
                //    fildintable.interfaces = null;
                //    //
                //    _context.FieldsofTable.Add(fildintable);
                //    //=
                //    // _context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name + "_id", tablename = filed.tablename + "-" + filed.table2, relation = null, interfaces = null });

                //    //3
                //    fildintable.fieldname = filed.table2 + "_id";
                //    fildintable.tablename = filed.tablename + "_" + filed.table2;
                //    fildintable.relation = null;
                //    fildintable.interfaces = null;
                //    //
                //    _context.FieldsofTable.Add(fildintable);
                //    //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.table2+"_id", tablename = filed.tablename+"-"+filed.table2, relation = null,interfaces=null});

                //    //insert into relation table

                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });
                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });

                //    //create interface table

                //    _context.Creatmodel(filed.tablename + "_" + filed.table2);//insert id pishfarz
                //    // _context.FieldsofTables.Add(new FieldsofTables { fieldname = "id", tablename = filed.tablename + "_" + filed.table2, relation = null });
                //    ///
                //    _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.name + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true });
                //    _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.table2 + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true });
                //    //
                //    //insert to Tablelist

                //    _context.Tables.Add(new Tables { collection = filed.tablename + "_" + filed.table2, note = null, singleton = false, });


                //}
                

                ///1401-1-16
                ///
                for(int i=0;i<Item.S.Count;i++)
                {
                    var iddb = _context.DatabaseLists.FirstOrDefault(d => d.DBName == request.DbName && d.IsRemoved==false).id;
                    var idtb = _context.Tables.FirstOrDefault(t => t.collection == request.TableName && (t.IdDBase == iddb)).id;
                    string type = _context.FieldsofTable.FirstOrDefault(f => f.fieldname == Item.S[i] && f.IdTable == idtb).interfaces;
                    if(type=="image"||type=="file")
                    {
                        /////convert base64(wich string) to address
                        ///

                        //byte[] bytes = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");

                        //Image image;
                        //using (MemoryStream ms = new MemoryStream(bytes))
                        //{
                        //    image = Image.FromStream(ms);
                        //}

                        ////
                        ///
                        int j = i - (Item.S.Count - Item.I.Count);
                        var strm = Item.I[j];
                        //this is a simple white background image
                        var myfilename = string.Format(@"{0}", Guid.NewGuid());

                        //Generate unique filename
                        string filepath = "Uploads/" + myfilename + ".jpeg";
                        var bytess = Convert.FromBase64String(strm);
                        using (var imageFile = new FileStream(filepath, FileMode.Create))
                        {
                            imageFile.Write(bytess, 0, bytess.Length);
                            imageFile.Flush();
                            ////save in root
                          //  await imageFile.CopyToAsync(fileStream);
                        }

                        Item.I[j] = "file:///C:/Users/Tech-8/source/repos/CmsRebin/Endpoint.WebAPI/"+filepath;

                    }
                }


                //
                _context.InsertItem(Item.S,Item.I,Item.TableName,request.DbName);
               
                _context.SaveChanges();

                return new ResultDto<ResultCreateItemdDto>()
                {
                    Data = new ResultCreateItemdDto()
                    {
                        //TableId = table.id,
                        ItemId = _context.Getiditem(request.DbName,Item.TableName),
                       
                    },
                    IsSuccess = true,
                    Message = " رکورد اضافه شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateItemdDto>()
                {
                    Data = new ResultCreateItemdDto()
                    {
                        ItemId = 0,
                    },
                    IsSuccess = false,
                    Message = " رکورد اضافه نشد !!!!"
                };
            }
        }

        public ResultDto<ResultCreateItemdDto> Execute2token(RequestCreateItemdDto request)
        {
            //RequestCreateItemdDto request
            try
            {
                if (string.IsNullOrWhiteSpace(request.DbName))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام Db را وارد نمایید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.TableName))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را وارد نمایید"
                    };
                }
                if (!(request.S.Count > 0))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "فیلدها را وارد نمایید"
                    };
                }
                if (!(request.I.Count > 0))
                {
                    return new ResultDto<ResultCreateItemdDto>()
                    {
                        Data = new ResultCreateItemdDto()
                        {
                            ItemId = 0,
                        },
                        IsSuccess = false,
                        Message = "مقادیر فیلدها را وارد نمایید"
                    };
                }
                //////Chech the RElations on filds
                ///


                //search in filesdoftables(tablename);
                //for (filsd in resultsearch)
                //if(resultsearch[i].relation = null)
                ///.....high
                ///
                /////if(resultsearch[i].relation = 1-1 )
                ///.....insert this filad and [virtual that role on this table and viversa]
                ///
                /////if(resultsearch[i].relation = 1-n )
                ///.....
                ///
                ////////if(resultsearch[i].relation = m-n )
                ///.....
                ///


                ///////// 
                ///
                ItemdDto Item = new ItemdDto()
                {
                    TableName = request.TableName,
                    I = request.I,
                    S = request.S,
                    DbName = request.DbName
                };

                //FieldsofTables fildintable = new FieldsofTables { fieldname = filed.name, tablename = filed.tablename, relation = filed.Relation };
                ////evey time add file, insert this table,
                //_context.FieldsofTable.Add(fildintable);
                /////
                //// insert evry where else m-n. but i insert it even m-n rel.
                ////_context.inserfiled(filed);
                /////

                //if (request.Relation == "-")
                //{
                //    _context.inserfiled(filed);
                //}
                //if (filed.Relation == "1-1")
                //{
                //    _context.inserfiled(filed);
                //    //insert into relation table
                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });

                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });
                //}
                //if (filed.Relation == "m-1")
                //{
                //    _context.inserfiled(filed);
                //    //insert into relation table
                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });
                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });
                //}
                //if (filed.Relation == "m-n")
                //{

                //    //insert into interfacetable- 3 record is added.

                //    //1 is inert above
                //    //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name, tablename = filed.tablename, relation = filed.Relation });
                //    //2
                //    fildintable.fieldname = filed.name + "_id";
                //    fildintable.tablename = filed.tablename + "_" + filed.table2;
                //    fildintable.relation = null;
                //    fildintable.interfaces = null;
                //    //
                //    _context.FieldsofTable.Add(fildintable);
                //    //=
                //    // _context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name + "_id", tablename = filed.tablename + "-" + filed.table2, relation = null, interfaces = null });

                //    //3
                //    fildintable.fieldname = filed.table2 + "_id";
                //    fildintable.tablename = filed.tablename + "_" + filed.table2;
                //    fildintable.relation = null;
                //    fildintable.interfaces = null;
                //    //
                //    _context.FieldsofTable.Add(fildintable);
                //    //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.table2+"_id", tablename = filed.tablename+"-"+filed.table2, relation = null,interfaces=null});

                //    //insert into relation table

                //    //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });
                //    _context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });

                //    //create interface table

                //    _context.Creatmodel(filed.tablename + "_" + filed.table2);//insert id pishfarz
                //    // _context.FieldsofTables.Add(new FieldsofTables { fieldname = "id", tablename = filed.tablename + "_" + filed.table2, relation = null });
                //    ///
                //    _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.name + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true });
                //    _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.table2 + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true });
                //    //
                //    //insert to Tablelist

                //    _context.Tables.Add(new Tables { collection = filed.tablename + "_" + filed.table2, note = null, singleton = false, });


                //}
                _context.InsertItemtoTokenLevel2(Item.S, Item.I, Item.TableName, request.DbName);
                
                _context.SaveChanges();

                return new ResultDto<ResultCreateItemdDto>()
                {
                    Data = new ResultCreateItemdDto()
                    {
                        //TableId = table.id,
                       // ItemId = _context.Getiditem(request.DbName, Item.TableName),
                       ItemId=Convert.ToInt32( request.I[0]),

                    },
                    IsSuccess = true,
                    Message = " رکورد اضافه شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateItemdDto>()
                {
                    Data = new ResultCreateItemdDto()
                    {
                        ItemId = 0,
                    },
                    IsSuccess = false,
                    Message = " رکورد اضافه نشد !!!!"
                };
            }
        }

        //public List<Tables> gettables()
        //{
        //    return _context.Tables.AsQueryable().ToList();
        //}

        //public List<TypeofField> gettypefilse()
        //{
        //    return _context.TypeofField.AsQueryable().ToList();
        //}

        //public List<TypeofReleation> gettyperelation()
        //{
        //    return _context.TypeofReleation.AsQueryable().ToList();
        //}
    }
    public class RequestCreateItemdDto
    {
        public List<string> S { get; set; }
        //public List<List<string>> I { get; set; }
        public List<string> I { get; set; }
        public string TableName { get; set; }
        public string DbName { get; set; }
    }

    public class ResultCreateItemdDto
    {
        public long ItemId { get; set; }
    }
    public class ItemdDto
    {
        public int ItemId { get; set; }
        public List<string> S { get; set; }
        public List<string> I { get; set; }
        //public List<List<string>> I { get; set; }
        public string TableName { get; set; }
        public string DbName { get; set; }
    }

}
