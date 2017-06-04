using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookS.Models;
using BookS.Areas.admin.Models;
using System.Net.Mail;

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
        
        public ActionResult Buy(int id)
        {
            deviceDetail devD = new deviceDetail();
            devD.sheet = new List<Sheet1_>();

            devD.detailS = new Sheet1_();
            devD.detailD = new DEVICE();
            var dev = db.Sheet1_s.ToList();

            foreach (var i in dev)
            {
                if (i.MaDev == id)
                {
                    devD.sheet.Add(i);
                }
            }

            devD.detailD = db.DEVICEs.SingleOrDefault(n => n.MaDevice == id);
            var temp = db.Sheet1_s.ToList();

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
       
        //kiem tra dang nhap
        //neu dang nhap eoi thì tiếp tục hoàn thành các thông tin giao hang
        //neu chua dang nhap thi cho người mua đăng nhập và tiếp tục hoàn thành
        //chuyen toi dangnhap/NguoiDung
        //trong dây co dang nhap va dang ky
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
            
        }

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<Giohang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGiohang(int iMaDevice,string MaChiTiet, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.chitiet.MaChiTiet == MaChiTiet);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaDevice,MaChiTiet);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);

            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                dTongTien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return dTongTien;
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(string iMasp)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.chitiet.MaChiTiet == iMasp);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.chitiet.MaChiTiet == iMasp);
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("GioHang", "Giohang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaAll()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CapnhatGiohang(string iMasp, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.chitiet.MaChiTiet == iMasp);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("thongTinGiaoHang", "Giohang");
            }

            if (Session["Giohang"] == null)
            {
                return RedirectToAction("index", "BookS");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            KHACH_HANG kh = (KHACH_HANG)Session["Taikhoan"];

            //khach hang da dang nhap
            //luu cac thong tin 
            //don dat hang
            //chi tiet don hang
            //thong tin nguoi nhan
            DON_DAT_HANG ddh = new DON_DAT_HANG();
            if (kh != null)
            {
                //luu tru don dat hang
                
                List<Giohang> gh = Laygiohang();
                ddh.MaKH = kh.MaKH;
                ddh.NgayDH = DateTime.Now;
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
                ddh.NgayGiao = DateTime.Parse(ngaygiao);
                ddh.TinhTrangGiaoHang = false;
                ddh.DaThanhToan = false;
                ddh.Name = collection["name"];
                ddh.Phone = collection["phone"];
                ddh.Address = collection["address"];
                ddh.Email = collection["email"];
                db.DON_DAT_HANGs.InsertOnSubmit(ddh);
                db.SubmitChanges();
                int temp = db.DON_DAT_HANGs.ToList().Last().MaDH;
                //Them chi tiet don hang
                foreach (var item in gh)
                {
                    CT_DON_HANG ctdh = new CT_DON_HANG();
                    
                    ctdh.MaDH = temp;
                    ctdh.MaDevice = item.iMasach;
                    ctdh.SoLuong = item.iSoluong;
                    ctdh.DonGia = (decimal)item.dDongia;
                    db.CT_DON_HANGs.InsertOnSubmit(ctdh);
                }

                db.SubmitChanges();
                //ke ca co dang nhap va khong dang nhap
                
                
                
            }
            //neu khong dang nhap
            //luu thong tin dia chi nguoi nhan
            //don dat hang
            else
            {
                Response.Write("<script>alert('that bai')</script>");
            }
            
            
            return RedirectToAction("Xacnhandonhang","Giohang",new { sdt = ddh.Phone});
        }

        public ActionResult Xacnhandonhang(string sdt)
        {
           
            ViewBag.sdt = sdt;

            return View();
        }



        //bao gom car thong tin người dùng và thông tin yêu cầu nhập
        //yeu cau thong tin ngươi dung, neu khong co thi yeu cau dang nhap
        //khong dang nhạp thi mu o ngoai
        public ActionResult thongTinGiaoHang()
        {
            
            return View();
        }

        //kiem tra lan cuoi
        [HttpGet]
        public ActionResult checkOut()
        {
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("index", "BookS");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        [HttpPost]
        public ActionResult checkOut(FormCollection collection)
        {
            

            //khach hang da dang nhap
            //luu cac thong tin 
            //don dat hang
            //chi tiet don hang
            //thong tin nguoi nhan
            DON_DAT_HANG ddh = new DON_DAT_HANG();
          

                List<Giohang> gh = Laygiohang();
            ddh.MaKH = 2;
                ddh.NgayDH = DateTime.Now;
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
                ddh.NgayGiao = DateTime.Parse(ngaygiao);
                ddh.TinhTrangGiaoHang = false;
                ddh.DaThanhToan = false;
                ddh.Name = collection["name"];
                ddh.Phone = collection["phone"];
                ddh.Address = collection["address"];
                ddh.Email = collection["email"];
                db.DON_DAT_HANGs.InsertOnSubmit(ddh);
                db.SubmitChanges();
                //Them chi tiet don hang
                foreach (var item in gh)
                {
                    CT_DON_HANG ctdh = new CT_DON_HANG();
                    int temp = db.DON_DAT_HANGs.ToList().Last().MaDH;
                    ctdh.MaDH = ddh.MaDH;
                    ctdh.MaDevice = item.iMasach;
                    ctdh.SoLuong = item.iSoluong;
                    ctdh.DonGia = (decimal)item.dDongia;
                    db.CT_DON_HANGs.InsertOnSubmit(ctdh);
                }
                //ke ca co dang nhap va khong dang nhap
                try
                {
                    db.SubmitChanges();
                    Session["Giohang"] = null;
                }
                catch (Exception)
                {

                }

            return RedirectToAction("Xacnhandonhang", "Giohang", new { email = ddh.Email });
        }


    }
}