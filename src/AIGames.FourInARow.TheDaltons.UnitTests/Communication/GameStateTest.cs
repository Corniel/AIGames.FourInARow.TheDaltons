using AIGames.FourInARow.TheDaltons.Communication;
using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Communication
{
	[TestFixture]
	public class GameStateTest
	{
		[Test]
		public void Ply_Player1Round1_1()
		{
			var state = new GameState()
			{
				YourBot = PlayerName.Player1,
				Round = 1,
			};

			var act = state.Ply;
			var exp = 1;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Ply_Player1Round10_20()
		{
			var state = new GameState()
			{
				YourBot = PlayerName.Player2,
				Round = 10,
			};

			var act = state.Ply;
			var exp = 20;
			Assert.AreEqual(exp, act);
		}
	}
}
