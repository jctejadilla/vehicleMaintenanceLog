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
using System.IO;

namespace VehicleMaintenanceLog
{
    public partial class VehicleInfo : Form
    {
        public VehicleInfo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string vin = vintxt.Text;
            string license = lptxt.Text;
            string make = maketxt.Text;
            string model = modeltxt.Text;
            int year = int.Parse(yeartxt.Text);
            string ownerName = ownertxt.Text;
            string phoneNum = phoneNumtxt.Text;


            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

            //Check if VIN already exists
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
                            // VIN already exists, show message
                            MessageBox.Show("A vehicle with this VIN already exists.");
                        }
                        else
                        {
                            // Step 2: Insert the new vehicle information if VIN doesn't exist
                            string insertQuery = "INSERT INTO VehicleInformation (VIN, LicensePlate, Make, Model, Year, OwnerName, PhoneNumber) " +
                                                 "VALUES (@VIN, @LicensePlate, @Make, @Model, @Year, @OwnerName, @PhoneNumber)";

                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                // Add parameters to prevent SQL injection
                                insertCommand.Parameters.AddWithValue("@VIN", vin);
                                insertCommand.Parameters.AddWithValue("@LicensePlate", license);
                                insertCommand.Parameters.AddWithValue("@Make", make);
                                insertCommand.Parameters.AddWithValue("@Model", model);
                                insertCommand.Parameters.AddWithValue("@Year", year);
                                insertCommand.Parameters.AddWithValue("@OwnerName", ownerName);
                                insertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNum);

                                // Execute the insert command
                                int rowsAffected = insertCommand.ExecuteNonQuery();

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
            string query = "SELECT * FROM VehicleInformation";

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


        private void VehicleInfo_Load(object sender, EventArgs e)
        {
            LoadVehicleInformation();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main mn = new main();
            mn.Show();
            this.Close();
        }

        private void yeartxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; 
            }
        }

        private void phoneNumtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Retrieve data from textboxes
            string vin = vintxt.Text;
            string license = lptxt.Text;
            string make = maketxt.Text;
            string model = modeltxt.Text;
            int year = int.Parse(yeartxt.Text); 
            string ownerName = ownertxt.Text;
            string phoneNum = phoneNumtxt.Text;

            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

            // Step 2: Check if the VIN exists in the database
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
                            // VIN exists, perform update
                            string updateQuery = "UPDATE VehicleInformation SET " +
                                                 "LicensePlate = @LicensePlate, Make = @Make, Model = @Model, " +
                                                 "Year = @Year, OwnerName = @OwnerName, PhoneNumber = @PhoneNumber " +
                                                 "WHERE VIN = @VIN";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                // Add parameters to prevent SQL injection
                                updateCmd.Parameters.AddWithValue("@VIN", vin);
                                updateCmd.Parameters.AddWithValue("@LicensePlate", license);
                                updateCmd.Parameters.AddWithValue("@Make", make);
                                updateCmd.Parameters.AddWithValue("@Model", model);
                                updateCmd.Parameters.AddWithValue("@Year", year);
                                updateCmd.Parameters.AddWithValue("@OwnerName", ownerName);
                                updateCmd.Parameters.AddWithValue("@PhoneNumber", phoneNum);

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

        private void button5_Click(object sender, EventArgs e)
        {
            int vin = int.Parse(vintxt.Text);
            string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";
            string query = $"select * from VehicleInformation where [VIN] = {vin};";
            using (SqlConnection conn = new SqlConnection(con))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    lptxt.Text = dataTable.Rows[0]["LicensePlate"].ToString();
                    maketxt.Text = dataTable.Rows[0]["Make"].ToString();
                    modeltxt.Text = dataTable.Rows[0]["Model"].ToString();
                    yeartxt.Text = dataTable.Rows[0]["Year"].ToString();
                    ownertxt.Text = dataTable.Rows[0]["OwnerName"].ToString();
                    phoneNumtxt.Text = dataTable.Rows[0]["PhoneNumber"].ToString();
                }
                else {
                    MessageBox.Show($"VIN not found.", "Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    vintxt.Clear(); ;
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if any row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the VIN of the selected row (assuming the VIN is the first column)
                string vin = dataGridView1.SelectedRows[0].Cells["VIN"].Value.ToString(); // Adjust if necessary

                string con = @"Data Source=DESKTOP-GC6KPUL;Initial Catalog=VehicleMaintenanceLogDB;Integrated Security=True;TrustServerCertificate=True";

                // SQL query to delete the vehicle based on VIN
                string deleteQuery = "DELETE FROM VehicleInformation WHERE VIN = @VIN";

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

        private void button4_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private void ExportToCsv()
        {
            try
            {
                string csvFilePath = @"C:\Users\jctej\Downloads\VehicleInformation.csv";

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

    }
}
