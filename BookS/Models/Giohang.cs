using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookS.Models;

namespace BookS.Models
{
    public class Giohang
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        
        public int iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        //chi tiet san pham cho nguoi dung biet minh mua cai nao
        public Sheet1_ chitiet { get; set; }
        public Double dThanhtien
        {
            get { return (iSoluong * dDongia)*22000; }
        }
        public Giohang(int Masach,string Machitiet)
        {
            this.iMasach = Masach;
            DEVICE sach = db.DEVICEs.SingleOrDefault(n => n.MaDevice == Masach);
            this.sTensach = sach.TenDevice;
            this.sAnhbia = sach.AnhBia;
            this.dDongia = double.Parse(sach.GiaBan.ToString());
            this.iSoluong = 1;
            
            this.chitiet = db.Sheet1_s.SingleOrDefault(n =>n.MaChiTiet == Machitiet);
        }
    }
}