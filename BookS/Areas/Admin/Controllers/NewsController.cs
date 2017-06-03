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
    public class NewsController : Controller
    {
        DataClasses2DataContext db = new DataClasses2DataContext();
        // GET: admin/News
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var deviceL = db.TIN_TUC_tts.ToList().OrderBy(n => n.MaTin);

            return View(deviceL.ToPagedList(pageNum, pageSize));
        }

        public ActionResult detail(int id)
        {
            var tin = db.TIN_TUC_tts.SingleOrDefault(n => n.MaTin == id);
            return View(tin);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tin = db.TIN_TUC_tts.SingleOrDefault(n => n.MaTin == id);

            if (tin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ChuDe = new SelectList(db.CHU_DE_tts.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.NguoiDang = new SelectList(db.ADMIN_tts.ToList().OrderBy(n => n.TenAdmin), "MaAdmin", "TenAdmin");
            return View(tin);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TIN_TUC_tt tt, HttpPostedFileBase fileUpload)
        {

            ViewBag.ChuDe = new SelectList(db.CHU_DE_tts.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.NguoiDang = new SelectList(db.ADMIN_tts.ToList().OrderBy(n => n.TenAdmin), "MaAdmin", "TenAdmin");
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
                    var path = Path.Combine(Server.MapPath("~/image/bia_TinTuc"), fileName);
                    //kiem tra file ton tai chu
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    tt.AnhBia = fileName;
                    UpdateModel(tt);
                    db.SubmitChanges();
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ChuDe = new SelectList(db.CHU_DE_tts.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.NguoiDang = new SelectList(db.ADMIN_tts.ToList().OrderBy(n => n.TenAdmin), "MaAdmin", "TenAdmin");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatNew(TIN_TUC_tt dev, HttpPostedFileBase fileUpload)
        {

            ViewBag.ChuDe = new SelectList(db.CHU_DE_tts.ToList().OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe");
            ViewBag.NguoiDang = new SelectList(db.ADMIN_tts.ToList().OrderBy(n => n.TenAdmin), "MaAdmin", "TenAdmin");

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
                    db.TIN_TUC_tts.InsertOnSubmit(dev);
                    db.SubmitChanges();
                }

                return View();
            }



        }

    }
}