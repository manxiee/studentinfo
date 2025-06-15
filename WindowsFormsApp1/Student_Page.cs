using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace StudentInfoApp
{
    public partial class Student_Page : Form
    {
        private string connectionString = "server=localhost;user id=root;database=StudentInfoDB"; // No password

        public Student_Page()
        {
            InitializeComponent();
            LoadStudentData();
            StudentDataGridView.DataError += StudentDataGridView_DataError;
            StudentDataGridView.CellContentClick += StudentDataGridView_CellContentClick;
        }

        private void LoadStudentData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT studentId, CONCAT(firstName, ' ', middleName, ' ', lastName) AS fullName FROM StudentRecordTB";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Bind data to DataGridView
                StudentDataGridView.DataSource = dataTable;

                // Only add the button column once
                if (!StudentDataGridView.Columns.Contains("ViewButtonColumn"))
                {
                    DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn
                    {
                        HeaderText = "",
                        Text = "VIEW",
                        Name = "ViewButtonColumn",
                        UseColumnTextForButtonValue = true
                    };
                    StudentDataGridView.Columns.Add(viewButtonColumn);
                }
            }
        }

        private void StudentDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == StudentDataGridView.Columns["ViewButtonColumn"].Index)
            {
                // Safely get studentId by column name
                var studentIdCell = StudentDataGridView.Rows[e.RowIndex].Cells["studentId"];

                if (studentIdCell != null && int.TryParse(studentIdCell.Value?.ToString(), out int studentId))
                {
                    StudentPage_Individual studentPageIndividual = new StudentPage_Individual(studentId);
                    studentPageIndividual.Show();
                }
                else
                {
                    MessageBox.Show("The student ID is null or invalid.");
                }
            }
        }

        private void StudentDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("An error occurred while processing data. Please check the format.");
            e.ThrowException = false;
        }
    }
}