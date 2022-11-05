using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace TaskManager.Repository
{
    public class DbContext
    {
        public IDbConnection Connection { get; protected set; }
        public IDbTransaction? Transaction { get; set; } = null;
 
        public DbContext(string conn)
        {
            string fileLocation = string.Concat(Path.GetFullPath("."), "\\Repository\\db\\database");
            //string fileLocation = @"D:\database.db";
            conn = conn.Replace("{AppDir}", fileLocation);

            if (!File.Exists(fileLocation))
            {
                CreateDatabase(conn);
            }
            Connection = CreateConnection(conn);
        }

        protected IDbConnection CreateConnection(string connectionString)
        {
            return new SqliteConnection(connectionString);
        }

        private void CreateDatabase(string connectionString)
        {
            //File.Create(string.Concat(Path.GetFullPath("."), "\\Repository\\db\\database.db"));
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    Create table ToDo(
                        id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        title varchar(50) not null,
                        description varchar(300) not null,
                        isDone INTEGER not null,
                        isActive INTEGER not null,
                        check((isActive = 0 OR isActive = 1) AND (isDone = 0 OR isDone = 1)
                    ))
                ";
                conn.Execute(sql);

                conn.Dispose();
            }
        }
    }
}
