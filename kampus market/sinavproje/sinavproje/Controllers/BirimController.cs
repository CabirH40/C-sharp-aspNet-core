using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;

namespace sinavproje.Controllers
{
    public class BirimController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();

        // GET: Birim
        public ActionResult Index()
        {
           
                return View();
          
        }

        public ActionResult birimInsert(dbbirim model)
        {
            dbbirim birim = new dbbirim();
            birim.birim = model.birim;
            birim.aciklama = model.aciklama;

            db.dbbirims.Add(birim);
            db.SaveChanges();

            return RedirectToAction("birimList");
        }

        public ActionResult birimList()
        {
            var birimList = db.dbbirims.ToList();
            return View(birimList);
        }

        [HttpGet]
        public ActionResult birimUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbbirims.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult birimUpdate(dbbirim model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("birimList");
                }
                catch (DbEntityValidationException ex)
                {
                    // يمكن استخدام الكود التالي لعرض الأخطاء التي تم اكتشافها أثناء التحقق من صحة الكيان
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    // يمكنك التعامل مع الأخطاء هنا، مثل إرسالها إلى العرض لعرضها للمستخدم أو تسجيلها لمزيد من التحقق.
                }
            }

            return View(model);
        }


        public ActionResult birimDelete(int id)
        {
            var kDel = db.dbbirims.Where(x => x.Id == id).First();
            db.dbbirims.Remove(kDel);
            db.SaveChanges();

            var list = db.dbbirims.ToList();
            return View("birimList", list);
        }
    }
}
