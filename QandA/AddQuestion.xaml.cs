using System.Windows;
using System.Windows.Controls;
using Npgsql;
namespace QandA
{
	/// <summary>
	/// Logika interakcji dla klasy AddQuestion.xaml
	/// </summary>
	public partial class AddQuestion : UserControl
	{
		private readonly Grid _mainGrid;
		private readonly string Username;
		public AddQuestion(Grid mainGrid,string username)
		{
			Username = username;
			InitializeComponent();
			_mainGrid = mainGrid;
		}



		private void Add(object sender, RoutedEventArgs e)
		{
			if (IsUnique() && NotNull())
			{
				try
				{
					string connstring =
						"Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
					NpgsqlConnection connection = new NpgsqlConnection(connstring);
					connection.Open();
					NpgsqlCommand cmd = new NpgsqlCommand(
						"insert into qanda (question, answer1,answer2,answer3,correctanswer) values(:question, :answer1,:answer2,:answer3,:correctanswer)",
						connection);

					cmd.Parameters.Add(new NpgsqlParameter("question", Question.Text));
					cmd.Parameters.Add(new NpgsqlParameter("answer1", Answer1.Text));
					cmd.Parameters.Add(new NpgsqlParameter("answer2", Answer2.Text));
					cmd.Parameters.Add(new NpgsqlParameter("answer3", Answer3.Text));
					cmd.Parameters.Add(new NpgsqlParameter("correctanswer", CorrectAnswer.Text));
					cmd.ExecuteNonQuery();
					connection.Close();
					Clear();
				}
				catch
				{
					MessageBox.Show("something is wrong, try again later ");
				}
			}
			else MessageBox.Show("Answers are empty or duplicated");
		}

		private void Back(object sender, RoutedEventArgs e)
		{
			_mainGrid.Children.Clear();
			_mainGrid.Children.Add(new Game(_mainGrid,Username));
		}

		private void Clear()
		{
			_mainGrid.Children.Clear();
			_mainGrid.Children.Add(new AddQuestion(_mainGrid,Username));
		}

		private void Question_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		bool IsUnique()
		{
			if (CorrectAnswer.Text == Answer1.Text || CorrectAnswer.Text == Answer2.Text ||
			    CorrectAnswer.Text == Answer3.Text||Answer2.Text==Answer3.Text||Answer1.Text==Answer3.Text||Answer2.Text==Answer1.Text)
		
				return false;
			return true;
		}

		bool NotNull()
		{
			if (CorrectAnswer.Text == "" || Answer1.Text == "" || Answer2.Text == "" || Answer3.Text == "" ||
			    Question.Text == "")
				return false;
			return true;


		}
	}
}



	
	

