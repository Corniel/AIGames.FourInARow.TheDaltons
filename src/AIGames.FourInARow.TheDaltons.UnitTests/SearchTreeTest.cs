using NUnit.Framework;
using System;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class SearchTreeTest
	{
		[Test]
		public void GetMove_Initial_3()
		{
			var tree = new SearchTree();
			var act = tree.GetMove(Field.Empty, 1, TimeSpan.MaxValue, TimeSpan.FromSeconds(2000));
			var exp = (byte)3;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_OneOption_3()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,0,0,0;
				0,0,0,1,0,0,0;
				0,0,2,1,2,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 6, TimeSpan.MaxValue, TimeSpan.FromSeconds(2000));
			Console.WriteLine(tree.Logger);

			var exp = (byte)3;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_FieldWith_()
		{
			var field = Field.Parse(@"
				0,0,0,1,1,0,0;
				2,0,0,2,1,0,0;
				2,0,0,1,2,0,0;
				1,0,0,1,1,0,0;
				2,0,0,2,1,0,0;
				2,0,0,1,2,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 18, TimeSpan.MaxValue, TimeSpan.MaxValue);
		}
	}
}
