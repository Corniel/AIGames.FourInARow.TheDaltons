﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class FieldTest
	{
		[Test]
		public void GetParseRows_None_AllUniqueWithLength7()
		{
			var keyPattern = new Regex("^[01]{7}$");
			var actual = Field.GetParseRows();

			Assert.AreEqual(128, actual.Count, "128 items");
			Assert.AreEqual(127UL, actual.Values.Max(), "Maximum value should be 127");
			CollectionAssert.AllItemsAreUnique(actual.Values, "All values should be unique.");
			Assert.IsTrue(actual.Keys.All(key =>keyPattern.IsMatch(key)), "All keys should match the pattern.");
		}

		[Test]
		public void Count_Empty_0()
		{
			var act = Field.Empty.Count;
			var exp = 0;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Count_SomeField_7()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,2,0,0;
				0,0,2,1,2,0,0");

			var act = field.Count;
			var exp = 7;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetScores_None_AllUnique()
		{
			var actual = Field.GetScores();

			Assert.AreEqual(69, actual.Length, "69 items");

			foreach (var score in actual)
			{
				Console.WriteLine(Field.ToString(score));
				Console.WriteLine();
			}
		}

		[Test]
		public void IsScoreRed_FieldWithScore_IsTrue()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,2,0,0;
				0,0,2,1,2,0,0");

			var act = field.IsScoreRed();
			var exp = true;

			Assert.AreEqual(exp, act);
		}
		[Test]
		public void IsScoreYellow_FieldWithScore_IsTrue()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,2,0,0,0;
				0,0,0,1,2,0,0;
				0,0,1,1,2,2,1;
				0,0,2,1,2,1,2");

			var act = field.IsScoreYellow();
			var exp = true;

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Parse_FirsRow_AreEqual()
		{
			var str = "0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;1,0,0,0,0,0,2";
			var act = Field.Parse(str);
			var exp = new Field(0x40, 0x01);

			Assert.AreEqual(exp, act);
			Assert.AreEqual(str, exp.ToString(), "ToString() should be equal to the input.");
		}
	}
}
