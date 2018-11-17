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
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
	    int score = 0;
	    int counter=0;

	    int baseLength;
	    private int[] _usedQuestionNumbers = new int[10];
	    private TData[] _data;
	    private readonly Grid _mainGrid;
	    private readonly string Username;
	    int whatqstnow;
	    private DateTime startTime;
	    
		public Game(Grid mainGrid,string username)
        {
			InitializeComponent();
	        _mainGrid = mainGrid;
	        Username = username;
	        tb1.Text = "SCORE  " + score.ToString();
	        counter = 0;
	        Fill();
			FillScreen();
		}
    void Fill()
	{
		
		CheckLength();
		_data = new TData[baseLength];

		for (int i = 0; i < baseLength; i++)
			_data[i]=new TData(i+1);	// (i+1) sql base index first possition is 1 not null,

		
	}

	void CheckLength()
	{
		string connstring =
			"Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
		NpgsqlConnection connection = new NpgsqlConnection(connstring);
		connection.Open();
		NpgsqlCommand command = new NpgsqlCommand("select max(questionid) from qanda", connection);
		NpgsqlDataReader dataReader = command.ExecuteReader();
		dataReader.Read();
		baseLength = (int)dataReader[0];
		connection.Close();
	}



	int RandUniqueQuestion()
	{
		bool i;
		Random rnd = new Random();
		do
		{
			_usedQuestionNumbers[counter] = rnd.Next(0, baseLength);
			i = true;
			for (int j = 0; j < counter; j++)
				if (_usedQuestionNumbers[j] == _usedQuestionNumbers[counter])
					i = false;
		}
		while (i == false);
		return _usedQuestionNumbers[counter];
	}

	void FillScreen()
	{
		startTime = DateTime.Now; //	save the start time of show the question
						whatqstnow = RandUniqueQuestion();
		Random rnd = new Random();
		Question.Text = _data[whatqstnow].Question[0];
		Button1.Content = _data[whatqstnow].Question[rnd.Next(1, 5)];
			do Button2.Content = _data[whatqstnow].Question[rnd.Next(1, 5)]; 
		while (Button2.Content == Button1.Content);
		do Button3.Content = _data[whatqstnow].Question[rnd.Next(1, 5)];
		while (Button3.Content == Button1.Content || Button3.Content == Button2.Content);
			do Button4.Content = _data[whatqstnow].Question[rnd.Next(1, 5)];
		while (Button4.Content == Button1.Content || Button4.Content == Button2.Content ||
			   Button4.Content == Button3.Content);
	}



	void Correct()
	{
		score += 250 + 100000/Points();
		tb1.Text = "SCORE  " + score.ToString();
		counter++;
		if (counter < 10)
		{
			FillScreen();

		}
		else

		{
			MessageBox.Show("Good job");
			SendScore();
			ClearGame();
		}
	}
	void Bad()
	{
		MessageBox.Show("Bad answer, play again! ");
		SendScore();
		ClearGame();
	}




	private void Click(object sender, RoutedEventArgs e)
	{
		if (((Button)sender).Content.ToString() == _data[whatqstnow].Question[4])
		{
			Correct();
		}
		else Bad();

	}



	void ClearGame()
	{	
		
		_mainGrid.Children.Clear();
		_mainGrid.Children.Add(new ScoreTable(_mainGrid,Username));
	}

	private void Question_TextChanged(object sender, TextChangedEventArgs e)
	{

	}

	    void SendScore()
	    {
		    try
		    {
			    string connstring =
				    "Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
			    NpgsqlConnection connection = new NpgsqlConnection(connstring);
			    connection.Open();

			    NpgsqlCommand cmdcheck = new NpgsqlCommand(
				    "select bestscore from qandauser where nickname='" + Username + "'",
				    connection);
			    NpgsqlDataReader dataReader = cmdcheck.ExecuteReader();
			    dataReader.Read();

			    if ((int) dataReader[0] < score)
			    {
				    connection.Close();
				    connection.Open();
				    NpgsqlCommand cmdsend = new NpgsqlCommand(
					    "Update qandauser set bestscore=" + score + " where nickname='" + Username + "'",
					    connection);
				    cmdsend.ExecuteNonQuery();
			    }

			    connection.Close();
		    }
		    catch
		    {
			    MessageBox.Show("Something went wrong with saving your score, sorry");
		    }



	    }

	    int Points()
	    {
		    DateTime stopTime = DateTime.Now;
		    TimeSpan difference= stopTime - startTime;
			return difference.Milliseconds;
	    }

		private void ButtonAddQuestionClick(object sender, RoutedEventArgs e)
		{
			_mainGrid.Children.Clear();
			_mainGrid.Children.Add(new AddQuestion(_mainGrid, Username));

		}
	}
}
