using AIGames.FourInARow.TheDaltons;
using AIGames.FourInARow.TheDaltons.Communication;
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
			Duration = TimeSpan.FromSeconds(30);
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
			}
			catch (Exception x)
			{
				Console.WriteLine(x.Message);
			}

			var files = dir.GetFiles("*.log").OrderByDescending(f => f.Name).ToList();

			var saved = 0;
			var skipped = 0;
			var total = files.Count;

			foreach (var file in files)
			{
				Console.WriteLine(@"{4:hh\:mm\:ss} Saved: {0}, skipped: {1}, {2} out {3}",
						saved, skipped, saved + skipped, total, sw.Elapsed);

				Load(file);

				foreach (var field in Fields)
				{
					if (Nodes.ContainsKey(field) || Nodes.ContainsKey(field.Flip()))
					{
						skipped++;
						continue;
					}
				}

				if (Fields.Count >= MinPly)
				{
					var field = Fields.Last();
					if (field.IsScoreRed())
					{
						Analyse(true);
						saved++;
						Nodes.Save(dir);
					}
					else if (field.IsScoreYellow())
					{
						Analyse(false);
						saved++;
						Nodes.Save(dir);
					}
					else
					{
						skipped++;
					}
				}
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

		public void Analyse(bool redToMove)
		{
			var tree = new SearchTree();
			tree.Initialize(Nodes.Values);
			var fields = Fields
				.Where(f => f.RedToMove == redToMove)
				.OrderByDescending(f => f.Count)
				.ToList();

			Field target = Field.Empty;
			var score = 0;

			foreach (var field in fields)
			{
				tree.GetMove(field, Duration, Duration);

				Console.WriteLine(tree.Logger);

				var test = tree.Root.Score;
				if (redToMove && test < Scores.RedWins[42])
				{
					break;
				}
				else if (!redToMove && test > Scores.YelWins[41])
				{
					break;
				}
				target = field;
				score = test;
				var formatted = Scores.GetFormatted(score);

				if (field.Count < 19) { break; }
			}
			Console.WriteLine("{0} {1}", Scores.GetFormatted(score), target);

			if (target.GetHashCode() > target.Flip().GetHashCode())
			{
				target = target.Flip();
			}
			Nodes[target] = new SearchTreeKnownNode(target, score);
		}
	}
}
