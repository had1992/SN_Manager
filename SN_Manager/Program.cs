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
            sql = new SqLiteHelper("mydb.db");

            sql.ExecuteNonQuery(
                    @"CREATE TABLE IF NOT EXISTS orderTable( 
                        ID             INTEGER PRIMARY KEY ASC AUTOINCREMENT UNIQUE,
                        OrderName      TEXT,
                        OrderSize      INTEGER,
                        CurrentSize    INTEGER DEFAULT(0),
                        BelongsToOrder TEXT,
                        MachineModel   TEXT    NOT NULL DEFAULT('default'),
                        CreateTime     TEXT,
                        Remark         TEXT
                    );");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
