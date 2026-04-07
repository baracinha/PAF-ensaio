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
        int perguntaAtual = 0; // Índice da pergunta que está a aparecer
        int certas = 0;
        int erradas = 0;
        public gestaousers()
        {
            InitializeComponent();
        }

        DataTable dtPerguntas; // Guarda as perguntas da disciplina selecionada
        

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
            try
            {
                if (string.IsNullOrEmpty(textUSER.Text)&&
                    string.IsNullOrEmpty(textPSSRD.Text))
                {
                    MessageBox.Show("insere um username e uma password");
                }
                else
                {
                    insertUser(textUSER.Text, textPSSRD.Text, comboBox1.SelectedItem.ToString());
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir: " + ex.Message);
            }
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

        private void button6_Click(object sender, EventArgs e)
        {
            // Verifica se existem perguntas carregadas e se NÃO estamos na primeira (índice 0)
            if (dtPerguntas != null && perguntaAtual > 0)
            {
                perguntaAtual--; // Recua um número no índice
                MostrarPergunta(perguntaAtual); // Atualiza o ecrã com a pergunta anterior
            }
            else
            {
                MessageBox.Show("Já estás na primeira pergunta!");
            }
        
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dtPerguntas == null) return;

            // 1. CAPTURAR A RESPOSTA (Antes de qualquer outra coisa!)
            // Vamos criar uma variável para saber se o utilizador acertou NESTA pergunta
            bool acertouNesta = false;

            if (radioButton1.Checked && Convert.ToInt32(radioButton1.Tag) == 1) acertouNesta = true;
            else if (radioButton2.Checked && Convert.ToInt32(radioButton2.Tag) == 1) acertouNesta = true;
            else if (radioButton3.Checked && Convert.ToInt32(radioButton3.Tag) == 1) acertouNesta = true;
            else if (radioButton4.Checked && Convert.ToInt32(radioButton4.Tag) == 1) acertouNesta = true;

            // 2. ATUALIZAR OS CONTADORES GLOBAIS
            if (acertouNesta) certas++;
            else erradas++;

            // 3. NAVEGAÇÃO
            if (perguntaAtual < dtPerguntas.Rows.Count - 1)
            {
                perguntaAtual++;
                MostrarPergunta(perguntaAtual);
            }
            else
            {
                // 4. FINALIZAR
                GravarResultadoNoBanco();
                MessageBox.Show("Fim do Teste!\nAcertos: " + certas + "\nErros: " + erradas);

                // Limpar para o próximo teste
                certas = 0;
                erradas = 0;
                perguntaAtual = 0;
            }
        }

        private void GravarResultadoNoBanco()
        {
            int idDisc = comboBoxDisciplinas.SelectedIndex + 1;

            // Validação extra para segurança
            if (idDisc < 1 || idDisc > 3)
            {
                MessageBox.Show("Erro: Disciplina inválida.");
                return;
            }

            string sqlTeste = @"INSERT INTO testes (id_utilizador, id_disciplina, data_teste, total_perguntas, total_corretas, total_erradas) 
                    VALUES (@uid, @did, NOW(), @total, @c, @e)";

            MySqlParameter[] p = {
    new MySqlParameter("@uid", session.user.id), // ATENÇÃO: Confirma se esta variável não é 0!
    new MySqlParameter("@did", idDisc),
    new MySqlParameter("@total", dtPerguntas.Rows.Count),
    new MySqlParameter("@c", certas),
    new MySqlParameter("@e", erradas)
};

            internalAPI.Executar(sqlTeste, p);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 1. Verificar se selecionou algo (1: Mat, 2: Port, 3: Prog)
            int idDisc = comboBoxDisciplinas.SelectedIndex + 1;

            // 2. Query para buscar as perguntas
            string sql = "SELECT * FROM perguntas WHERE id_disciplina = @id AND ativo = 1";
            MySqlParameter[] p = { new MySqlParameter("@id", idDisc) };

            dtPerguntas = internalAPI.Consulta(sql, p);

            if (dtPerguntas.Rows.Count > 0)
            {
                perguntaAtual = 0;
                MostrarPergunta(perguntaAtual);
            }
            else
            {
                MessageBox.Show("Não há perguntas para esta disciplina.");
            }
        }
        private void MostrarPergunta(int index)
        {
            try
            {
                lblEnunciado.Text = dtPerguntas.Rows[index]["enunciado"].ToString();
                int idPergunta = Convert.ToInt32(dtPerguntas.Rows[index]["id_pergunta"]);

                string sqlOp = "SELECT * FROM opcoes WHERE id_pergunta = @idp";
                MySqlParameter[] p = { new MySqlParameter("@idp", idPergunta) };
                DataTable dtOpcoes = internalAPI.Consulta(sqlOp, p);

                // Só preenche se a base de dados devolveu pelo menos 4 opções
                if (dtOpcoes.Rows.Count >= 4)
                {
                    radioButton1.Text = dtOpcoes.Rows[0]["texto_opcao"].ToString();
                    radioButton1.Tag = dtOpcoes.Rows[0]["correta"];

                    radioButton2.Text = dtOpcoes.Rows[1]["texto_opcao"].ToString();
                    radioButton2.Tag = dtOpcoes.Rows[1]["correta"];

                    radioButton3.Text = dtOpcoes.Rows[2]["texto_opcao"].ToString();
                    radioButton3.Tag = dtOpcoes.Rows[2]["correta"];

                    radioButton4.Text = dtOpcoes.Rows[3]["texto_opcao"].ToString();
                    radioButton4.Tag = dtOpcoes.Rows[3]["correta"];
                }

                // Limpa as seleções para a nova pergunta
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pergunta: " + ex.Message);
            }
        }

        private void CarregarHistorico()
        {
            // Query que junta o nome da disciplina com os resultados
            string sql = @"SELECT d.nome_disciplina AS Disciplina, t.data_teste AS Data, 
                          t.total_corretas AS Certas, t.total_erradas AS Erradas
                   FROM testes t
                   JOIN disciplinas d ON t.id_disciplina = d.id_disciplina
                   WHERE t.id_utilizador = @uid
                   ORDER BY t.data_teste DESC";

            MySqlParameter[] p = { new MySqlParameter("@uid", session.user.id) };

            DataTable dt = internalAPI.Consulta(sql, p);
            dataGridView1.DataSource = dt; // dgvHistorico é o nome do teu DataGridView
        }

        private void AtualizarGrafico(int certas, int erradas)
        {
            int total = certas + erradas;

            // Atualiza os textos das labels (label12 e label13 na tua imagem)
            label12.Text = certas.ToString();
            label13.Text = erradas.ToString();

            // Se estiveres a usar Panels para as barras (ex: panelCertas, panelErradas)
            // Vamos definir uma largura máxima de 200 pixels
            int larguraMax = 200;

            if (total > 0)
            {
                progressBar1.Width = (certas * larguraMax) / total;
                progressBar2.Width = (erradas * larguraMax) / total;
            }

            progressBar1.BackColor = Color.Green
            progressBar2.BackColor = Color.Red;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                CarregarHistorico();

                // Se houver dados no histórico, vamos atualizar o gráfico com o último teste feito
                if (dataGridView1.Rows.Count > 0)
                {
                    // Vai buscar os valores das colunas "Certas" e "Erradas" da primeira linha (o teste mais recente)
                    int c = Convert.ToInt32(dataGridView1.Rows[0].Cells["Certas"].Value);
                    int er = Convert.ToInt32(dataGridView1.Rows[0].Cells["Erradas"].Value);

                    AtualizarGrafico(c, er);
                }
            }
        }
    }
}
