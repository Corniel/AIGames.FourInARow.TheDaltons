using System.Collections.Generic;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	public class LastBookNode: SearchTreeRedNode
	{
		public LastBookNode(Field field, int value) : base(field, 9, value) { }

		public override int Apply(byte depth, SearchTree tree, int alpha, int beta)
		{
			// If no children, get moves.
			if (children == null)
			{
				var items = tree.Generator.GetMoves(Field, (Depth & 1) == 1);
				var childDepth = (byte)(Depth + 1);

				children = new List<SearchTreeYellowNode>();

				foreach (var item in items.Where(e => e != Field.Empty))
				{
					var child = tree.GetNode(item, childDepth);
					if (child is SearchTreeYellowNode)
					{
						children.Add((SearchTreeYellowNode)child);
					}
					// one of the responses is losing. So this tree is losing.
					else if (child is SearchTreeEndNode)
					{
						children.Clear();
						Score = child.Score;
						return Score;
					}
				}
			}
			foreach (var child in children)
			{
				child.Apply(depth, tree, SearchTreeNode.InitialAlpha, SearchTreeNode.InitialBeta);
			}
			children.Sort();
			return Value;
		}
	}
}
