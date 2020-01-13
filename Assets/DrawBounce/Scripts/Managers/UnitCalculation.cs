
public class UnitCalculation
{
	public static string GetCoinText(int coin)
	{
		float value = 0f;
		string unit = "";

		if (coin >= 1000000000)
		{
			unit = "G";
			value = (float)coin / 1000000000f;
		}
		else if (coin >= 1000000)
		{
			unit = "M";
			value = (float)coin / 1000000f;
		}
		else if (coin >= 1000)
		{
			unit = "K";
			value = (float)coin / 1000f;
		}
		else
		{
			return coin.ToString();
		}
		
		return string.Format("{0:f1} {1}", value, unit);
	}

	public static string GetHeightText(float height, bool decimal2 = false)
	{
		string distText = "";

		if (height >= 1000f)
		{
			float kilo = height / 1000f;

			if(decimal2)
				distText = string.Format("{0:f2} KM", kilo);
			else
				distText = string.Format("{0:f1} KM", kilo);
		}
		else
		{
			int meter = (int)height;
			distText = string.Format("{0} M", meter);
		}

		return distText;
	}
}
