using AIGames.FourInARow.Analyser;
using NUnit.Framework;
using System.IO;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Analyser
{
	[TestFixture]
	public class GameAnalyserTest
	{
		[Test]
		public void Analyse_GameWithWinForYellow_()
		{
			var analyser = new GameAnalyser();
			analyser.Load(new FileInfo(@"C:\Code\AIGames.FourInARow.TheDaltons\games\game0.dat"));
			analyser.Analyse();
		}
	}
}
