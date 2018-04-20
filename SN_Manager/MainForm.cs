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

            this.WindowState = FormWindowState.Maximized;

            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
            //dataGridView1.DataSource = readAllOrder;

            textBox1.KeyDown += TextBox1_KeyDown;
            this.SizeChanged += MainForm_SizeChanged;

            dataGridView1.DoubleClick += DataGridView1_DoubleClick;

            asc.controlAutoSize(this);
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int currentSelectedRowIndex = dataGridView1.CurrentRow.Index;
                string selectedOrderName = dataGridView1.Rows[currentSelectedRowIndex].Cells["订单名称"].Value.ToString();
                Form makeOrderForm = new MakeOrderForm(selectedOrderName);
                makeOrderForm.ShowDialog();
                DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
                refreshDataGridViewByDataTable(readAllOrder);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string keyWord = textBox1.Text;
                DataTable searchResult = Program.sql.ExecuteQuery("select * from orderTable where OrderName like " + "'" + keyWord + "%" + "'");
                refreshDataGridViewByDataTable(searchResult);
            }
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
            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
        }

        private void 创建订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form createOrderForm = new CreateOrderForm();
            createOrderForm.ShowDialog();
            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
        }

        private void refreshDataGridViewByDataTable(DataTable dataTable)
        {
            dataTable.Columns["OrderName"].ColumnName = "订单名称";
            dataTable.Columns["OrderSize"].ColumnName = "订单大小";
            dataTable.Columns["CurrentSize"].ColumnName = "当前订单大小";
            dataTable.Columns["MachineModel"].ColumnName = "机器型号";
            dataTable.Columns["CreateTime"].ColumnName = "创建时间";
            dataTable.Columns["Remark"].ColumnName = "备注";
            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["BelongsToOrder"].Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form createOrderForm = new CreateOrderForm();
            createOrderForm.ShowDialog();
            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string keyWord = textBox1.Text;
            DataTable searchResult = Program.sql.ExecuteQuery("select * from orderTable where OrderName like "+"'"+keyWord+"%"+"'");
            refreshDataGridViewByDataTable(searchResult);
        }

        private void 管理密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form changePassWord = new ChangePassWord();
            changePassWord.ShowDialog();
        }

        AutoSizeFormClass asc = new AutoSizeFormClass();
        private void MainForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }

        private void 更改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Update orderTable 
            int currentSelectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            string currentSelectedOrderName = dataGridView1.Rows[currentSelectedRowIndex].Cells["订单名称"].Value.ToString();
            string currentSelectedOrderSize = dataGridView1.Rows[currentSelectedRowIndex].Cells["订单大小"].Value.ToString();
            string currentSelectedCurrentSize = dataGridView1.Rows[currentSelectedRowIndex].Cells["当前订单大小"].Value.ToString();
            string currentSelectedOrderBelongsToOrder = dataGridView1.Rows[currentSelectedRowIndex].Cells["BelongsToOrder"].Value.ToString();
            string currentSelectedOrderMachineModel = dataGridView1.Rows[currentSelectedRowIndex].Cells["机器型号"].Value.ToString();
            string currentSelectedOrderCreateTime = dataGridView1.Rows[currentSelectedRowIndex].Cells["创建时间"].Value.ToString();
            string currentSelectedOrderRemark = dataGridView1.Rows[currentSelectedRowIndex].Cells["备注"].Value.ToString();

            Dictionary<string, string> orderInfo = new Dictionary<string, string>();
            orderInfo["OrderName"] = currentSelectedOrderName;
            orderInfo["OrderSize"] = currentSelectedOrderSize;
            orderInfo["CurrentSize"] = currentSelectedCurrentSize;
            orderInfo["BelongsToOrder"] = currentSelectedOrderBelongsToOrder;
            orderInfo["MachineModel"] = currentSelectedOrderMachineModel;
            orderInfo["CreateTime"] = currentSelectedOrderCreateTime;
            orderInfo["Remark"] = currentSelectedOrderRemark;

            Form updateOrderInfoForm = new UpdateOrderInfoForm(orderInfo);
            updateOrderInfoForm.ShowDialog();

            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DeleteOrder and deletetable
            Form identify = new IdentifyPassWord();
            if(identify.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;

            string deleteOrderWarning = "将要删除以下订单：\n";
            foreach (DataGridViewRow row in selectedRows)
            {
                string selectedOrderName = row.Cells["订单名称"].Value.ToString();
                deleteOrderWarning += (selectedOrderName+"\n");
            }
            deleteOrderWarning += ("一共"+ selectedRows.Count.ToString()+"个订单");
            if(MessageBox.Show(this,
                deleteOrderWarning,
                "Warning",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            foreach (DataGridViewRow row in selectedRows)
            {
                string selectedOrderName = row.Cells["订单名称"].Value.ToString();
                Program.sql.ExecuteNonQuery("drop table '" + selectedOrderName + "'");
                Program.sql.DeleteValuesAND("orderTable",
                    new string[] { "OrderName" },
                    new string[] { selectedOrderName },
                    new string[] { "=" });
            }

            MessageBox.Show(this,"删除"+selectedRows.Count.ToString()+"个订单成功","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            DataTable readAllOrder = Program.sql.ExecuteQuery("select * from orderTable order by orderName");
            refreshDataGridViewByDataTable(readAllOrder);
        }
    }
}
