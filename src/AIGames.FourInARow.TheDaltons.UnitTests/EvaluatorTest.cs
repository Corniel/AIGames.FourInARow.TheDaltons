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
		public void GetScore_FieldWithForcedYel_WinsOn20()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,2,2,0,2
				0,0,1,2,1,0,1
				0,0,2,1,2,0,2
				0,0,1,2,1,0,1");

			var act = Evaluator.GetScore(field, 16);
			var exp = Scores.YelWins[20];
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
			var exp = Scores.YelWins[38];
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
			var exp = Scores.RedWins[39];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
	}
}
