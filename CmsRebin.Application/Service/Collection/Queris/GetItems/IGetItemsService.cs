using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using CmsRebin.Infrastructure.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Queris.GetItems
{
    public interface IGetItemsService
    {
        ReslutGetItemsdDto Execute(RequestGetItemsdDto request);
        ReslutGetItemsdDto ExecutebyTID(int id);
        ReslutGetItemsdDto getitembyfilters(Filterrequest request);
        ReslutGetItemsdDto getitembyonfilte(Filterrequestonefild request);
        public string getonevalueofitem(Filterrequestonefild request);
        //public ReslutGetItemsdDto ExecutebyTIDSeachkey(RequestDtoIDs req);

        //List<string> Execute2(RequestGetItemsdDto request);
    }
    public class GetItemsService : IGetItemsService
    {
        private readonly IDatabaseContext _context;
        private readonly IGetFiledsService _getFiledsService;
        //private readonly IdentityUser
        // SignInManager<IdentityUser> signinmanger
        public GetItemsService(IDatabaseContext context, IGetFiledsService getFiledsService)
        {
            _context = context;
            _getFiledsService = getFiledsService;

        }

        public ReslutGetItemsdDto ExecutebyTID(int id)
        {
            var t = _context.Tables.Where(t => t.id == id).FirstOrDefault();
            string nametable = t.collection;
            var DbName = _context.DatabaseLists.Where(b => b.id == t.IdDBase).FirstOrDefault();

            // var ITEMS = _context.GetItemsSandidrelfild(nametable,f,DbName)// bayad in ra piade konam.
            var items = _context.GetItems(nametable, DbName.DBName);
            //why is write it?????
            List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = nametable, DbName = DbName.DBName }).Fileds;
    
            List<GetItemslistDto> IItms = new List<GetItemslistDto>();
           // List<List<string>> IItms1 = new List<List<string>>();
            List<List<object>> itemN_M = new List<List<object>>();
            var filedlistm_n = new List<string>();
            var table2m_n = new List<long>();
            foreach (var f in Filds)
            {
                if (f.typeReleaton == "m-n")
                {
                    ///edit in 14001012
                    // long idDb =_context.DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
                    long idtablethisfilf = id;
                    long idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(f.typeReleaton)).FirstOrDefault().id;
                    long idfildnew = f.Id; // _context.FieldsofTable.Where(t => t.fieldname.Equals(f.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
                    var RR = _context.RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
                    ////
                    table2m_n.Add(RR.many_collection);
                    /////edited 14001012
                    var t2 = _context.Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();
                    
                    filedlistm_n.Add(f.fieldname);
                    
                    var itmnm = _context.GetItems(nametable + "_" + t2, DbName.DBName);

                     itemN_M.Add( itmnm  );

                }

            }
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
                    var r = _context.FieldsofTable.Where(f => f.fieldname.Equals(property.Name) && f.IdTable == id).FirstOrDefault();
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
                    List<string> IItms1 = new List<string>();
                    for (int j = 0; j < itemN_M[ii].Count; j++)
                    {
                        JObject jsonnm = JObject.FromObject(itemN_M[ii][j]);
                        if (jsonnm.Properties().ToList()[5].Value.ToString() == json.Properties().First().Value.ToString())
                        {
                            IItms1.Add(jsonnm.Properties().ToList()[6].Value.ToString());
                        }

                    }
                    result.valuefiledlistList.Add(IItms1);
                }
                IItms.Add(result);
            }
            /////new 1400-9-28 // for showinf headers
            ///
            if (items.Count == 0)
            {
                var f = _context.FieldsofTable.Where(f => f.IdTable == id && f.IsRemoved == false).ToList();
                var r = _context.FieldsofTable.Where(f => f.IdTable == id && f.IsRemoved == false).Select(t => t.relation).ToList();
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
                IItms.Add(new GetItemslistDto { fieldnamelist = f.Select(t => t.fieldname).ToList(), releationfiled = r, table2id = table2id ,valuefiledlistList=v,valuefiledlist=new List<string>()});
            }
            /////////////
            ///

            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }
        //public ReslutGetItemsdDto ExecutebyTIDSeachkey(RequestDtoIDs req)
        //{


        //    var t = _context.Tables.Where(t => t.id == req.TableName).FirstOrDefault();
        //    string nametable = t.collection;
        //    var DbName = _context.DatabaseLists.Where(b => b.id == t.IdDBase).FirstOrDefault();

        //    // var ITEMS = _context.GetItemsSandidrelfild(nametable,f,DbName)// bayad in ra piade konam.
        //    var items = _context.GetItems(nametable, DbName.DBName);
     
        //    //why is write it?????
        //    List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = nametable, DbName = DbName.DBName }).Fileds;
        //    // var items2 = _context.GetItemsSnmRel(nametable, Filds, DbName.DBName); 

        //    //var items = _context.GetColumname(request.nametable);


        //    //DataTable dt = new DataTable();
        //    //dt.Load(items);

        //    //int rowsCount = 0;
        //    // List<GetItemsDto> ITM = new List<GetItemsDto>() ;

        //    //foreach (var t in dt)
        //    //{
        //    //    ITM.Add(new GetItemsDto
        //    //    {
        //    //        Id=0,
        //    //        fieldname=t.
        //    //    });

        //    //}

        //    List<GetItemslistDto> IItms = new List<GetItemslistDto>();
        //    List<List<string>> IItms1 = new List<List<string>>();
        //    //List<List<object>> itemN_M = new List<List<object>>();
        //    var filedlistm_n = new List<string>();
        //    var table2m_n = new List<long>();
        //    foreach (var f in Filds)
        //    {
        //        if (f.typeReleaton == "m-n")
        //        {
        //            ///edit in 14001012
        //            // long idDb =_context.DatabaseLists.Where(d => d.DBName.Equals(DbName) && d.IsRemoved == false).FirstOrDefault().id;
        //            long idtablethisfilf = req.TableName;
        //            long idrelation = _context.TypeofReleation.Where(r => r.typerelation.Equals(f.typeReleaton)).FirstOrDefault().id;
        //            long idfildnew = f.Id; // _context.FieldsofTable.Where(t => t.fieldname.Equals(f.fieldname) && t.IdTable.Equals(idtablethisfilf) && t.IsRemoved == false).FirstOrDefault().id;
        //            var RR = _context.RelationsofTable.Where(r => r.one_collection.Equals(idtablethisfilf) && r.one_field.Equals(idfildnew)).FirstOrDefault();
        //            ////
        //            table2m_n.Add(RR.many_collection);
        //            /////edited 14001012
        //            var t2 = _context.Tables.Where(t => t.id.Equals(RR.many_collection)).Select(t => t.collection).FirstOrDefault().ToString();
        //            var itmnm = _context.GetItems(nametable + "_" + t2, DbName.DBName);

        //            // itemN_M.Add(new List<object> { itmnm });

        //            filedlistm_n.Add(f.fieldname);
        //            /////
        //            if (itmnm != null)
        //            {
        //                var valuefiledlistm_n = new List<string>();
        //                for (int i = 0; i < itmnm.Count; i++)
        //                {
        //                    //GetItemslistDto result = new GetItemslistDto();
        //                    // result.fieldnamelist = new List<string>();

        //                    JObject json = JObject.FromObject(itmnm[i]);
        //                    foreach (JProperty property in json.Properties())
        //                    {

        //                        //if (property.Name != "id"
        //                        //    && property.Name != "InsertTime"
        //                        //    && property.Name != "UpdateTime"
        //                        //    && property.Name != "IsRemoved"
        //                        //    && property.Name != "RemoveTime")
        //                        //{


        //                        ////if (property.Name == (f.fieldname + "_id"))
        //                        ////{
        //                        ////    if (Convert.ToInt32((property.Value.ToString())) == f.Id)
        //                        ////    {
        //                        ////        result.fieldnamelist.Add(property.Name);
        //                        ////        result.valuefiledlist.Add(property.Value.ToString());
        //                        ////    }
        //                        ////}
        //                        ///
        //                        if (property.Name == (t2 + "_id"))
        //                        {
        //                            // result.fieldnamelist.Add(property.Name);
        //                            valuefiledlistm_n.Add(property.Value.ToString());
        //                            break;
        //                        }

        //                    }
        //                }
        //                ////
        //                ///

        //                IItms1.Add(valuefiledlistm_n);
        //            }
        //        }

        //    }



        //    //List<object> ItemFinal = new List<object>();
        //    //ItemFinal = itemN_M[0];
        //    //for (int i = 1; i < itemN_M.Count; i++)
        //    //    foreach (var it in itemN_M[i])
        //    //        ItemFinal.Add(it);

        //    //List<object> IItms = new List<object>();
        //    for (int i = 0; i < items.Count; i++)
        //    {
        //        GetItemslistDto result = new GetItemslistDto();
        //        result.fieldnamelist = new List<string>();
        //        result.valuefiledlistList = new List<List<string>>();
        //        result.releationfiled = new List<string>();
        //        result.table2id = new List<long>();
        //        JObject json = JObject.FromObject(items[i]);
        //        var temp = new List<string>();
        //        foreach (JProperty property in json.Properties())
        //        {
        //            temp = new List<string>();
        //            result.fieldnamelist.Add(property.Name);
        //            temp.Add(property.Value.ToString());
        //            result.valuefiledlistList.Add(temp);
        //            var r = _context.FieldsofTable.Where(f => f.fieldname.Equals(property.Name) && f.IdTable == req.TableName).FirstOrDefault();
        //            //var t2id = _context.RelationsofTable.Where(t => t.one_collection == r.id).FirstOrDefault().many_collection;
        //            var t2id = _context.RelationsofTable.Where(t => t.one_field == r.id).FirstOrDefault();
        //            if (r != null)
        //            {
        //                if (r.relation != null && r.relation != "None")
        //                {
        //                    result.releationfiled.Add(r.relation);
        //                    //result.table2id.Add(t2id);
        //                    result.table2id.Add(t2id.many_collection);
        //                }
        //                else
        //                {
        //                    result.releationfiled.Add("None");
        //                    result.table2id.Add(-1);
        //                }
        //            }
        //            else result.releationfiled.Add("None");



        //        }
        //        for (int ii = 0; ii < filedlistm_n.Count; ii++)
        //        {
        //            result.fieldnamelist.Add(filedlistm_n[ii]);
        //            result.releationfiled.Add("m-n");
        //            result.table2id.Add(table2m_n[ii]);
        //            if (IItms1 != null)
        //                result.valuefiledlistList.Add(IItms1[ii]);
        //        }
        //        IItms.Add(result);
        //        //IItms.Add(result.valuefiledlist);

        //    }


        //    // IItms.Add(new GetItemslistDto { fieldnamelist})



        //    /////new 1400-9-28 // for showinf headers
        //    ///
        //    if (items.Count == 0)
        //    {
        //        var f = _context.FieldsofTable.Where(f => f.IdTable == req.TableName && f.IsRemoved == false).ToList();
        //        var r = _context.FieldsofTable.Where(f => f.IdTable == req.TableName && f.IsRemoved == false).Select(t => t.relation).ToList();
        //        var table2id = new List<long>();
        //        var v = new List<List<string>>();
        //        for (int F = 0; F < f.Count; F++)
        //        {
        //            var t2id = _context.RelationsofTable.Where(t => t.one_field == f[F].id).FirstOrDefault();
        //            if (r[F] != null && r[F] != "None")
        //            {

        //                table2id.Add(t2id.many_collection);

        //            }
        //            else
        //            {
        //                table2id.Add(-1);
        //            }

        //            var tv = new List<string>();
        //            v.Add(tv);
        //        }
        //        IItms.Add(new GetItemslistDto { fieldnamelist = f.Select(t => t.fieldname).ToList(), releationfiled = r, table2id = table2id, valuefiledlistList = v, valuefiledlist = new List<string>() });
        //    }
        //    /////////////
        //    ///

        //    return new ReslutGetItemsdDto
        //    {
        //        Rows = items.Count,
        //        ITM = IItms,
        //    };
        //}
        public ReslutGetItemsdDto Execute(RequestGetItemsdDto request)
        {

            //why is write it?????
            //List<GetFiledsDto> Filds = _getFiledsService.Execute(new RequestGetFiledDto { nametable = request.nametable ,DbName=request.DbName}).Fileds;



            var items = _context.GetItems(request.nametable, request.DbName);

            //var items = _context.GetItemsS(request.nametable, Filds,request.DbName); 

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
            //List<object> IItms = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                GetItemslistDto result = new GetItemslistDto();
                result.fieldnamelist = new List<string>();
                result.valuefiledlist = new List<string>();
                JObject json = JObject.FromObject(items[i]);
                foreach (JProperty property in json.Properties())
                {
                    result.fieldnamelist.Add(property.Name);
                    result.valuefiledlist.Add(property.Value.ToString());
                }
                IItms.Add(result);
                //IItms.Add(result.valuefiledlist);

            }
            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }
        ////////////////////////beacse some of fild dont save in table Such m-n fileds.
        ///
        public List<string> Execute2(RequestGetItemsdDto request)
        {
            //var items = _context.GetItems(request.nametable);
            var col = _context.GetColumname(request.nametable, request.DbName);
            List<string> fieldnamelist = new List<string>();

            for (int i = 0; i < col.Count; i++)
            {
                JObject json = JObject.FromObject(col[i]);
                foreach (JProperty property in json.Properties())
                {
                    fieldnamelist.Add(property.Value.ToString());
                }

            }
            return fieldnamelist;
        }




        public ReslutGetItemsdDto getitembyfilters(Filterrequest request)
        {
            var items = _context.GetItembyFilterallfild(request.filters ,request.DTname, request.DbName);
            List<GetItemslistDto> IItms = new List<GetItemslistDto>();
            //List<object> IItms = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                GetItemslistDto result = new GetItemslistDto();
                result.fieldnamelist = new List<string>();
                result.valuefiledlist = new List<string>();
                JObject json = JObject.FromObject(items[i]);
                foreach (JProperty property in json.Properties())
                {
                    result.fieldnamelist.Add(property.Name);
                    result.valuefiledlist.Add(property.Value.ToString());
                }
                IItms.Add(result);
                //IItms.Add(result.valuefiledlist);

            }
            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }
        public ReslutGetItemsdDto getitembyonfilte(Filterrequestonefild request)
        {
            var items = _context.GetItembyFilteronefile(request.fildgeted, request.filters, request.DTname, request.DbName);
            List<GetItemslistDto> IItms = new List<GetItemslistDto>();
            //List<object> IItms = new List<object>();
            for (int i = 0; i < items.Count; i++)
            {
                GetItemslistDto result = new GetItemslistDto();
                result.fieldnamelist = new List<string>();
                result.valuefiledlist = new List<string>();
                JObject json = JObject.FromObject(items[i]);
                foreach (JProperty property in json.Properties())
                {
                    result.fieldnamelist.Add(property.Name);
                    result.valuefiledlist.Add(property.Value.ToString());
                }
                IItms.Add(result);
                //IItms.Add(result.valuefiledlist);
            }
            return new ReslutGetItemsdDto
            {
                Rows = items.Count,
                ITM = IItms,
            };
        }

        public string getonevalueofitem(Filterrequestonefild request)
        {
            var items = _context.GetItembyFilteronefile(request.fildgeted,request.filters, request.DTname, request.DbName);


            string result = "";

                JObject json = JObject.FromObject(items[0]);
                foreach (JProperty property in json.Properties())
                {
                result=property.Value.ToString();
                } 
            return result;
        }
        /////////////////////////////////////////////////////////////////////////
    }
    //public class GetItemsDto
    //{
    //    public long Id { get; set; }
    //    public string fieldname { get; set; }
    //    public string valuefiled { get; set; }
    //}
    public class GetItemslistDto
    {
        public List<string> fieldnamelist { get; set; }
        public List<string> valuefiledlist { get; set; }
        public List<List<string>> valuefiledlistList { get; set; }
        public List<string> releationfiled { get; set; }
        public List<long> table2id { get; set; }
    }
    public class GetItemsDto
    {
        public List<string> fieldnamelist { get; set; }
        public List<string> valuefiledlist { get; set; }
    }
    //public class RequestGetItemsdDto
    //{
    //    public string BDName { get; set}
    //    public string nametable { get; set; }
    //    public string SearchKey { get; set; }
    //    public int Page { get; set; }
    //}

    public class RequestGetItemsdDto
    {
        public string DbName { get; set; }
        public string nametable { get; set; }
        public string SearchKey { get; set; }
        public int Page { get; set; }
    }


    public class Filterrequest
    {
        public Equation filters { get; set; }
        public string DTname { get; set; }
        public string DbName { get; set; }

    }

    public class Filterrequestonefild
    {
        public string fildgeted { get; set; }
        public Equation filters { get; set; }
        public string DTname { get; set; }
        public string DbName { get; set; }

    }

    public class ReslutGetItemsdDto
    {
        //public List< object> ITM { get; set; }

        public List<GetItemslistDto> ITM { get; set; }
        public int Rows { get; set; }

        
    }

}

