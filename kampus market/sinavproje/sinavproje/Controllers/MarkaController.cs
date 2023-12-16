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
    public class markaController : Controller
    {stokTakip02Entities db = new stokTakip02Entities();
     // GET: Marka
     public ActionResult Index()
        {
     List<SelectListItem> kategoriler = (
       from i in db.dbkategoris.ToList()
       select new SelectListItem
        {Text = i.kategoriAdi,Value = i.Id.ToString()}).ToList();
            ViewBag.kategoriler = kategoriler;
            return View();
        }
        public ActionResult markaInsert(dbmarka model, HttpPostedFileBase file)
        {
            dbmarka marka = new dbmarka();
            marka.markaAdi = model.markaAdi;
            marka.kategoriId = model.kategoriId;
            marka.aciklama = model.aciklama;

            if (file != null && file.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/webPages/01/assets/imgMarka/" + ImageName);
                file.SaveAs(physicalPath);
                marka.img1 = ImageName;
            }

            db.dbmarkas.Add(marka);
            db.SaveChanges();

            return RedirectToAction("markaList");
        }
        [HttpGet]
        public ActionResult markaUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbmarkas.Find(id);
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

            ViewBag.kategoriler = kategoriler;

            return View(model);
        }
        [HttpPost]
        public ActionResult markaUpdate(dbmarka model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;

                if (file != null && file.ContentLength > 0)
                {
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/webPages/01/assets/imgMarka/" + ImageName);
                    file.SaveAs(physicalPath);
                    model.img1 = ImageName;
                }

                db.SaveChanges();
                return RedirectToAction("markaList");
            }

            List<SelectListItem> kategoriler = (
                from i in db.dbkategoris.ToList()
                select new SelectListItem
                {
                    Text = i.kategoriAdi,
                    Value = i.Id.ToString()
                }
            ).ToList();

            ViewBag.kategoriler = kategoriler;

            return View(model);
        }
        public ActionResult markaList()
        {
            List<dbmarka> markaT = db.dbmarkas.ToList();
            List<dbkategori> kategoriT = db.dbkategoris.ToList();

            var multipleTables = from mt in markaT
                                 join kt in kategoriT on mt.kategoriId equals kt.Id into table1
                                 from kt in table1.DefaultIfEmpty()
                                 select new modelmarkakategori
                                 {
                                     markatables = mt,
                                     kategoritables = kt,
                                    
                                 };

            return View(multipleTables); // Convert the result to a list before passing it to the view
        }
      
        public ActionResult markaDelete(int id)
        {
            var markaDel = db.dbmarkas.Find(id);
            if (markaDel != null)
            {
                db.dbmarkas.Remove(markaDel);
                db.SaveChanges();
            }

            var markaList = db.dbmarkas.ToList();
            var model = markaList.Select(marka => new modelmarkakategori
            {
                markatables = marka,
                kategoritables = db.dbkategoris.FirstOrDefault(kt => kt.Id == marka.kategoriId)
            }).ToList();

            return View("markaList", model.AsEnumerable());
        }
     
    }
}