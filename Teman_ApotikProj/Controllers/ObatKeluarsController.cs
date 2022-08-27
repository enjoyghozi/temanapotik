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
    public class ObatKeluarsController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: ObatKeluars
        //public ActionResult Index()
        //{
        //    var obatKeluar = db.ObatKeluar.Include(o => o.Obat).Include(o => o.Pasien);
        //    return View(obatKeluar.ToList());
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

            var menu_angkringan = from m in db.ObatKeluar
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
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Id_Jenis_Obat);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Tgl_Keluar);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Total_Harga);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: ObatKeluars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatKeluar obatKeluar = db.ObatKeluar.Find(id);
            if (obatKeluar == null)
            {
                return HttpNotFound();
            }
            return View(obatKeluar);
        }

        // GET: ObatKeluars/Create
        public ActionResult Create()
        {
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat");
            ViewBag.Id_Pasien = new SelectList(db.Pasien, "Id_Pasien", "Nama_Pasien");
            return View();
        }

        // POST: ObatKeluars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Transaksi_Keluar,Tgl_Keluar,Id_Pasien,Id_Obat,Id_Jenis_Obat,Jumlah_Keluar,Total_Harga")] ObatKeluar obatKeluar)
        {
            if (ModelState.IsValid)
            {
                db.ObatKeluar.Add(obatKeluar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatKeluar.Id_Obat);
            ViewBag.Id_Pasien = new SelectList(db.Pasien, "Id_Pasien", "Nama_Pasien", obatKeluar.Id_Pasien);
            return View(obatKeluar);
        }

        // GET: ObatKeluars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatKeluar obatKeluar = db.ObatKeluar.Find(id);
            if (obatKeluar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatKeluar.Id_Obat);
            ViewBag.Id_Pasien = new SelectList(db.Pasien, "Id_Pasien", "Nama_Pasien", obatKeluar.Id_Pasien);
            return View(obatKeluar);
        }

        // POST: ObatKeluars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Transaksi_Keluar,Tgl_Keluar,Id_Pasien,Id_Obat,Id_Jenis_Obat,Jumlah_Keluar,Total_Harga")] ObatKeluar obatKeluar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obatKeluar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatKeluar.Id_Obat);
            ViewBag.Id_Pasien = new SelectList(db.Pasien, "Id_Pasien", "Nama_Pasien", obatKeluar.Id_Pasien);
            return View(obatKeluar);
        }

        // GET: ObatKeluars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatKeluar obatKeluar = db.ObatKeluar.Find(id);
            if (obatKeluar == null)
            {
                return HttpNotFound();
            }
            return View(obatKeluar);
        }

        // POST: ObatKeluars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ObatKeluar obatKeluar = db.ObatKeluar.Find(id);
            db.ObatKeluar.Remove(obatKeluar);
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
