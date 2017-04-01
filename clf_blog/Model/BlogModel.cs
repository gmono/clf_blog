using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
namespace clf_blog.Model
{
    public class BlogModel
    {
        public class Blog
        {
            public long Id;
            public string Title;
            public string Content;
            public DateTime Time;
            public long Type;
            public TextReader Data;
            public long AuthorId;
            public string ArticleInfo;
        }
        static string source = null;
        public BlogModel()
        {
            if(source==null)
            {
                var builder = new SqliteConnectionStringBuilder();
                DirectoryInfo info=new DirectoryInfo("Data/MainDB.sqlite3");
                builder.DataSource =info.FullName;
                source = builder.ConnectionString;
            }
        }


        /// <summary>
        /// 读取一行并取出其内容 返回一个Blog对象
        /// </summary>
        /// <param name="reader">查询结果Reader</param>
        /// <returns>一行的内容</returns>
        private Blog ToBlog(SqliteDataReader reader,bool isdata=false)
        {
            bool isok = reader.Read();
            if (isok)
            {
                Blog bg = new Blog();
                bg.Id = reader.GetInt64(0);
                bg.Title = reader.GetString(1);
                bg.Content = reader.GetString(2);
                bg.Type = reader.GetInt64(3);
                bg.Time =new DateTime(reader.GetInt64(4));
                bg.Data = reader.GetTextReader(5);
                bg.AuthorId = reader.GetInt64(6);
                bg.ArticleInfo = reader.IsDBNull(7)? null:reader.GetString(7);
                //bg.Data = reader.GetBlob(4,false);
                return bg;
            }
            return null;
        }

        public Blog GetBlogFromIndex(long index)
        {
            if (index <= 0)
            {
                //返回错误页
                Blog ret = new Blog()
                {
                    Title = "错误",
                    Content = "没有此文章！",
                    Time = DateTime.Now,
                    Id = 0,
                    Data = null
                };
                return ret;
            }
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("select * from Blog where Id={0}", index);
                var reader = cmd.ExecuteReader();
                Blog bg = ToBlog(reader);
                return bg;
            }
        }
        public Blog[] GetBlogListOfRange(long start, long end,long[] types=null,DateTime? stime=null,DateTime? etime=null)
        {
            if (start <= 0 || end < start) return null;
            List<Blog> ret = new List<Blog>();
        
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                //根据参数构造查询字符串
                string nowcmd = "Blog"; //默认为表名
                if(types!=null)
                {
                    string[] tss = (from t in types select string.Format("Type={0}", t)).ToArray();
                    string ts = "";
                    for(int i=0;i<tss.Length;++i)
                    {
                        if (i != 0) ts += " OR ";
                        ts += " "+tss[i];
                    }
                    nowcmd = string.Format("select * from Blog where {0}",ts);
                }
                if(stime!=null&&etime!=null)
                {
                    nowcmd = string.Format("select * from ({0}) where Time>={1} and Time<={2} order by Time desc",nowcmd, stime.Value.Ticks, etime.Value.Ticks);
                }
                nowcmd = string.Format("select * from ({0}) where Id>={1} and Id<={2}", nowcmd, start, end);
                cmd.CommandText = nowcmd;
                var reader = cmd.ExecuteReader();
                Blog bg = null;
                while ((bg = ToBlog(reader)) != null)
                {
                    ret.Add(bg);
                }
                if (ret.Count == 0) return null;
                return ret.ToArray();
            }
        }
        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <returns>返回文章总数</returns>
        public long GetSum()
        {
            using (var connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = "select count(Id) from Blog";
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt64(0);
            }
        }
        /// <summary>
        /// 得到最新的几个博客
        /// </summary>
        /// <param name="count">最新的几个文章</param>
        /// <returns></returns>
        public Blog[] GetNew(long count)
        {
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                List<Blog> ret = new List<Blog>();
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("select * from Blog order by Time desc limit 0,{0}",count-1);
                var reader = cmd.ExecuteReader();
                Blog bg = null;
                while ((bg = ToBlog(reader)) != null)
                {
                    ret.Add(bg);
                }
                if (ret.Count == 0) return null;
                return ret.ToArray();
            }
        }
        /// <summary>
        /// 得到最热的几个博客
        /// </summary>
        /// <param name="count">最热的几个文章</param>
        /// <returns></returns>
        public Blog[] GetHot(long count)
        {
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                List<Blog> ret = new List<Blog>();
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("select * from Blog order by SeeSum desc limit 0,{0}", count - 1);
                var reader = cmd.ExecuteReader();
                Blog bg = null;
                while ((bg = ToBlog(reader)) != null)
                {
                    ret.Add(bg);
                }
                if (ret.Count == 0) return null;
                return ret.ToArray();
            }
        }

    }
}
