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
        private string orderName;
        private string machineModel;
        private DataTable orderContent;
        private int maxOrderSize;
        private bool hasChanged;
        public MakeOrderForm(string inputOrderName)
        {
            orderName = inputOrderName;
            hasChanged = false;


            InitializeComponent();

            DataTable orderInfo = Program.sql.ExecuteQuery("select * from orderTable where OrderName = '" + orderName + "'");
            textBox3.Text = orderInfo.Rows[0]["MachineModel"].ToString();
            machineModel = textBox3.Text;
            textBox4.Text = orderInfo.Rows[0]["CurrentSize"].ToString();
            textBox5.Text = orderInfo.Rows[0]["OrderName"].ToString();
            textBox6.Text = orderInfo.Rows[0]["OrderSize"].ToString();
            maxOrderSize = int.Parse(textBox6.Text);

            this.SizeChanged += MakeOrderForm_SizeChanged;

            orderContent = Program.sql.ExecuteQuery("select * from '" + orderName + "'");
            orderContent.Columns.Add("IsChanged", Type.GetType("System.Boolean"));
            orderContent.Columns.Add("IsNew", Type.GetType("System.Boolean"));
            for (int i = 0; i < orderContent.Rows.Count; i++)
            {
                orderContent.Rows[i]["IsChanged"] = false;
                orderContent.Rows[i]["IsNew"] = false;
            }

            refreshDataGridViewByDataTable(orderContent);

            this.FormClosing += MakeOrderForm_FormClosing;

            asc.controlAutoSize(this);
        }

        private void MakeOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChanged == true)
            {
                if (MessageBox.Show(
                    this,
                    "有更改未提交，是否提交？",
                    "Warning",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updateOrder();
                }
            }
            int currentOrderTabSize = Program.sql.ExecuteQuery("select * from '" + orderName + "'").Rows.Count;
            Program.sql.UpdateValues(
                "orderTable",
                new string[] { "CurrentSize" },
                new string[] { currentOrderTabSize.ToString() },
                "OrderName",
                orderName);
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
            updateOrder();
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
            dataGridView1.Columns["Id"].Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string RobotSN = textBox1.Text;
            string ChargeBaseSN = textBox2.Text;
            if(RobotSN == "" || ChargeBaseSN == "")
            {
                MessageBox.Show("未完成输入，无法录入！");
                return;
            }

            for(int i = 0; i < orderContent.Rows.Count; i++)
            {
                if(orderContent.Rows[i]["RobotSN"].ToString() == RobotSN)
                {
                    orderContent.Rows[i]["ChargeBaseSN"] = ChargeBaseSN;
                    orderContent.Rows[i]["DATETIME"] = DateTime.Now.ToString();
                    orderContent.Rows[i]["IsChanged"] = true;
                    
                    textBox1.Clear();
                    textBox2.Clear();

                    hasChanged = true;

                    //refreshDataGridViewByDataTable(orderContent);
                    return;
                }
            }
            if (orderContent.Rows.Count == maxOrderSize)
            {
                if (MessageBox.Show(
                    this, 
                    "订单已满，无法添加新订单,是否提交订单？", 
                    "Warning", 
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updateOrder();
                }
                return;
            }

            int maxId = 0;
            if (orderContent.Rows.Count  != 0)
            {
                maxId = int.Parse(orderContent.Rows[orderContent.Rows.Count - 1]["Id"].ToString());
            }
            orderContent.Rows.Add(new object[] { maxId+1, RobotSN , ChargeBaseSN , DateTime.Now.ToString(),false,true});

            textBox1.Clear();
            textBox2.Clear();

            hasChanged = true;

            //refreshDataGridViewByDataTable(orderContent);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;

            string deleteOrderWarning = "将要删除以下RobotSN：\n";
            foreach (DataGridViewRow row in selectedRows)
            {
                string selectedOrderName = row.Cells["RobotSN"].Value.ToString();
                deleteOrderWarning += (selectedOrderName + "\n");
            }
            deleteOrderWarning += ("一共" + selectedRows.Count.ToString() + "个SN");
            if (MessageBox.Show(this,
                deleteOrderWarning,
                "Warning",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) != DialogResult.OK)
            {
                return;
            }

            foreach (DataGridViewRow row in selectedRows)
            {
                if (row.Cells["IsNew"].Value.Equals(false))
                {
                    string Id = row.Cells["Id"].Value.ToString();
                    Program.sql.DeleteValuesAND(orderName,
                        new string[] { "Id" },
                        new string[] { Id },
                        new string[] { "=" });
                }
            }

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    orderContent.Rows.RemoveAt(i);
                }
            }

            int currentOrderTabSize = orderContent.Rows.Count;
            textBox4.Text = currentOrderTabSize.ToString();

            MessageBox.Show(this, "删除" + selectedRows.Count.ToString() + "个RobotSN成功", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void updateOrder()
        {
            if ( hasChanged == false )
            {
                return;
            }

            for (int i = 0; i < orderContent.Rows.Count; i++)
            {
                if (orderContent.Rows[i]["IsNew"].Equals(true))
                {
                    //insert
                    string robotSN = orderContent.Rows[i]["RobotSN"].ToString();
                    string chargeBaseSN = orderContent.Rows[i]["ChargeBaseSN"].ToString();
                    string time = orderContent.Rows[i]["DATETIME"].ToString();

                    Program.sql.InsertValues(orderName, new string[] { "NULL", robotSN, chargeBaseSN, time });

                    orderContent.Rows[i]["IsNew"] = false;
                    orderContent.Rows[i]["IsChanged"] = false;
                    continue;
                }
                if (orderContent.Rows[i]["IsChanged"].Equals(true))
                {
                    //update
                    string robotSN = orderContent.Rows[i]["RobotSN"].ToString();
                    string chargeBaseSN = orderContent.Rows[i]["ChargeBaseSN"].ToString();
                    string time = orderContent.Rows[i]["DATETIME"].ToString();

                    Program.sql.UpdateValues(
                        orderName,
                        new string[] { "RobotSN", "ChargeBaseSN", "DATETIME" },
                        new string[] { robotSN, chargeBaseSN, time },
                        "Id", orderContent.Rows[i]["Id"].ToString());

                    orderContent.Rows[i]["IsChanged"] = false;
                }
            }
            int currentOrderTabSize = orderContent.Rows.Count;
            textBox4.Text = currentOrderTabSize.ToString();

            hasChanged = false;

            //refreshDataGridViewByDataTable(orderContent);
        }
    }
}
