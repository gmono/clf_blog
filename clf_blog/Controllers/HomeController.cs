﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using clf_blog.Model;
using clf_blog.PageModel;
using Newtonsoft.Json;
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

        public IActionResult BlogList(string json)
        {
            int pagelen = 10;
            int pageid = 0;
            long[] types = null;
            DateTime? stime = null;
            DateTime? etime = null;
            var query = HttpContext.Request.Query;
            if (query["pagelen"].Count>0) int.TryParse(query["pagelen"][0], out pagelen);
            if (query["pageid"].Count > 0) int.TryParse(query["pageid"][0], out pageid);
            if (query["type"].Count > 0)
            {
                types = new long[1];
                long.TryParse(query["type"][0], out types[0]);
            }
            if (query["stime"].Count > 0)
            {
                DateTime ttime;
                if(DateTime.TryParse(query["stime"][0], out ttime))
                    stime = ttime;
            }
            if (query["etime"].Count > 0)
            {
                DateTime ttime;
                if (DateTime.TryParse(query["etime"][0], out ttime))
                    etime = ttime;
            }
            //获取博客列表
            BlogModel mod = new BlogModel();
            long pagesum;
            BlogModel.Blog[] data=mod.GetBlogListOfRange(pagelen, pageid,out pagesum,types,stime,etime);
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
