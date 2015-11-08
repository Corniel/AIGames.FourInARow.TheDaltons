using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTree
	{
		public SearchTree()
		{
			Sw = new Stopwatch();
			Generator = new MoveGenerator();
		}

		public MoveGenerator Generator { get; set; }
		public bool TimeLeft { get { return Sw.Elapsed < Max; } }
		protected TimeSpan Max { get; set; }

		public Stopwatch Sw { get; protected set; }

		public byte GetMove(Field field, int turn, TimeSpan min, TimeSpan max)
		{
			if (turn == 1) { return 3; }
			Sw.Restart();
			Max = max;

			var moves = Generator.GetMoves(field, (turn & 1) == 1);
			var root = GetNode(field, (byte)turn);

			for (byte i = (byte)(turn + 1); TimeLeft && i < 43; i++)
			{
				root.Apply(i, this, int.MinValue, int.MaxValue);
				//Console.WriteLine("{0} {1}, Nodes: {2} ({3:0.00}k/s), Trans: {4}",
				//	Sw.Elapsed,
				//	i,
				//	Count,
				//	(Count + Transpositions) / (Sw.Elapsed.TotalMilliseconds),
				//	Transpositions);
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

		public SearchTreeNode GetNode(Field search, byte turn)
		{
			SearchTreeNode node;

			var odd = (turn & 1) == 1;

			if (!tree[turn].TryGetValue(search, out node))
			{
				if (turn > 7 && odd ? search.IsScoreRed() : search.IsScoreYellow())
				{
					node = new SearchTreeEndNode(search, turn, odd);
				}
				else if (odd)
				{
					node = new SearchTreeRedNode(search, turn);
				}
				else
				{
					node = new SearchTreeYellowNode(search, turn);
				}
				tree[turn][search] = node;
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
			for (var round = 1; round < 43; round++)
			{
				tree[round] = new Dictionary<Field, SearchTreeNode>();
			}
			return tree;
		}
		public int Transpositions { get; protected set; }
		public int Count { get; protected set; }
	}
}
