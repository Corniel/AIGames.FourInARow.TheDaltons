using AIGames.FourInARow.TheDaltons.Communication;
using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Communication
{
	[TestFixture]
	public class MoveInstructionTest
	{
		[Test]
		public void Ctor_1_NoMoves()
		{
			var instruction = new MoveInstruction(1);
			var act = instruction.ToString();
			var exp = "place_disc 1";

			Assert.AreEqual(exp, act);
		}
	}
}
