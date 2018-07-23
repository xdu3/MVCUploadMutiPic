using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace uploadMutiPic.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private IHostingEnvironment _hostingEnvironment;
        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public void OnGet()
        {
        }



        public IActionResult UploadFiles()
        {
            return View();
        }


        [HttpPost, ActionName("UploadFiles")]
        [ValidateAntiForgeryToken]
        public  IActionResult UploadFilesAction()
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    string folderName = "Upload";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    string now = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string subFinder = Path.Combine(newPath, now);
                    if (!Directory.Exists(subFinder))
                    {
                        Directory.CreateDirectory(subFinder);
                    }

                    foreach (IFormFile item in files)
                    {
                        
                        if (item.Length > 0)
                        {
                            if (item.Length>5*1024*1024)
                            //3mb = 3*1024
                            {
                                ModelState.AddModelError("","You can't upload file more then 5MB");
                                return View();
                            }

                            var extension = item.FileName.Substring(item.FileName.LastIndexOf("."), item.FileName.Length - item.FileName.LastIndexOf("."));
                            extension = extension.ToLower();
                            string[] fileType = { ".png", ".jpg", ".jpeg" };
                            if (!fileType.Contains(extension))
                            {
                                ModelState.AddModelError("", "You can only upload image files");
                                return View();
                            }
                            string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                                string fullPath = Path.Combine(subFinder, fileName);
                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                        }
                        

                    }
                    return this.Content("Success");
                }
                else
                {
                    return this.Content("Fail");
                }
            }
            return View();
        }


    }
}