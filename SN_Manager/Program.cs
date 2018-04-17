using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SN_Manager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 

        public static SqLiteHelper sql;

        [STAThread]
        static void Main()
        {
            sql = new SqLiteHelper("data source=mydb.db");

            SQLiteDataReader readTableName = sql.ExecuteQuery("SELECT name FROM sqlite_master WHERE name = 'orderTable' ORDER BY name");
            if (!readTableName.Read())
            {
                //创建名为orderTable的数据表
                sql.CreateTable("orderTable", new string[] { "ID", "OrderName", "OrderSize", "BelongsToOrder" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });
            }    

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
