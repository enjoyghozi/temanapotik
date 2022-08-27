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
    public class HargasController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: Hargas
        //public ActionResult Index()
        //{
        //    var harga = db.Harga.Include(h => h.Obat);
        //    return View(harga.ToList());
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

            var menu_angkringan = from m in db.Harga
                                  select m;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    menu_angkringan = menu_angkringan.Where(s => s.Id_Harga.GetType(searchString));
            //}

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Id_Obat);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Id_Jenis_Obat);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Harga_Akhir);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Harga_Awal);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Hargas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Harga harga = db.Harga.Find(id);
            if (harga == null)
            {
                return HttpNotFound();
            }
            return View(harga);
        }

        // GET: Hargas/Create
        public ActionResult Create()
        {
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat");
            return View();
        }

        // POST: Hargas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Harga,Id_Obat,Id_Jenis_Obat,Harga_Awal,Diskon,Harga_Akhir")] Harga harga)
        {
            if (ModelState.IsValid)
            {
                db.Harga.Add(harga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", harga.Id_Obat);
            return View(harga);
        }

        // GET: Hargas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Harga harga = db.Harga.Find(id);
            if (harga == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", harga.Id_Obat);
            return View(harga);
        }

        // POST: Hargas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Harga,Id_Obat,Id_Jenis_Obat,Harga_Awal,Diskon,Harga_Akhir")] Harga harga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(harga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", harga.Id_Obat);
            return View(harga);
        }

        // GET: Hargas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Harga harga = db.Harga.Find(id);
            if (harga == null)
            {
                return HttpNotFound();
            }
            return View(harga);
        }

        // POST: Hargas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Harga harga = db.Harga.Find(id);
            db.Harga.Remove(harga);
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
