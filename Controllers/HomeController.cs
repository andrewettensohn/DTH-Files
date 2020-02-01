using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DTH_Upload.Models;
using Microsoft.Extensions.FileProviders;

namespace DTH_Upload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;

        public HomeController(ILogger<HomeController> logger, IFileProvider fileProvider)
        {
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public IActionResult Index(string folderName)
        {

            var model = new FileViewModel
            {
                PhysicalFiles = _fileProvider.GetDirectoryContents("/" + folderName)
            };
            model.FileCount = model.PhysicalFiles.Count();

            if(folderName != null)
            {
                model.CurrentDirectory = folderName;
            }
            else
            {
                model.CurrentDirectory = "Root";
            }
           
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
