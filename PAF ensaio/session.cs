using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    internal class session
    {
        public static class user
        {
            public static int id { get; set; }
            public static string nivel { get; set; }
            public static string username { get; set; }
            public static DateTime LoginTime { get; set; }   
        }
    }
}
