using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using clf_blog.Model;
using clf_blog.PageModel;
namespace clf_blog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var blg=new BlogModel();
            var mod=new IndexModel();
            mod.Hots=blg.GetHot(10);
            mod.News=blg.GetNew(10);
            var msg=new MessageModel();
            mod.TopMsgs=msg.GetTopMsgs(10);
            return View(mod);
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
            long pagesum;
            BlogModel.Blog[] data=mod.GetBlogListOfRange(10, 0,out pagesum);
            ViewData["PageSum"] = pagesum;
            ViewData["NowPage"]=1;
            return View(data);
        }
        public IActionResult Types()
        {
            var ts=new TypeModel();
            var tss=ts.GetTypes();
            return View(tss);
        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Article(long id)
        {
            var bg=new BlogModel();
            var data=bg.GetBlogFromIndex(id);
            return View(data);
        }
    }
}
