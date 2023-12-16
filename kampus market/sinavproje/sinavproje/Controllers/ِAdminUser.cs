using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;
using sinavproje.Models;


namespace sinavproje.Controllers
{
    public class AdminUserController : Controller
    {
        stokTakip02Entities db = new stokTakip02Entities();

        // GET: Default
        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null)
            {
                // المستخدم لم يقم بتسجيل الدخول، يتم توجيهه إلى صفحة تسجيل الدخول
                return RedirectToAction("adminLogin");
            }
            else
            {
                // استخراج اسم المستخدم من قاعدة البيانات
                string loggedInUser = Session["LoggedIn"].ToString();
                var user = db.dbusers.FirstOrDefault(u => u.userName == loggedInUser);

                if (user != null)
                {
                    string username = user.userName;
                    ViewBag.Username = username;
                }
            }

            // يمكنك تنفيذ الإجراء المطلوب عند تسجيل الدخول الناجح هنا

            return View();
        }

        [HttpGet]
        public ActionResult adminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult adminLogin(dbuser model)
        {
            if (ModelState.IsValid)
            {
                // التحقق من صحة معلومات تسجيل الدخول
                var user = db.dbusers.FirstOrDefault(u => u.userName == model.userName && u.password == model.password);
                if (user != null)
                {
                    // تم تسجيل الدخول بنجاح
                    // يتم تعيين حالة الجلسة للإشارة إلى تسجيل الدخول الناجح
                    Session["LoggedIn"] = model.userName;

                    // يمكنك تنفيذ الإجراء المطلوب (على سبيل المثال، تحويل المستخدم إلى صفحة الرئيسية)

                    // استخراج اسم المستخدم
                    var userName = model.userName;

                    // تعيين القيمة في ViewBag
                   

                    return RedirectToAction("AuserList");
                }
                else
                {
                    ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة.");
                }
            }

            return View(model);
        }

        public ActionResult userInsert(dbuser model)
        {
           
            {
                dbuser user = new dbuser()
                {
                    userName = model.userName,
                    userSurname = model.userSurname,
                    sehirId = model.sehirId,
                    ilceId = model.ilceId,
                    departmandId = model.departmandId,
                    maas = model.maas,
                    izin = model.izin,
                    password = model.password,
                    role = model.role
                };

                db.dbusers.Add(user);
                db.SaveChanges();
            }

            return RedirectToAction("AuserList");
        }
        public ActionResult AuserList()
        {
            var usrlist = db.dbusers.ToList();
            return View(usrlist);
        }
        [HttpGet]
        public ActionResult AuserUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbusers.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AuserUpdate(dbuser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AuserList");
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
        public ActionResult AuserDelete(int id)
        {
            var uDel = db.dbusers.FirstOrDefault(x => x.Id == id);
            if (uDel != null)
            {
                db.dbusers.Remove(uDel);
                db.SaveChanges();
            }

            var list = db.dbusers.ToList();
            return View("AuserList", list);

        }
        public ActionResult Logout()
        {
            // Clear the session and redirect the user to the login page
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("adminLogin");
        }
    }
}