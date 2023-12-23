namespace Day2;

public static class GameValidityChecker
{
	private record GameInfo
	{
		public struct GemsInfo
		{
			public GemsInfo() {}

			public int Red { get; set; } = 0;
			public int Green { get; set; } = 0;
			public int Blue { get; set; } = 0;

			public static bool operator >(GemsInfo a, GemsInfo b)
			{
				return a.Red > b.Red || a.Green > b.Green || a.Blue > b.Blue;
			}

			public static bool operator <(GemsInfo a, GemsInfo b)
			{
				return a.Red < b.Red && a.Green < b.Green && a.Blue < b.Blue;
			}

			public static GemsInfo Deserialize(string input)
			{
				var result = new GemsInfo();
				foreach (var value in input.Split(", "))
				{
					var tokens = value.Split(' ');

					var count = int.Parse(tokens[0]);
					switch (tokens[1])
					{
						case { } a when a.Contains("red"):
							result.Red = count;
							break;
						case { } a when a.Contains("green"):
							result.Green = count;
							break;
						case { } a when a.Contains("blue"):
							result.Blue = count;
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(input));
					}
				}

				return result;
			}

			public override string ToString()
			{
				return $"red {Red}, green {Green}, blue {Blue}";
			}
		}

		public int Id { get; private init; }
		public GemsInfo BagConfig { get; private set; }

		public List<GemsInfo> GemsShowed { get; private init; } = [];

		private void RecalculateMinBagConfiguration()
		{
			BagConfig = new GemsInfo
			{
				Red = GemsShowed.Max(info => info.Red),
				Green = GemsShowed.Max(info => info.Green),
				Blue = GemsShowed.Max(info => info.Blue)
			};
		}

		public static GameInfo Deserialize(string text, string? bagConfiguration)
		{
			var splitInfo = text.Split(": ");
			var result = new GameInfo
			{
				Id = int.Parse(splitInfo[0].Split(' ')[1]),
				GemsShowed = splitInfo[1].Split("; ").Select(GemsInfo.Deserialize).ToList()
			};

			if (bagConfiguration is not null)
			{
				result.BagConfig = GemsInfo.Deserialize(bagConfiguration);
			}
			else
			{
				result.RecalculateMinBagConfiguration();
			}

			return result;
		}
	}

	private static IEnumerable<GameInfo> ProcessInput(string input, string? bagConfiguration = null)
	{
		return input.ReplaceLineEndings("\n").Split("\n")
					.Select(gameInfo => GameInfo.Deserialize(gameInfo, bagConfiguration));
	}

	public static int EvaluateValidGamesIdSum(string input, string bagConfiguration)
	{
		return ProcessInput(input, bagConfiguration)
				.Where(game => game.GemsShowed.TrueForAll(x => !(x > game.BagConfig)))
				.Sum(game => game.Id);
	}

	public static int EvaluatePowerOfMinCubesSum(string input)
	{
		return ProcessInput(input).Sum(game => game.BagConfig.Red * game.BagConfig.Green * game.BagConfig.Blue);
	}
}