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
            public IEnumerable<byte> Data;
            public long DataLength;
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

                string time = reader.GetString(3);
                string[] tms = time.Split('-');
                DateTime dt = new DateTime(int.Parse(tms[0]), int.Parse(tms[1]), int.Parse(tms[2]));
                bg.Time = dt;
                bg.Type = reader.GetInt64(4);
                //bg.Data = reader.GetBlob(4,false);
                if(isdata)
                {
                    byte[] buf = new byte[4*1024 * 1024];//最大允许4M的用户自定义数据 多的放文件
                    long size=reader.GetBytes(5, 0, buf, 0, 4 * 1024 * 1024);
                    bg.DataLength = size;
                    bg.Data = buf.Take((int)size);
                }
                else
                {
                    bg.Data = null;
                    bg.DataLength = 0;
                }
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
