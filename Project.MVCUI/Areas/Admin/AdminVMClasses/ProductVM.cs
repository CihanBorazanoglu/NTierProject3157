using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.MVCUI.Areas.Admin.AdminVMClasses
{
    public class ProductVM //PaginationVM ile neredeyse ayni gorevi yapiyor gibi gozukebilir. Aslinda cok benzer gorevleri yapmaktadir ancak PaginationVM sadece alisveris tarafinda kullanilacak ve sayfalandirmayi yapacak bir VM iken ProductVM sadece admin tarafinda kullanilmasi icin tasarlanmis bir VM class'tir.
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}