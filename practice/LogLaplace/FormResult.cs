using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogLaplace
{
    public partial class FormResult : Form
    {
        public FormResult()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
        }

        private void FormResult_Load(object sender, EventArgs e)
        {

        }
    }
}
