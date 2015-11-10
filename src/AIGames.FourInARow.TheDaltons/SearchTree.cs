using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTree
	{
		public SearchTree()
		{
			Sw = new Stopwatch();
			Logger = new StringBuilder();
			Generator = new MoveGenerator();
			Rnd = new Random();
			Init();
		}

		private void Init()
		{
			foreach (var field in Book.GetLoss())
			{
				var ply = field.Count + 1;
				var redToMove = (ply & 1) == 1;

				if (redToMove)
				{
					tree[ply][field] = new SearchTreeRedNode(field, (byte)ply, Scores.YelMin);
				}
				else
				{
					tree[ply][field] = new SearchTreeRedNode(field, (byte)ply, Scores.YelMin);
				}
			}
			foreach (var field in Book.GetDraws())
			{
				var ply = field.Count + 1;
				var redToMove = (ply & 1) == 1;

				if (redToMove)
				{
					tree[ply][field] = new SearchTreeRedNode(field, (byte)ply, Scores.Draw);
				}
				else
				{
					tree[ply][field] = new SearchTreeRedNode(field, (byte)ply, Scores.Draw);
				}
			}
		}

		public MoveGenerator Generator { get; set; }
		public bool TimeLeft { get { return Sw.Elapsed < Max; } }
		protected TimeSpan Max { get; set; }
		protected Random Rnd { get; set; }

		public Stopwatch Sw { get; protected set; }
		public StringBuilder Logger { get; protected set; }

		public SearchTreeNode Root { get; set; }

		public byte GetMove(Field field, int ply, TimeSpan min, TimeSpan max)
		{
			var maxDepth = ply < 10 ? 9 : 43;
			var redToMove = (ply & 1) == 1;

			Sw.Restart();
			Logger.Clear();
			Max = max;

			byte move = 3;

			var lookup = CreateLookup(field, ply);
			// Instant winning is not handled properly eslewhere.
			foreach (var key in lookup.Keys)
			{
				if(lookup.Count == 1 || (redToMove ? key.IsScoreRed() : key.IsScoreYellow()))
				{
					return lookup[key];
				}
			}

			Root = GetNode(field, (byte)ply);
			var count = Count;

			for (byte depth = (byte)(ply + 1); TimeLeft && depth < maxDepth; depth++)
			{
				Root.Apply(depth, this, int.MinValue, int.MaxValue);

				move = GetMove(Root, lookup);

				var log = new PlyLog(ply, move, Root.Score, depth, Sw.Elapsed);
				Logger.Append(log).AppendLine();

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
				tree[i].Clear();
				trans[i] = 0;
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
				// Losses and draws are already added, so the missings a wins.
				if (ply == 9)
				{
					if (redToMove)
					{
						tree[ply][search] = new SearchTreeRedNode(search, ply, Scores.RedMin);
					}
					else
					{
						tree[ply][search] = new SearchTreeRedNode(search, ply, Scores.RedMin);
					}
				}
				else
				{
					var score = Evaluator.GetScore(search, ply);

					// If the node is final for the other color, no need to search deeper.
					if (score >= Scores.RedMin || score <= Scores.YelMin)
					{
						node = new SearchTreeEndNode(search, ply, score);
					}
					else if (ply == 42)
					{
						node = new SearchTreeEndNode(search, 42, 0);
					}
					else if (redToMove)
					{
						node = new SearchTreeRedNode(search, ply, score);
					}
					else
					{
						node = new SearchTreeYellowNode(search, ply, score);
					}
					tree[ply][search] = node;
				}
			}
			else
			{
				trans[ply]++;
			}
			return node;
		}

		private Dictionary<Field, SearchTreeNode>[] tree = GetTree();
		private int[] trans = new int[43];
		private static Dictionary<Field, SearchTreeNode>[] GetTree()
		{
			var tree = new Dictionary<Field, SearchTreeNode>[43];
			for (var ply = 0; ply < 43; ply++)
			{
				tree[ply] = new Dictionary<Field, SearchTreeNode>();
			}
			return tree;
		}
		public int Transpositions { get { return trans.Sum(); } }
		public int NodeCount { get { return tree.Sum(item => item.Count); } }
		public int Count { get { return NodeCount + Transpositions; } }
	}
}
