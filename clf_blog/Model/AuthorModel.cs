using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
namespace clf_blog.Model
{
    public class AuthorModel
    {
        public class Author
        {
            public long Id;
            public string Name;
            public string Info;
        }
        static string source = null;
        public AuthorModel()
        {
            if (source == null)
            {
                var builder = new SqliteConnectionStringBuilder();
                DirectoryInfo info = new DirectoryInfo("Data/MainDB.sqlite3");
                builder.DataSource = info.FullName;
                source = builder.ConnectionString;
            }
        }
        public Author ToAuthor(SqliteDataReader reader)
        {
            bool isok = reader.Read();
            if(isok)
            {
                Author ret = new Author();
                ret.Id = reader.GetInt64(0);
                ret.Name = reader.GetString(1);
                ret.Info = reader.GetString(2);
                return ret;
            }
            return null;
        }
        public Author GetAuthorInfo(long Id)
        {
            string qstr = string.Format("select * from Author where Id={0}", Id);
            using (SqliteConnection connect = new SqliteConnection(source))
            {
                var cmd = connect.CreateCommand();
                cmd.CommandText = qstr;
                var reader=cmd.ExecuteReader();
                Author ret = ToAuthor(reader);
                return ret;
            }
        }
    }
}
