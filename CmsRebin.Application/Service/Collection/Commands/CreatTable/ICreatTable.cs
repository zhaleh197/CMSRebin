using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.CreatTable
{
   public interface ICreatTable
    {
        ResultDto<ResultCreateTableDto> Execute(RequestCreateTableDto request);
        ResultDto<ResultCreateTableDto> Execute2(RequestCreateTableDto2 request);
    }


    public class CreatTableService : ICreatTable
    {
        private readonly IDatabaseContext _context;
  
        public CreatTableService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultCreateTableDto> Execute(RequestCreateTableDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.collection))
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را وارد نمایید"
                    };
                }

                //Tables table = new Tables()
                //{
                //    collection = "khodammmm",
                //    note = "dastiiiiii",
                //    singleton = request.singleton
                //};
                long idb = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName)).FirstOrDefault().id;
                Tables table = new Tables()
                {
                    collection = request.collection,
                    note = request.note,
                    singleton = request.singleton,
                    IdDBase = idb
                };
                var t = _context.Tables.Where(t => t.collection.Equals(request.collection)&& t.IsRemoved == false && t.IdDBase==idb).FirstOrDefault();

                ///
                if (t!=null) //string.IsNullOrWhiteSpace(t.collection)
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را تکراری ستد"
                    };
                }

                //creat Table
                var res=_context.Creatmodel(table.collection,request.DbName);

                //insert to Tablelist
                if (res > 0)
                {
                    var r = _context.Tables.Add(table);
                    _context.SaveChanges();
                    var tt = r.Entity.id;
                    //_context.inserToListTable(table);
                    //var tttt = _context.Tables.Where(t => t.collection.Equals(request.collection) && t.IsRemoved == false).FirstOrDefault();

                    /////////////////////insert id in fildtable= 1400-09-15 inserted.

                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "id",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "int",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });

                    ///////InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime in fildtable= 1400-09-30 inserted ;
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "InsertTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "UpdateTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "IsRemoved",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "bool",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "RemoveTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    ///////////////////////////////////////
                    _context.SaveChanges();

                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = table.id,
                        },
                        IsSuccess = true,
                        Message = " جدول ایجاد شد",
                    };
                }
                else
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = " جدول ایجاد نشد !!!!"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateTableDto>() 
                {
                    Data = new ResultCreateTableDto()
                    {
                        TableId = 0,
                    },
                    IsSuccess = false,
                    Message = " جدول ایجاد نشد !!!!"
                };
            }
        }
        public ResultDto<ResultCreateTableDto> Execute2(RequestCreateTableDto2 request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.collection))
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را وارد نمایید"
                    };
                }

                //Tables table = new Tables()
                //{
                //    collection = "khodammmm",
                //    note = "dastiiiiii",
                //    singleton = request.singleton
                //};
                long idb = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName)).FirstOrDefault().id;
                Tables table = new Tables()
                {
                    collection = request.collection,
                    note = request.note,
                    singleton = request.singleton,
                    IdDBase = idb
                };
                var t = _context.Tables.Where(t => t.collection.Equals(request.collection) && t.IsRemoved == false && t.IdDBase == idb).FirstOrDefault();

                ///
                if (t != null) //string.IsNullOrWhiteSpace(t.collection)
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول را تکراری ستد"
                    };
                }

                //creat Table
                var res = _context.Creatmodel2(table.collection, request.DbName,request.forignkey,request.table2);

                //insert to Tablelist
                if (res > 0)
                {
                    var r = _context.Tables.Add(table);
                    _context.SaveChanges();
                    var tt = r.Entity.id;
                    //_context.inserToListTable(table);
                    //var tttt = _context.Tables.Where(t => t.collection.Equals(request.collection) && t.IsRemoved == false).FirstOrDefault();

                    /////////////////////insert id in fildtable= 1400-09-15 inserted.

                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "id",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "int",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });

                    ///////InsertTime  datetime , UpdateTime  datetime , IsRemoved  bit , RemoveTime  datetime in fildtable= 1400-09-30 inserted ;
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "InsertTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "UpdateTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "IsRemoved",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "bool",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    _context.FieldsofTable.Add(new FieldsofTables
                    {
                        fieldname = "RemoveTime",
                        IdTable = tt,
                        InsertTime = DateTime.Now,
                        interfaces = "datetime",
                        IsRemoved = false,
                        relation = "None",
                        RemoveTime = null,
                        UpdateTime = null
                    });
                    ///////////////////////////////////////
                    _context.SaveChanges();


                    ////////insert in relation tables if forignkey!=null 1400-11-24
                    if (request.forignkey != null)
                    {
                        long idt2 = _context.Tables.Where(t => t.collection == request.table2 && t.IdDBase == _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName) && d.IsRemoved == false).FirstOrDefault().id).FirstOrDefault().id;
                        long idf2 = _context.FieldsofTable.Where(t => t.fieldname == request.forignkey && t.IdTable == idt2&&t.IsRemoved==false).FirstOrDefault().id;
                        long idf1 = _context.FieldsofTable.Where(t => t.fieldname == "id" && t.IdTable == table.id&&t.IsRemoved==false).FirstOrDefault().id;
                        long idr = _context.TypeofReleation.Where(t => t.typerelation == "1-1").FirstOrDefault().id;

                        _context.RelationsofTable.Add(new RelationsofTables { many_collection = idt2, many_field = idf2, one_collection = table.id, one_field = idf1, TypeofReleation = idr });
                    }
                    
                    _context.SaveChanges();
                    ////////
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = table.id,
                        },
                        IsSuccess = true,
                        Message = " جدول ایجاد شد",
                    };
                }

              
                else
                {
                    return new ResultDto<ResultCreateTableDto>()
                    {
                        Data = new ResultCreateTableDto()
                        {
                            TableId = 0,
                        },
                        IsSuccess = false,
                        Message = " جدول ایجاد نشد !!!!"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateTableDto>()
                {
                    Data = new ResultCreateTableDto()
                    {
                        TableId = 0,
                    },
                    IsSuccess = false,
                    Message = " جدول ایجاد نشد !!!!"
                };
            }
        }
    }
    public class RequestCreateTableDto
    {
        public string DbName { get; set; }
        public string collection { get; set; }
        public string note { get; set; }
        public bool singleton { get; set; }
        

    }
    public class RequestCreateTableDto2
    {
        public string DbName { get; set; }
        public string collection { get; set; }
        public string note { get; set; }
        public bool singleton { get; set; }
        public string table2 { get; set; }
        public string forignkey { get; set; }


    }

    public class ResultCreateTableDto
    {
        public long TableId { get; set; }
    }




}
