using NUnit.Framework;
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
