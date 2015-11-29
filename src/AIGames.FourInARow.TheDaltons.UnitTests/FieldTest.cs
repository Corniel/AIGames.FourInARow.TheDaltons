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
		public void GetHashCode_Empty_0()
		{
			var field = Field.Empty;
			var act = field.GetHashCode();
			var exp = 0;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_SomeField_68422170()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,2,0,0;
				0,0,2,1,2,0,0");
			var act = field.GetHashCode();
			var exp = 68422170;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetHashCode_SomeFieldSlightlyDifferent_68421643()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,1,0,0;
				0,0,0,1,2,0,0;
				0,0,2,1,2,0,0");
			var act = field.GetHashCode();
			var exp = 68421643;
			Assert.AreEqual(exp, act);
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
			var exp = "0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;0,0,0,0,0,0,0;1,0,0,0,0,0,2";
			var act = Field.Parse(exp);
			
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void GetBytes_RoundTrip_AreEqual()
		{
			var field = Field.Parse(@"
				2,2,1,1,2,1,0
				1,1,2,1,2,1,0
				2,2,1,2,2,1,0
				1,2,1,1,1,2,0
				2,1,2,2,2,1,0
				1,2,2,1,1,2,0");

			var bytes = field.GetBytes();
			var act = Field.FromBytes(bytes);

			Assert.AreEqual(field, act);
		}

		[Test]
		public void GetConnect4_None_AllUnique()
		{
			var actual = new FieldConnect4Generator();

			Assert.AreEqual(69, actual.Connect4.Length, "69 items");
			
			Console.WriteLine(FieldConnect4Generator.ToString(actual.Connect4));
			Console.WriteLine(FieldConnect4Generator.ToString(actual.Connect3Out4));
			Console.WriteLine(FieldConnect4Generator.ToString(actual.Connect4Threat));
		}
	}
}
