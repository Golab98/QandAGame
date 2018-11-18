using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
using Npgsql;

namespace QandA
{
	/// <summary>
	/// Logika interakcji dla klasy WelcameScreen.xaml
	/// </summary>
	public partial class WelcameScreen : UserControl
	{
		private readonly Grid _mainGrid;
		public WelcameScreen(Grid mainGrid)
		{
			InitializeComponent();
			_mainGrid = mainGrid;
	}

	

		private void MEnter(object sender, MouseEventArgs e)
		{	if(((TextBox)sender).Text=="Password"|| ((TextBox)sender).Text=="User")
			((TextBox)sender).Text = "";
		}

		private void FindUser(object sender, RoutedEventArgs e)
		{
			try
			{
				string connstring =
					"Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";

		

				NpgsqlConnection connection = new NpgsqlConnection(connstring);
				connection.Open();
				NpgsqlCommand command =
					new NpgsqlCommand("SELECT password FROM qandauser where nickname='" + tbUser.Text + "'",
						connection);
				NpgsqlDataReader dataReader = command.ExecuteReader();
				dataReader.Read();

				if (pbPassword.Password == dataReader[0].ToString())
				{
					connection.Close();

					_mainGrid.Children.Clear();
					_mainGrid.Children.Add(new Game(_mainGrid, tbUser.Text));

				}
				else
				{   connection.Close();
					MessageBox.Show("wrong password");

				}
					
			}
			catch
			{
				MessageBox.Show("User not found, check nickname or register");
			}
		}

		private void Register(object sender, RoutedEventArgs e)
		{
			if (tbUser.Text.Length > 4 && pbPassword.Password.Length > 4)
			{

				try
				{
					string connstring =
						"Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
					NpgsqlConnection connection = new NpgsqlConnection(connstring);
					connection.Open();

					NpgsqlCommand cmd = new NpgsqlCommand(
						"insert into qandauser  values(:nickname, :password, 0)",
						connection);
					cmd.Parameters.Add(new NpgsqlParameter("nickname", tbUser.Text));
					cmd.Parameters.Add(new NpgsqlParameter("password", pbPassword.Password));
					cmd.ExecuteNonQuery();
					connection.Close();
					MessageBox.Show("Registered, now you are able to log in");
				}
				catch
				{
					MessageBox.Show("username already taken");
				}



			}
			else MessageBox.Show("password and username min length is 5 characters");



		}
		}
	}
	
	

