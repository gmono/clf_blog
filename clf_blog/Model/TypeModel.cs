using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
namespace clf_blog.Model
{
    /// <summary>
    /// 此类提供对类型列表和描述信息的访问
    /// </summary>
    public class TypeModel
    {
        public class TypeInfo
        {
            public long Id;
            public string Name;
            public string Info;
        }
        static string source = null;
        public TypeModel()
        {
            if (source == null)
            {
                var builder = new SqliteConnectionStringBuilder();
                DirectoryInfo info = new DirectoryInfo("Data/MainDB.sqlite3");
                builder.DataSource = info.FullName;
                source = builder.ConnectionString;
            }
        }
        public TypeInfo ToType(SqliteDataReader reader)
        {
            bool isok = reader.Read();
            if (isok)
            {
                TypeInfo ret = new TypeInfo();
                ret.Id = reader.GetInt64(0);
                ret.Name = reader.GetString(1);
                ret.Info = reader.GetString(2);
                return ret;
            }
            return null;
        }
    }

}
