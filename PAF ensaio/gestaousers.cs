using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAF_ensaio
{
    public partial class gestaousers : Form
    {
        public gestaousers()
        {
            InitializeComponent();
        }

        private void listGrelha()
        {

            try
            {
                string sql = "SELECT id, username, nivel FROM utilizadores";
                DataTable dt = internalAPI.Consulta(sql);
                ViewUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void searchUser(string campo, string elemento)
        {
            string sql = $"SELECT id, username, nivel FROM utilizadores WHERE {campo} = @elemento";
            MySqlParameter[] p = { new MySqlParameter("@elemento", elemento) };
            
            try
            {
                DataTable dt = internalAPI.Consulta(sql, p);
                ViewUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao pesquisar: " + ex.Message);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void gestaousers_Load(object sender, EventArgs e)
        {
            lbl_username.Text = session.user.username;
            lbl_nivel.Text = session.user.nivel;

            if (session.user.nivel != "admin")
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textUSER.Text) &&
                string.IsNullOrWhiteSpace(textID.Text) &&
                (comboBox1.SelectedIndex == -1) &&
                string.IsNullOrWhiteSpace(textPSSRD.Text))
            {
                listGrelha();
            }
            else if (!string.IsNullOrWhiteSpace(textID.Text))
            {
                searchUser("id", textID.Text);
            }
            else if (!string.IsNullOrWhiteSpace(textUSER.Text))
            {
                searchUser("username", textUSER.Text);
            }
            else if (!(comboBox1.SelectedIndex == -1))
            {
                searchUser("nivel", comboBox1.SelectedItem.ToString());
            }
            
        }
    }
}
