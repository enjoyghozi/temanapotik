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
    public class ObatsController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: Obats
        //public ActionResult Index()
        //{
        //    var obat = db.Obat.Include(o => o.JenisObat);
        //    return View(obat.ToList());
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

            var menu_angkringan = from m in db.Obat
                                  select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                menu_angkringan = menu_angkringan.Where(s => s.Nama_Obat.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Id_Obat);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Id_Jenis_Obat);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.JenisObat);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Nama_Obat);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Obats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
            if (obat == null)
            {
                return HttpNotFound();
            }
            return View(obat);
        }

        // GET: Obats/Create
        public ActionResult Create()
        {
            ViewBag.Id_Jenis_Obat = new SelectList(db.JenisObat, "Id_Jenis_Obat", "Jenis_Obat");
            return View();
        }

        // POST: Obats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Obat,Kode_Obat,Nama_Obat,Id_Jenis_Obat")] Obat obat)
        {
            if (ModelState.IsValid)
            {
                db.Obat.Add(obat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Jenis_Obat = new SelectList(db.JenisObat, "Id_Jenis_Obat", "Jenis_Obat", obat.Id_Jenis_Obat);
            return View(obat);
        }

        // GET: Obats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
            if (obat == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Jenis_Obat = new SelectList(db.JenisObat, "Id_Jenis_Obat", "Jenis_Obat", obat.Id_Jenis_Obat);
            return View(obat);
        }

        // POST: Obats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Obat,Kode_Obat,Nama_Obat,Id_Jenis_Obat")] Obat obat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Jenis_Obat = new SelectList(db.JenisObat, "Id_Jenis_Obat", "Jenis_Obat", obat.Id_Jenis_Obat);
            return View(obat);
        }

        // GET: Obats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obat obat = db.Obat.Find(id);
            if (obat == null)
            {
                return HttpNotFound();
            }
            return View(obat);
        }

        // POST: Obats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Obat obat = db.Obat.Find(id);
            db.Obat.Remove(obat);
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
