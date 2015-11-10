using NUnit.Framework;
using System;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class PlyLogTest
	{
		[Test]
		public void ToString_Zero_FormattedString()
		{
			var act = new PlyLog(13, 5, 0, 23, TimeSpan.FromSeconds(0.987));
			var exp = "13/23. =0.00: {5} 0.987s";
			Assert.AreEqual(exp, act.ToString());
		}
		[Test]
		public void ToString_Plus207_FormattedString()
		{
			var act = new PlyLog(13, 5, 207, 23, TimeSpan.FromSeconds(0.987));
			var exp = "13/23. +2.07: {5} 0.987s";
			Assert.AreEqual(exp, act.ToString());
		}
		[Test]
		public void ToString_Min1207_FormattedString()
		{
			var act = new PlyLog(13, 5, -1207, 23, TimeSpan.FromSeconds(0.987));
			var exp = "13/23. -12.07: {5} 0.987s";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void ToString_RedWins_FormattedString()
		{
			var act = new PlyLog(13, 5, Scores.RedWins(20), 23, TimeSpan.FromSeconds(0.987));
			var exp = "13/23. +oo 20: {5} 0.987s";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void ToString_YellowdWins_FormattedString()
		{
			var act = new PlyLog(13, 5, Scores.YelWins(17), 23, TimeSpan.FromSeconds(0.987));
			var exp = "13/23. -oo 17: {5} 0.987s";
			Assert.AreEqual(exp, act.ToString());
		}
	}
}
