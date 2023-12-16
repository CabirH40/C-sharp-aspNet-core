using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sinavproje.Controllers
{
    public class kategoriController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();
        // GET: kategori
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KategoriInsert(dbkategori model)
        {
            dbkategori katagori = new dbkategori();
            katagori.kategoriAdi = model.kategoriAdi;
            katagori.aciklama = model.aciklama;


            db.dbkategoris.Add(katagori);
            db.SaveChanges();//kaiyt işlemi gerekli kod satıtı
            return RedirectToAction("kategoriList");
        } 
    public ActionResult KategoriList()
        {
            var katlist = db.dbkategoris.ToList();
            return View(katlist);
        }
    [HttpGet]
    public ActionResult kategoriUpdate(int? id)
        {
            var model = db.dbkategoris.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult kategoriUpdate(dbkategori model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("KategoriList");
        }
        public ActionResult kategoriDelete(int id)
        {
            var kDel = db.dbkategoris.Where(x => x.Id == id).First();
            db.dbkategoris.Remove(kDel);
            db.SaveChanges();

            var list = db.dbkategoris.ToList();
            return View("KategoriList",list);
        }



    }}