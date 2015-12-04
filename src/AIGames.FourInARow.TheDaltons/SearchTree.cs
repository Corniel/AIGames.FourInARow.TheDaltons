using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTree : ISearchTree
	{
		public const int MaximumDepth = 43;

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
				tree[9][field] = new SearchTreeBookNode(field, 9, Scores.YelWin >> 3);
			}
			foreach (var field in Book.GetDraws())
			{
				tree[9][field] = new SearchTreeBookNode(field, 9, Scores.Draw);
			}

			var ply1 = Generator.GetMoves(Field.Empty, true);

			tree[1][ply1[0]] = new SearchTreeBookNode(ply1[0], 1, Scores.YelWin >> 3);
			tree[1][ply1[1]] = new SearchTreeBookNode(ply1[1], 1, Scores.YelWin >> 3);
			tree[1][ply1[2]] = new SearchTreeBookNode(ply1[2], 1, Scores.Draw);
			tree[1][ply1[3]] = new SearchTreeBookNode(ply1[3], 1, Scores.RedWin >> 3);
			tree[1][ply1[4]] = new SearchTreeBookNode(ply1[4], 1, Scores.Draw);
			tree[1][ply1[5]] = new SearchTreeBookNode(ply1[5], 1, Scores.YelWin >> 3);
			tree[1][ply1[6]] = new SearchTreeBookNode(ply1[6], 1, Scores.YelWin >> 3);
		}

		public MoveGenerator Generator { get; set; }
		public bool TimeLeft { get { return Sw.Elapsed < Max; } }
		protected TimeSpan Max { get; set; }
		protected Random Rnd { get; set; }

		public Stopwatch Sw { get; protected set; }
		public StringBuilder Logger { get; protected set; }

		public ISearchTreeNode Root { get; set; }

		private byte depth = 1;

		public byte GetMove(Field field, TimeSpan min, TimeSpan max)
		{
			byte ply = (byte)(field.Count + 1);
			if (depth <= ply) { depth = (byte)(ply + 1); }
			var redToMove = (ply & 1) == 1;

			if (depth > MaximumDepth) { depth = MaximumDepth; }

			Sw.Restart();
			Logger.Clear();
			Max = max;

			var candidates = new MoveCandidates(redToMove);
			candidates.Add(field, ply, this);
			var move = candidates.GetMove();
			
			Root = GetNode(field, ply);
			Root.Add(candidates);

			for (/**/; depth <= MaximumDepth; depth++)
			{
				Root.Apply(depth, this, Scores.InitialAlpha, Scores.InitialBeta);

				move = candidates.GetMove();

				var log = new PlyLog(ply, move, Root.Score, depth, Sw.Elapsed);
				Logger.Append(log).AppendLine();

				// Don't spoil time.
				if (Sw.Elapsed > min || !TimeLeft) { break; }
			}
			return move;
		}

		public void Clear(int round)
		{
			for (var i = 1; i < round ; i++)
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
		public ISearchTreeNode GetNode(Field search, byte ply)
		{
			ISearchTreeNode node;

			var redToMove = (ply & 1) == 1;

			if (!tree[ply].TryGetValue(search, out node))
			{
				// Losses and draws are already added, so the missing a wins.
				if (ply == 9)
				{
					if (search.IsScoreYellow())
					{
						node = new SearchTreeEndNode(search, 9, Scores.YelWins(9));
					}
					else
					{
						node = new SearchTreeBookNode(search, 9, Scores.RedWin >> 3);
					}
				}
				else
				{
					var score = Evaluator.GetScore(search, ply);

					// If the node is final for the other color, no need to search deeper.
					if (score == Scores.RedWins(ply) || score == Scores.YelWins(ply))
					{
						node = new SearchTreeEndNode(search, ply, score);
					}
					// Game is done.
					else if (ply == MaximumDepth)
					{
						node = new SearchTreeEndNode(search, MaximumDepth, 0);
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

		public Field[] GetMoves(Field field, bool IsRed)
		{
			return Generator.GetMoves(field, IsRed);
		}

		private Dictionary<Field, ISearchTreeNode>[] tree = GetTree();
		private int[] trans = new int[MaximumDepth + 1];
		private static Dictionary<Field, ISearchTreeNode>[] GetTree()
		{
			var tree = new Dictionary<Field, ISearchTreeNode>[MaximumDepth + 1];
			for (var ply = 0; ply <= MaximumDepth; ply++)
			{
				tree[ply] = new Dictionary<Field, ISearchTreeNode>();
			}
			return tree;
		}
		public int Transpositions { get { return trans.Sum(); } }
		public int NodeCount { get { return tree.Sum(item => item.Count); } }
		public int Count { get { return NodeCount + Transpositions; } }
	}
}
