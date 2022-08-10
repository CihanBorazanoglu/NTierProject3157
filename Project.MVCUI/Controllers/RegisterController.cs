using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {

        UserRepository _uRep;
        ProfileRepository _proRep;

        public RegisterController()
        {
            _uRep = new UserRepository();
            _proRep = new ProfileRepository();
        }

        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUser appUser, AppUserProfile userProfile)
        {
            appUser.Password = DantexCrypt.Crypt(appUser.Password); //sifreyi kriptoladik

            if (_uRep.Any(x => x.UserName == appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanici Ismi Daha Once Alinmis";
                return View();
            }
            else if(_uRep.Any(x => x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Boyle Bir Email Zaten Kayitli";
                return View();
            }

            //Kullanici Basarili Bir Sekilde Register Islemini Tamamladiysa Ona Mail Gonder

            string gonderilecekEmail = "Tebrikler. Hesabiniz Olusturulöustur. Hesabinizi Aktif Etmek Icin https://localhost:44300/Register/Activation/" + appUser.ActivationCode + " Linkine Tiklayabilirsiniz";

            MailService.Send(appUser.Email, body: gonderilecekEmail, subject: "Hesap Aktivasyon!");
            _uRep.Add(appUser); // Siz kullanici yaninda profili ekleyecek olsa bile oncelikle repository'nin bu metodunu calistirmalisiniz. Cunku AppUser'in ID'si ilk basta olusmali. Cunku bizim kurdugumuz birebir iliskide AppUser zorunlu alan, profile ise opsiyonel alandir. Dolayisiyla Profile'in ID'si identity degildir. O yuzden Profile eklenecekken ID belirlenmek zorundadir. Birebir iliski oldugundan dolayi da Profile'in ID'si AppUser'a denk gelmelidir. Ilk basta AppUser'in ID'si SaveChanges ile olusmali ki sonra Profile'i rahatca ekleyebilelim.

            if(!string.IsNullOrEmpty(userProfile.FirstName.Trim()) || !string.IsNullOrEmpty(userProfile.LastName.Trim()))
            {
                userProfile.ID = appUser.ID;
                _proRep.Add(userProfile);
            }

            return View("RegisterOK");
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _uRep.FirstOrDefault(x => x.ActivationCode == id);
            if(aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _uRep.Update(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesabiniz Aktif Hale Getirildi";
                return RedirectToAction("Login", "Home");
            }

            //Supheli bir aktivite
            TempData["HesapAktifMi"] = "Hesabiniz Bulunamadi";
            // Todo: Odev => Bir baska database'e supheli olan bu request'i logla
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RegisterOK()
        {
            return View();
        }
    }
}