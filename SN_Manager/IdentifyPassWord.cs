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
    public partial class IdentifyPassWord : Form
    {
        public IdentifyPassWord()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enterPassWord = textBox1.Text;
            DataTable realPassWord = Program.sql.ExecuteQuery("select * from PASSWORD");
            if (enterPassWord == realPassWord.Rows[0][0].ToString())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "密码错误！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
