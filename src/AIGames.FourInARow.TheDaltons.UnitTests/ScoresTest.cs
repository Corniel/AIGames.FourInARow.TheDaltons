using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class ScoresTest
	{
		[Test]
		public void GetPlyToWinning_Min15_ByteMax()
		{
			byte exp = byte.MaxValue;
			byte act = Scores.GetPlyToWinning(-15);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetPlyToWinning_YelWins30_30()
		{
			byte exp = 30;
			byte act = Scores.GetPlyToWinning(Scores.YelWins[30]);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetPlyToWinning_RedWins31_31()
		{
			byte exp = 31;
			byte act = Scores.GetPlyToWinning(Scores.RedWins[31]);

			Assert.AreEqual(exp, act);
		}
	}
}
