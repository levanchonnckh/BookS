using BookS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Areas.Admin.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace BookS.Areas.Admin.Controllers
{
  
    public class HomeController : Controller
    {
        DataClasses1DataContext _data = new DataClasses1DataContext();
        // GET: Admin/Home
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            var tendn = col["email"];
            var mk = col["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(mk))
            {
                ViewData["loi2"] = "phai nhap mat khau";
            }
            else
            {
                ADMIN ad = _data.ADMINs.SingleOrDefault(n => n.USER_ADMIN == tendn && n.PASSWORD == mk);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Sach", "Home");
                }
                else
                {
                    ViewBag.Thongbao = "dang nhap that bai";
                }
            }
            return View();
        }

     

        public ActionResult Sach(int? page)
        {
            
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(_data.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
       
        public ActionResult Themsachmoi()
        {
            ViewBag.MaCD = new SelectList(_data.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_data.NHA_XUAT_BANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        public ActionResult Themsachmoi(SACH sach, HttpPostedFileBase file)
        {
            ViewBag.MaCD = new SelectList(_data.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_data.NHA_XUAT_BANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (file == null)
            {
                ViewBag.thongbao = "vui long chon hinh anh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);
                    
                    
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hinh anh da ton tai";
                    }
                    else
                    {
                        file.SaveAs(path);
                      
                    }
                    sach.Anhbia = fileName;
                    _data.SACHes.InsertOnSubmit(sach);
                    _data.SubmitChanges();
                }
                return View();

            }

            
            
           
        }


        public ActionResult Chitietsach(int id)
        {
            SACH sach = _data.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }

        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = _data.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }

        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = _data.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                _data.SACHes.DeleteOnSubmit(sach);
                _data.SubmitChanges();
            }
            catch (Exception)
            {
                ViewBag.Masach = "XOA KHONG THANH CONG";
            }
            
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = _data.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            ViewBag.MaCD = new SelectList(_data.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_data.NHA_XUAT_BANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View(sach);
        }


        [HttpPost]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase file)
        {
            ViewBag.MaCD = new SelectList(_data.CHU_DEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_data.NHA_XUAT_BANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (file == null)
            {
                ViewBag.thongbao = "vui long chon hinh anh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);


                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "hinh anh da ton tai";
                    }
                    else
                    {
                        file.SaveAs(path);

                    }
                    sach.Anhbia = fileName;
                    UpdateModel(sach);
                    _data.SubmitChanges();
                }
                return View();

            }




        }

    }
}