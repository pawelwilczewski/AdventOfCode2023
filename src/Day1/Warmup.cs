namespace Day1;

public static class FloorInfo
{
	public static int CalculateFinalFloor(string input)
	{
		return input.Count(c => c == '(') - input.Count(c => c == ')');
	}

	public static int CalculateBasementEnteredPosition(string input)
	{
		var currentFloor = 0;

		for (var i = 0; i < input.Length; ++i)
		{
			switch (input[i])
			{
				case '(':
					++currentFloor;
					break;
				case ')':
					--currentFloor;
					break;
				default:
					throw new ArgumentException("Invalid input!");
			}

			if (currentFloor < 0)
			{
				return i + 1;
			}
		}

		return -1;
	}
}