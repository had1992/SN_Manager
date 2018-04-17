using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SN_Manager
{
    public partial class CreateOrderForm : Form
    {
        public CreateOrderForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string orderName = this.textBox1.Text;
            string orderSize = this.textBox2.Text;

            SQLiteDataReader checkIsExisted = Program.sql.ExecuteQuery("select * from orderTable where BelongsToOrder=" + "'" + orderName + "'");
            int existNum = 0;
            while (checkIsExisted.Read())
            {
                existNum++;
            }

            if (existNum != 0)
            {
                Program.sql.InsertValues("orderTable", new string[] { "1", orderName +"-"+ existNum.ToString(), orderSize, orderName });
            } else
            {
                Program.sql.InsertValues("orderTable", new string[] { "1", orderName, orderSize, orderName });
            }
            
        }
    }
}
