using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SN_Manager
{
    public partial class UpdateOrderInfoForm : Form
    {
        Dictionary<string, string> orderInfo;
        public UpdateOrderInfoForm(Dictionary<string,string> inputOrderInfo)
        {
            orderInfo = inputOrderInfo;

            InitializeComponent();

            this.textBox1.Text = orderInfo["OrderName"];
            this.textBox2.Text = orderInfo["OrderSize"];
            this.textBox4.Text = orderInfo["BelongsToOrder"];
            this.textBox3.Text = orderInfo["MachineModel"];
            this.textBox5.Text = orderInfo["CreateTime"];
            this.richTextBox1.Text = orderInfo["Remark"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "" ||
                textBox2.Text == "" ||
                textBox3.Text == "" ||
                textBox4.Text == "" ||
                textBox5.Text == "" )
            {
                MessageBox.Show("有空白项，没有改变数据！");
                return;
            }

            int tryParseResult;
            if (!int.TryParse(textBox2.Text, out tryParseResult))
            {
                MessageBox.Show("输入的订单大小不是有效数值，没有改变数据！");
                return;
            }

            if (textBox1.Text == orderInfo["OrderName"] &&
                textBox2.Text == orderInfo["OrderSize"] &&
                textBox3.Text == orderInfo["MachineModel"] &&
                textBox4.Text == orderInfo["BelongsToOrder"] &&
                textBox5.Text == orderInfo["CreateTime"] &&
                richTextBox1.Text == orderInfo["Remark"])
            {
                MessageBox.Show("没有改变数据！");
            } else
            {
                DataTable checkBelongsToOrderRight = Program.sql.ExecuteQuery(
                    "select count(*) from orderTable where BelongsToOrder = '" + this.textBox4.Text + "'");
                if (checkBelongsToOrderRight.Rows[0][0].ToString() == "0")
                {
                    MessageBox.Show("原订单名称不存在，没有改变数据！");
                } else
                {
                    if(MessageBox.Show(
                        this,
                        "确认更改吗？",
                        "Warning",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                    Program.sql.UpdateValues("orderTable", 
                        new string[] { "OrderName","OrderSize","BelongsToOrder","MachineModel","CreateTime","Remark" },
                        new string[] { textBox1.Text , textBox2.Text , this.textBox4.Text , this.textBox3.Text , this.textBox5.Text , this.richTextBox1.Text },
                        "OrderName",
                        orderInfo["OrderName"]);

                    if (textBox1.Text != orderInfo["OrderName"])
                    {
                        Program.sql.ExecuteNonQuery("Alter Table '"+ orderInfo["OrderName"] + "' RENAME TO '" + textBox1.Text + "'");
                    }

                    MessageBox.Show("改变数据成功！");
                    this.Close();
                }
            }
        }
    }
}
