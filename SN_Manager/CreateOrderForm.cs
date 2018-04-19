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
            string machineModel = this.textBox3.Text;
            string Remark = this.richTextBox1.Text;

            if(orderName == "" || orderSize == "" || machineModel == "")
            {
                MessageBox.Show(this, "未完成输入，无法创建订单！");
                return;
            }

            int tryParseResult;
            if (!int.TryParse(orderSize, out tryParseResult))
            {
                MessageBox.Show("输入的订单大小不是有效数值，无法创建订单！");
                return;
            }

            DataTable checkIsExisted = Program.sql.ExecuteQuery("select * from orderTable where BelongsToOrder=" + "'" + orderName + "'");
            int existNum = checkIsExisted.Rows.Count;
            string tempOrderName = orderName;
            if (existNum != 0)
            {
                tempOrderName += "-";
                tempOrderName += existNum.ToString();
            }

            Program.sql.InsertValues(
                    "orderTable",
                    new string[] { "NULL", tempOrderName, orderSize,"0", orderName, machineModel, DateTime.Now.ToString(), Remark });
            Program.sql.CreateTable(
                tempOrderName,
                new string[] { "RobotSN", "ChargeBaseSN", "MODEL", "WORKORDER", "DATETIME" },
                new string[] { "TEXT", "TEXT", "TEXT", "TEXT", "TEXT" });

            if (MessageBox.Show(this, "订单创建成功，是否打开订单输入界面？\n订单编号：" + tempOrderName + "\n订单大小：" + orderSize.ToString() + "\n机器型号：" + machineModel + "\n备注：" + Remark,
                "Success",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Form makeOrderForm = new MakeOrderForm(tempOrderName);
                makeOrderForm.Show();
            }
        }
    }
}
