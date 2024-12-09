using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleMaintenanceLog
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MaintenanceRecords maintenanceRecords = new MaintenanceRecords();
            maintenanceRecords.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VehicleInfo info = new VehicleInfo();
            info.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();

            this.Close();
        }
    }
}
