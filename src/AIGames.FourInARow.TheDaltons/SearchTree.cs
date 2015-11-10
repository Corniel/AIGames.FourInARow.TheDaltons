using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTree : ISearchTree
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
				tree[9][field] = new SearchTreeBookNode(field, Scores.YelWin);
			}
			foreach (var field in Book.GetDraws())
			{
				tree[9][field] = new SearchTreeBookNode(field, Scores.Draw);
			}
		}

		public MoveGenerator Generator { get; set; }
		public bool TimeLeft { get { return Sw.Elapsed < Max; } }
		protected TimeSpan Max { get; set; }
		protected Random Rnd { get; set; }

		public Stopwatch Sw { get; protected set; }
		public StringBuilder Logger { get; protected set; }

		public ISearchTreeNode Root { get; set; }

		public byte GetMove(Field field, byte ply, TimeSpan min, TimeSpan max)
		{
			var redToMove = (ply & 1) == 1;

			byte maxDepth = 43;
			byte minDepth = (byte)(ply + 1);
			
			Sw.Restart();
			Logger.Clear();
			Max = max;

			var candidates = new MoveCandidates(redToMove);
			candidates.Add(field, ply, this);
			var move = candidates.GetMove();
			
			Root = GetNode(field, ply);

			for (byte depth = minDepth; depth < maxDepth; depth++)
			{
				Root.Apply(depth, this, Scores.InitialAlpha, Scores.InitialBeta);

				move = candidates.GetMove();

				var log = new PlyLog(ply, move, Root.Score, depth, Sw.Elapsed);
				Logger.Append(log).AppendLine();

				// Don't spoil time.
				if (Root.IsFinal || Sw.Elapsed > min || !TimeLeft) { break; }
			}
			return move;
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
						node = new SearchTreeBookNode(search, Scores.RedWin);
					}
				}
				else
				{
					var score = Evaluator.GetScore(search, ply);

					// If the node is final for the other color, no need to search deeper.
					if (score >= Scores.RedWin || score <= Scores.YelWin)
					{
						node = new SearchTreeEndNode(search, ply, score);
					}
					// Game is done.
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

		public Field[] GetMoves(Field field, bool IsRed)
		{
			return Generator.GetMoves(field, IsRed);
		}

		private Dictionary<Field, ISearchTreeNode>[] tree = GetTree();
		private int[] trans = new int[43];
		private static Dictionary<Field, ISearchTreeNode>[] GetTree()
		{
			var tree = new Dictionary<Field, ISearchTreeNode>[43];
			for (var ply = 0; ply < 43; ply++)
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
