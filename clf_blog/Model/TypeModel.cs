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
        public TypeInfo GetTypeInfoFromId(long id)
        {
            using (var connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("select * from Type where Id={0}", id);
                var reader = cmd.ExecuteReader();
                var ret = ToType(reader);
                return ret;
            }
        }
        
        public TypeInfo[] GetTypes()
        {
            using(var connect=new SqliteConnection(source))
            {
                connect.Open();
                var cmd=connect.CreateCommand();
                cmd.CommandText="select * from Type order by id asc";
                var reader=cmd.ExecuteReader();
                var ret=new List<TypeInfo>();
                TypeInfo now=null;
                while((now=ToType(reader))!=null)
                {
                    ret.Add(now);
                }
                return ret.ToArray();
            }
        }
    }

}
