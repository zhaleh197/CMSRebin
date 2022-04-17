using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Infrastructure.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.Queries
{
    public class GetEverythings : IGetEverythings
    {
        private readonly IDatabaseContext _context;
        private readonly IGetFiledsService _getFiledsService;
        public GetEverythings(IDatabaseContext context, IGetFiledsService getFiledsService)
        {
            _context = context;
            _getFiledsService = getFiledsService;

        }
        public ReslutGetDto Execute(RequestGetDto request )
        {
            var Listitem = _context.GetItembyFilterallfild(request.filters, request.nametable,request.DbName).AsQueryable().ToList() ;
           
            return new ReslutGetDto
            {
                Rows = 0,
                Items = Listitem,
            };
            
        }
        public string getonevalueofitem2(Filterrequestonefild2 request)
        {
            var items = _context.GetItembyFilteronefile(request.fildgeted, request.filters, request.DTname, request.DbName);


            string result = "";

            JObject json = JObject.FromObject(items[0]);
            foreach (JProperty property in json.Properties())
            {
                result = property.Value.ToString();
            }
            return result;
        }


        public ReslutGetItemsdDto Execute2(RequestGetDto request)
        {
            var items = _context.GetItembyFilterallfild(request.filters, request.nametable, request.DbName).AsQueryable().ToList();
            
            
            
            string nametable = request.nametable;
            var DbName = request.DbName;
            var Db = _context.DatabaseLists.Where(b => b.DBName == DbName&&b.IsRemoved==false).FirstOrDefault();

            var tbid = _context.Tables.Where(t => t.collection == nametable&&t.IdDBase==Db.id&&t.IsRemoved==false).FirstOrDefault().id;
            
              

                //why is write it?????
                List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = nametable, DbName = DbName }).Fileds;
                // var items2 = _context.GetItemsSnmRel(nametable, Filds, DbName.DBName); 

                //var items = _context.GetColumname(request.nametable);


                //DataTable dt = new DataTable();
                //dt.Load(items);

                //int rowsCount = 0;
                // List<GetItemsDto> ITM = new List<GetItemsDto>() ;

                //foreach (var t in dt)
                //{
                //    ITM.Add(new GetItemsDto
                //    {
                //        Id=0,
                //        fieldname=t.
                //    });

                //}

                List<GetItemslistDto> IItms = new List<GetItemslistDto>();
                List<List<string>> IItms1 = new List<List<string>>();
                //List<List<object>> itemN_M = new List<List<object>>();
                var filedlistm_n = new List<string>();
                var table2m_n = new List<long>();
                foreach (var f in Filds)
                {
                    if (f.typeReleaton == "m-n")
                    {
                        ///edit in 14001012
                        // long idDb =_context.DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
                        long idtablethisfilf = tbid;
                        long idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(f.typeReleaton)).FirstOrDefault().id;
                        long idfildnew = f.Id; // _context.FieldsofTable.Where(t => t.fieldname.Equals(f.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                        var RR = _context.RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                        ////
                        table2m_n.Add(RR.many_collection);
                        /////edited 14001012
                        var t2 = _context.Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();
                        var itmnm = _context.GetItems(nametable + "_" + t2, DbName);

                        // itemN_M.Add(new List<object> { itmnm });

                        filedlistm_n.Add(f.fieldname);
                        /////
                        if (itmnm != null)
                        {
                            var valuefiledlistm_n = new List<string>();
                            for (int i = 0; i < itmnm.Count; i++)
                            {
                                //GetItemslistDto result = new GetItemslistDto();
                                // result.fieldnamelist = new List<string>();

                                JObject json = JObject.FromObject(itmnm[i]);
                                foreach (JProperty property in json.Properties())
                                {

                                    //if (property.Name != "id"
                                    //    && property.Name != "InsertTime"
                                    //    && property.Name != "UpdateTime"
                                    //    && property.Name != "IsRemoved"
                                    //    && property.Name != "RemoveTime")
                                    //{


                                    ////if (property.Name == (f.fieldname + "_id"))
                                    ////{
                                    ////    if (Convert.ToInt32((property.Value.ToString())) == f.Id)
                                    ////    {
                                    ////        result.fieldnamelist.Add(property.Name);
                                    ////        result.valuefiledlist.Add(property.Value.ToString());
                                    ////    }
                                    ////}
                                    ///
                                    if (property.Name == (t2 + "_id"))
                                    {
                                        // result.fieldnamelist.Add(property.Name);
                                        valuefiledlistm_n.Add(property.Value.ToString());
                                        break;
                                    }

                                }
                            }
                            ////
                            ///

                            IItms1.Add(valuefiledlistm_n);
                        }
                    }

                }



                //List<object> ItemFinal = new List<object>();
                //ItemFinal = itemN_M[0];
                //for (int i = 1; i < itemN_M.Count; i++)
                //    foreach (var it in itemN_M[i])
                //        ItemFinal.Add(it);

                //List<object> IItms = new List<object>();
                for (int i = 0; i < items.Count; i++)
                {
                    GetItemslistDto result = new GetItemslistDto();
                    result.fieldnamelist = new List<string>();
                    result.valuefiledlistList = new List<List<string>>();
                    result.releationfiled = new List<string>();
                    result.table2id = new List<long>();
                    JObject json = JObject.FromObject(items[i]);
                    var temp = new List<string>();
                    foreach (JProperty property in json.Properties())
                    {
                        temp = new List<string>();
                        result.fieldnamelist.Add(property.Name);
                        temp.Add(property.Value.ToString());
                        result.valuefiledlistList.Add(temp);
                        var r = _context.FieldsofTable.Where(f => f.fieldname.Equals(property.Name) && f.IdTable == tbid).FirstOrDefault();
                        //var t2id = _context.RelationsofTable.Where(t => t.one_collection == r.id).FirstOrDefault().many_collection;
                        var t2id = _context.RelationsofTable.Where(t => t.one_field == r.id).FirstOrDefault();
                        if (r != null)
                        {
                            if (r.relation != null && r.relation != "None")
                            {
                                result.releationfiled.Add(r.relation);
                                //result.table2id.Add(t2id);
                                result.table2id.Add(t2id.many_collection);
                            }
                            else
                            {
                                result.releationfiled.Add("None");
                                result.table2id.Add(-1);
                            }
                        }
                        else result.releationfiled.Add("None");



                    }
                    for (int ii = 0; ii < filedlistm_n.Count; ii++)
                    {
                        result.fieldnamelist.Add(filedlistm_n[ii]);
                        result.releationfiled.Add("m-n");
                        result.table2id.Add(table2m_n[ii]);
                        if (IItms1 != null)
                            result.valuefiledlistList.Add(IItms1[ii]);
                    }
                    IItms.Add(result);
                    //IItms.Add(result.valuefiledlist);

                }


                // IItms.Add(new GetItemslistDto { fieldnamelist})



                /////new 1400-9-28 // for showinf headers
                ///
                if (items.Count == 0)
                {
                    var f = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).ToList();
                    var r = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).Select(t => t.relation).ToList();
                    var table2id = new List<long>();
                    var v = new List<List<string>>();
                    for (int F = 0; F < f.Count; F++)
                    {
                        var t2id = _context.RelationsofTable.Where(t => t.one_field == f[F].id).FirstOrDefault();
                        if (r[F] != null && r[F] != "None")
                        {

                            table2id.Add(t2id.many_collection);

                        }
                        else
                        {
                            table2id.Add(-1);
                        }

                        var tv = new List<string>();
                        v.Add(tv);
                    }
                    IItms.Add(new GetItemslistDto { fieldnamelist = f.Select(t => t.fieldname).ToList(), releationfiled = r, table2id = table2id, valuefiledlistList = v, valuefiledlist = new List<string>() });
                }
                /////////////
                ///

                return new ReslutGetItemsdDto
                {
                    Rows = items.Count,
                    ITM = IItms,
                };
        }

        public ReslutGetItemsdDto Execute3(RequestGetDto request)
        {
            var items = _context.GetNestedSelectonefilter(request.filters, request.nametable, request.DbName).AsQueryable().ToList();
            string nametable = request.nametable;
            var DbName = request.DbName;
            var Db = _context.DatabaseLists.Where(b => b.DBName == DbName && b.IsRemoved == false).FirstOrDefault();

            var tbid = _context.Tables.Where(t => t.collection == nametable && t.IdDBase == Db.id && t.IsRemoved == false).FirstOrDefault().id;



            //why is write it?????
            List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = nametable, DbName = DbName }).Fileds;
            // var items2 = _context.GetItemsSnmRel(nametable, Filds, DbName.DBName); 

            //var items = _context.GetColumname(request.nametable);


            //DataTable dt = new DataTable();
            //dt.Load(items);

            //int rowsCount = 0;
            // List<GetItemsDto> ITM = new List<GetItemsDto>() ;

            //foreach (var t in dt)
            //{
            //    ITM.Add(new GetItemsDto
            //    {
            //        Id=0,
            //        fieldname=t.
            //    });

            //}

            List<GetItemslistDto> IItms = new List<GetItemslistDto>();
            List<List<string>> IItms1 = new List<List<string>>();
            //List<List<object>> itemN_M = new List<List<object>>();
            var filedlistm_n = new List<string>();
            var table2m_n = new List<long>();
            foreach (var f in Filds)
            {
                if (f.typeReleaton == "m-n")
                {
                    ///edit in 14001012
                    // long idDb =_context.DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
                    long idtablethisfilf = tbid;
                    long idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(f.typeReleaton)).FirstOrDefault().id;
                    long idfildnew = f.Id; // _context.FieldsofTable.Where(t => t.fieldname.Equals(f.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                    var RR = _context.RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                    ////
                    table2m_n.Add(RR.many_collection);
                    /////edited 14001012
                    var t2 = _context.Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();
                    var itmnm = _context.GetItems(nametable + "_" + t2, DbName);
                    //var itmnm = _context.GetNestedSelectonefilterbyListofValue(new Equation { Filname= },  nametable + "_" + t2, DbName,);
                    // itemN_M.Add(new List<object> { itmnm });

                    filedlistm_n.Add(f.fieldname);
                    /////
                    if (itmnm != null)
                    {
                        var valuefiledlistm_n = new List<string>();
                        for (int i = 0; i < itmnm.Count; i++)
                        {
                            //GetItemslistDto result = new GetItemslistDto();
                            // result.fieldnamelist = new List<string>();

                            JObject json = JObject.FromObject(itmnm[i]);
                            foreach (JProperty property in json.Properties())
                            {

                                //if (property.Name != "id"
                                //    && property.Name != "InsertTime"
                                //    && property.Name != "UpdateTime"
                                //    && property.Name != "IsRemoved"
                                //    && property.Name != "RemoveTime")
                                //{


                                ////if (property.Name == (f.fieldname + "_id"))
                                ////{
                                ////    if (Convert.ToInt32((property.Value.ToString())) == f.Id)
                                ////    {
                                ////        result.fieldnamelist.Add(property.Name);
                                ////        result.valuefiledlist.Add(property.Value.ToString());
                                ////    }
                                ////}
                                ///
                                if (property.Name == (t2 + "_id"))
                                {
                                    // result.fieldnamelist.Add(property.Name);
                                    
                                    valuefiledlistm_n.Add(property.Value.ToString());
                                    break;
                                }

                            }
                        }
                        ////
                        ///

                        IItms1.Add(valuefiledlistm_n);
                    }
                }

            }



            //List<object> ItemFinal = new List<object>();
            //ItemFinal = itemN_M[0];
            //for (int i = 1; i < itemN_M.Count; i++)
            //    foreach (var it in itemN_M[i])
            //        ItemFinal.Add(it);

            //List<object> IItms = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                GetItemslistDto result = new GetItemslistDto();
                result.fieldnamelist = new List<string>();
                result.valuefiledlistList = new List<List<string>>();
                result.releationfiled = new List<string>();
                result.table2id = new List<long>();
                JObject json = JObject.FromObject(items[i]);
                var temp = new List<string>();
                foreach (JProperty property in json.Properties())
                {
                    temp = new List<string>();
                    result.fieldnamelist.Add(property.Name);
                    temp.Add(property.Value.ToString());
                    result.valuefiledlistList.Add(temp);
                    var r = _context.FieldsofTable.Where(f => f.fieldname.Equals(property.Name) && f.IdTable == tbid).FirstOrDefault();
                    //var t2id = _context.RelationsofTable.Where(t => t.one_collection == r.id).FirstOrDefault().many_collection;
                    var t2id = _context.RelationsofTable.Where(t => t.one_field == r.id).FirstOrDefault();
                    if (r != null)
                    {
                        if (r.relation != null && r.relation != "None")
                        {
                            result.releationfiled.Add(r.relation);
                            //result.table2id.Add(t2id);
                            result.table2id.Add(t2id.many_collection);
                        }
                        else
                        {
                            result.releationfiled.Add("None");
                            result.table2id.Add(-1);
                        }
                    }
                    else result.releationfiled.Add("None");



                }
                for (int ii = 0; ii < filedlistm_n.Count; ii++)
                {
                    result.fieldnamelist.Add(filedlistm_n[ii]);
                    result.releationfiled.Add("m-n");
                    result.table2id.Add(table2m_n[ii]);
                    if (IItms1 != null)
                        result.valuefiledlistList.Add(IItms1[ii]);
                }
                IItms.Add(result);
                //IItms.Add(result.valuefiledlist);

            }


            // IItms.Add(new GetItemslistDto { fieldnamelist})



            /////new 1400-9-28 // for showinf headers
            ///
            if (items.Count == 0)
            {
                var f = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).ToList();
                var r = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).Select(t => t.relation).ToList();
                var table2id = new List<long>();
                var v = new List<List<string>>();
                for (int F = 0; F < f.Count; F++)
                {
                    var t2id = _context.RelationsofTable.Where(t => t.one_field == f[F].id).FirstOrDefault();
                    if (r[F] != null && r[F] != "None")
                    {

                        table2id.Add(t2id.many_collection);

                    }
                    else
                    {
                        table2id.Add(-1);
                    }

                    var tv = new List<string>();
                    v.Add(tv);
                }
                IItms.Add(new GetItemslistDto { fieldnamelist = f.Select(t => t.fieldname).ToList(), releationfiled = r, table2id = table2id, valuefiledlistList = v, valuefiledlist = new List<string>() });
            }
            /////////////
            ///

            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }
        public string getTablename(int id, string DbName)
        {
            var Db = _context.DatabaseLists.Where(b => b.DBName == DbName && b.IsRemoved == false).FirstOrDefault();
            string name =  _context.Tables.Where(t => t.id == id && t.IdDBase == Db.id && t.IsRemoved == false).FirstOrDefault().collection;
            return name;
        }
        public ReslutGetItemsdDto Execute4(RequestGetDto request)
        {
            var items = _context.GetNestedSelectonefilterbyListofValue(request.filters, request.nametable, request.DbName).AsQueryable().ToList();
            
            List<string> valuesid = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                valuesid.Add(JObject.FromObject(items[i]).Properties().ToList()[0].Value.ToString());
            }

            string nametable = request.nametable;
            var DbName = request.DbName;
            var Db = _context.DatabaseLists.Where(b => b.DBName == DbName && b.IsRemoved == false).FirstOrDefault();

            var tbid = _context.Tables.Where(t => t.collection == nametable && t.IdDBase == Db.id && t.IsRemoved == false).FirstOrDefault().id;

            //why is write it?????
            List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = nametable, DbName = DbName }).Fileds;
            // var items2 = _context.GetItemsSnmRel(nametable, Filds, DbName.DBName); 

            //var items = _context.GetColumname(request.nametable);


            //DataTable dt = new DataTable();
            //dt.Load(items);

            //int rowsCount = 0;
            // List<GetItemsDto> ITM = new List<GetItemsDto>() ;

            //foreach (var t in dt)
            //{
            //    ITM.Add(new GetItemsDto
            //    {
            //        Id=0,
            //        fieldname=t.
            //    });

            //}

            List<GetItemslistDto> IItms = new List<GetItemslistDto>();
            List<List<string>> IItms1 = new List<List<string>>();
            //List<List<object>> itemN_M = new List<List<object>>();
            var filedlistm_n = new List<string>();
            var table2m_n = new List<long>();
            foreach (var f in Filds)
            {
                if (f.typeReleaton == "m-n")
                {
                    ///edit in 14001012
                    // long idDb =_context.DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
                    long idtablethisfilf = tbid;
                    long idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(f.typeReleaton)).FirstOrDefault().id;
                    long idfildnew = f.Id; // _context.FieldsofTable.Where(t => t.fieldname.Equals(f.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                    var RR = _context.RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                    ////
                    table2m_n.Add(RR.many_collection);
                    /////edited 14001012
                    var t2 = _context.Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();
                    
                    
                    var itmnm0test = _context.GetItems(nametable + "_" + t2, DbName);
                    
                    var itmnm = _context.GetNestedSelectonefilterbyListofValue(new Equation { Filname=new List<string>() { f.fieldname+"_id" },Value = valuesid }, nametable + "_" + t2, DbName).AsQueryable().ToList();

                    // itemN_M.Add(new List<object> { itmnm });


                    filedlistm_n.Add(f.fieldname);
                    /////
                    if (itmnm != null)
                    {
                        var valuefiledlistm_n = new List<string>();
                        for (int i = 0; i < itmnm.Count; i++)
                        {
                            //GetItemslistDto result = new GetItemslistDto();
                            // result.fieldnamelist = new List<string>();

                            JObject json = JObject.FromObject(itmnm[i]);
                            foreach (JProperty property in json.Properties())
                            {

                                //if (property.Name != "id"
                                //    && property.Name != "InsertTime"
                                //    && property.Name != "UpdateTime"
                                //    && property.Name != "IsRemoved"
                                //    && property.Name != "RemoveTime")
                                //{


                                ////if (property.Name == (f.fieldname + "_id"))
                                ////{
                                ////    if (Convert.ToInt32((property.Value.ToString())) == f.Id)
                                ////    {
                                ////        result.fieldnamelist.Add(property.Name);
                                ////        result.valuefiledlist.Add(property.Value.ToString());
                                ////    }
                                ////}
                                ///
                                if (property.Name == (t2 + "_id"))
                                {
                                    // result.fieldnamelist.Add(property.Name);

                                    valuefiledlistm_n.Add(property.Value.ToString());
                                    break;
                                }

                            }
                        }
                        ////
                        ///

                        IItms1.Add(valuefiledlistm_n);
                    }
                }

            }



            //List<object> ItemFinal = new List<object>();
            //ItemFinal = itemN_M[0];
            //for (int i = 1; i < itemN_M.Count; i++)
            //    foreach (var it in itemN_M[i])
            //        ItemFinal.Add(it);

            //List<object> IItms = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                GetItemslistDto result = new GetItemslistDto();
                result.fieldnamelist = new List<string>();
                result.valuefiledlistList = new List<List<string>>();
                result.releationfiled = new List<string>();
                result.table2id = new List<long>();
                JObject json = JObject.FromObject(items[i]);
                var temp = new List<string>();
                foreach (JProperty property in json.Properties())
                {
                    temp = new List<string>();
                    result.fieldnamelist.Add(property.Name);
                    temp.Add(property.Value.ToString());
                    result.valuefiledlistList.Add(temp);
                    var r = _context.FieldsofTable.Where(f => f.fieldname.Equals(property.Name) && f.IdTable == tbid).FirstOrDefault();
                    //var t2id = _context.RelationsofTable.Where(t => t.one_collection == r.id).FirstOrDefault().many_collection;
                    var t2id = _context.RelationsofTable.Where(t => t.one_field == r.id).FirstOrDefault();
                    if (r != null)
                    {
                        if (r.relation != null && r.relation != "None")
                        {
                            result.releationfiled.Add(r.relation);
                            //result.table2id.Add(t2id);
                            result.table2id.Add(t2id.many_collection);
                        }
                        else
                        {
                            result.releationfiled.Add("None");
                            result.table2id.Add(-1);
                        }
                    }
                    else result.releationfiled.Add("None");
                }
                for (int ii = 0; ii < filedlistm_n.Count; ii++)
                {
                    result.fieldnamelist.Add(filedlistm_n[ii]);
                    result.releationfiled.Add("m-n");
                    result.table2id.Add(table2m_n[ii]);
                    if (IItms1 != null)
                        result.valuefiledlistList.Add(IItms1[ii]);
                }
                IItms.Add(result);
                //IItms.Add(result.valuefiledlist);

            }


            // IItms.Add(new GetItemslistDto { fieldnamelist})



            /////new 1400-9-28 // for showinf headers
            ///
            if (items.Count == 0)
            {
                var f = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).ToList();
                var r = _context.FieldsofTable.Where(f => f.IdTable == tbid && f.IsRemoved == false).Select(t => t.relation).ToList();
                var table2id = new List<long>();
                var v = new List<List<string>>();
                for (int F = 0; F < f.Count; F++)
                {
                    var t2id = _context.RelationsofTable.Where(t => t.one_field == f[F].id).FirstOrDefault();
                    if (r[F] != null && r[F] != "None")
                    {

                        table2id.Add(t2id.many_collection);

                    }
                    else
                    {
                        table2id.Add(-1);
                    }

                    var tv = new List<string>();
                    v.Add(tv);
                }
                IItms.Add(new GetItemslistDto { fieldnamelist = f.Select(t => t.fieldname).ToList(), releationfiled = r, table2id = table2id, valuefiledlistList = v, valuefiledlist = new List<string>() });
            }
            /////////////
            ///

            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }


        public List<ReslutGetItemsdDto> DFS(ReslutGetItemsdDto table, string DBname)
        {
            List<ReslutGetItemsdDto> result1 = new List<ReslutGetItemsdDto>();
            for (int i = 0; i < table.ITM[0].table2id.Count; i++)//deep search
            {
                long t = table.ITM[0].table2id[i];
                if (t != -1)
                {
                    List<string> value = new List<string>();
                  for(int itm = 0; itm < table.ITM.Count; itm++) 
                    for (int v = 0; v < table.ITM[itm].valuefiledlistList[i].Count; v++)
                        value.Add(table.ITM[itm].valuefiledlistList[i][v]);
                    //get table name
                    string tname =getTablename(Convert.ToInt32(t), DBname);
                    //
                    var restep = Execute4(
                        new RequestGetDto {
                        filters = new Equation 
                        { Filname = new List<string> 
                        { "id" },
                            Value = value },
                        nametable = tname,
                        DbName = DBname }
                    );
                    result1.Add(restep);
                    //result2.Push(restep);
                    //tep = result2.Pop();
                }
            }
            return result1;
        }





    }
    public class ReslutGetDto
    {
        public List<object> Items { get; set; }
        public int Rows { get; set; }


    }

    public class RequestGetDto
    {
        public string DbName { get; set; }
        public string nametable { get; set; }
        public Equation filters { get; set; }
    }
    public class Filterrequestonefild2
    {
        public string fildgeted { get; set; }
        public Equation filters { get; set; }
        public string DTname { get; set; }
        public string DbName { get; set; }

    }
}
