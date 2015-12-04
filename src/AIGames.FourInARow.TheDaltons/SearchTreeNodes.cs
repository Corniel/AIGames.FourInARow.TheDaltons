using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeNodes : List<ISearchTreeNode>
	{
		public SearchTreeNodes() : base(7) { }

		public void SortAsc()
		{
			for (var index = Count -1; index >= 0; index--)
			{
				var val = this[index].Score;

				for (var swap = index + 1; swap < Count; swap++)
				{
					var other = this[swap];

					if (val >= other.Score) { break; }
					this[swap] = this[swap - 1];
					this[swap - 1] = other;
				}
			}
		}
		public void SortDesc()
		{
			for (var index = Count - 1; index >= 0; index--)
			{
				var val = this[index].Score;

				for (var swap = index + 1; swap < Count; swap++)
				{
					var other = this[swap];

					if (val <= other.Score) { break; }
					this[swap] = this[swap - 1];
					this[swap - 1] = other;
				}
			}
		}
	}
}
