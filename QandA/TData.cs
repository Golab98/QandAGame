using System.Windows;
using Npgsql;
namespace QandA
{

	class TData
{
	
	public string []Question=new string[5];    //values  Question, Answer1, Answer2, Answer3, CorrectAnswer;

		public TData(int counter)
		{
			try
			{
				string connstring =
					"Server=127.0.0.1; Port=5432; User Id=postgres; Password=password; Database=QandA;";
				NpgsqlConnection connection = new NpgsqlConnection(connstring);
				connection.Open();
				NpgsqlCommand command =
					new NpgsqlCommand("SELECT * FROM qanda where questionid=" + counter, connection);
				NpgsqlDataReader dataReader = command.ExecuteReader();
				dataReader.Read();
				Question[0] = dataReader[1].ToString();
				Question[1] = dataReader[2].ToString();
				Question[2] = dataReader[3].ToString();
				Question[3] = dataReader[4].ToString();
				Question[4] = dataReader[5].ToString();
				connection.Close();
			}
			catch
			{
				MessageBox.Show("nastąpił błąd przy wczytywaniu bazy danych");
			}

		}

	


	
	}
}
