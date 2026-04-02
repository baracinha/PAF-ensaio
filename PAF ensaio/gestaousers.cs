using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X9;
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

        private void updateUser(string field, string elemento, int condicao,string id)
        {
            while (condicao == 1)
            {
                string sql = $"UPDATE utilizadores SET {field} = @elemento WHERE id = @id";
                MySqlParameter[] p = { new MySqlParameter("@elemento", elemento), new MySqlParameter("@id", id) };

                int resultado = internalAPI.Executar(sql, p);

                if (resultado > 0)
                {
                    MessageBox.Show(field + " atualizado com sucesso!");
                    condicao = 0;
                }
            }
        }

        private void insertUser(string username, string password, string nivel)
        {
            try
            {
                string sql = "INSERT INTO utilizadores (username, password, nivel) VALUES (@username, @password, @nivel)";
                MySqlParameter[] p = {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password),
                    new MySqlParameter("@nivel", nivel)
                    };
                int resultado = internalAPI.Executar(sql, p);
                if (resultado > 0)
                {
                    MessageBox.Show("Utilizador inserido com sucesso!");
                }
                else 
                {                     
                    MessageBox.Show("Falha ao inserir utilizador."); }
                }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insertUser(textUSER.Text, textPSSRD.Text, comboBox1.SelectedItem.ToString());
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(textUSER.Text))
                {
                    updateUser("username", textUSER.Text, 1, textID.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textPSSRD.Text))
                {
                    updateUser("password", textPSSRD.Text, 1, textID.Text);
                }
                else if (!(comboBox1.SelectedIndex == -1))
                {
                    updateUser("nivel", comboBox1.SelectedItem.ToString(), 1, textID.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
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
