using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace PAF_ensaio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_erro.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM utilizadores where username = @u and password = @p";
            MySqlParameter[] p = { new MySqlParameter("@u", textBox1.Text), new MySqlParameter("@p", textBox2.Text) };

            DataTable dt = internalAPI.Consulta(sql, p);
            if (dt.Rows.Count > 0)
            {
                Form1.ActiveForm.Hide();
                session.user.id = Convert.ToInt32(dt.Rows[0]["id"]);
                session.user.nivel = dt.Rows[0]["nivel"].ToString();
                session.user.username = dt.Rows[0]["username"].ToString();
                session.user.LoginTime = DateTime.Now;
                gestaousers g = new gestaousers();
                g.ShowDialog();
            }
            else
            {
                lbl_erro.Visible = true;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            lbl_erro.Visible = false;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            lbl_erro.Visible = false;
        }
    }
}
