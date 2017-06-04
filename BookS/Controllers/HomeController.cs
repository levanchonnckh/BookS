using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;
using PagedList.Mvc;
using BookS.Areas.admin.Models;

namespace BookS.Controllers
{
    public class HomeController : Controller
    {


        DataClasses1DataContext _data = new DataClasses1DataContext();

        // GET: Home
        public ActionResult Index()
        {
            New_Hot ret = new New_Hot();
            ret.moi = new List<DEVICE>();
            ret.banChay = new List<CT_DON_HANG>();
            var moi = _data.DEVICEs.OrderBy(n => n.NgayCapNhat).Take(3);
            ret.moi = moi;

            var query = _data.CT_DON_HANGs.Take(3);
            
            ret.banChay = query;


            return View(ret);
        }

       


        public ActionResult SPChude(int id)
        {
            //id = 1 iphone
            //id = 2 iPad
            //id = 3 iMac
            //id = 4 MacBook
            //id = 5 watch
            //id = 6 accessoris
            device_head ret = new device_head();
            ret.icon = new List<ICON>();
            ret.device = new List<DEVICE>();
            
            var data = (from de in _data.DEVICEs select de).ToList();
            
            var icon = (from ic in _data.ICONs select ic).ToList();
            foreach(var i in data)
            {
                if (i.MaCD == id)
                {
                    ret.device.Add(i);
                }
            }

            foreach(var i in icon)
            {
                if (Int32.Parse(i.MaCD) == id)
                {
                    ret.icon.Add(i);
                }
            }

            return View(ret);
        }


        public ActionResult ChitietSP(int id)
        {
            //hien thi thoung tin tong quan cua san pham
            var chitiet = _data.CHI_TIETs.ToList();
            List<CHI_TIET> retct = new List<CHI_TIET>();
            foreach(var i in chitiet)
            {
                if (i.MaDev == id)
                {
                    retct.Add(i);
                }
            }
            return View(retct);
        }

        public ActionResult ChitietSP1(int id)
        {

            deviceDetail devD = new deviceDetail();
            devD.sheet = new List<Sheet1_>();

            devD.detailS = new Sheet1_();
            devD.detailD = new DEVICE();
            var dev = _data.Sheet1_s.ToList();

            foreach (var i in dev)
            {
                if (i.MaDev == id)
                {
                    //
                    devD.sheet.Add(i);
                }
            }

            devD.detailD = _data.DEVICEs.SingleOrDefault(n => n.MaDevice == id);
            var temp = _data.Sheet1_s.ToList();

            foreach (var i in temp)
            {
                if (i.MaDev == id)
                {
                    devD.detailS = i;
                    break;
                }
            }


            if (devD == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(devD);
        }
        
        public ActionResult contact()
        {
            return View();
        }

    }
}