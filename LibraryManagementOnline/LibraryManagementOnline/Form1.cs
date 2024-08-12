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
using System.Security.Cryptography.X509Certificates;

namespace LibraryManagementOnline
{
    public partial class MainForm : Form
    {
        private string connectionString = "Server=NQK\\MSSQLSERVER01;Database=Final;Integrated Security=True;";
        public MainForm()
        {
            InitializeComponent();
            connectionString = "Server=NQK\\MSSQLSERVER01;Database=Final;Integrated Security=True;";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadBooks_Click(object sender, EventArgs e)
        {
            string query = "SELECT BookID, Title, Author, Publisher, YearPublished FROM Books";
            FillData(query);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Books (Title, Author, Publisher, YearPublished, ISBN, Genre, CopiesAvailable) VALUES (@Title, @Author, @Publisher, @YearPublished, @ISBN, @Genre, @CopiesAvailable)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                    cmd.Parameters.AddWithValue("@Publisher", txtPublisher.Text);
                    cmd.Parameters.AddWithValue("@YearPublished", txtYear.Text);
                    cmd.Parameters.AddWithValue("@ISBN", txtISBN.Text);
                    cmd.Parameters.AddWithValue("@Genre", txtGenre.Text);
                    cmd.Parameters.AddWithValue("@CopiesAvailable", txtCopiesAvailable.Text);
                    cmd.ExecuteNonQuery();

                    // Load updated data into DataGridView
                    string reloadQuery = "SELECT * FROM Books";
                    FillData(reloadQuery);

                    // Clear input fields
                    ClearInputFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sách: " + ex.Message);
                }
            }
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            // Implement borrowing logic here
            MessageBox.Show("The book borrowing function has not been implemented.");
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            // Implement returning logic here
            MessageBox.Show("The book return function has not been implemented yet.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            string query;
            try
            {
                int.Parse(searchValue);
                query = "SELECT BookID, Title, Author, Publisher, YearPublished FROM Books WHERE (Title LIKE '%" + searchValue + "%') OR (BookID = @ID)";
            }
            catch (Exception)
            {
                query = "SELECT BookID, Title, Author, Publisher, YearPublished FROM Books WHERE (Title LIKE '%" + searchValue + "%')";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if(int.TryParse(searchValue, out int id))
                    {
                        cmd.Parameters.AddWithValue(query, id);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dgvBooks.DataSource = dt;
                    }
                    else
                    {
                        dgvBooks.DataSource = null;
                        MessageBox.Show("No books found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when searching for books: " + ex.Message);
                }
            }
        }

        private void FillData(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvBooks.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void ClearInputFields()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            txtYear.Clear();
            txtISBN.Clear();
            txtGenre.Clear();
            txtCopiesAvailable.Clear();
        }
    }
}
            
    

