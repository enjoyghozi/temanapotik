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
    public class PasiensController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: Pasiens
        //public ActionResult Index()
        //{
        //    return View(db.Pasien.ToList());
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

            var menu_angkringan = from m in db.Pasien
                                  select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                menu_angkringan = menu_angkringan.Where(s => s.Nama_Pasien.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Nama_Pasien);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Diagnosa_Penyakit);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.ObatKeluar);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Alamat_Pasien);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Pasiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasien pasien = db.Pasien.Find(id);
            if (pasien == null)
            {
                return HttpNotFound();
            }
            return View(pasien);
        }

        // GET: Pasiens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pasiens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Pasien,Nama_Pasien,Alamat_Pasien,Diagnosa_Penyakit")] Pasien pasien)
        {
            if (ModelState.IsValid)
            {
                db.Pasien.Add(pasien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pasien);
        }

        // GET: Pasiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasien pasien = db.Pasien.Find(id);
            if (pasien == null)
            {
                return HttpNotFound();
            }
            return View(pasien);
        }

        // POST: Pasiens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Pasien,Nama_Pasien,Alamat_Pasien,Diagnosa_Penyakit")] Pasien pasien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pasien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pasien);
        }

        // GET: Pasiens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pasien pasien = db.Pasien.Find(id);
            if (pasien == null)
            {
                return HttpNotFound();
            }
            return View(pasien);
        }

        // POST: Pasiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pasien pasien = db.Pasien.Find(id);
            db.Pasien.Remove(pasien);
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
