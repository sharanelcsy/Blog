using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace MvcBlog.Controllers
{

    public class UyeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye, string Sifre)
        {
            var md5pass = Crypto.Hash(Sifre, "MD5");
            var login = db.Uyes.Where(u => u.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();

            if (login != null && login.KullaniciAdi == uye.KullaniciAdi && login.Email == uye.Email )
            {
                Session["uyeid"] = login.UyeId;
                Session["kullaniciadi"] = login.KullaniciAdi;
                Session["yetkiid"] = login.YetkiId;
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Uyari = "Kullanıcının bilgileri hatalıdır. kontrol ediniz";
                return View();
            }

        }

        public ActionResult Logout()
        {
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye, string Sifre)
        {
            var md5pass = Sifre;
            if (ModelState.IsValid)
            {




                uye.YetkiId = 2;
                uye.Sifre = Crypto.Hash(md5pass, "MD5");
                db.Uyes.Add(uye);
                db.SaveChanges();
                Session["uyeid"] = uye.UyeId;
                Session["kullaniciadi"] = uye.KullaniciAdi;
                return RedirectToAction("Index", "Home");



            }
                return View(uye);
            
        }
        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Edit(Uye uye, int id,  string Sifre)
        {
            if (ModelState.IsValid)
            {
                var md5pass = Sifre;
                var uyes = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
               
                

                 
                
                uyes.AdSoyad = uye.AdSoyad;
                uyes.KullaniciAdi = uye.KullaniciAdi;
                uyes.Sifre = Crypto.Hash(md5pass, "MD5");
                uyes.Email = uye.Email;
                db.SaveChanges();
                Session["kullaniciadi"] = uye.KullaniciAdi;
                return RedirectToAction("Index", "Home", new { id = uyes.UyeId });

            }
            return View();
        }
        public ActionResult UyeProfil(int id)
        {
            //Eğer giriş yapacak kullanıcı kendi hesabına gidecekse normal profil gösterilsin(düzeltilecek)
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            return View(uye);
        }
    }
}