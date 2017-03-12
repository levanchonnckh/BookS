using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;

namespace BookS.Controllers
{
    public class HomeController : Controller
    {


        DataClasses1DataContext _data = new DataClasses1DataContext();

        // GET: Home
        public ActionResult Index()
        {

            var sachL = LaySach(5);
            
            return View(sachL);
        }

        public List<SACH> LaySach(int count)
        {
            return _data.SACHes.Take(count).ToList();
        }
        

        public ActionResult Chude()
        {
            var chude = from cd in _data.CHU_DEs select cd;
            return PartialView(chude);
        }
        public ActionResult NXB()
        {
            var chude = from cd in _data.NHA_XUAT_BANs select cd;
            return PartialView(chude);
        }

        public ActionResult SPChude(int id)
        {
            var sach = from s in _data.SACHes where s.MaCD == id select s;
            return PartialView(sach);
        }

        public ActionResult SPNXB(int id)
        {
            var sach = from s in _data.SACHes where s.MaNXB == id select s;
            return PartialView(sach);
        }

        public ActionResult ChitietSP(int id)
        {
            var sach = from s in _data.SACHes where s.MaSach == id select s;
            return PartialView(sach);
        }

    }
}