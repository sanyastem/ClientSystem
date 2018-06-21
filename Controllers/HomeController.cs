using DataBase;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ClientSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Static()
        {
            return View();
        }
        public JsonResult json()
        {
            var db = new ApplicationDbContext();
            var user = User.Identity.GetUserId();
            var json = from js in db.Recordings.ToList()
                       where js.IdManager.ToString() == user && js.Times.Month == DateTime.Now.Month && js.Times.Year == DateTime.Now.Year
                       orderby js.Times
                       select new
                       {
                           data1 = js.Times.Day,
                           data2 = db.Recordings.Where(x => x.IdManager == js.IdManager && js.Times.Month == DateTime.Now.Month && js.Times.Year == DateTime.Now.Year && x.Times.Day == js.Times.Day).Count()
                       };
            
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JsonManager()
        {
            List<string> col = new List<string>()
            {
                "#dd3ec6",
                "#9c4f65",
                "#f020bf",
                "#86ecb0",
                "#f62536",
                "#86c662",
                "#be16dd",
                "#8090a4",
                "#9fbd50",
                "#869660",
                "#8914c0",
                "#ae53fc",
                "#644343",
                "#883d4f",
                "#b4fcfb",
                "#8a5a4e",
                "#6b614b",
                "#3cf677",
                "#2ebcef",
                "#ddd97f",
                "#dc69de",
                "#f9dfd7",
                "#eb9e5f",
                "#533ff0",
                "#bc9485",
                "#ee1f58",
                "#e760e4",
                "#7ac249",
                "#407100",
                "#742b1a",
            };
            var t = col.Count;
            var i = 0;
            var db = new ApplicationDbContext();
            var user = User.Identity.GetUserId();
            var json = from js in db.Recordings.ToList()
                       where i<t
                       orderby js.Times
                       select new
                       {
                           label = db.Users.Where(x => x.Id == js.IdManager.ToString()).First().UserName,
                           data = db.Recordings.Where(x => x.IdManager == js.IdManager).Count(),
                           color = col[i++]
                       };

            return Json(json.GroupBy(x => x.label).Select(y => y.First()), JsonRequestBehavior.AllowGet);
        }
    }
}