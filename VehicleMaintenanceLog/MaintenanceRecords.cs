using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleMaintenanceLog
{
    public partial class MaintenanceRecords : Form
    {
        public MaintenanceRecords()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            main mn = new main();
            mn.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string vin = vintxt.Text;
            DateTime dateofservice = dateTimePicker1.Value;
            string serviceperformed = servicePerformedtxt.Text;
            string currentmilieage = currentMileagetxt.Text;
            string costofservice = costServicetxt.Text;

            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";
            string checkQuery = "SELECT COUNT(*) FROM VehicleInformation WHERE VIN = @VIN";

            using (SqlConnection connection = new SqlConnection(con))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@VIN", vin);

                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            // VIN exists
                            string insertQuery = "INSERT INTO MaintenanceRecords (VIN, DateOfService, ServicePerformed, Mileage, CostOfService) " +
                                                 "VALUES (@VIN, @dateofservice, @serviceperformed, @mileage, @costofservice)";

                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                            {
                                // Add parameters to prevent SQL injection
                                insertCmd.Parameters.AddWithValue("@VIN", vin);
                                insertCmd.Parameters.AddWithValue("@dateofservice", dateofservice);
                                insertCmd.Parameters.AddWithValue("@serviceperformed", serviceperformed);
                                insertCmd.Parameters.AddWithValue("@mileage", currentmilieage);
                                insertCmd.Parameters.AddWithValue("@costofservice", costofservice);

                                // Execute the insert command
                                int rowsAffected = insertCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Vehicle information saved successfully!");

                                    // Refresh the DataGridView
                                    LoadVehicleInformation();
                                }
                                else
                                {
                                    MessageBox.Show("Error saving vehicle information.");
                                }
                            }

                        }
                        else
                        {
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

        }

        private void LoadVehicleInformation()
        {
            string connectionString = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

            // SQL Query to fetch data
            string query = "SELECT * FROM MaintenanceRecords";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void MaintenanceRecords_Load(object sender, EventArgs e)
        {
            LoadVehicleInformation();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            // Retrieve data from textboxes
            string vin = vintxt.Text;
            DateTime dateofservice = dateTimePicker1.Value;
            string serviceperformed = servicePerformedtxt.Text;
            string currentmilieage = currentMileagetxt.Text;
            string costofservice = costServicetxt.Text;

            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

            // Step 2: Check if the VIN exists in the database
            string checkQuery = "SELECT COUNT(*) FROM MaintenanceRecords WHERE VIN = @VIN";

            using (SqlConnection connection = new SqlConnection(con))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@VIN", vin);

                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            // VIN exists, perform update
                            string updateQuery = "UPDATE MaintenanceRecords SET " +
                                                 "DateOfService= @dateofservice, ServicePerformed= @serviceperformed, Mileage= @mileage, CostOfService =  @costofservice " +
                                                 "WHERE VIN = @VIN";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                // Add parameters to prevent SQL injection
                                updateCmd.Parameters.AddWithValue("@VIN", vin);
                                updateCmd.Parameters.AddWithValue("@dateofservice", dateofservice);
                                updateCmd.Parameters.AddWithValue("@serviceperformed", serviceperformed);
                                updateCmd.Parameters.AddWithValue("@mileage", currentmilieage);
                                updateCmd.Parameters.AddWithValue("@costofservice", costofservice);

                                // Execute the update command
                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Vehicle information updated successfully!");

                                    // Step 3: Update the DataGridView to reflect the updated data
                                    LoadVehicleInformation();
                                }
                                else
                                {
                                    MessageBox.Show("Error updating vehicle information.");
                                }
                            }
                        }
                        else
                        {
                            // VIN does not exist, show an error
                            MessageBox.Show("No vehicle found with this VIN.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Check if any row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the VIN of the selected row (assuming the VIN is the first column)
                string vin = dataGridView1.SelectedRows[0].Cells["VIN"].Value.ToString(); // Adjust if necessary

                string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

                // SQL query to delete the vehicle based on VIN
                string deleteQuery = "DELETE FROM MaintenanceRecords WHERE VIN = @VIN";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                        {
                            // Add parameters to prevent SQL injection
                            deleteCommand.Parameters.AddWithValue("@VIN", vin);

                            // Execute the delete command
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Vehicle information deleted successfully!");

                                // Step 3: Refresh the DataGridView to reflect the changes
                                LoadVehicleInformation();
                            }
                            else
                            {
                                MessageBox.Show("Error deleting vehicle information.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a vehicle to delete.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string csvFilePath = @"C:\Users\jctej\Downloads\MaintenanceRecords.csv";

                StringBuilder csvContent = new StringBuilder();

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csvContent.Append(column.HeaderText + ",");
                }

                csvContent.Length--;

                csvContent.AppendLine();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = cell.Value?.ToString().Replace(",", "");
                        csvContent.Append(cellValue + ",");
                    }

                    csvContent.Length--;
                    csvContent.AppendLine();
                }

                File.WriteAllText(csvFilePath, csvContent.ToString());

                MessageBox.Show("Data saved to Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int vin = int.Parse(vintxt.Text);
            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";
            string query = $"select * from MaintenanceRecords where [VIN] = {vin};";
            using (SqlConnection conn = new SqlConnection(con))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    dateTimePicker1.Text = dataTable.Rows[0]["DateOfService"].ToString();
                    servicePerformedtxt.Text = dataTable.Rows[0]["ServicePerformed"].ToString();
                    currentMileagetxt.Text = dataTable.Rows[0]["Mileage"].ToString();
                    costServicetxt.Text = dataTable.Rows[0]["CostOfService"].ToString();
                }
                else
                {
                    MessageBox.Show($"VIN not found.", "Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    vintxt.Clear(); ;
                }

            }
        }
    }
}
