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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            fresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fresh();
        }

        private void 创建订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form createOrderForm = new CreateOrderForm();
            createOrderForm.Show();
        }

        private void fresh()
        {
            //清空列表 
            listView1.Items.Clear();

            SQLiteDataReader readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");

            this.listView1.BeginUpdate();
            while (readAllOrder.Read())
            {
                ListViewItem it = new ListViewItem();
                it.Text = readAllOrder["ID"].ToString();
                it.SubItems.Add(readAllOrder["OrderName"].ToString());
                it.SubItems.Add(readAllOrder["OrderSize"].ToString());
                listView1.Items.Add(it);
            }
            this.listView1.EndUpdate();
        }
    }
}
