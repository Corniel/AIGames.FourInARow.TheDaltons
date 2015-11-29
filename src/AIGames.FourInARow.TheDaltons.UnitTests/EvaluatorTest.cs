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
		public void GetScore_FieldWithForcedRed_WinOn21()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0
				0,0,0,0,0,0,0
				0,0,0,1,1,0,1
				0,0,2,1,2,0,2
				0,0,1,2,1,0,1
				0,2,2,1,2,0,2");

			var act = Evaluator.GetScore(field, 17);
			var exp = Scores.RedWins(21);
			Assert.AreEqual(Scores.GetFormatted(exp), Scores.GetFormatted(act));
		}
	}
}
