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
    public partial class gestaousers : Form
    {
        public gestaousers()
        {
            InitializeComponent();
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
    }
}
