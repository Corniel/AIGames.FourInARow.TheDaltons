using System;
using System.Collections.Generic;
namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeKnownNode : SearchTreeSubNode
	{
		public SearchTreeKnownNode(Field field, int score)
			: base(field, (byte)(field.Count +1), score) { }

		/// <summary>Gets the node with a flipped position.</summary>
		public SearchTreeKnownNode Flip()
		{
			return new SearchTreeKnownNode(Field.Flip(), Score);
		}

		protected override int ApplyChildren(byte depth, ISearchTree tree, int alpha, int beta)
		{
			for (var i = 0; i < Children.Count; i++)
			{
				Children[i].Apply(depth, tree, alpha, beta);
			}
			// never update this score.
			return Score;
		}
	}
}
