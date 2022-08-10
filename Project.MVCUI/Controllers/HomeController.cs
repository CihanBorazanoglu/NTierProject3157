using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Enums;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        UserRepository _uRep;
        public HomeController()
        {
            _uRep = new UserRepository();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AppUser user)
        {
            AppUser yakalanan = _uRep.FirstOrDefault(x => x.UserName == user.UserName);
            if(yakalanan == null)
            {
                ViewBag.Kullanici = "Kullanici Bulunamadi";
                return View();
            }

            string decrypted = DantexCrypt.DeCrypt(yakalanan.Password);
            if(user.Password == decrypted && yakalanan.Role == UserRole.Admin)
            {
                if (!yakalanan.Active) return AktifKontrol();
                Session["admin"] = yakalanan;
                return RedirectToAction("CategoryList", "Category", new { area = "Admin" });
            }
            else if(yakalanan.Role == UserRole.Member && user.Password == decrypted)
            {
                if(!yakalanan.Active)return AktifKontrol();
                Session["member"] = yakalanan;
                return RedirectToAction("ShoppingList", "Shopping");
            }
            ViewBag.Kullanici = "Kullanici Bulunamadi";
            return View();
        }

        private ActionResult AktifKontrol()
        {
            ViewBag.Kullanici = "Lutfen Hesabinizi Aktif Hale Getiriniz";
            return View("Login");
        }
    }
}