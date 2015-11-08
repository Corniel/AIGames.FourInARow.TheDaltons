using AIGames.FourInARow.TheDaltons.Communication;
using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Communication
{
	[TestFixture]
	public class MoveInstructionTest
	{
		[Test]
		public void Ctor_1_PlaceDisc5()
		{
			var instruction = new MoveInstruction(1);
			var act = instruction.ToString();
			var exp = "place_disc 5";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Ctor_3_PlaceDisc3()
		{
			var instruction = new MoveInstruction(3);
			var act = instruction.ToString();
			var exp = "place_disc 3";

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Ctor_6_PlaceDisc0()
		{
			var instruction = new MoveInstruction(6);
			var act = instruction.ToString();
			var exp = "place_disc 0";

			Assert.AreEqual(exp, act);
		}
	}
}
