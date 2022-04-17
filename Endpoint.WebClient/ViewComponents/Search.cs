using CmsRebin.Application.Service.Collection.Queris.GetItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.WebClient.ViewComponents
{
    public class Search : ViewComponent
    {
        //private readonly IGetItemsService _getItemsService;
        //public Search(IGetItemsService getItemsService)
        //{
        //    _getItemsService = getItemsService;
        //}


        public IViewComponentResult Invoke()
        {
            return View(viewName: "Search");
        }
    }
}
