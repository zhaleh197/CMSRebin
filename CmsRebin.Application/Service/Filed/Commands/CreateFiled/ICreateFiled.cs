using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Collections;
using CmsRebin.Domain.Entities.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Filed.Commands.AddField
{
    public interface ICreateFiled
    {
        ResultDto<ResultCreateFiledDto> Execute(RequestCreateFieldDto request);
        List<string> gettypefilse();
        List<string> gettyperelation();
        List<Tables> gettables();
    }

    public class CreateFiledService : ICreateFiled
    {
        private readonly IDatabaseContext _context;

        public CreateFiledService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultCreateFiledDto> Execute(RequestCreateFieldDto request)
        {
            string table2 = null;
            string forignkey = null;
            try
            {

                if (string.IsNullOrWhiteSpace(request.name))
                {
                    return new ResultDto<ResultCreateFiledDto>()
                    {
                        Data = new ResultCreateFiledDto()
                        {
                            FiledId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام فیلد را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.tablename))
                {
                    return new ResultDto<ResultCreateFiledDto>()
                    {
                        Data = new ResultCreateFiledDto()
                        {
                            FiledId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام جدول مربوط به این فیلد را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.DbName))
                {
                    return new ResultDto<ResultCreateFiledDto>()
                    {
                        Data = new ResultCreateFiledDto()
                        {
                            FiledId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام دیتابیس را وارد نمایید"
                    };
                }

                var iddbas = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName) && d.IsRemoved == false).FirstOrDefault().id;
                var idtablethisfilf = _context.Tables.Where(t => t.collection.Equals(request.tablename) && t.IdDBase.Equals(iddbas) && t.IsRemoved == false).FirstOrDefault().id;


                if (request.Relation != "None" && request.Relation != null)
                {
                    /////1400-12-08- for when you enter name of table2 - not id of table2
                    try
                    {
                        int table2id = Convert.ToInt32(request.table2);
                    }
                    catch (Exception)
                    {
                        var rfd=_context.Tables.Where(t => t.collection == request.table2 && t.IdDBase == iddbas && t.IsRemoved == false).FirstOrDefault();
                        request.table2 = rfd.id.ToString();
                    }

                    try
                    {
                        int Foringketid = Convert.ToInt32(request.forignkey);
                    }
                    catch(Exception)
                    {
                       var fky= _context.FieldsofTable.Where(f => f.fieldname.Equals(request.forignkey) && f.IdTable.Equals(Convert.ToInt32(request.table2))).FirstOrDefault();
                        request.forignkey = fky.id.ToString();
                    }
                    ///////




                    table2 = _context.Tables.Where(t => t.id.Equals(Convert.ToInt32(request.table2)) && t.IdDBase.Equals(iddbas) && t.IsRemoved == false).FirstOrDefault().collection;
                    forignkey = _context.FieldsofTable.Where(f => f.id.Equals(Convert.ToInt32(request.forignkey)) && f.IdTable.Equals(Convert.ToInt32(request.table2))).FirstOrDefault().fieldname;

                }


                var ttt = _context.FieldsofTable.Where(f => f.fieldname.Equals(request.name) &&
                      f.IdTable.Equals(idtablethisfilf)).Count();
                if (ttt > 0)
                //if (_context.FieldsofTable.Where(f => f.fieldname.Equals(request.name) && 
                //f.Tables.collection.Equals(request.tablename)).Count() > 0)
                {
                    return new ResultDto<ResultCreateFiledDto>()
                    {
                        Data = new ResultCreateFiledDto()
                        {
                            FiledId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام فیلد را در این جدول تکراری ست"
                    };

                }


                FiledDto filed = new FiledDto()
                {
                    name = request.name,
                    tablename = request.tablename,
                    type = request.type,
                    Nullable = request.Nullable,
                    Uniqe = request.Uniqe,
                    Relation = request.Relation,
                    table2 = table2,
                    forignkey = forignkey,
                    DbName = request.DbName
                };
                ///
                //var iddbas = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName)).FirstOrDefault().id;
                //var idtablethisfilf = _context.Tables.Where(t => t.collection.Equals(filed.tablename) && t.IdDBase.Equals(iddbas) && t.IsRemoved==false).FirstOrDefault().id;
                FieldsofTables fildintable = new FieldsofTables { fieldname = filed.name, InsertTime = DateTime.Now, IsRemoved = false, IdTable = idtablethisfilf, relation = filed.Relation,interfaces=filed.type };
                ///
                //evey time add file, insert this table,



                var r = _context.FieldsofTable.Add(fildintable);
                _context.SaveChanges();
                ///


                // insert evry where else m-n. but i insert it even m-n rel.
                //_context.inserfiled(filed);
                ///

                if (!string.IsNullOrWhiteSpace(request.Relation))
                {
                    if (filed.Relation.Contains("1-1"))
                    {
                        _context.inserfiled(filed, request.DbName);
                        //insert into relation table
                        //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });

                        //var idtablesourcefilde = _context.Tables.Where(t => t.id.Equals(Convert.ToInt32(filed.table2)) && t.IdDBase.Equals(iddbas)).FirstOrDefault();
                        //var idsourcefild = _context.FieldsofTable.Where(f => f.id.Equals(Convert.ToInt32(filed.forignkey)) && f.IdTable.Equals(idtablesourcefilde.id)).FirstOrDefault().id;

                        //_context.SaveChanges();
                        var idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(filed.Relation)).FirstOrDefault().id;
                        var idfildnew = _context.FieldsofTable.Where(t => t.fieldname.Equals(fildintable.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault();
                        _context.RelationsofTable.Add(new RelationsofTables { one_collection = idtablethisfilf, one_field = idfildnew.id, many_collection = Convert.ToInt32(request.table2), many_field = Convert.ToInt32(request.forignkey), TypeofReleation = idrelation });
                    }
                    else if (filed.Relation.Contains("1-n"))//m-1
                    {
                        _context.inserfiled(filed, request.DbName);
                        //insert into relation table
                        //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });


                        //var idtablesourcefilde = _context.Tables.Where(t => t.collection.Equals(filed.table2) && t.IdDBase.Equals(iddbas)).FirstOrDefault().id;
                        //var idsourcefild = _context.FieldsofTable.Where(f => f.fieldname.Equals(filed.forignkey) && f.IdTable.Equals(idtablesourcefilde)).FirstOrDefault().id;
                        //var idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(filed.Relation)).FirstOrDefault().id;


                        //_context.RelationsofTable.Add(new RelationsofTables { one_collection = idtablethisfilf, one_field = r.Entity.id, many_collection = idtablesourcefilde, many_field = idsourcefild, TypeofReleation = idrelation });

                        //var idtablesourcefilde = _context.Tables.Where(t => t.id.Equals(Convert.ToInt32(filed.table2)) && t.IdDBase.Equals(iddbas)).FirstOrDefault();
                        //var idsourcefild = _context.FieldsofTable.Where(f => f.id.Equals(Convert.ToInt32(filed.forignkey)) && f.IdTable.Equals(idtablesourcefilde.id)).FirstOrDefault().id;
                        //_context.SaveChanges();
                        var idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(filed.Relation)).FirstOrDefault().id;
                        var idfildnew = _context.FieldsofTable.Where(t => t.fieldname.Equals(fildintable.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault();
                        _context.RelationsofTable.Add(new RelationsofTables { one_collection = idtablethisfilf, one_field = idfildnew.id, many_collection = Convert.ToInt32(request.table2), many_field = Convert.ToInt32(request.forignkey), TypeofReleation = idrelation });



                        //_context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });
                    }
                    else if (filed.Relation.Contains("m-n"))
                    {
                        //insert to Tablelist
                        long idnewfildinsert = 0;
                        var tmid = _context.Tables.Where(t => t.collection == (filed.tablename + "_" + filed.table2)).FirstOrDefault();
                        if (tmid == null)
                        {
                            var TNEW = _context.Tables.Add(new Tables
                            {
                                collection = (filed.tablename + "_" + filed.table2),
                                note = null,
                                singleton = false,
                                IdDBase = iddbas
                            });
                            _context.SaveChanges();
                            idnewfildinsert = TNEW.Entity.id;
                        }
                        else
                            idnewfildinsert = tmid.id;
                        //insert into interfacetable- 3 record is added.

                        //1 is inert above
                        //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name, tablename = filed.tablename, relation = filed.Relation });
                        //2


                        //fildintable.fieldname = filed.name + "_id";
                        //fildintable.IdTable = TNEW.Entity.id;
                        //fildintable.relation = null;
                        //fildintable.interfaces = null;
                        //fildintable.InsertTime = DateTime.Now;



                        //
                        _context.FieldsofTable.Add(new FieldsofTables
                        {
                            fieldname = filed.name + "_id",
                            IdTable = idnewfildinsert,
                            InsertTime = DateTime.Now,
                            IsRemoved = false,
                            interfaces=filed.type
                        });
                        //=
                        // _context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.name + "_id", tablename = filed.tablename + "-" + filed.table2, relation = null, interfaces = null });

                        //3
                        //fildintable.fieldname = filed.table2 + "_id";
                        //fildintable.tablename = filed.tablename + "_" + filed.table2;
                        //fildintable.relation = null;
                        //fildintable.interfaces = null;
                        ////
                        //_context.FieldsofTable.Add(fildintable);

                        _context.FieldsofTable.Add(new FieldsofTables
                        {
                            fieldname = filed.table2 + "_id",
                            IdTable = idnewfildinsert,
                            InsertTime = DateTime.Now,
                            IsRemoved = false,
                            interfaces=filed.type

                        });

                        //_context.FieldsofTables.Add(new FieldsofTables { fieldname = filed.table2+"_id", tablename = filed.tablename+"-"+filed.table2, relation = null,interfaces=null});

                        //insert into relation table

                        //_context.RelationsofTables.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = new TypeofReleation { typerelation = filed.Relation } });
                        //_context.RelationsofTable.Add(new RelationsofTables { one_collection = filed.tablename, one_field = filed.name, many_collection = filed.table2, many_field = filed.forignkey, TypeofReleation = filed.Relation });




                        //var idtablesourcefilde = _context.Tables.Where(t => t.collection.Equals(filed.table2) && t.IdDBase.Equals(iddbas)).FirstOrDefault().id;
                        //var idsourcefild = _context.FieldsofTable.Where(f => f.fieldname.Equals(filed.forignkey) && f.IdTable.Equals(idtablesourcefilde)).FirstOrDefault().id;
                        //var idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(filed.Relation)).FirstOrDefault().id;


                        //_context.RelationsofTable.Add(new RelationsofTables { one_collection = idtablethisfilf, one_field = r.Entity.id, many_collection = idtablesourcefilde, many_field = idsourcefild, TypeofReleation = idrelation });
                        //var idtablesourcefilde = _context.Tables.Where(t => t.id.Equals(Convert.ToInt32(filed.table2)) && t.IdDBase.Equals(iddbas)).FirstOrDefault();
                        //var idsourcefild = _context.FieldsofTable.Where(f => f.id.Equals(Convert.ToInt32(filed.forignkey)) && f.IdTable.Equals(idtablesourcefilde.id)).FirstOrDefault().id;

                        _context.SaveChanges();

                        var idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(filed.Relation)).FirstOrDefault().id;
                        var idfildnew = _context.FieldsofTable.Where(t => t.fieldname.Equals(fildintable.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault();
                        _context.RelationsofTable.Add(new RelationsofTables { one_collection = idtablethisfilf, one_field = idfildnew.id, many_collection = Convert.ToInt32(request.table2), many_field = Convert.ToInt32(request.forignkey), TypeofReleation = idrelation });


                        //create interface table

                        _context.Creatmodel((filed.tablename + "_" + filed.table2), request.DbName);//insert id pishfarz
                                                                                                    // _context.FieldsofTables.Add(new FieldsofTables { fieldname = "id", tablename = filed.tablename + "_" + filed.table2, relation = null });
                        ///
                        _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.name + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true }, request.DbName);
                        _context.inserfiled(new FiledDto { tablename = filed.tablename + "_" + filed.table2, name = filed.table2 + "_id", type = "Int", Relation = null, Nullable = false, Uniqe = true }, request.DbName);
                        //
                        ////insert to Tablelist

                        //_context.Tables.Add(new Tables { collection = filed.tablename + "_" + filed.table2, note = null, singleton = false,});

                    }
                    else
                    {
                        _context.inserfiled(filed, request.DbName);
                    }
                }
                else
                {
                    _context.inserfiled(filed, request.DbName);
                }

                // var r = _context.FieldsofTable.Add(fildintable);
                _context.SaveChanges();

                return new ResultDto<ResultCreateFiledDto>()
                {
                    Data = new ResultCreateFiledDto()
                    {
                        //TableId = table.id,

                        FiledId = fildintable.id,
                    },
                    IsSuccess = true,
                    Message = " فیلد اضافه شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateFiledDto>()
                {
                    Data = new ResultCreateFiledDto()
                    {
                        FiledId = 0,
                    },
                    IsSuccess = false,
                    Message = " فیلد اضافه نشد !!!!"
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

        public List<Tables> gettables()
        {
            List<string> v = new List<string>();

            var t = _context.Tables.Where(t => t.IsRemoved == false).ToList();
            //foreach (var itm in t)
            //    v.Add(itm.collection);
            return t;
        }

        public List<string> gettypefilse()
        {
            List<string> v = new List<string>();

            var t = _context.TypeofField.ToList();
            foreach (var itm in t)
                v.Add(itm.typefield);
            return v;
        }

        public List<string> gettyperelation()
        {
            List<string> v = new List<string>();

            var t = _context.TypeofReleation.ToList();
            foreach (var itm in t)
                v.Add(itm.typerelation);
            return v;
        }


    }

    public class RequestCreateFieldDto
    {
        public string DbName { get; set; }
        public string tablename { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool Nullable { get; set; }
        public bool Uniqe { get; set; }
        public string Relation { get; set; }

        public string table2 { get; set; }

        public string forignkey { get; set; }


    }

    public class ResultCreateFiledDto
    {
        public long FiledId { get; set; }
    }
    public class FiledDto
    {
        public int id { get; set; }
        public string DbName { get; set; }

        public string tablename { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool Nullable { get; set; }
        public bool Uniqe { get; set; }
        public string Relation { get; set; }

        public string table2 { get; set; }

        public string forignkey { get; set; }
    }

}
