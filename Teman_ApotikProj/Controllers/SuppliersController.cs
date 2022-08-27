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
    public class SuppliersController : Controller
    {
        private TemanApotikkEntities db = new TemanApotikkEntities();

        // GET: Suppliers
        //public ActionResult Index()
        //{
        //    return View(db.Supplier.ToList());
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

            var menu_angkringan = from m in db.Supplier
                                  select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                menu_angkringan = menu_angkringan.Where(s => s.Nama_Suplier.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Nama_Suplier);
                    break;
                case "Date":
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Alamat_Supplier);
                    break;
                case "date_desc":
                    menu_angkringan = menu_angkringan.OrderByDescending(s => s.Id_Jenis_Obat);
                    break;
                default:
                    menu_angkringan = menu_angkringan.OrderBy(s => s.Id_Supplier);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(menu_angkringan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Supplier,Nama_Suplier,Id_Jenis_Obat,Alamat_Supplier")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Supplier,Nama_Suplier,Id_Jenis_Obat,Alamat_Supplier")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Supplier.Find(id);
            db.Supplier.Remove(supplier);
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
