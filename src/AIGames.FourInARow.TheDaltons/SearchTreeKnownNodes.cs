using System;
using System.Collections.Generic;
using System.IO;

namespace AIGames.FourInARow.TheDaltons
{
	public class SearchTreeKnownNodes : Dictionary<Field, SearchTreeKnownNode>
	{
		#region I/O

		public static SearchTreeKnownNodes Load(DirectoryInfo dir)
		{
			return Load(new FileInfo(Path.Combine(dir.FullName, "_data.b64")));
		}

		public static SearchTreeKnownNodes Load(FileInfo file)
		{
			using (var stream = file.OpenRead())
			{
				return Load(stream);
			}
		}

		public static SearchTreeKnownNodes Load(Stream stream)
		{
			return Load(new StreamReader(stream).ReadToEnd());
		}

		public static SearchTreeKnownNodes Load(string base64)
		{
			var dict = new SearchTreeKnownNodes();

			var bytes = Convert.FromBase64String(base64);

			for (var index = 0; index < bytes.Length; index += 16)
			{
				// actually step size is 17;
				var turns = bytes[index++];

				var subset = new byte[16];
				Array.Copy(bytes, index, subset, 0, 16);
				var field = Field.FromBytes(subset);
				var score = field.RedToMove ? Scores.RedWins[turns] : Scores.YelWins[turns];
				var node = new SearchTreeKnownNode(field, score);
				dict[field] = node;
			}

			return dict;
		}

		public void Save(DirectoryInfo dir)
		{
			Save(new FileInfo(Path.Combine(dir.FullName, "_data.b64")));
		}

		public void Save(FileInfo file)
		{
			using (var stream = file.OpenWrite())
			{
				Save(stream);
			}
		}

		public void Save(Stream stream)
		{
			var bytes = new List<byte>();

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

				var score = node.Score;
				byte turn = (byte)(score > 0 ? Scores.Red - score : Scores.Yel + score);

				bytes.Add(turn);
				bytes.AddRange(field.GetBytes());
			}

			var writer = new StreamWriter(stream);
			writer.Write(Convert.ToBase64String(bytes.ToArray()));
			writer.Flush();
		}

		#endregion
	}
}
