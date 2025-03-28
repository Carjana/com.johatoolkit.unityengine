using System;

namespace JohaToolkit.unity.Extensions
{
    public static class TimeSpanExtensions
    {
        // Milliseconds
        public static TimeSpan Milliseconds(this float milliseconds) => TimeSpan.FromMilliseconds(milliseconds);
        public static TimeSpan Milliseconds(this int milliseconds) => Milliseconds((float)milliseconds);
        
        // Seconds
        public static TimeSpan Seconds(this float seconds) => TimeSpan.FromSeconds(seconds);
        public static TimeSpan Seconds(this int seconds) => Seconds((float)seconds);
        
        // Minutes
        public static TimeSpan Minutes(this float minutes) => TimeSpan.FromMinutes(minutes);
        public static TimeSpan Minutes(this int minutes) => Minutes((float)minutes);
        
        // Hours
        public static TimeSpan Hours(this float hours) => TimeSpan.FromHours(hours);
        public static TimeSpan Hours(this int hours) => Hours((float)hours);
        
        // Days
        public static TimeSpan Days(this float days) => TimeSpan.FromDays(days);
        public static TimeSpan Days(this int days) => Days((float)days);

        // ToString
        public static string ToString_HH_MM_SS(this TimeSpan timeSpan) => timeSpan.ToString(@"hh\:mm\:ss");
        public static string ToString_H_MM_SS(this TimeSpan timeSpan) => timeSpan.ToString(@"h\:mm\:ss");
        public static string ToString_MM_SS(this TimeSpan timeSpan) => timeSpan.ToString(@"mm\:ss");
        public static string ToString_M_SS(this TimeSpan timeSpan) => timeSpan.ToString(@"m\:ss");
        public static string ToString_SS(this TimeSpan timeSpan) => timeSpan.ToString(@"ss");
        public static string ToString_S(this TimeSpan timeSpan) => timeSpan.ToString(@"s");
    }
}
