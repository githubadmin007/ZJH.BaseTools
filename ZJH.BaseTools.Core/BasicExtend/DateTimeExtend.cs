using System;
using System.Collections.Generic;
using System.Text;

namespace ZJH.BaseTools.BasicExtend
{
    public static class DateTimeExtend
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime time, bool isUnix = false)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (time.Kind != DateTimeKind.Utc)
            {
                startTime = TimeZoneInfo.ConvertTimeFromUtc(startTime, TimeZoneInfo.Local);
            }
            long t = (time.Ticks - startTime.Ticks) / 10000;
            if (isUnix) {
                t = t / 1000;
            }
            return t;
        }
    }
}
