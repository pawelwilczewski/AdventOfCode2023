using System.Text;

namespace Day1;

public static class Decoder
{
	private static readonly Dictionary<string, string> spelledDigits = new()
	{
		{ "one", "1" },
		{ "two", "2" },
		{ "three", "3" },
		{ "four", "4" },
		{ "five", "5" },
		{ "six", "6" },
		{ "seven", "7" },
		{ "eight", "8" },
		{ "nine", "9" }
	};

	public static int EvaluateCalibrationValue(string input)
	{
		var words = input.Split();
		var result = 0;
		foreach (var word in words)
		{
			var digits = word.Where(char.IsDigit).ToList();
			if (digits.Count <= 0) continue;

			result += int.Parse(new string(new[] { digits[0], digits[^1] }));
		}

		return result;
	}

	public static int EvaluateCalibrationValueComplex(string input)
	{
		var outputBuilder = new StringBuilder();

		for (var i = 0; i < input.Length; ++i)
		{
			var replaced = false;
			foreach (var (spelled, digit) in spelledDigits)
			{
				if (i + spelled.Length > input.Length) continue;

				if (input.Substring(i, spelled.Length) == spelled)
				{
					outputBuilder.Append(digit);
					replaced = true;
					break;
				}
			}

			if (!replaced)
			{
				outputBuilder.Append(input[i]);
			}
		}

		return EvaluateCalibrationValue(outputBuilder.ToString());
	}
}
