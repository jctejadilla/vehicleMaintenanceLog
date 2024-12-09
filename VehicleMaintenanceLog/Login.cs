using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VehicleMaintenanceLog
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void logBtn_Click(object sender, EventArgs e)
        {
            // Store the connection string securely, e.g., in a configuration file.
            string connectionString = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string username = textBox1.Text;
                string password = textBox2.Text;

                // Use parameterized query to prevent SQL injection
                string query = "SELECT COUNT(1) FROM login WHERE Username = @Username AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // Ensure passwords are hashed and compared with stored hashes

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        main mn = new main();
                        mn.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }
                }
            }
        }

    }
}
