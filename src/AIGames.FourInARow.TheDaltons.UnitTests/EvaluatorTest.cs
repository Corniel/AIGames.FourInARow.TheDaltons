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
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,1,0,1
				0,0,2,1,2,0,2
				0,0,1,2,1,0,1
				0,2,2,1,2,0,2");

			var act = Evaluator.GetScore(field, 17);
			var exp = Scores.RedWins[21];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}


		[Test]
		public void GetScore_FieldWithTwoDelays_WinsOn41()
		{
			var field = Field.Parse(@"
				1,1,0,1,0,0,2
				2,2,0,1,1,1,2
				1,1,0,2,2,1,1
				2,2,0,1,2,2,2
				1,2,2,2,1,1,1
				1,1,2,1,2,2,2");

			var act = Evaluator.GetScore(field, 37);
			var exp = Scores.RedWins[41];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
		[Test]
		public void GetScore_FieldWithOneDelay_WinsOn41()
		{
			var field = Field.Parse(@"
				1,1,0,1,1,0,2
				2,2,0,1,1,1,2
				1,1,0,2,2,1,1
				2,2,0,1,2,2,2
				1,2,2,2,1,1,1
				1,1,2,1,2,2,2");

			var act = Evaluator.GetScore(field, 38);
			var exp = Scores.RedWins[41];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_FieldWith26Delays_WinsOn14()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,2,0
				0,0,1,1,0,1,0
				0,0,2,2,0,2,0
				0,0,2,1,0,1,0");

			var act = Evaluator.GetScore(field, 11);
			var exp = Scores.YelWins[14];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
		[Test]
		public void GetScore_FieldWith25Delays_WinsOn14()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,2,0
				0,0,1,1,0,1,0
				0,0,2,2,0,2,0
				0,1,2,1,0,1,0");

			var act = Evaluator.GetScore(field, 12);
			var exp = Scores.YelWins[14];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
		

		[Test]
		public void GetScore_FieldWithForcedYel_WinsOn22()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,1,0,0,0
				0,0,0,2,2,0,2
				0,0,1,2,1,0,1
				0,0,2,1,2,0,1
				0,1,2,2,1,0,1");
			var act = Evaluator.GetScore(field, 18);
			var exp = Scores.YelWins[22];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_FieldWithEvenThreatForYel_WinsOn38()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				2,2,0,2,1,0,0
				1,1,0,2,1,0,0");

			var act = Evaluator.GetScore(field, 9);
			var exp = -1001;
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_FieldWithOddThreatForRed_WinsOn39()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				1,1,0,1,0,0,0
				2,2,0,1,1,2,2
				1,1,0,2,1,2,2");

			var act = Evaluator.GetScore(field, 16);
			var exp = 1001;
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_PostionOn40Col0_RedWins39()
		{
			var field = Field.Parse(@"
				1,1,2,1,2,1,0
				2,2,1,1,1,2,0
				1,2,1,2,2,1,0
				2,2,2,1,1,1,1
				2,1,1,1,2,2,2
				1,1,2,1,2,2,2");

			var act = Evaluator.GetScore(field, 40);
			var exp =  Scores.RedWins[39];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_PostionOn40Col5_RedWins39()
		{
			var field = Field.Parse(@"
				2,0,1,1,2,1,1
				2,0,1,1,1,2,1
				1,2,1,2,2,1,1
				2,1,2,1,1,2,2
				2,2,1,2,2,1,2
				1,2,2,1,1,2,2");

			var act = Evaluator.GetScore(field, 40);
			var exp =  Scores.RedWins[39];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_DrawOn42_0()
		{
			var field = Field.Parse(@"
				2,1,1,2,2,1,1
				1,2,2,2,1,2,2
				2,1,1,2,2,1,1
				1,2,1,1,1,2,1
				2,1,1,2,2,2,1
				1,1,2,1,2,2,2");

			var act = Evaluator.GetScore(field, 42);
			var exp =  Scores.Draw;
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
	}
}
