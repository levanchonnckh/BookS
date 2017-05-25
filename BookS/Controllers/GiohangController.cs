using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;

namespace BookS.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        

        public ActionResult Dathang()
        {
            return View();
        }
        //kiem tra dang nhap
        //neu dang nhap eoi thì tiếp tục hoàn thành các thông tin giao hang
        //neu chua dang nhap thi cho người mua đăng nhập và tiếp tục hoàn thành
        //chuyen toi dangnhap/NguoiDung
        //trong dây co dang nhap va dang ky
        public ActionResult GioHang()
        {
            return View();
        }
    
        //bao gom car thong tin người dùng và thông tin yêu cầu nhập
        public ActionResult thongTinGiaoHang()
        {
            return View();
        }

        public ActionResult checkOut()
        {
            return View();
        }
    }
}