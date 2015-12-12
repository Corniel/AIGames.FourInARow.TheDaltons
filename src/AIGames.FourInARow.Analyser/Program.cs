using System.IO;

namespace AIGames.FourInARow.Analyser
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var anlyser = new GameAnalyser();

			var dir = new DirectoryInfo(@"C:\Code\AIGames.Challenger\games\four-in-a-row");

			anlyser.Analyse(dir);
		}
	}
}
