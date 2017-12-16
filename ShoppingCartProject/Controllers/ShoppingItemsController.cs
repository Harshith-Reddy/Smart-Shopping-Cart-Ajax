using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCartProject.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ShoppingCartProject.Controllers
{
    [Authorize]
    public class ShoppingItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingItems
        public ActionResult Index()
        {
            var shoppingItems = db.ShoppingItems.Include(s => s.category);
            return View(shoppingItems.ToList());
        }

        public ActionResult getShoppingItemsList()
        {
            List<ShoppingItem> shoppingItems = db.ShoppingItems.OrderBy(x => x.DateAdded).ToList();

            var json = JsonConvert.SerializeObject(shoppingItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return Content(json);
        }

        // GET: ShoppingItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);
            if (shoppingItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingItem);
        }

        // GET: ShoppingItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: ShoppingItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemName,DateAdded,Price,Description,CategoryId")] ShoppingItem shoppingItem)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingItems.Add(shoppingItem);
                db.SaveChanges();

                if (Request.IsAjaxRequest())
                {
                    var items = db.ShoppingItems.Where(m => m.ItemName == shoppingItem.ItemName && m.CategoryId == shoppingItem.CategoryId && m.Description == shoppingItem.Description && m.Price==shoppingItem.Price).ToList();

                    var newiD = items[0].Id;                    
                    ShoppingItem addedSI = db.ShoppingItems.Find(newiD);
                    var jsonq = new JavaScriptSerializer().Serialize(addedSI);
                    //var jsonq = JsonConvert.SerializeObject(addedSI, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    return Content(jsonq);                    
                }
                else
                {
                    return RedirectToAction("Index");
                }                
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", shoppingItem.CategoryId);
            return View(shoppingItem);
        }

        // GET: ShoppingItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);
            if (shoppingItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", shoppingItem.CategoryId);
            return View(shoppingItem);
        }

        // POST: ShoppingItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,DateAdded,Price,Description,CategoryId")] ShoppingItem shoppingItem)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(shoppingItem).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
                var activityInDb = db.ShoppingItems.Find(shoppingItem.Id);
                // Activity does not exist in database and it's new one
                if (activityInDb == null)
                {
                    db.ShoppingItems.Add(shoppingItem);

                }
                else
                {
                    // Activity already exist in database and modify it
                    db.Entry(activityInDb).CurrentValues.SetValues(shoppingItem);
                    db.Entry(activityInDb).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (Request.IsAjaxRequest())
            {
                var siItems = db.ShoppingItems.Where(m => m.Id == shoppingItem.Id).ToList();
                var jsonq = JsonConvert.SerializeObject(siItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                return Content(jsonq);
                
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", shoppingItem.CategoryId);
            return View(shoppingItem);
        }

        // GET: ShoppingItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);
            if (shoppingItem == null)
            {
                return HttpNotFound();
            }
            return View(shoppingItem);
        }

        // POST: ShoppingItems/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);
            db.ShoppingItems.Remove(shoppingItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult SmartIndex()
        {            
            return View();
        }

        public ActionResult getCategories()
        {
            var siItems = db.Categories.ToList();            
            var json = JsonConvert.SerializeObject(siItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(json);            
        }

        public ActionResult getReviews(int id)
        {
            var toDoItems = db.ShoppingItems.Find(id);
            //var si = db.ShoppingItems.Find(id);
            var json = JsonConvert.SerializeObject(toDoItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            //var jsonSI = JsonConvert.SerializeObject(si, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            //json += jsonSI;
            return Content(json);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int n)
        {
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);

            if (shoppingItem == null)
            {
                return HttpNotFound();
            }
            //TODOGHR Quantity
            shoppingItem.updateQuantity(n);
            db.SaveChanges();

            if (Request.IsAjaxRequest())
            {
                var shoppItems = db.ShoppingItems.OrderBy(x => x.DateAdded).Where(m => m.Id == id).ToList();                
                var json = JsonConvert.SerializeObject(shoppItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Content(json);
                
            }
            else
            {
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public ActionResult AddModalReview(int id, string subj,string desc)
        {
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);

            if (shoppingItem != null)
            {


                //TODOGHR Quantity
                Review r = new Review();
                r.ShoppingItemId = id;
                r.subjectLine = subj;
                r.description = desc;
                shoppingItem.reviews.Add(r);
                //db.Entry(shoppingItem).CurrentValues.SetValues(shoppingItem);
                //db.Entry(shoppingItem).State = EntityState.Modified;
                db.SaveChanges();

                if (Request.IsAjaxRequest())
                {
                    var shoppItems = db.ShoppingItems.OrderBy(x => x.DateAdded).Where(m => m.Id == id).ToList();
                    var json = JsonConvert.SerializeObject(shoppItems, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return Content(json);

                }
                else
                {
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }
    }
}
