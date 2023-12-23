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
		}

		public int Id { get; init; }
		public GemsInfo BagConfig { get; init; }

		public List<GemsInfo> GemsShowed { get; init; } = [];
	}

	private static List<GameInfo> ProcessInput(string bagConfiguration, string input)
	{
		var result = new List<GameInfo>();
		foreach (var gameInfo in input.ReplaceLineEndings("\n").Split("\n"))
		{
			var splitInfo = gameInfo.Split(": ");
			result.Add(new GameInfo
			{
				Id = int.Parse(splitInfo[0].Split(' ')[1]),
				BagConfig = GameInfo.GemsInfo.Deserialize(bagConfiguration),
				GemsShowed = splitInfo[1].Split("; ").Select(GameInfo.GemsInfo.Deserialize).ToList()
			});
		}

		return result;
	}

	public static int EvaluateValidGamesIdSum(string bagConfiguration, string input)
	{
		return ProcessInput(bagConfiguration, input)
				.Where(game => game.GemsShowed.TrueForAll(x => !(x > game.BagConfig)))
				.Sum(game => game.Id);
	
}