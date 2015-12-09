using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class EvaluatorTest
	{
		[Test]
		public void GetScore_FieldWithForcedRed_WinsOn21()
		{
			AssertScore(Scores.RedWins[21], @"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,1,0,1
				0,0,2,1,2,0,2
				0,0,1,2,1,0,1
				0,2,2,1,2,0,2");
		}


		[Test]
		public void GetScore_FieldWithTwoDelays_WinsOn41()
		{
			AssertScore(Scores.RedWins[41], @"
				1,1,0,1,0,0,2
				2,2,0,1,1,1,2
				1,1,0,2,2,1,1
				2,2,0,1,2,2,2
				1,2,2,2,1,1,1
				1,1,2,1,2,2,2");
		}
		[Test]
		public void GetScore_FieldWithOneDelay_WinsOn41()
		{
			AssertScore(Scores.RedWins[41], @"
				1,1,0,1,1,0,2
				2,2,0,1,1,1,2
				1,1,0,2,2,1,1
				2,2,0,1,2,2,2
				1,2,2,2,1,1,1
				1,1,2,1,2,2,2");
		}

		[Test]
		public void GetScore_FieldWith26Delays_WinsOn14()
		{
			AssertScore(Scores.YelWins[14], @"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,2,0
				0,0,1,1,0,1,0
				0,0,2,2,0,2,0
				0,0,2,1,0,1,0");
		}
		[Test]
		public void GetScore_FieldWith25Delays_WinsOn14()
		{
			AssertScore(Scores.YelWins[14], @"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,2,0
				0,0,1,1,0,1,0
				0,0,2,2,0,2,0
				0,1,2,1,0,1,0");
		}
		

		[Test]
		public void GetScore_FieldWithForcedYel_WinsOn22()
		{
			AssertScore(Scores.YelWins[22], @"
				0,0,0,0,0,0,0
				0,0,0,1,0,0,0
				0,0,0,2,2,0,2
				0,0,1,2,1,0,1
				0,0,2,1,2,0,1
				0,1,2,2,1,0,1");
		}

		[Test]
		public void GetScore_FieldWithEvenThreatForYel_WinsOn38()
		{
			AssertScore(-1001, @"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				2,2,0,2,1,0,0
				1,1,0,2,1,0,0");
		}

		[Test]
		public void GetScore_FieldWithOddThreatForRed_WinsOn39()
		{
			AssertScore(+1001, @"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				1,1,0,1,0,0,0
				2,2,0,1,1,2,2
				1,1,0,2,1,2,2");
		}

		[Test]
		public void GetScore_PostionOn40Col0_RedWins39()
		{
			AssertScore(Scores.RedWins[39], @"
				1,1,2,1,2,1,0
				2,2,1,1,1,2,0
				1,2,1,2,2,1,0
				2,2,2,1,1,1,1
				2,1,1,1,2,2,2
				1,1,2,1,2,2,2");
		}

		[Test]
		public void GetScore_PostionOn40Col5_RedWins39()
		{
			AssertScore(Scores.RedWins[39], @"
				2,0,1,1,2,1,1
				2,0,1,1,1,2,1
				1,0,1,2,2,1,1
				2,1,2,1,1,2,2
				2,2,1,2,2,1,2
				1,2,2,1,1,2,2");
		}

		[Test]
		public void GetScore_DrawOn42_0()
		{
			AssertScore(Scores.Draw, @"
				2,1,1,2,2,1,1
				1,2,2,2,1,2,2
				2,1,1,2,2,1,1
				1,2,1,1,1,2,1
				2,1,1,2,2,2,1
				1,1,2,1,2,2,2");
		}

		public void AssertScore(int expected, string str)
		{
			var field = Field.Parse(str);
			var ply = (byte)(field.Count + 1);
			var red = Bits.Count(field.GetRed());
			var yel = Bits.Count(field.GetYellow());

			var dif = red - yel;

			if (dif < 0 || dif > 1)
			{
				Assert.Fail("Invalid field: {0} red, {1} yellow.", red, yel);
			}

			var actual = Evaluator.GetScore(field, ply);

			if (actual != expected)
			{
				Assert.Fail("Expected: {0} but was {1}", Scores.GetFormatted(expected), Scores.GetFormatted(actual));
			}
		}
	}
}
