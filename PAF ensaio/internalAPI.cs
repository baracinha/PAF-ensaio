using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace PAF_ensaio
{
    internal class internalAPI
    {
        private static string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["conexao"].ConnectionString;

        public static DataTable Consulta(string sql, MySqlParameter[] parametros = null)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parametros != null)
                    {
                        cmd.Parameters.AddRange(parametros);
                    }
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public static int Executar(string sql, MySqlParameter[] parametros = null)
        {
            using (MySqlConnection conn = new MySqlConnection(conexao))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parametros != null) cmd.Parameters.AddRange(parametros);

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log ou tratamento de erro
                        throw new Exception("Erro ao executar comando: " + ex.Message);


                    }
                }
            }
        }
    }
}
