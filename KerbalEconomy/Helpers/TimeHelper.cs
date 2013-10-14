namespace KerbalEconomy.Helpers
{
    public class TimeHelper
    {
        public const double YEAR = 31536000d;
        public const double DAY = 86400d;
        public const double HOUR = 3600d;
        public const double MINUTE = 60d;

        public static string FromUniversalTime(double value)
        {
            int year = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;

            // Years
            if (value >= YEAR)
            {
                while (value >= YEAR)
                {
                    year++;
                    value -= YEAR;
                }
            }

            // Days
            if (value >= DAY)
            {
                while (value >= DAY)
                {
                    day++;
                    value -= DAY;
                }
            }

            // Hours
            if (value >= HOUR)
            {
                while (value >= HOUR)
                {
                    hour++;
                    value -= HOUR;
                }
            }

            // Minutes
            if (value >= MINUTE)
            {
                while (value >= MINUTE)
                {
                    minute++;
                    value -= MINUTE;
                }
            }

            second = (int)value;

            return day + "/" + year + " " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
        }
    }
}
