using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Teman_ApotikProj.Models;

namespace Teman_ApotikProj.Controllers
{
    public class StokObatsController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: StokObats
        //public ActionResult Index()
        //{
        //    var stokObat = db.StokObat.Include(s => s.Obat);
        //    return View(stokObat.ToList());
        //}

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var menu_angkringan = from m in db.StokObat
                                  select m;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    menu_angkringan = menu_angkringan.Where(s => s.Obat.Contains(searchString));
            //}

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Id_Obat);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Id_Stok_Obat);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Stock_In_Hand);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.id_Jenis_Obat);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: StokObats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokObat = db.StokObat.Find(id);
            if (stokObat == null)
            {
                return HttpNotFound();
            }
            return View(stokObat);
        }

        // GET: StokObats/Create
        public ActionResult Create()
        {
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat");
            return View();
        }

        // POST: StokObats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Stok_Obat,Id_Obat,id_Jenis_Obat,Stock_In_Hand")] StokObat stokObat)
        {
            if (ModelState.IsValid)
            {
                db.StokObat.Add(stokObat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", stokObat.Id_Obat);
            return View(stokObat);
        }

        // GET: StokObats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokObat = db.StokObat.Find(id);
            if (stokObat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", stokObat.Id_Obat);
            return View(stokObat);
        }

        // POST: StokObats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Stok_Obat,Id_Obat,id_Jenis_Obat,Stock_In_Hand")] StokObat stokObat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stokObat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", stokObat.Id_Obat);
            return View(stokObat);
        }

        // GET: StokObats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StokObat stokObat = db.StokObat.Find(id);
            if (stokObat == null)
            {
                return HttpNotFound();
            }
            return View(stokObat);
        }

        // POST: StokObats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StokObat stokObat = db.StokObat.Find(id);
            db.StokObat.Remove(stokObat);
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
    }
}
