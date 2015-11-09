using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTree
	{
		public SearchTree()
		{
			Sw = new Stopwatch();
			Logger = new StringBuilder();
			Generator = new MoveGenerator();
		}

		public MoveGenerator Generator { get; set; }
		public bool TimeLeft { get { return Sw.Elapsed < Max; } }
		protected TimeSpan Max { get; set; }

		public Stopwatch Sw { get; protected set; }
		public StringBuilder Logger { get; protected set; }

		public byte GetMove(Field field, int ply, TimeSpan min, TimeSpan max)
		{
			if (ply == 1) { return 3; }

			Sw.Restart();
			Logger.Clear();
			Max = max;

			byte move = 3;

			var lookup = CreateLookup(field, ply);
			var root = GetNode(field, (byte)ply);
			var count = Count;

			for (byte depth = (byte)(ply); TimeLeft && depth < 43; depth++)
			{
				root.Apply(depth, this, int.MinValue, int.MaxValue);

				move = GetMove(root, lookup);

				Logger.AppendFormat("{2} {3}, Move: {0}, Score: {1}, Nodes: {4} ({5:0.00}k/s), Trans: {6}",
					move,
					root.Score,
					Sw.Elapsed,
					depth - ply,
					NodeCount,
					Count / (Sw.Elapsed.TotalMilliseconds),
					Transpositions).AppendLine();

				// Don't spoil time.
				if (Sw.Elapsed > min || Count == count) { break; }
				count = Count;
			}
			return move;
		}

		public static byte GetMove(SearchTreeNode node, Dictionary<Field, byte> lookup)
		{
			var max = node.Score;
			var children = node.GetChildren()
				.Where(ch => ch.Score == max)
				.Select(ch => ch.Field)
				.ToList();

			var moves = lookup.Where(kvp => children.Contains(kvp.Key))
				.Select(kvp => kvp.Value)
				.ToArray();

			if (moves.Length > 0)
			{
				// with multiple options choose the middle one.
				return moves[moves.Length >> 1];
			}
			return 3;
		}

		private Dictionary<Field, byte> CreateLookup(Field field, int ply)
		{
			var lookup = new Dictionary<Field, byte>();
			var moves = Generator.GetMoves(field, (ply & 1) == 1);
			for (byte col = 0; col < moves.Length; col++)
			{
				var child = moves[col];
				if (child != Field.Empty)
				{
					lookup[child] = col;
				}
			}
			return lookup;
		}

		public void Clear(int round)
		{
			for (var i = 1; i < round; i++)
			{
				tree[i] = null;
			}
		}

		/// <summary>Gets a node with the field to search for.</summary>
		/// <param name="search">
		/// The field to search for.
		/// </param>
		/// <param name="ply">
		/// The current ply. This should be 1 higher than the discs at the field.
		/// </param>
		/// <returns>
		/// An existing node if already existing, otherwise a new one.
		/// </returns>
		public SearchTreeNode GetNode(Field search, byte ply)
		{
			SearchTreeNode node;

			var redToMove = (ply & 1) == 1;

			if (!tree[ply].TryGetValue(search, out node))
			{
				// If the node is final for the other color, no need to search deeper.
				if (ply > 7 && redToMove ? search.IsScoreYellow() : search.IsScoreRed())
				{
					node = new SearchTreeEndNode(search, ply, redToMove);
				}
				else if (redToMove)
				{
					node = new SearchTreeRedNode(search, ply);
				}
				else
				{
					node = new SearchTreeYellowNode(search, ply);
				}
				tree[ply][search] = node;
				NodeCount++;
			}
			else
			{
				Transpositions++;
			}
			return node;
		}

		private Dictionary<Field, SearchTreeNode>[] tree = GetTree();
		private static Dictionary<Field, SearchTreeNode>[] GetTree()
		{
			var tree = new Dictionary<Field, SearchTreeNode>[43];
			for (var ply = 1; ply < 43; ply++)
			{
				tree[ply] = new Dictionary<Field, SearchTreeNode>();
			}
			return tree;
		}
		public int Transpositions { get; protected set; }
		public int NodeCount { get; protected set; }
		public int Count { get { return NodeCount + Transpositions; } }
	}
}
