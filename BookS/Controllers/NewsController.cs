using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using PagedList;


namespace BookS.Controllers
{
    public class NewsController : Controller
    {

        DataClasses2DataContext _data = new DataClasses2DataContext();
        // GET: News 

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);


            var sachL = LaySach(7);

            return View(sachL.ToPagedList(pageNum, pageSize));
           
        }

        public List<TIN_TUC_tt> LaySach(int count)
        {
            return _data.TIN_TUC_tts.Take(count).ToList();
        }

        public ActionResult Introduct(int id)
        {
            var tin = _data.TIN_TUC_tts.SingleOrDefault(n => n.MaTin == id);
            return View(tin);
        }
    }
}