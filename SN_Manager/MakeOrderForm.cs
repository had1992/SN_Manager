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
    public partial class MakeOrderForm : Form
    {
        private int currentOrderTabSize;         //当前订单的大小
        private string OrderName;
        public MakeOrderForm(string orderName)
        {
            OrderName = orderName;
            
            InitializeComponent();

            DataTable orderInfo = Program.sql.ExecuteQuery("select * from orderTable where OrderName = '" + OrderName + "'");
            textBox3.Text = orderInfo.Rows[0]["MachineModel"].ToString();
            textBox4.Text = orderInfo.Rows[0]["CurrentSize"].ToString();
            currentOrderTabSize = int.Parse(textBox4.Text);
            textBox5.Text = orderInfo.Rows[0]["OrderName"].ToString();
            textBox6.Text = orderInfo.Rows[0]["OrderSize"].ToString();

            this.SizeChanged += MakeOrderForm_SizeChanged;

            DataTable orderContent = Program.sql.ExecuteQuery("select * from '" + OrderName + "'");
            refreshDataGridViewByDataTable(orderContent);

            asc.controlAutoSize(this);
        }

        private void MakeOrderForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void MakeOrderForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        AutoSizeFormClass asc = new AutoSizeFormClass();
        private void MainForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void refreshDataGridViewByDataTable(DataTable dataTable)
        {
            dataGridView1.DataSource = dataTable;
        }
    }
}
