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
		public void GetScore_FieldWithForcedRed_WinsOn23()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,1,0,1
				0,0,2,1,2,0,2
				0,0,1,2,1,0,1
				0,2,2,1,2,0,2");

			var act = Evaluator.GetScore(field, 17);
			var exp = Scores.RedWins[23];
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}

		[Test]
		public void GetScore_FieldWithForcedYel_WinsOn24()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,1,0,0,0
				0,0,0,2,2,0,2
				0,0,1,2,1,0,1
				0,0,2,1,2,0,1
				0,1,2,2,1,0,1");
			var act = Evaluator.GetScore(field, 18);
			var exp = Scores.YelWins[24];
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
	}
}
