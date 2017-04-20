using BookS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Areas.Admin.Models;

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

        public ActionResult ThemSachMoi()
        {
            return View();
        }

        public ActionResult QuanLySach()
        {
            var sach_nxb = from sach in _data.SACHes
                           join nxb in _data.NHA_XUAT_BANs on sach.MaNXB equals nxb.MaNXB
                           select new Sach_NXB(sach, nxb.TenNXB);
            
            return View(sach_nxb.ToList());
        }
       

    }
}