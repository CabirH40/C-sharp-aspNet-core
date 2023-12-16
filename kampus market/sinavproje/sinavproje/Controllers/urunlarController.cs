using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sinavproje.models;
using System.IO;
using System.Data.Entity;
using System.Net;
using System.Data.SqlClient;

namespace sinavproje.Controllers
{ public class urunlarController : Controller
    { stokTakip02Entities db = new stokTakip02Entities();
        
        public ActionResult Index()
        {
            List<SelectListItem> kategoriler = (
                from i in db.dbkategoris.ToList()
                select new SelectListItem
                {
                    Text = i.kategoriAdi,
                    Value = i.Id.ToString()
                }).ToList();

            List<SelectListItem> markalar = (
                from i in db.dbmarkas.ToList()
                select new SelectListItem
                {
                    Text = i.markaAdi,
                    Value = i.Id.ToString()
                }).ToList();

            List<SelectListItem> birimler = (
                from i in db.dbbirims.ToList()
                select new SelectListItem
                {
                    Text = i.birim,
                    Value = i.Id.ToString()
                }).ToList();

            ViewBag.kategoriler = kategoriler;
            ViewBag.markalar = markalar;
            ViewBag.birimler = birimler;

            return View();
        }
        public ActionResult urunList()
        {
            List<dburunler> urunT = db.dburunlers.ToList();
            List<dbmarka> markaT = db.dbmarkas.ToList();
            List<dbkategori> kategoriT = db.dbkategoris.ToList();
            List<dbbirim> birimT = db.dbbirims.ToList();

            var multipleTables = from ut in urunT
                                 join mt in markaT on ut.markaId equals mt.Id into table1
                                 from mt in table1.DefaultIfEmpty()
                                 join kt in kategoriT on ut.kategoriId equals kt.Id into table2
                                 from kt in table2.DefaultIfEmpty()
                                 join bt in birimT on ut.birimId equals bt.Id into table3
                                 from bt in table3.DefaultIfEmpty()
                                 select new modelmarkakategori
                                 {
                                     uruntables = ut,
                                     markatables = mt,
                                     kategoritables = kt,
                                     birimtables = bt
                                 };

            return View(multipleTables.ToList());
        }
        public ActionResult urunInsert(dburunler model, HttpPostedFileBase file)
        {
            dburunler urun= new dburunler();
            urun.markaId = model.markaId;
            urun.kategoriId = model.kategoriId;
            urun.birimId = model.birimId;
            urun.urunId = model.urunId;
            urun.barkodNo = model.barkodNo;
            urun.alisFiyati = model.alisFiyati;
            urun.satsFiyati = model.satsFiyati;
            urun.KDV = model.KDV;
            urun.uretimTarih = model.uretimTarih;
            urun.tarih = model.tarih;
            urun.TTE = model.TTE;
            urun.urunAdi = model.urunAdi;

            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/webPages/01/assets/imgurun/" + ImageName);
                file.SaveAs(physicalPath);
                urun.img2 = ImageName;
            }
            db.dburunlers.Add(urun);
            db.SaveChanges();

            return RedirectToAction("urunList");
        }
        [HttpGet]
        public ActionResult urunUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dburunlers.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> kategoriler = (
                from i in db.dbkategoris.ToList()
                select new SelectListItem
                {
                    Text = i.kategoriAdi,
                    Value = i.Id.ToString()
                }
            ).ToList();

            List<SelectListItem> markalar = (
                from i in db.dbmarkas.ToList()
                select new SelectListItem
                {
                    Text = i.markaAdi,
                    Value = i.Id.ToString()
                }
            ).ToList();

            List<SelectListItem> birimler = (
                from i in db.dbbirims.ToList()
                select new SelectListItem
                {
                    Text = i.birim,
                    Value = i.Id.ToString()
                }
            ).ToList();

            ViewBag.kategoriler = kategoriler;
            ViewBag.markalar = markalar;
            ViewBag.birimler = birimler;

            return View(model);
        }
        [HttpPost]
        public ActionResult urunUpdate(dburunler model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("urunList");
            }
            // If ModelState is not valid, redisplay the view with the existing model and dropdown lists
            List<SelectListItem> kategoriler = (
                from i in db.dbkategoris.ToList()
                select new SelectListItem
                {
                    Text = i.kategoriAdi,
                    Value = i.Id.ToString()
                }
            ).ToList();

            List<SelectListItem> markalar = (
                from i in db.dbmarkas.ToList()
                select new SelectListItem
                {
                    Text = i.markaAdi,
                    Value = i.Id.ToString()
                }
            ).ToList();

            List<SelectListItem> birimler = (
                from i in db.dbbirims.ToList()
                select new SelectListItem
                {
                    Text = i.birim,
                    Value = i.Id.ToString()
                }
            ).ToList();

            ViewBag.kategoriler = kategoriler;
            ViewBag.markalar = markalar;
            ViewBag.birimler = birimler;

            return View(model);
        }
        public ActionResult urunDelete(int id)
        {
            var urunDel = db.dburunlers.Find(id);
            if (urunDel != null)
            {
                db.dburunlers.Remove(urunDel);
                db.SaveChanges();
            }

            var urunList = db.dburunlers.ToList();
            var model = urunList.Select(urun => new modelmarkakategori
            {
                uruntables = urun,
                kategoritables = db.dbkategoris.FirstOrDefault(kt => kt.Id == urun.kategoriId),
                markatables = db.dbmarkas.FirstOrDefault(mt => mt.Id == urun.markaId),
                birimtables = db.dbbirims.FirstOrDefault(bt => bt.Id == urun.birimId)
            }).ToList();

            return View("urunList", model.AsEnumerable());
        }
        public ActionResult product(string search)
        {
            List<dbmarka> markaT = db.dbmarkas.ToList();
            List<dbkategori> kategoriT = db.dbkategoris.ToList();
            List<dburunler> uruniT = db.dburunlers.ToList();
            List<dbbirim> birimT = db.dbbirims.ToList();

            var multipleTables = from ut in uruniT
                                 join kt in kategoriT on ut.kategoriId equals kt.Id into table1
                                 from kt in table1.DefaultIfEmpty()
                                 join mt in markaT on ut.markaId equals mt.Id into table2
                                 from mt in table2.DefaultIfEmpty()
                                 join bt in birimT on ut.birimId equals bt.Id into table3
                                 from bt in table3.DefaultIfEmpty()
                                 select new modelmarkakategori
                                 {
                                     markatables = mt,
                                     kategoritables = kt,
                                     uruntables = ut,
                                     birimtables = bt
                                 };

            if (!string.IsNullOrEmpty(search))
            {
                multipleTables = multipleTables.Where(ut => ut.uruntables.urunAdi.Contains(search) || ut.markatables.markaAdi.Contains(search) || ut.kategoritables.kategoriAdi.Contains(search));
            }

            return View(multipleTables.ToList());
        }

        public ActionResult single_product(int? id)
        {
            if (id == null)
            {
                // Handle case when id is not provided
                return RedirectToAction("product");
            }

            dburunler selectedurun = db.dburunlers.FirstOrDefault(ut => ut.Id == id);
            if (selectedurun == null)
            {
                // Handle case when the selected product is not found
                return RedirectToAction("product");
            }

            dbkategori selectedKategori = db.dbkategoris.FirstOrDefault(kt => kt.Id == selectedurun.kategoriId);
            dbmarka selectedmarka = db.dbmarkas.FirstOrDefault(mt => mt.Id == selectedurun.markaId);
            dbbirim selectedbirim = db.dbbirims.FirstOrDefault(bt => bt.Id == selectedurun.birimId);
            var model = new sinavproje.models.modelmarkakategori()
            {
                markatables = selectedmarka,
                kategoritables = selectedKategori,
                uruntables = selectedurun,
                birimtables= selectedbirim
            };
            return View("single_product", new List<modelmarkakategori> { model });


        }
        public ActionResult sepetinsert(int id)
        {
            var urun = db.dburunlers.Find(id);
            
            if (urun != null)
            {
                string urunkatidstr = urun.kategoriId.ToString();
                int urunkatid = int.Parse(urunkatidstr);

                var s = new dbsepet
                {
                    kategoriId = urunkatid,
                    urunId = urun.Id,
                    tarih = DateTime.Now,
                    saat = DateTime.Now,
                    miktar = 1
                };

                s.birimfiyat = urun.satsFiyati;
                s.toplamfiyat = s.birimfiyat;

                db.dbsepets.Add(s); // Add the new item to the DbSet
                db.SaveChanges(); // Save changes to the database

                Session["CartCount"] = ((int?)Session["CartCount"] ?? 0) + 1;
            }

            return RedirectToAction("Single_Product", "urunlar");
        }
        public ActionResult hemenlal(int id)
        {
            var urun = db.dburunlers.Find(id);
            if (urun != null)
            {
                string urunkatidstr = urun.kategoriId.ToString();
                int urunkatid = int.Parse(urunkatidstr);

                var s = new dbsepet
                {
                    kategoriId = urunkatid,
                    urunId = urun.Id,
                    tarih = DateTime.Now,
                    saat = DateTime.Now,
                    miktar = 1
                };

                s.birimfiyat = urun.satsFiyati;
                s.toplamfiyat = s.birimfiyat;

                db.dbsepets.Add(s); // Add the new item to the DbSet
                db.SaveChanges(); // Save changes to the database

                Session["CartCount"] = ((int?)Session["CartCount"] ?? 0) + 1;
            }

            return RedirectToAction("sepetMusteri", "sepet");
        }
        public ActionResult payment()
        {
            return View();
        }




    }
}