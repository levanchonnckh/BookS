using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;
using System.IO;

namespace BookS.Areas.admin.Controllers
{
    public class AdminController : Controller
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: admin/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var tenDn = f["email"];
            var mk = f["password"];
            if (String.IsNullOrEmpty(tenDn) || String.IsNullOrEmpty(mk))
            {
                ViewData["loi"] = "Email hoặc mất khẩu không đúng";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.USER_ADMIN == tenDn && n.PASSWORD == mk);
                if (ad != null)
                {
                    Session["email"] = tenDn;
                    return RedirectToAction("admin_home","Admin");
                }
            }
            return this.Index();
        }

        public ActionResult admin_home()
        {
            if (!string.IsNullOrEmpty(Session["email"] as string))
            {
                return RedirectToAction("device","Admin");
            }
            else
            {
                ViewData["loi"] = "bạn không có quyền truy cập";
            }

            return Content("bạn không có quyền truy cập");
        }

        public ActionResult device(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var deviceL = db.DEVICEs.ToList().OrderBy(n=>n.MaDevice);

            return View(deviceL.ToPagedList(pageNum, pageSize));
            
        }

        [HttpGet]
        public ActionResult CreatNew()
        {
            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatNew(DEVICE dev,HttpPostedFileBase fileUpload)
        {
            

            ViewBag.MaCD = new SelectList(db.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNSX = new SelectList(db.NHA_SAN_XUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");

            if (fileUpload == null)
            {
                ViewBag.thongbao = "vui lòng chọn hình ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan file
                    var path = Path.Combine(Server.MapPath("~/image/DEVICE"), fileName);
                    //kiem tra file ton tai chu
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    dev.AnhBia = fileName;
                    db.DEVICEs.InsertOnSubmit(dev);
                    db.SubmitChanges();
                }

                return View();
            }


           
        }
    }
}