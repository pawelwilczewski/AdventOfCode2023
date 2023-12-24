namespace Day3;

public static class EngineCheck
{
	private class EngineSchematics
	{
		private const string REGULAR_CHARS = "0123456789.";

		public List<char[]> Schematic { get; private init; } = [];

		private void ApplyToNeighbours(int x, int y, Action<int, int> action)
		{
			var startX = Math.Max(x - 1, 0);
			var startY = Math.Max(y - 1, 0);
			var stopX = Math.Min(x + 2, Schematic[y].Length);
			var stopY = Math.Min(y + 2, Schematic.Count);

			for (y = startY; y < stopY; ++y)
			{
				for (x = startX; x < stopX; ++x)
				{
					action(x, y);
				}
			}
		}

		public bool HasNeighbouringSpecialChar(int x, int y)
		{
			var startX = Math.Max(x - 1, 0);
			var startY = Math.Max(y - 1, 0);
			var stopX = Math.Min(x + 2, Schematic[y].Length);
			var stopY = Math.Min(y + 2, Schematic.Count);

			for (y = startY; y < stopY; ++y)
			{
				for (x = startX; x < stopX; ++x)
				{
					if (!REGULAR_CHARS.Contains(Schematic[y][x])) return true;
				}
			}

			return false;
		}

		public bool IsDigit(int x, int y)
		{
			return char.IsDigit(Schematic[y][x]);
		}

		public bool HasConsecutiveDigit(int x, int y)
		{
			return x < Schematic[y].Length - 1 && char.IsDigit(Schematic[y][x + 1]);
		}

		public bool IsSpecialSymbol(int x, int y)
		{
			return !REGULAR_CHARS.Contains(Schematic[y][x]);
		}

		private int? FindFullNumber(int x, int y, out int startX)
		{
			if (char.IsDigit(Schematic[y][x]))
			{
				var left = x;
				do
				{
					--left;
				} while (left >= 0 && char.IsDigit(Schematic[y][left]));

				startX = left + 1;

				var right = x;
				do
				{
					++right;
				} while (right < Schematic[y].Length && char.IsDigit(Schematic[y][right]));

				return int.Parse(new string(Schematic[y][startX..right]));
			}

			startX = -1;
			return null;
		}

		public List<int> GetNumbersAdjacent(int x, int y)
		{
			var addedCoords = new List<Tuple<int, int>>();
			var result = new List<int>();
			ApplyToNeighbours(x, y, (currentX, currentY) =>
			{
				var number = FindFullNumber(currentX, currentY, out var startX);
				if (number is not null && !addedCoords.Contains(new Tuple<int, int>(startX, currentY)))
				{
					result.Add(number.Value);
					addedCoords.Add(new Tuple<int, int>(startX, currentY));
				}
			});
			return result;
		}

		public static EngineSchematics Deserialize(string input)
		{
			return new EngineSchematics
			{
				Schematic = input.ReplaceLineEndings("\n").Split("\n").Select(row => row.ToCharArray()).ToList()
			};
		}
	}

	public static int SumValidParts(string input)
	{
		var result = 0;
		var schematics = EngineSchematics.Deserialize(input);
		for (var y = 0; y < schematics.Schematic.Count; ++y)
		{
			for (var x = 0; x < schematics.Schematic[y].Length; ++x)
			{
				if (!schematics.IsDigit(x, y)) continue;

				var number = 0;
				var isValidPart = false;
				do
				{
					number = number * 10 + schematics.Schematic[y][x] - '0';
					if (!isValidPart && schematics.HasNeighbouringSpecialChar(x, y))
					{
						isValidPart = true;
					}
				} while (schematics.HasConsecutiveDigit(x++, y));

				if (isValidPart)
				{
					result += number;
				}
			}
		}

		return result;
	}

	public static int GearsRatioSum(string input)
	{
		var result = 0;
		var schematics = EngineSchematics.Deserialize(input);
		for (var y = 0; y < schematics.Schematic.Count; ++y)
		{
			for (var x = 0; x < schematics.Schematic[y].Length; ++x)
			{
				if (!schematics.IsSpecialSymbol(x, y)) continue;

				var neighbouringNumbers = schematics.GetNumbersAdjacent(x, y);
				if (neighbouringNumbers.Count > 1)
				{
					result += neighbouringNumbers.Aggregate(1, (current, value) => current * value);
				}
			}
		}

		return result;
	}
}