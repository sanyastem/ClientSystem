using DataBase;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace ClientSystem.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var user = Guid.Parse(User.Identity.GetUserId());
                return View(db.Clients.Where(x=>x.IdManager == user).ToList());
            }
           
        }
        public ActionResult Category()
        {
            using (var db =new ApplicationDbContext())
            {
                return View(db.CategoryClients.ToList());
            }
            
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(CategoryClient cat)
        {
            using (var db = new ApplicationDbContext())
            {

                db.CategoryClients.Add(cat);
                db.SaveChanges();
            }
            return RedirectToAction("Category");
        }
        public ActionResult RemoveCategory(int? id)
        {
            try
            {

           
            using (var db = new ApplicationDbContext())
            {
                db.CategoryClients.Remove(db.CategoryClients.Find(id));
                db.SaveChanges();
            }
            return RedirectToAction("Category");
            }
            catch (Exception)
            {

                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.CategoryClients, "Id", "Name");
            ViewBag.CatSer = Category;
            return View(db.CategoryClients.Find(id));
            
        }
        [HttpPost]
        public ActionResult EditCategory(CategoryClient cat)
        {
            using (var db = new ApplicationDbContext())
            {
                var c = db.CategoryClients.Find(cat.Id);
                c.Name = cat.Name;
                db.SaveChanges();
            }
            return RedirectToAction("Category");
        }
        [HttpGet]
        public ActionResult ViewClient(int? id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    return View(db.Clients.Include(x=>x.CategoryClient).FirstOrDefault(x=>x.Id == id));
                }
            }
            catch (Exception)
            {

                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.CategoryClients, "Id", "Name");
            ViewBag.CatSer = Category;
            try
            {
                
                    return View(db.Clients.Find(id));
                
            }
            catch (Exception)
            {

                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Edit(Client client)
        {
            var db = new ApplicationDbContext();

            client.IdManager = Guid.Parse(User.Identity.GetUserId());    
            db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Add()
        {
            var db = new ApplicationDbContext();
            SelectList Category = new SelectList(db.CategoryClients, "Id", "Name");
            ViewBag.CatSer = Category;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Client client)
        {
            using (var db = new ApplicationDbContext())
            {
                client.IdManager = Guid.Parse(User.Identity.GetUserId());
                db.Clients.Add(client);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Remove(int? id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Clients.Remove(db.Clients.Find(id));
                    db.SaveChanges();

                }
            }
            
            
            catch (Exception)
            {

                HttpNotFound();
            }
            return RedirectToAction("Index");
        }

    }
}