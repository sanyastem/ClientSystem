using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace ClientSystem.Controllers
{
    [Authorize]
    public class EntryController : Controller
    {
        // GET: Entry
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.Recordings.ToList());
            }
        }
        public JsonResult json()
        {
            var db = new ApplicationDbContext();
            var obj = from a in db.Recordings.ToList()
                      select new { id = a.Id,
                          title = "Клиент " + db.Clients.Include(x => x.Recordings).FirstOrDefault().FIO + ",на услугу " + db.Services.Include(x => x.Recording).FirstOrDefault().Name,
                          start = a.Times.Year + "-" + a.Times.Month + "-" + a.Times.Day,
                          allDay = false,
                          description = "Клиент :" + db.Clients.Include(x => x.Recordings).FirstOrDefault().FIO + ",на услугу" + db.Services.Include(x => x.Recording).FirstOrDefault().Name + " и на сумму " + a.Sum
                      };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RecDay()
        {
            var db = new ApplicationDbContext();
            var obj = from a in db.Recordings.ToList()
                      where a.Times.Day == DateTime.Now.Day && a.Times.Month == DateTime.Now.Month && a.Times.Year == DateTime.Now.Year
                      select new
                      {
                          id = a.Id,
                          title = "Клиент " + db.Clients.Include(x => x.Recordings).FirstOrDefault().FIO + ",на услугу " + db.Services.Include(x => x.Recording).FirstOrDefault().Name,
                          allDay = false,
                          description = "Клиент :" + db.Clients.Include(x => x.Recordings).FirstOrDefault().FIO + ",на услугу" + db.Services.Include(x => x.Recording).FirstOrDefault().Name + " и на сумму " + a.Sum
                      };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewList()
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.Clients, "Id", "FIO");
            ViewBag.CatSer = Category;
            var user = Guid.Parse(User.Identity.GetUserId());
            return View(db.Recordings.Include(x=>x.Clients).Where(x=>x.IdManager == user).ToList());
        }
        [HttpGet]
        public ActionResult EntryInc()
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.Clients, "Id", "FIO");
            ViewBag.CatSer = Category;
            SelectList svm = new SelectList(db.Services,"Id","Name");
            ViewBag.Serv = svm;
            return View();
        }
        [HttpPost]
        public ActionResult EntryInc(Recording rec,string date,string time)
        {
            
            var da = date.Split('-');
            var t = time.Split(':');
            var d = new DateTimeOffset(int.Parse(da[0]), int.Parse(da[1]), int.Parse(da[2]), int.Parse(t[0]), int.Parse(t[1]),00,TimeSpan.Zero).DateTime;
            using (var db = new ApplicationDbContext())
            {
                rec.IdManager = Guid.Parse(User.Identity.GetUserId());
                rec.Times = d;
                rec.Sum = db.Services.Find(rec.IdServices).Price;
                db.Recordings.Add(rec);
                db.SaveChanges();
            }
            return RedirectToAction("ViewList");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
                var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.Clients, "Id", "FIO");
            ViewBag.CatSer = Category;
            SelectList svm = new SelectList(db.Services, "Id", "Name");
            ViewBag.Serv = svm;
            return View(db.Recordings.Find(id));
            
        }
        public ActionResult Views(int? id)
        {
            var db = new ApplicationDbContext();
            return View(db.Recordings.Include(x=>x.Clients).Include(a=>a.Service).Where(b=>b.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(Recording rec)
        {
            using (var db = new ApplicationDbContext())
            {
                rec.IdManager = Guid.Parse(User.Identity.GetUserId());
                rec.Sum = db.Services.Where(x => x.Id == rec.IdServices).FirstOrDefault().Price;
                db.Entry(rec).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ViewList");
        }
        public ActionResult Remove(int? id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Recordings.Remove(db.Recordings.Find(id));
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                HttpNotFound();
            }
            
            return RedirectToAction("ViewList");
        }
        public JsonResult TimeView(string date, string date1, string date2)
        {
            if (date1[0] == '0')
            {
                date1 = date1.TrimStart('0');
            }
            using (var db = new ApplicationDbContext())
            {
                List<Recording> reci = new List<Recording>();
                var recs = db.Recordings.ToList();
                foreach (var item in recs)
                {
                    if (item.Times.Year.ToString() == date && item.Times.Month.ToString() == date1 && item.Times.Day.ToString() == date2)
                    {
                        reci.Add(item);
                    }
                }
                var js = from s in reci
                         select new
                         {
                             dataT = s.Times.TimeOfDay.ToString()
                         };
                return Json(js,JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}