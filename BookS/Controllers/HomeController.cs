using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;
using PagedList.Mvc;

namespace BookS.Controllers
{
    public class HomeController : Controller
    {


        DataClasses1DataContext _data = new DataClasses1DataContext();

        // GET: Home
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var sachL = LaySach(7);
            
            return View(sachL.ToPagedList(pageNum,pageSize));
        }

        //lay sach moi theo ngay cap nhat
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
            var sach = _data.SACHes.SingleOrDefault(n => n.MaSach == id);
            return PartialView(sach);
        }

    }
}