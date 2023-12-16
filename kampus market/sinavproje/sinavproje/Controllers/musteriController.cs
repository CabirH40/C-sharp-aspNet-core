
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

namespace sinavproje.Controllers
{
    public class MusteriController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MusteriInsert(dbmusteri model, HttpPostedFileBase file)
        {
            dbmusteri musteri = new dbmusteri();
            musteri.musteriAdi = model.musteriAdi;
            musteri.telefon = model.telefon;
            musteri.email = model.email;
            
            musteri.kayitTarihi = model.kayitTarihi;
            musteri.puan = model.puan;
            musteri.aciklama = model.aciklama;
            musteri.adres = model.adres;
            musteri.sehirId = model.sehirId;
            musteri.ilceId = model.ilceId;
            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/webPages/01/assets/imgmusteri/" + ImageName);
                file.SaveAs(physicalPath);
                musteri.resim = ImageName;
               
            }


            db.dbmusteris.Add(musteri);
            db.SaveChanges();

            return RedirectToAction("musteriList");
        }

        [HttpGet]
        public ActionResult musteriUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbmusteris.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult musteriUpdate(dbmusteri model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("musteriList");
            }

            return View(model);
        }


        public ActionResult musteriDelete(int id)
        {
            var kDel = db.dbmusteris.FirstOrDefault(x => x.Id == id);
            if (kDel != null)
            {
                db.dbmusteris.Remove(kDel);
                db.SaveChanges();
            }

            var list = db.dbmusteris.ToList();
            return View("musteriList", list);
        }

        public ActionResult musteriList()
        {
            var musteriList = db.dbmusteris.ToList();
            return View(musteriList);
        }
    }
}