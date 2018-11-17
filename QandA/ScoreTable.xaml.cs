using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy ScoreTable.xaml
    /// </summary>
    public partial class ScoreTable : UserControl
    {	private readonly Grid _mainGrid;
	    private readonly string Username;
        public ScoreTable(Grid mainGrid,string username)
        {
            InitializeComponent();
	        Username = username;
	        _mainGrid = mainGrid;
			Fill();
        }

	    void Fill()
	    {


		    try
		    {
			    string connstring =
				    "Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
			    NpgsqlConnection connection = new NpgsqlConnection(connstring);
			    connection.Open();
			    NpgsqlCommand command =
				    new NpgsqlCommand("SELECT nickname, bestscore FROM qandauser order by bestscore desc limit 5 ",
					    connection);

			    NpgsqlDataReader dataReader = command.ExecuteReader();
			    for (int i = 0; dataReader.Read(); i++)
			    {
				    tbListuser.Text += dataReader[0].ToString() + "\n";
				    tbListscore.Text += dataReader[1].ToString() + "\n";
			    }

			    connection.Close();

		    }
		    catch
		    {
			    MessageBox.Show("nastąpił błąd przy wczytywaniu najlepszych wyników");
		    }
	    }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_mainGrid.Children.Clear();
			_mainGrid.Children.Add(new Game(_mainGrid, Username));
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			_mainGrid.Children.Clear();
			_mainGrid.Children.Add(new WelcameScreen(_mainGrid));
		}
	}
}
