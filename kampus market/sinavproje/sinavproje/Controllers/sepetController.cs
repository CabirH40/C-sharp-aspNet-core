using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;
using sinavproje.models;

namespace sinavproje.Controllers
{
    public class sepetController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();
        // GET: sepet
        public ActionResult Index()
        {
            List<SelectListItem> kategoriler = (
                  from i in db.dbkategoris.ToList()
                  select new SelectListItem
                  {
                      Text = i.kategoriAdi,
                      Value = i.Id.ToString()
                  }).ToList();

            List<SelectListItem> musteri = (
                from i in db.dbmusteris.ToList()
                select new SelectListItem
                {
                    Text = i.musteriAdi,
                    Value = i.Id.ToString()
                }).ToList();

            List<SelectListItem> satislar = (
                from i in db.dbSatislars.ToList()
                select new SelectListItem
                {
                    Text = i.Id.ToString(),
                    Value = i.Id.ToString()
                }).ToList();
            List<SelectListItem> urunler = (
               from i in db.dburunlers.ToList()
               select new SelectListItem
               {
                   Text = i.urunId.ToString(),
                   Value = i.Id.ToString()
               }).ToList();

            ViewBag.kategoriler = kategoriler;
            ViewBag.musteri = musteri;
            ViewBag.satislar = satislar;
            ViewBag.urunler = urunler;
            return View();
            
        }
      
        public ActionResult sepetList()
        {
            List<dbsepet> sepetler = db.dbsepets.ToList();
            List<dbkategori> kategoriler = db.dbkategoris.ToList();
            List<dbmusteri> musteriler = db.dbmusteris.ToList();
            List<dburunler> urunler = db.dburunlers.ToList();
            
            var multipleTables = from sepet in sepetler
                                 join kategori in kategoriler on sepet.kategoriId equals kategori.Id into table1
                                 from kategori in table1.DefaultIfEmpty()
                                 join musteri in musteriler on sepet.musteriId equals musteri.Id into table2
                                 from musteri in table2.DefaultIfEmpty()
                                 join urun in urunler on sepet.urunId equals urun.Id into table3
                                 from urun in table3.DefaultIfEmpty()
                                 select new modelmarkakategori
                                 {
                                     sepettables = sepet,
                                     kategoritables = kategori,
                                     musteritables = musteri,
                                     uruntables = urun
                                 };

            return View(multipleTables);
        }
        public ActionResult sepetDelete(int id)
        {
            var sepetDel = db.dbsepets.Find(id);
            if (sepetDel != null)
            {
                // حذف السجلات المرتبطة في جدول dbSatislar أولاً
                var satislar = db.dbSatislars.Where(s => s.sepetId == id).ToList();
                db.dbSatislars.RemoveRange(satislar);

                // ثم حذف سلة (dbsepets)
                db.dbsepets.Remove(sepetDel);
                db.SaveChanges();

                // تحديث العداد
                int cartCount = Convert.ToInt32(Session["CartCount"] ?? 0);
                cartCount--;
                Session["CartCount"] = cartCount;
            }

            var sepetList = db.dbsepets.ToList();
            var model = sepetList.Select(sepet => new modelmarkakategori
            {
                sepettables = sepet,
                kategoritables = db.dbkategoris.FirstOrDefault(kategori => kategori.Id == sepet.kategoriId),
                musteritables = db.dbmusteris.FirstOrDefault(musteri => musteri.Id == sepet.musteriId),
                uruntables = db.dburunlers.FirstOrDefault(urun => urun.Id == sepet.urunId)
            }).ToList();

            return View("sepetMusteri", model);
        }
        public ActionResult sepetMusteri()
        {
           
            List<dbkategori> kategoriT = db.dbkategoris.ToList();
            List<dburunler> uruniT = db.dburunlers.ToList();
            List<dbmusteri> musteriT = db.dbmusteris.ToList();
            List<dbsepet> sepetT = db.dbsepets.ToList();
            var multipleTables = from st in sepetT
                                 join kt in kategoriT on st.kategoriId equals kt.Id into table1
                                 from kt in table1.DefaultIfEmpty()
                                 join mt in musteriT on st.musteriId equals mt.Id into table2
                                 from mt in table2.DefaultIfEmpty()
                                 join ut in uruniT on st.urunId equals ut.Id into table3
                                 from ut in table3.DefaultIfEmpty()
                                 
                                 select new modelmarkakategori
                                 {
                                     sepettables = st,
                                     musteritables = mt,
                                     kategoritables = kt,
                                     uruntables = ut
                                 };
            return View(multipleTables);
        }
        public ActionResult Azalt(int id)
        {
            var model = db.dbsepets.Find(id);
            if(model.miktar == 1)
            {
                db.dbsepets.Remove(model);
                db.SaveChanges();
            }
            model.miktar--;
            model.toplamfiyat = model.birimfiyat * model.miktar;
            db.SaveChanges();
            return RedirectToAction("sepetMusteri");
        }
        public ActionResult Artir(int id)
        {
            var model = db.dbsepets.Find(id);
            if (model != null)
            {
                model.miktar++;
                model.toplamfiyat = model.birimfiyat * model.miktar;
                db.SaveChanges();
            }
            return RedirectToAction("sepetMusteri");
        }
        public ActionResult AddToCart(int productId)
        {
            // Logic to add the product to the cart...

            // تحديث قيمة العداد في ملف تعريف الارتباط (Cookies)
            int cartCount = Convert.ToInt32(Request.Cookies["CartCount"]?.Value ?? "0");
            cartCount++;
            Response.Cookies["CartCount"].Value = cartCount.ToString();
            Response.Cookies["CartCount"].Expires = DateTime.Now.AddDays(1);

            // Redirect to the cart or desired page
            return RedirectToAction("Cart");
        }
    }
}