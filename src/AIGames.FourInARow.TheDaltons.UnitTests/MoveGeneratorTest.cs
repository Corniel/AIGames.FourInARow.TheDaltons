using NUnit.Framework;

namespace AIGames.FourInARow.TheDaltons.UnitTests
{
	[TestFixture]
	public class MoveGeneratorTest
	{
		[Test]
		public void Test()
		{
			var generator = new MoveGenerator();
			var field = Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				1,0,0,1,2,0,0");

			var act = generator.GetMoves(field, true);
			var exp = new Field[]
			{
				// Column 0
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				1,0,0,1,2,0,1"),

				// Column 1
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				1,0,0,1,2,1,0"),

				// Column 2
				Field.Empty,

				// Column 3
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,1,1,0,0;
				1,0,0,1,2,0,0"),

				// Column 4
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				1,0,1,1,2,0,0"),

				// Column 5
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				1,1,0,1,2,0,0"),
				
				// Column 6
				Field.Parse(@"
				0,0,0,0,2,0,0;
				0,0,0,0,1,0,0;
				0,0,0,0,2,0,0;
				0,0,0,0,2,0,0;
				1,0,0,0,1,0,0;
				1,0,0,1,2,0,0"),
			};

			CollectionAssert.AreEqual(exp, act);

		}
	}
}
