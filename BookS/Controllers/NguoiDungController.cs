﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using BookS.Models;

namespace BookS.Controllers
{
    public class NguoiDungController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACH_HANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = "Họ tên không được để trống!";
            }else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi2"] = "Tên đăng nhập không được bỏ trống!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Mật khẩu không được để trống!";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["loi4"] = "Cần nhập lại mật khẩu!";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["loi5"] = "Cần phải nhập email1";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi6"] = "Điện thoại không được để trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KHACH_HANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");                
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult Dangxuat()
        {
            KHACH_HANG kh = new KHACH_HANG();
            kh = Session["Taikhoan"] as KHACH_HANG;
            //co nguoi dang nhap
            if (kh != null)
            {
                Session.RemoveAll();
            }
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult loginP()
        {
            return PartialView();
        }
        //
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection,string returnUrl)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn)|| String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập!";
                ViewData["Loi2"] = "Phải nhập mật khẩu!";
                return View();
            }
            else
            {
                KHACH_HANG kh = db.KHACH_HANGs.SingleOrDefault(n => n.Email == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công!";
                    Session["Taikhoan"] = kh;
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    return View();
                }
            }
            if (String.IsNullOrEmpty(returnUrl))
            {
                return View();
            }
            else
            {
                return Redirect(returnUrl);
            }
            
            
        }

        public ActionResult loginPart()
        {
            KHACH_HANG kh = new KHACH_HANG();
            kh = Session["Taikhoan"] as KHACH_HANG;
            

            return PartialView(kh);
        }
    }

    
    
}