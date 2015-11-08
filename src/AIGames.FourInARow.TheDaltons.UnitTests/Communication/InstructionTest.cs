using AIGames.FourInARow.TheDaltons.Communication;
using NUnit.Framework;
using System;

namespace AIGames.FourInARow.TheDaltons.UnitTests.Communication
{
	[TestFixture]
	public class InstructionTest
	{
		#region Action

		[Test]
		public void Parse_RequestMoveInstruction_12345ms()
		{
			var act = Instruction.Parse("action move 12345");
			var exp = new RequestMoveInstruction(TimeSpan.FromMilliseconds(12345));

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Settings

		[Test]
		public void Parse_YourBotInstruction_Player2()
		{
			var act = Instruction.Parse("settings your_bot player2");
			var exp = new YourBotInstruction(PlayerName.Player2);

			Assert.AreEqual(exp, act);
		}

		#endregion
	}
}
