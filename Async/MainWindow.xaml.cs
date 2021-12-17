using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Async
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        string cs = "";
        SqlDataReader reader;
        DataSet set;
        SqlDataAdapter DA;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();    
        }

        private void bookIdtxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(BookIdtxtbox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                BookIdtxtbox.Text = BookIdtxtbox.Text.Remove(BookIdtxtbox.Text.Length - 1);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                conn.Open();

                string query = @"Select * from Students where Students.[Username]=@username AND Students.[Password]=@password";
                
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@username";
                param.SqlDbType = SqlDbType.NVarChar;
                param.Value = UsernameTxtbox.Text;

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@password";
                param2.SqlDbType = SqlDbType.NVarChar;
                param2.Value = PasswordTxtBox.Text;

                var command = new SqlCommand(query, conn);

                command.Parameters.Add(param);
                command.Parameters.Add(param2);
              
                var reader = command.ExecuteReader();
                string answer = "";
                while (reader.Read())
                {
                    answer = reader.GetValue(1).ToString();
                }

                if (answer != "")
                {
                    signInPanel.Visibility = Visibility.Hidden;
                    UserPanel.Visibility = Visibility.Visible;

                    using (SqlConnection conn2 = new SqlConnection())
                    {
                        conn2.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                        conn2.Open();
                        string query2 = @"select * from Libs";
                        var command2 = new SqlCommand(query2, conn2);
                        using (var reader2 = command2.ExecuteReader())
                        {
                            while (reader2.Read())
                            {
                                libscombobox.Items.Add(reader2.GetValue(1) + " " + reader2.GetValue(2));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Student not found...");
                }
            }
        }

        private void takeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                conn.Open();

                string query = @"insert into S_Cards(Id,Id_Student,Id_Book,DateOut,Id_Lib)
                   values(@MyId,@StudentId,@BookId,@DateOut,@LibId)";
                               

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@StudentId";
                param1.SqlDbType = SqlDbType.Int;
                param1.Value = 7;
                                
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@BookId";
                param2.SqlDbType = SqlDbType.Int;
                param2.Value = Int32.Parse(BookIdtxtbox.Text);
                               
                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@MyId";
                param3.SqlDbType = SqlDbType.Int;
                param3.Value =888;
                                
                SqlParameter param4 = new SqlParameter();
                param4.ParameterName = "@DateOut";
                param4.SqlDbType = SqlDbType.DateTime;
                param4.Value = DateTime.Now;                        
                              
                SqlParameter param5 = new SqlParameter();
                param5.ParameterName = "@LibId";
                param5.SqlDbType = SqlDbType.Int;
                param5.Value = Int32.Parse(libscombobox.SelectedIndex.ToString()) + 1;                              

                var command = new SqlCommand(query, conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.Parameters.Add(param3);
                command.Parameters.Add(param4);
                command.Parameters.Add(param5);               

                var result = command.ExecuteNonQuery();

                MessageBox.Show("You take a book...");
            }
        }
    }
}
