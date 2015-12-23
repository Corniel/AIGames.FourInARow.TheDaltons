using System.Collections.Generic;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeNodes
	{
		private ISearchTreeNode[] items = new ISearchTreeNode[7];

		public int Count { get; private set; }

		public ISearchTreeNode this[int index]
		{
			get { return items[index]; }
			set { items[index] = value; }
		}

		internal void Add(ISearchTreeNode node)
		{
			items[Count++] = node;
		}
		internal void AddRange(IEnumerable<ISearchTreeNode> nodes)
		{
			foreach (var node in nodes)
			{
				items[Count++] = node;
			}
		}
	}
}
