using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
namespace coretest.Model
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
            public Stream Data;
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
                bg.Time = reader.GetDateTime(4);
                bg.Data=reader.GetStream(5);
                bg.AuthorId = reader.GetInt64(6);
                bg.ArticleInfo = reader.GetString(7);
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
                    Time = new DateTime(),
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
        public Blog[] GetBlogListOfRange(long start, long end)
        {
            if (start <= 0 || end < start) return null;
            List<Blog> ret = new List<Blog>();
        
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("select * from Blog where Id>={0} and Id<={1}", start, end);
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
