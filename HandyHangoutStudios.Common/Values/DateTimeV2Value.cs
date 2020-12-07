//    HandyHangoutStudios.Common, common classes for use by the Handy Hangout Dev Team
//    Copyright (C) 2020 John Marsden

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.

using HandyHangoutStudios.Parsers.Models;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
using System;
using System.Collections.Generic;

namespace HandyHangoutStudios.Parsers.Resolutions
{
    public class DateTimeV2Value
    {
        public TimexProperty Timex { get; init; }
        public DateTimeV2Type Type { get; init; }
        public object Value { get; init; }

        private DateTimeZone timeZone;

        public DateTimeV2Value() {}

        public DateTimeV2Value(IDictionary<string, string> value, DateTimeZone timeZone)
        {
            this.timeZone = timeZone;
            Timex = new TimexProperty(value["timex"]);
            Type = Enum.Parse<DateTimeV2Type>(value["type"], true);

            Value = Type switch
            {
                DateTimeV2Type.Date => CreateDateValue(value["value"]),
                DateTimeV2Type.DateRange => CreateDateRangeValue(value["start"], value["end"]),
                DateTimeV2Type.DateTime => CreateDateTimeValue(value["value"]),
                DateTimeV2Type.DateTimeRange => CreateDateTimeRange(value["start"], value["end"]),
                DateTimeV2Type.Duration => CreateDurationValue(value["value"]),
                DateTimeV2Type.Time => CreateTimeValue(value["value"]),
                DateTimeV2Type.TimeRange => CreateTimeRangeValue(value["start"], value["end"]),
                DateTimeV2Type.Set => null,
                _ => throw new Exception("A type was passed that hasn't been implemented for the Value in this model.")
            };
        }

        private static Range<LocalTime> CreateTimeRangeValue(string start, string end)
        {
            LocalTimePattern pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
            ParseResult<LocalTime> startTimeParsed = pattern.Parse(start);
            ParseResult<LocalTime> endTimeParsed = pattern.Parse(end);
            return new Range<LocalTime>()
            {
                Start = startTimeParsed.GetValueOrThrow(),
                End = endTimeParsed.GetValueOrThrow()
            };
        }

        private LocalTime CreateTimeValue(string value)
        {
            if (this.Timex.Now is true)
            {
                return SystemClock.Instance.InZone(this.timeZone).GetCurrentLocalDateTime().TimeOfDay;
            }
            ParseResult<LocalTime> timeParsed = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss").Parse(value);
            return timeParsed.GetValueOrThrow();
        }

        private static Duration CreateDurationValue(string value)
        {
            return Duration.FromSeconds(long.Parse(value));
        }

        private static Range<LocalDateTime> CreateDateTimeRange(string start, string end)
        {
            LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
            ParseResult<LocalDateTime> startDateTimeParsed = pattern.Parse(start);
            ParseResult<LocalDateTime> endDateTimeParsed = pattern.Parse(end);
            return new Range<LocalDateTime>()
            {
                Start = startDateTimeParsed.GetValueOrThrow(),
                End = endDateTimeParsed.GetValueOrThrow()
            };
        }

        private LocalDate CreateDateValue(string value)
        {
            if (this.Timex.Now is true)
            {
                return SystemClock.Instance.InZone(this.timeZone).GetCurrentLocalDateTime().Date;
            }

            ParseResult<LocalDate> dateParsed = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd").Parse(value);
            return dateParsed.GetValueOrThrow();
        }

        private LocalDateTime CreateDateTimeValue(string value)
        {
            if (this.Timex.Now is true)
            {
                return SystemClock.Instance.InZone(this.timeZone).GetCurrentLocalDateTime();
            }

            LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
            ParseResult<LocalDateTime> dateTimeParsed = pattern.Parse(value);
            return dateTimeParsed.GetValueOrThrow();
        }

        private static Range<LocalDate> CreateDateRangeValue(string start, string end)
        {

            LocalDatePattern pattern = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd");
            ParseResult<LocalDate> startDateParsed = pattern.Parse(start);
            ParseResult<LocalDate> endDateParsed = pattern.Parse(end);
            return new Range<LocalDate>()
            {
                Start = startDateParsed.GetValueOrThrow(),
                End = endDateParsed.GetValueOrThrow()
            };
        }
    }
}
