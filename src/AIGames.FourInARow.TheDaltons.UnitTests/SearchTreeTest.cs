using NUnit.Framework;
using System;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class SearchTreeTest
	{
		[Test, Category(Category.IntegrationTest)]
		public void GetMove_Initial_Performance()
		{
			var tree = new SearchTree();
			var act = tree.GetMove(Field.Empty, 1, TimeSpan.MaxValue, TimeSpan.FromSeconds(5));
			Console.WriteLine(tree.Logger);

			var exp = (byte)3;
			Assert.AreEqual(exp, act);
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_AlmostInitial_Performance()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,2,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 3, TimeSpan.MaxValue, TimeSpan.FromSeconds(10));
			Console.WriteLine(tree.Logger);
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_ResponseOnCol0_Performance()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,1");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 2, TimeSpan.MaxValue, TimeSpan.FromSeconds(2));
			Console.WriteLine(tree.Logger);
			Assert.AreEqual(Scores.YelWin, tree.Root.Score);
		}
		[Test, Category(Category.IntegrationTest)]
		public void GetMove_ResponseOnCol2_Performance()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,1,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 2, TimeSpan.MaxValue, TimeSpan.FromSeconds(2));
			Console.WriteLine(tree.Logger);
			Assert.AreNotEqual(Scores.RedWin, tree.Root.Score);
		}
		[Test, Category(Category.IntegrationTest)]
		public void GetMove_ResponseOnCol3_Performance()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,1,0,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 2, TimeSpan.MaxValue, TimeSpan.FromSeconds(2));
			Console.WriteLine(tree.Logger);
			Assert.AreEqual(Scores.RedWin, tree.Root.Score);
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_3MovesDone_Performance()
		{
			var field = Field.Parse(@"
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,0,0,0;
				0,0,0,0,1,0,0;
				0,0,0,1,2,0,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 4, TimeSpan.MaxValue, TimeSpan.FromSeconds(10));
			Console.WriteLine(tree.Logger);
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
			var act = tree.GetMove(field, 6, TimeSpan.MaxValue, TimeSpan.FromSeconds(1));
			Console.WriteLine(tree.Logger);

			var exp = (byte)3;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_WinningOption_1()
		{
			var field = Field.Parse(@"
				0,0,0,1,0,0,0;
				0,0,2,1,0,0,0;
				0,0,2,1,0,0,0;
				0,0,1,2,0,0,0;
				0,0,1,1,1,0,0;
				2,0,2,1,2,2,2");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 19, TimeSpan.MaxValue, TimeSpan.FromSeconds(0.1));
			Console.WriteLine(tree.Logger);

			var exp = (byte)1;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_NotInstantLosing_Not5()
		{
			var field = Field.Parse(@"
				0,0,1,2,1,0,0;
				0,0,1,1,2,0,0;
				0,0,2,1,2,0,0;
				2,0,2,1,2,2,0;
				1,2,2,2,1,1,0;
				2,1,1,1,2,1,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 27, TimeSpan.MaxValue, TimeSpan.FromSeconds(1000));
			Console.WriteLine(tree.Logger);

			var exp = (byte)5;
			Assert.AreNotEqual(exp, act);
		}

		[Test]
		public void GetMove_OneColumnLeft_0()
		{
			var field = Field.Parse(@"
				2,2,1,1,2,1,0
				1,1,2,1,2,1,0
				2,2,1,2,2,1,0
				1,2,1,1,1,2,0
				2,1,2,2,2,1,0
				1,2,2,1,1,2,0");
			var tree = new SearchTree();
			var act = tree.GetMove(field, 39, TimeSpan.MaxValue, TimeSpan.FromSeconds(1000));
			Console.WriteLine(tree.Logger);

			var exp = (byte)0;
			Assert.AreEqual(exp, act);
		}

	}
}
