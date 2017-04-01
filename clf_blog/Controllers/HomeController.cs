using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using clf_blog.Model;
namespace clf_blog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult BlogList()
        {
            //获取博客列表
            BlogModel mod = new BlogModel();
            BlogModel.Blog[] data=mod.GetBlogListOfRange(1, 10);
            ViewData["test"] = About();
            return View(data);
        }
        public IActionResult Types()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Article(long id)
        {
            return View(id);
        }
    }
}
