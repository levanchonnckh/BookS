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
        public ActionResult Index()
        {
            return View();
        }

        //lay sach moi theo ngay cap nhat
       


        
        

      
      
        public ActionResult SPChude(int id=1)
        {
            return View();
        }


        public ActionResult ChitietSP(int id)
        {
            return View();
        }

    }
}