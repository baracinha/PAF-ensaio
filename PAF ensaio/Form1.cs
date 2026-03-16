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

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM utilizadores where username = @u and password = @p";
            MySqlParameter[] p = { new MySqlParameter("@u", textBox1.Text), new MySqlParameter("@p", textBox2.Text) };

            DataTable dt = internalAPI.Consulta(sql, p);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Login bem sucedido!, bem vindo " );
            }
            else
            {
                MessageBox.Show("Credenciais inválidas.");
            }
        }
    }
}
