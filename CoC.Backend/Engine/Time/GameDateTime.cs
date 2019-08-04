using System;
using System.Collections.Generic;
using System.Text;

namespace CoC.Backend.Engine.Time
{
	public sealed class GameDateTime : IComparable<GameDateTime>// saveable
	{
		public readonly byte hour;
		public readonly int day;

		public GameDateTime(int dy, byte hr)
		{
			day = dy;
			hour = hr;
		}

		public int hoursTo(GameDateTime other)
		{
			int days = other.day - day;
			int hours = other.hour - hour;
			return days * 24 + hours;
		}
		public int hoursToNow()
		{
			return hoursTo(Now);
		}

		public static GameDateTime Now => new GameDateTime(GameEngine.CurrentDay, GameEngine.CurrentHour);

		public GameDateTime delta(int hours)
		{
			int deltaDays = (int)Math.Floor(1.0 * hours / 24); //for negative numbers, gets the correct div value.
			int mod = hours % 24;
			if (mod < 0) mod += 24;
			byte newHour = ((byte)mod).add(hour);
			int newDay = deltaDays + day;
			if (newHour > 24)
			{
				newDay++;
				newHour -= 24;
			}
			return new GameDateTime(newDay, newHour);
		}

		public string GetFormattedHourString()
		{
#warning add a 12 hour option, and also cultural variants based on weird places like using commas instead of colons. 
			return hour.ToString() + ":00";
		}

		public static GameDateTime HoursFromNow(int hours)
		{
			return Now.delta(hours);
		}

		public int CompareTo(GameDateTime other)
		{
			return -hoursTo(other);
		}
	}


}
