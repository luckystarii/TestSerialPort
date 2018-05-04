using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serialPort
{
    public partial class Truck_scales : Form
    {
        public Truck_scales()
        {
            InitializeComponent();
        }

        private void Truck_scales_Load(object sender, EventArgs e)
        {
            //Output: 1,234.00
            //txtTestPort1.Text = string.Format("{0:n}", 2435);
            
            //Output: 9,876
            //txtTestPort2.Text = string.Format("{0:n0}",2435);
            
            //Output: 9,876
            txtTestPort1.Text = 2435.ToString("#,#");
            
            //Output: 9,876.00
            txtTestPort2.Text = 2435.ToString("#,#.00");
        }
    }
}
