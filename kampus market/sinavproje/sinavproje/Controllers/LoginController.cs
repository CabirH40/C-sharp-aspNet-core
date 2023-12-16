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
    public class LoginController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult loginInsert(login model)
        {
            login hesap = new login();
            hesap.name = model.name;
            hesap.username = model.username;
            hesap.email = model.email;
            hesap.password = model.password;
            db.logins.Add(hesap);
            db.SaveChanges();

            return RedirectToAction("luserList");
        }
        public ActionResult login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult login(login model1, string returnUrl,dbsepet model2)
        {
            if (ModelState.IsValid)
            {
                // التحقق من صحة معلومات تسجيل الدخول
                var user = db.logins.FirstOrDefault(u => u.username == model1.username && u.password == model1.password);
                if (user != null)
                {
                    // تم تسجيل الدخول بنجاح
                    Session["UserName"] = user.username;
                    if (user.musteriId.HasValue)
                    {
                        model2.musteriId = user.musteriId.Value;
                    }
                    // يمكنك تنفيذ الإجراء المطلوب (على سبيل المثال، تحويل المستخدم إلى صفحة الرئيسية)
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("sepetMusteri", "sepet");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة.");
                }
            }

            return View(model1);
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(login model)
        {
            if (ModelState.IsValid)
            {
                // إنشاء حساب جديد
                db.logins.Add(model);
                db.SaveChanges();

                // تم إنشاء الحساب بنجاح
                // يمكنك تنفيذ الإجراء المطلوب (على سبيل المثال، تحويل المستخدم إلى صفحة تسجيل الدخول)
                return RedirectToAction("login");
            }

            return View(model);
        }

        public ActionResult AuserDelete(int id)
        {
            var UDel = db.logins.FirstOrDefault(x => x.id == id);
            if (UDel != null)
            {
                db.logins.Remove(UDel);
                db.SaveChanges();
            }

            var list = db.logins.ToList();
            return View("luserList", list);

        }


        public ActionResult luserList()
        {
            var lusrlist = db.logins.ToList();
            return View(lusrlist);
        }
        [HttpGet]
        public ActionResult loginUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.logins.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult loginUpdate(login model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("luserList");
            }

            return View(model);
        }
        public ActionResult Logout()
        {
           
            Session.Clear();
            Session.Abandon();

            Session.Remove("username");
            Session["cartItems"] = null;

            return RedirectToAction("login");
        }

    }
}
