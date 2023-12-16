using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sinavproje.models;
using System.Net;
using System.Data.Entity;

namespace sinavproje.Controllers
{
    public class SatislarController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();

        public ActionResult Index()
        {
            List<SelectListItem> sepet = (
                from i in db.dbsepets.ToList()
                select new SelectListItem
                {
                    Text = i.Id.ToString(),
                    Value = i.Id.ToString()
                }).ToList();

            ViewBag.sepet = sepet;

            List<SelectListItem> urunler = (
                from i in db.dburunlers.ToList()
                select new SelectListItem
                {
                    Text = i.Id.ToString(),
                    Value = i.Id.ToString()
                }).ToList();

            ViewBag.urunler = urunler;

            List<SelectListItem> satislar = (
                from i in db.dbSatislars.ToList()
                select new SelectListItem
                {
                    Text = i.sepetId.ToString(),
                    Value = i.sepetId.ToString()
                }).ToList();

            ViewBag.sepetler = satislar;

            return View();
        }

        [HttpPost]
        public ActionResult SatisInsertAll(int[] sepetId)
        {
            if (Session["UserName"] != null)
            {
                decimal totalPrice = 0;

                foreach (var id in sepetId)
                {
                    var sepet = db.dbsepets.Find(id);
                    if (sepet != null)
                    {
                        string sepeturunidstr = sepet.urunId.ToString();
                        int sepetkatid = int.Parse(sepeturunidstr);

                        var s = new dbSatislar
                        {
                            urunId = sepet.urunId,
                            sepetId = sepet.Id,
                           
                            tarih = sepet.tarih,
                            saat = sepet.saat,
                            toplamFiyat = sepet.toplamfiyat
                        };
                        s.musteriId = 14;
                        db.dbSatislars.Add(s);
                        totalPrice += (decimal)sepet.toplamfiyat;

                        db.dbsepets.Remove(sepet);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("sepetMusteri", "sepet");
            }

            // If user is not logged in, handle the appropriate action
            return RedirectToAction("Login", "Login");
        }

        public ActionResult SatisList()
        {
            List<dbSatislar> satislar = db.dbSatislars.ToList();
            List<dburunler> urunler = db.dburunlers.ToList();
            List<dbsepet> sepetler = db.dbsepets.ToList();

            var multipleTables = from satis in satislar
                                 join urun in urunler on satis.urunId equals urun.Id into table1
                                 from urun in table1.DefaultIfEmpty()
                                 join sepet in sepetler on satis.sepetId equals sepet.Id into table2
                                 from sepet in table2.DefaultIfEmpty()

                                 select new modelmarkakategori
                                 {
                                     satistables = satis,
                                     uruntables = urun,
                                     sepettables= sepet
                                     
                                 };

            return View(multipleTables);
        }

        public ActionResult SatisDelete(int id)
        {
            var satisDel = db.dbSatislars.Find(id);
            if (satisDel != null)
            {
                db.dbSatislars.Remove(satisDel);
                db.SaveChanges();
            }

            var satislar = db.dbSatislars.ToList();
            var urunler = db.dburunlers.ToList();
          
            var model = satislar.Select(satis => new modelmarkakategori
            {
                satistables = satis,
                uruntables = db.dburunlers.FirstOrDefault(urun => urun.Id == satis.urunId),
              
            }).ToList();

            return View("SatisList", model.AsEnumerable());
        }
        [HttpGet]
        public ActionResult SatisUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbSatislars.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // If ModelState is not valid, redisplay the view with the existing model and dropdown lists
            List<SelectListItem> urun = (
                from i in db.dburunlers.ToList()
                select new SelectListItem
                {
                    Text = i.urunId.ToString(),
                    Value = i.Id.ToString()
                }
            ).ToList();


            ViewBag.urun = urun;

            return View(model);
        }

        [HttpPost]
        public ActionResult SatisUpdate(dbSatislar model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SatisList");
            }

            return View(model);
        }
    }
}