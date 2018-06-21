using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace ClientSystem.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.Services.ToList());
            }
        }
        [HttpGet]
        public ActionResult AddService()
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.CategoryOfServices, "Id", "Name");
            ViewBag.CatSer = Category;
            return View();
        }
        [HttpPost]
        public ActionResult AddService(Services serv)
        {
            using (var db = new ApplicationDbContext())
            {
                
                db.Services.Add(serv);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult ViewsService(int? id)
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.Services.Include(x=>x.CategoryOfService).FirstOrDefault(x=>x.Id == id));
            }
            
        }
        public ActionResult RemoveService(int? id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Services.Remove(db.Services.Find(id));
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                ViewBag.Mes = "Нельзя удалить услугу , клиенты записаны на данную услугу.";
                throw;
            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditService(int? id)
        {
            var db = new ApplicationDbContext();
            
                SelectList Category = new SelectList(db.CategoryOfServices, "Id", "Name");
                ViewBag.CatSer = Category;
                return View(db.Services.Find(id));
            
        }
        [HttpPost]
        public ActionResult EditService(Services serv)
        {
            var db = new ApplicationDbContext();
                db.Entry(serv).State = EntityState.Modified;
                db.SaveChanges();
            
            return RedirectToAction("Index");
        }
        public ActionResult AllCategory()
        {
            using (var db = new ApplicationDbContext())
            {
                return View(db.CategoryOfServices.ToList());
            }
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            var db = new ApplicationDbContext();
            
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(CategoryOfServices cat)
        {
            using (var db = new ApplicationDbContext())
            {
                db.CategoryOfServices.Add(cat);
                db.SaveChanges();
            }
            return RedirectToAction("AllCategory");
        }
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            using (var db = new ApplicationDbContext())
            {
                
                return View(db.CategoryOfServices.Find(id));
            }
        }
        public ActionResult RemoveCategory(int? id)
        {
            using (var db = new ApplicationDbContext())
            {
                db.CategoryOfServices.Remove(db.CategoryOfServices.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("AllCategory");
        }
        [HttpPost]
        public ActionResult EditCategory(CategoryOfServices cat)
        {
            using (var db = new ApplicationDbContext())
            {
                var ct = db.CategoryOfServices.Find(cat.Id);
                ct.Name = cat.Name;
                db.SaveChanges();
            }
            return RedirectToAction("AllCategory");
        }
    }
}