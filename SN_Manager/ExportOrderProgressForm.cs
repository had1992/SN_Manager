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
    public partial class ExportOrderProgressForm : Form
    {
        private string saveRoad;
        public ExportOrderProgressForm(string inputSaveRoad)
        {
            saveRoad = inputSaveRoad;
            InitializeComponent();

            label1.Text = "";
            this.Shown += ExportOrderProgressForm_Shown;
        }

        private void ExportOrderProgressForm_Shown(object sender, EventArgs e)
        {
            ExportOrderToCsv exportTool = new ExportOrderToCsv();
            DataTable allOrderNameDataTable = Program.sql.ExecuteQuery("select OrderName from orderTable");
            int orderNum = allOrderNameDataTable.Rows.Count;
            progressBar1.Maximum = orderNum;
            for (int i = 0; i < orderNum; i++)
            {
                string orderName = allOrderNameDataTable.Rows[i]["OrderName"].ToString();

                label1.Text = "正在导出" + orderName + "订单，第" + (i + 1).ToString() + "个，一共" + orderNum.ToString() + "个。";

                progressBar1.Value = i + 1;

                exportTool.ExportOrderByOrderName(orderName, saveRoad);
            }
        }

        private void ExportOrderProgressForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
