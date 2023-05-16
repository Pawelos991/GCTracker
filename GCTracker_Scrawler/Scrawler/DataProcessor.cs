using System.Globalization;
using System.Text.RegularExpressions;

namespace GCTracker_Scrawler.Scrawler;

public static class DataProcessor
{
	public static decimal ProcessPriceValue(string price)
	{
		string proccesedPrice = Regex.Replace(price, @"[^\d,.]+", "").Replace(",", ".");
		return decimal.Parse(proccesedPrice, CultureInfo.InvariantCulture.NumberFormat);
	}

	public static string ProcessCardNameValue(string name)
	{
		name = Regex.Replace(name, @"^Karta graficzna\s+", "");
		
		int producentCodeStartIndex = name.IndexOf('(');
		int producentCodeEndIndex = name.IndexOf(')');
		
		if (producentCodeStartIndex >= 0 && producentCodeEndIndex >= 0)
		{
			name = name.Substring(0, producentCodeStartIndex) + name.Substring(producentCodeEndIndex + 1);
		}
		
		name = name.Trim();
		return name;
	}

	public static string ProcessProducentCodeValue(string producentCode)
	{
		producentCode = producentCode.Replace("Kod producenta:", "");
		producentCode = producentCode.Replace("[", "").Replace("]", "");
		producentCode = producentCode.Trim();

		return producentCode;
	}
}