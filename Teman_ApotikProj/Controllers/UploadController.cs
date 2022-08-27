using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teman_ApotikProj.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "Upload file sukses..";
                return View();
            }
            catch
            {
                ViewBag.Message = "Gagal Upload File!";
                return View();
            }
        }
    }
}