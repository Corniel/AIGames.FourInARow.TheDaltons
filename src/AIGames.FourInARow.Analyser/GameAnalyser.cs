using AIGames.FourInARow.TheDaltons;
using AIGames.FourInARow.TheDaltons.Communication;
using Qowaiv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AIGames.FourInARow.Analyser
{
	public class GameAnalyser
	{
		public int MinPly = 15;
		public GameAnalyser()
		{
			Duration = TimeSpan.FromSeconds(15);
			Fields = new List<Field>();
			Nodes = new SearchTreeKnownNodes();
		}

		public TimeSpan Duration { get; set; }
		public List<Field> Fields { get; set; }
		public SearchTreeKnownNodes Nodes { get; set; }

		public void Analyse(DirectoryInfo dir)
		{
			var sw = Stopwatch.StartNew();
			try
			{
				Nodes = SearchTreeKnownNodes.Load(dir);
				Console.WriteLine("Loaded {0} positions.", Nodes.Count);
			}
			catch (Exception x)
			{
				Console.WriteLine(x.Message);
			}

			var files = dir.GetFiles("*.log").OrderByDescending(f => f.Name).ToList();

			var saved = 0;
			var skipped = 0;
			double total = files.Count;

			foreach (var file in files)
			{
				Console.Write("\r");
				Console.Write(@"{4:hh\:mm\:ss} Saved: {0}, skipped: {1}, {2} out {3} ({5:0.00%})",
						saved, skipped, saved + skipped, total, sw.Elapsed, (Percentage)((saved + skipped)/total));

				Load(file);

				if(Fields.Any(field => 
					Nodes.ContainsKey(field) || 
					Nodes.ContainsKey(field.Flip())))
				{
					skipped++;
					continue;
				}
				if (Fields.Count >= MinPly && Fields.Last().Count < 42)
				{
					Console.WriteLine();
					if (Analyse())
					{
						saved++;
						Nodes.Save(dir);
					}
					else
					{
						skipped++;
					}
				}
				else { skipped++; }
			}
		}

		public void Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				Load(stream);
			}
		}

		public void Load(Stream stream)
		{
			Fields.Clear();
			var reader = new StreamReader(stream);
			var instructions = Instruction.Read(reader);

			foreach (var instruction in instructions)
			{
				if (instruction is FieldInstruction)
				{
					var field = ((FieldInstruction)instruction).Field;
					Fields.Add(field);
				}
			}
		}

		public bool Analyse()
		{
			var tree = new SearchTree();

			var last = Fields.Last();

			if (!last.IsScoreRed() && !last.IsScoreYellow())
			{
				tree.GetMove(last, Duration, Duration);
			}
			var node = tree.GetNode(last, (byte)(last.Count + 1));

			if (!Scores.IsWinning(node.Score)) { return false; }

			var redToMove = node.Score > 0;
			
			tree.Initialize(Nodes.Values);
			var fields = Fields
				.Where(f => f.RedToMove == redToMove)
				.OrderByDescending(f => f.Count)
				.ToList();

			Field target = Field.Empty;
			var score = 0;

			foreach (var field in fields)
			{
				var loop = Stopwatch.StartNew();
				tree.GetMove(field, Duration, Duration);

				Console.WriteLine(tree.Logger);

				var test = tree.Root.Score;
				if (redToMove && test < Scores.RedWin)
				{
					break;
				}
				else if (!redToMove && test > Scores.YelWin)
				{
					break;
				}
				target = field;
				score = test;

				if (loop.Elapsed >= Duration) { break; }
				if (field.Count < 19) { break; }
			}
			if (!Scores.IsWinning(score)) { return false; }

			Console.WriteLine("{0} [{2}] {1}", Scores.GetFormatted(score), target, target.Count + 1);

			if (target.GetHashCode() > target.Flip().GetHashCode())
			{
				target = target.Flip();
			}
			Nodes[target] = new SearchTreeKnownNode(target, score);

			return true;
		}
	}
}
