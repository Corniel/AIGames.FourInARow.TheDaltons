using System;
using System.Collections.Generic;
using System.IO;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeKnownNodes : Dictionary<Field, SearchTreeKnownNode>
	{
		private const string FileName = "_known_nodes.bin";

		public static SearchTreeKnownNodes Get()
		{
			return Load(Book.GetKnownString());
		}

		#region I/O

		public static SearchTreeKnownNodes Load(DirectoryInfo dir)
		{
			return Load(new FileInfo(Path.Combine(dir.FullName, FileName)));
		}

		public static SearchTreeKnownNodes Load(FileInfo file)
		{
			using (var stream = file.OpenRead())
			{
				return Load(stream);
			}
		}

		public static SearchTreeKnownNodes Load(string base64)
		{
			var dict = new SearchTreeKnownNodes();

			var bytes = Convert.FromBase64String(base64);

			using (var stream = new MemoryStream(bytes))
			{
				return Load(stream);
			}
		}

		public static SearchTreeKnownNodes Load(Stream stream)
		{
			var dict = new SearchTreeKnownNodes();

			var reader = new BinaryReader(stream);

			for (var index = 0; index + 16 < stream.Length; index += 17)
			{
				var turns = reader.ReadByte();
				var buffer = reader.ReadBytes(16);
				var field = Field.FromBytes(buffer);
				
				// Temp.
				if (turns == 255) { continue; }

				var score = field.RedToMove ? Scores.RedWins[turns] : Scores.YelWins[turns];
				var node = new SearchTreeKnownNode(field, score);
				dict[field] = node;
			}
			return dict;
		}

		public void Save(DirectoryInfo dir)
		{
			Save(new FileInfo(Path.Combine(dir.FullName, FileName)));
		}

		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			}
		}

		public void Save(Stream stream)
		{
			var writer = new BinaryWriter(stream);

			var done = new HashSet<Field>();

			foreach (var node in this.Values)
			{
				var field = node.Field;
				var flipd = field.Flip();

				if (done.Contains(field) || done.Contains(flipd)) { continue; }

				done.Add(field);
				done.Add(flipd);

				if (flipd.GetHashCode() < field.GetHashCode())
				{
					field = flipd;
				}

				var turn = Scores.GetPlyToWinning(node.Score);

				if (turn == Byte.MaxValue)
				{
				}

				writer.Write(turn);
				writer.Write(field.GetBytes());
			}
			writer.Flush();
		}

		

		#endregion
	}
}
