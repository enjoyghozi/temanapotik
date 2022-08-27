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
    public class ObatMasuksController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: ObatMasuks
        //public ActionResult Index()
        //{
        //    var obatMasuk = db.ObatMasuk.Include(o => o.Obat).Include(o => o.Supplier);
        //    return View(obatMasuk.ToList());
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

            var menu_angkringan = from m in db.ObatMasuk
                                  select m;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    menu_angkringan = menu_angkringan.Where(s => s.Tgl_Masuk.Contains(searchString));
            //}

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Id_Obat);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Obat);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Tgl_Masuk);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Jumlah_Masuk);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: ObatMasuks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatMasuk obatMasuk = db.ObatMasuk.Find(id);
            if (obatMasuk == null)
            {
                return HttpNotFound();
            }
            return View(obatMasuk);
        }

        // GET: ObatMasuks/Create
        public ActionResult Create()
        {
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat");
            ViewBag.Id_Supplier = new SelectList(db.Supplier, "Id_Supplier", "Nama_Suplier");
            return View();
        }

        // POST: ObatMasuks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Transaksi_Masuk,Id_Obat,Tgl_Masuk,Id_Supplier,Id_Jenis_Obat,Jumlah_Masuk,Harga_Beli")] ObatMasuk obatMasuk)
        {
            if (ModelState.IsValid)
            {
                db.ObatMasuk.Add(obatMasuk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatMasuk.Id_Obat);
            ViewBag.Id_Supplier = new SelectList(db.Supplier, "Id_Supplier", "Nama_Suplier", obatMasuk.Id_Supplier);
            return View(obatMasuk);
        }

        // GET: ObatMasuks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatMasuk obatMasuk = db.ObatMasuk.Find(id);
            if (obatMasuk == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatMasuk.Id_Obat);
            ViewBag.Id_Supplier = new SelectList(db.Supplier, "Id_Supplier", "Nama_Suplier", obatMasuk.Id_Supplier);
            return View(obatMasuk);
        }

        // POST: ObatMasuks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Transaksi_Masuk,Id_Obat,Tgl_Masuk,Id_Supplier,Id_Jenis_Obat,Jumlah_Masuk,Harga_Beli")] ObatMasuk obatMasuk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obatMasuk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Obat = new SelectList(db.Obat, "Id_Obat", "Kode_Obat", obatMasuk.Id_Obat);
            ViewBag.Id_Supplier = new SelectList(db.Supplier, "Id_Supplier", "Nama_Suplier", obatMasuk.Id_Supplier);
            return View(obatMasuk);
        }

        // GET: ObatMasuks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObatMasuk obatMasuk = db.ObatMasuk.Find(id);
            if (obatMasuk == null)
            {
                return HttpNotFound();
            }
            return View(obatMasuk);
        }

        // POST: ObatMasuks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ObatMasuk obatMasuk = db.ObatMasuk.Find(id);
            db.ObatMasuk.Remove(obatMasuk);
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
