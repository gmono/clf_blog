using System;
using clf_blog.Model;
namespace clf_blog.PageModel
{
    public class IndexModel
    {
        public BlogModel.Blog[] Hots=null;
        public BlogModel.Blog[] News=null;
        public MessageModel.Message[] TopMsgs=null;
    }
}