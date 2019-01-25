//Time.cs
//Description:
//Author: JustSomeGuy
//1/17/2019, 5:47 PM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoC.Engine
{
	internal static class Time
	{
		private static HashSet<ITimeAware> timeAwares = new HashSet<ITimeAware>();
		public static DateTime now => DateTime.Now;
		//unless we somehow hit 245,000 years i dont think this will be an issue.
		public static uint gameHoursPlayed { get; private set; } = 0;

		public static int currentHour => (int)(gameHoursPlayed % 24);
		public static int currentDay => (int)(gameHoursPlayed / 24);

		public static bool isHalloween => (now.Month == 10 && now.Day >= 28) || (now.Month == 11 && now.Day == 1); //Oct 28 - Nov 1
		public static bool isChrismas => now.Month == 12 && now.Day >= 25; //Dec 25 - Dec 31
		public static bool isChrismasDay => now.Month == 12 && now.Day == 25; //Dec 25 - Dec 31
		public static bool isNewYearsDay => now.Month == 1 && now.Day == 1; //Dec 25 - Dec 31
		public static bool isValentines => now.Month == 2 && now.Day >= 13 && now.Day <= 15; //Feb 13 - Feb 15
		public static bool isStPattysDay => now.Month == 3 && now.Day == 17; //Mar 17
		public static bool isAprilFools => now.Month == 4 && now.Day == 1; //Apr 1
		public static bool isEaster => easterCheck(now) || easterCheck(now.AddDays(1)) || easterCheck(now.AddDays(-1)); //Easter +/-1 Day
		public static bool isThanksgiving => now.Month == 11 && now.Day >= 21 && now.Day < 30; //Nov 21 - Nov 29

		public static bool isWinter => now.Month == 12 || now.Month == 1; //Dec 1 - Jan 31

		public static bool addTimeAware(ITimeAware item)
		{
			return timeAwares.Add(item);
		}

		public static bool removeTimeAware(ITimeAware item)
		{
			return timeAwares.Remove(item);
		}

		public static void registerTimePassing(uint numHours)
		{
			if (numHours != 0)
			{
				gameHoursPlayed += numHours;
				foreach (ITimeAware item in timeAwares)
				{
					item.ReactToTimePassing(numHours);
				}
			}
		}

		//easter is technically lunar. this is basically it
		//from https://www.codeproject.com/Articles/10860/Calculating-Christian-Holidays by Jan Schreuder
		//funnily enough, it's a modified version of other code, also on code project, which is longer available. 
		//tbh i have no idea if this works. i assume it does. It's licensed under CPOL, which is even more permissive than
		//GPL-3. so we're good here. 
		private static bool easterCheck(DateTime dateTime)
		{
			//mod: year changed from parameter to parsed from dateTime.
			int year = dateTime.Year;
			int day = 0;
			int month = 0;

			//mod: arbitrary ints renamed to less arbitrary values
			int goldNumber = year % 19;
			int century = year / 100;
			int equinoxToFullMoon = (century - century / 4 - (8 * century + 13) / 25 + 19 * goldNumber + 15) % 30;
			int sundayAfterFullMoon = equinoxToFullMoon - equinoxToFullMoon / 28 * (1 - equinoxToFullMoon / 28 * (29 / (equinoxToFullMoon + 1)) * ((21 - goldNumber) / 11));

			day = sundayAfterFullMoon - ((year + year / 4 + sundayAfterFullMoon + 2 - century + century / 4) % 7) + 28;
			month = 3;

			if (day > 31)
			{
				month++;
				day -= 31;
			}

			//mod: instead of returning the date of easter, check if that date's month and day match the value passed in.
			return day == dateTime.Day && month == dateTime.Month;
		}
	}
}
