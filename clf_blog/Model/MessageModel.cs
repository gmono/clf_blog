using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;
namespace clf_blog.Model
{
    public class MessageModel
    {
        public class Message
        {
            public long Id;
            public string Name;
            public string Content;
        }
        static string source = null;
        public MessageModel()
        {
            if (source == null)
            {
                var builder = new SqliteConnectionStringBuilder();
                DirectoryInfo info = new DirectoryInfo("Data/MainDB.sqlite3");
                builder.DataSource = info.FullName;
                source = builder.ConnectionString;
            }
        }
        public Message ToMessage(SqliteDataReader reader)
        {
            bool isok = reader.Read();
            if (isok)
            {
                Message ret = new Message();
                ret.Id = reader.GetInt64(0);
                ret.Name = reader.GetString(1);
                ret.Content = reader.GetString(2);
                return ret;
            }
            return null;
        }
        public Message[] GetTopMsgs(long count)
        {
            using (var connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Message ORDER BY Id ASC LIMIT 0,{0}",count);
                var reader = cmd.ExecuteReader();
                List<Message> ret = new List<Message>();
                Message msg = null;
                while((msg=ToMessage(reader))!=null)
                {
                    ret.Add(msg);
                }
                if (ret.Count == 0) return null;
                return ret.ToArray();
            }
        }
        public Message[] GetAllMsgs()
        {
            using (var connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = "SELECT * FROM Blog ORDER BY Id ASC";
                var reader = cmd.ExecuteReader();
                List<Message> ret = new List<Message>();
                Message msg = null;
                while ((msg = ToMessage(reader)) != null)
                {
                    ret.Add(msg);
                }
                if (ret.Count == 0) return null;
                return ret.ToArray();
            }
        }
        public long GetMsgCount()
        {
            using (var connect = new SqliteConnection(source))
            {
                connect.Open();
                var cmd = connect.CreateCommand();
                cmd.CommandText = string.Format("SELECT count(*) FROM Message");
                var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt64(0);
                
            }
        }
    }
}
