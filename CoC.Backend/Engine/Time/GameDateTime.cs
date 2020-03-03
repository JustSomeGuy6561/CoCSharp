using System;

namespace CoC.Backend.Engine.Time
{
	public sealed class GameDateTime : IComparable<GameDateTime>, IEquatable<GameDateTime>// saveable
	{
		public readonly byte hour;
		public readonly int day;

		public GameDateTime(int dy, byte hr)
		{
			day = dy;
			hour = hr;
		}

		public int HoursTo(GameDateTime other)
		{
			int days = other.day - day;
			int hours = other.hour - hour;
			return days * 24 + hours;
		}
		public int hoursToNow()
		{
			return HoursTo(Now);
		}

		public static GameDateTime Now => new GameDateTime(GameEngine.CurrentDay, GameEngine.CurrentHour);

		public GameDateTime Delta(int hours)
		{
			int deltaDays = (int)Math.Floor(1.0 * hours / 24); //for negative numbers, gets the correct div value.
			int mod = hours % 24;
			if (mod < 0) mod += 24;
			byte newHour = ((byte)mod).add(hour);
			int newDay = deltaDays + day;
			if (newHour >= 24)
			{
				newDay++;
				newHour -= 24;
			}
			return new GameDateTime(newDay, newHour);
		}

		public string GetFormattedHourString()
		{
			if (SaveData.BackendGlobalSave.data.UsesMilitaryTime)
			{
				return hour.ToString() + ":00";
			}
			else
			{
				string suffix = hour >= 12 ? "PM" : "AM";
				int hourHelper = hour;

				if (hourHelper > 12)
				{
					hourHelper -= 12;
				}
				if (hour == 0)
				{
					hourHelper = 12;
				}
				return hourHelper.ToString() + ":00 " + suffix;
			}
		}

		public static GameDateTime HoursFromNow(int hours)
		{
			return Now.Delta(hours);
		}

		public int CompareTo(GameDateTime other)
		{
			if (other is null) return 1;
			else return -HoursTo(other);
		}

		public static bool operator >(GameDateTime source, GameDateTime other)
		{
			if (other is null || source is null) return false;
			return source.CompareTo(other) > 0;
		}

		public static bool operator <(GameDateTime source, GameDateTime other)
		{
			if (other is null || source is null) return false;
			return source.CompareTo(other) < 0;
		}

		public static bool operator ==(GameDateTime source, GameDateTime other)
		{
			if (other is null && source is null) return true;
			else if (other is null || source is null) return false;
			else return source.Equals(other);
		}

		public static bool operator !=(GameDateTime source, GameDateTime other)
		{
			if (other is null && source is null) return false;
			else if (other is null || source is null) return true;
			else return !source.Equals(other);
		}

		public override bool Equals(object obj)
		{
			return obj is GameDateTime gameDateTime && this.Equals(gameDateTime);
		}

		public bool Equals(GameDateTime other)
		{
			return !(other is null) && this.hour == other.hour && this.day == other.day;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = 1684876132;
				hashCode = hashCode * -1521134295 + hour.GetHashCode();
				hashCode = hashCode * -1521134295 + day.GetHashCode();
				return hashCode;
			}
		}
	}


}
