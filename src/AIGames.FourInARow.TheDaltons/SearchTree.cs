using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			var moves = Generator.GetMoves(field, (ply & 1) == 1);
			var root = GetNode(field, (byte)ply);

			for (byte depth = (byte)(ply + 1); TimeLeft && depth < 43; depth++)
			{
				root.Apply(depth, this, int.MinValue, int.MaxValue);
				Logger.AppendFormat("{1} {2}, Score: {0}, Nodes: {3} ({4:0.00}k/s), Trans: {5}",
					root.Score,
					Sw.Elapsed,
					depth - ply,
					Count,
					(Count + Transpositions) / (Sw.Elapsed.TotalMilliseconds),
					Transpositions).AppendLine();

				if (Sw.Elapsed > min) { break; }
			}
			for (byte col = 0; col < moves.Length; col++)
			{
				var pos = moves[col];
				if (pos == root.Best)
				{
					return col;
				}
			}
			return 3;
		}

		public void Clear(int round)
		{
			for (var i = 1; i < round; i++)
			{
				tree[i] = null;
			}
		}

		public SearchTreeNode GetNode(Field search, byte ply)
		{
			SearchTreeNode node;

			var odd = (ply & 1) == 1;

			if (!tree[ply].TryGetValue(search, out node))
			{
				if (ply > 7 && odd ? search.IsScoreRed() : search.IsScoreYellow())
				{
					node = new SearchTreeEndNode(search, ply, odd);
				}
				else if (odd)
				{
					node = new SearchTreeRedNode(search, ply);
				}
				else
				{
					node = new SearchTreeYellowNode(search, ply);
				}
				tree[ply][search] = node;
				Count++;
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
		public int Count { get; protected set; }
	}
}
