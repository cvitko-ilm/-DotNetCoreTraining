using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp2._0.Models;
using WebApp2._0.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApp2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IOptions<DataSettings> _options;
        private readonly IOptionsSnapshot<DataSettings> _optionsChange;
        private readonly IFileProvider _fileProvider;

        public HomeController(IDataService dataServices, IServiceProvider serviceProvider, IOptions<DataSettings> options, 
            IOptionsSnapshot<DataSettings> optionsChange, IFileProvider fileProvider)
        {
            _dataService = dataServices;
            //_dataService = (IDataService)serviceProvider.GetService(typeof(IDataService));

            _options = options;
            _optionsChange = optionsChange;
            _fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            var viewModel = new UserModel() {
                Name = "ViewModelName",
                Phone = "(123) 456-7890"
            };

            ViewData["DS_Option1"] = _options.Value.Option1;
            ViewData["DS_Option2"] = _options.Value.Option2;
            ViewData["DS_Option_Change1"] = _optionsChange.Value.Option1;
            ViewData["DS_Option_Change2"] = _optionsChange.Value.Option2;
            IFileInfo myDataFile = _fileProvider.GetFileInfo("Files/MyData.txt");
            ViewData["MyDataFileName"] = myDataFile.Exists ? myDataFile.PhysicalPath : string.Empty;
            IFileInfo myEmbeddedFile = _fileProvider.GetFileInfo("Embedded.MyEmbedded.txt");
            ViewData["MyEmbeddedDataName"] = myEmbeddedFile.Exists ? myEmbeddedFile.Name : string.Empty;

            TempData["MyTempDataName"] = "My Temporary Data Name";

            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            var name = _dataService.GetName();
            //var name = ((IDataService)this.HttpContext.RequestServices.GetService(typeof(IDataService))).GetName();
            ViewData["Message"] = $"Your contact page. Name:  {name}";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files) {
                if (formFile.Length > 0) {
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
    }
}
