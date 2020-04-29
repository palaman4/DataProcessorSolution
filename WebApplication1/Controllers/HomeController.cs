using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.AppData;

using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult DataUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DataUpload(IFormCollection formdata)
        {
            var dataFields = formdata.Where(x => x.Key == "DataFields").Select(x => x.Value).First().ToString();
            var customField = formdata.Where(x => x.Key == "CustomField").Select(x => x.Value).First().ToString();
            var dataMappings = formdata.Where(x => x.Key == "DataMapping").Select(x => x.Value).First().ToString();
            
            if (String.IsNullOrEmpty(customField))
            {
                BaseDataService dataService = new BaseDataService(_config);
                
                if (dataService.ValidateDataIntoStaging(dataFields))
                {
                    if (dataService.ApplyMappingsToStaging(dataMappings))
                    {
                        dataService.InsertintoOutputTable();
                    }
                }
            }
            else
            {
                CustomDataService dataService = new CustomDataService(_config);
                dataService.customSourceField = customField;

                
                if (dataService.ValidateDataIntoStaging(dataFields))
                {
                    if (dataService.ApplyMappingsToStaging(dataMappings))
                    {
                        dataService.InsertintoOutputTable();
                    }
                }
            }
            return View();
        }

    }
 
}

//[{  "ColumnName":"Currency", "ReplaceValue":"CD","NewValue":"CAD"},{"ColumnName":"Currency", "ReplaceValue":"US","NewValue":"USD"},{"ColumnName":"Type", "ReplaceValue":"1","NewValue":"Trading"},{"ColumnName":"Type", "ReplaceValue":"2","NewValue":"RRSP"},{"ColumnName":"Type", "ReplaceValue":"3","NewValue":"RESP"},{"ColumnName":"Type", "ReplaceValue":"4","NewValue":"Fund"}]

  // test [{  "ColumnName":"Currency", "ReplaceValue":"C","NewValue":"CAD"},{"ColumnName":"Currency", "ReplaceValue":"U","NewValue":"USD"}]