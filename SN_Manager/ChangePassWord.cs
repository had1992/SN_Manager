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
    public partial class ChangePassWord : Form
    {
        public ChangePassWord()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string oldPassWord = textBox2.Text;
            DataTable realPassWord = Program.sql.ExecuteQuery("select * from PASSWORD");
            if (oldPassWord != realPassWord.Rows[0][0].ToString())
            {
                MessageBox.Show(this,"旧密码错误！","Error",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            string newPassWord1 = textBox3.Text;
            string newPassWord2 = textBox4.Text;
            if (newPassWord1 == "" || newPassWord2 == "")
            {
                MessageBox.Show(this, "请输入两次新密码！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (newPassWord1 != newPassWord2)
            {
                MessageBox.Show(this, "两次新密码不同，请重新输入！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            Program.sql.UpdateValues(
                "PASSWORD",
                new string[] { "DELETEPASSWORD" },
                new string[] { newPassWord1 },
                "DELETEPASSWORD",
                oldPassWord);
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            MessageBox.Show(this, " 密码修改成功！", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
