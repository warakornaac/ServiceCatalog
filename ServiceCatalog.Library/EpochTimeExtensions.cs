using System;
using System.Data.SqlTypes;
using System.Globalization;

namespace ServiceCatalog.Library
{
    public static class EpochTimeExtensions
    {
        public static string ToTrim(this string str)
        {
            str = str ?? "";
            return str.Trim();
        }
        public static string ToDateString(this DateTime strDate)
        {
            string dt = strDate.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            return dt;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToEpochTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            var ts = ticks / TimeSpan.TicksPerSecond;
            return ts;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToEpochTime(this DateTimeOffset dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).Ticks;
            var ts = ticks / TimeSpan.TicksPerSecond;
            return ts;
        }

        /// <summary>
        /// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> kind.
        /// </summary>
        public static DateTime ToDateTimeFromEpoch(this long intDate)
        {
            var timeInTicks = intDate * TimeSpan.TicksPerSecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
        }

        /// <summary>
        /// Converts the given epoch time to a UTC <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffsetFromEpoch(this long intDate)
        {
            var timeInTicks = intDate * TimeSpan.TicksPerSecond;
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(timeInTicks);
        }

        public static DateTime ToSQLDateTimeEn(this string strDate)
        {

            // Parse exactly from your input string to the native date format.
            //DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", new CultureInfo("th-TH"));
            // Part to SqlDateTime then            
            var stratDate = strDate.Split('/');

            var date = new DateTime(Convert.ToInt32(stratDate[2]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0]));
            return dt;
        }

        public static SqlDateTime ToSQLDateTime(this string strDate)
        {

            // Parse exactly from your input string to the native date format.
            //DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", new CultureInfo("th-TH"));
            // Part to SqlDateTime then            
            var stratDate = strDate.Split('/');

            var date = new DateTime(Convert.ToInt32(stratDate[2]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0])).ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            SqlDateTime dtSql = SqlDateTime.Parse(date);
            //SqlDateTime dt = SqlDateTime.Parse(Convert.ToDateTime(strDate).ToString("yyyy/MM/dd",  CultureInfo.InvariantCulture));
            return dtSql;
        }

        public static SqlDateTime ToSQLDateTime(this string strDate, string strTime)
        {

            // Parse exactly from your input string to the native date format.
            //DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", new CultureInfo("th-TH"));
            // Part to SqlDateTime then            
            var stratDate = strDate.Split('/');

            var date = new DateTime(Convert.ToInt32(stratDate[2]) - 543, Convert.ToInt32(stratDate[1]), Convert.ToInt32(stratDate[0])).ToString("yyyy-MM-dd", new CultureInfo("en-US")) + " " + strTime;
            SqlDateTime dtSql = SqlDateTime.Parse(date);
            //SqlDateTime dt = SqlDateTime.Parse(Convert.ToDateTime(strDate).ToString("yyyy/MM/dd",  CultureInfo.InvariantCulture));
            return dtSql;
        }
        public static TimeSpan ToTimeSpan(this string strTime)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(strTime, out time))
            {
                return new TimeSpan();
            }
            return time;
        }

        public static DateTime ToDateTime(this string strDate)
        {
            DateTime dt = DateTime.ParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return dt;
        }

        public static string ToDayOfWeekTH(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday: return DayOfWeekTH.Monday;
                case DayOfWeek.Tuesday: return DayOfWeekTH.Tuesday;
                case DayOfWeek.Wednesday: return DayOfWeekTH.Wednesday;
                case DayOfWeek.Thursday: return DayOfWeekTH.Thursday;
                case DayOfWeek.Friday: return DayOfWeekTH.Friday;
                case DayOfWeek.Saturday: return DayOfWeekTH.Saturday;
                case DayOfWeek.Sunday: return DayOfWeekTH.Sunday;
                default: return string.Empty;
            }
        }

        public static string ToDayOfWeekTH(this string day)
        {
            if (string.Equals(day, DayOfWeek.Monday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Monday;
            }
            if (string.Equals(day, DayOfWeek.Tuesday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Tuesday;
            }

            if (string.Equals(day, DayOfWeek.Wednesday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Wednesday;
            }

            if (string.Equals(day, DayOfWeek.Thursday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Thursday;
            }

            if (string.Equals(day, DayOfWeek.Friday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Friday;
            }

            if (string.Equals(day, DayOfWeek.Saturday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Saturday;
            }

            if (string.Equals(day, DayOfWeek.Sunday.ToString(), StringComparison.InvariantCulture))
            {
                return DayOfWeekTH.Sunday;
            }
            return string.Empty;
        }
    }

    public static class DayOfWeekTH
    {
        public const string Sunday = "อาทิตย์";
        public const string Monday = "จันทร์";
        public const string Tuesday = "อังคาร";
        public const string Wednesday = "พุธ";
        public const string Thursday = "พฤหัสบดี";
        public const string Friday = "ศุกร์";
        public const string Saturday = "เสาร์";
    }
}
