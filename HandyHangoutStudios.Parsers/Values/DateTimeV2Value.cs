using HandyHangoutStudios.Parsers.Models;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyHangoutStudios.Parsers.Resolutions
{
    public class DateTimeV2Value
    {
        public TimexProperty Timex { get; init; }
        public DateTimeV2Type Type { get; init; }
        public object Value { get; init; }

        public DateTimeV2Value()
        {

        }

        public DateTimeV2Value(IDictionary<string, string> value)
        {
            Timex = new TimexProperty(value["timex"]);
            Type = Enum.Parse<DateTimeV2Type>(value["type"], true);

            if (Type is DateTimeV2Type.Date)
            {
                ParseResult<LocalDate> dateParsed = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd").Parse(value["value"]);
                Value = dateParsed.GetValueOrThrow();
            }
            else if (Type is DateTimeV2Type.DateRange)
            {
                LocalDatePattern pattern = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd");
                ParseResult<LocalDate> startDateParsed = pattern.Parse(value["start"]);
                ParseResult<LocalDate> endDateParsed = pattern.Parse(value["end"]);
                Value = new Range<LocalDate>()
                {
                    Start = startDateParsed.GetValueOrThrow(),
                    End = endDateParsed.GetValueOrThrow()
                };
            }
            else if (Type is DateTimeV2Type.DateTime)
            {
                LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
                ParseResult<LocalDateTime> dateTimeParsed = pattern.Parse(value["value"]);
                Value = dateTimeParsed.GetValueOrThrow();
            }
            else if (Type is DateTimeV2Type.DateTimeRange)
            {
                LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
                ParseResult<LocalDateTime> startDateTimeParsed = pattern.Parse(value["start"]);
                ParseResult<LocalDateTime> endDateTimeParsed = pattern.Parse(value["end"]);
                Value = new Range<LocalDateTime>()
                {
                    Start = startDateTimeParsed.GetValueOrThrow(),
                    End = endDateTimeParsed.GetValueOrThrow()
                };
            }
            else if (Type is DateTimeV2Type.Duration)
            {
                long seconds = long.Parse(value["value"]);
                Value = Period.FromSeconds(seconds);
            }
            else if (Type is DateTimeV2Type.Set)
            {
                Value = null;
            }
            else if (Type is DateTimeV2Type.Time)
            {
                ParseResult<LocalTime> timeParsed = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss").Parse(value["value"]);
                Value = timeParsed.GetValueOrThrow();
            }
            else if (Type is DateTimeV2Type.TimeRange)
            {
                LocalTimePattern pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
                ParseResult<LocalTime> startTimeParsed = pattern.Parse(value["start"]);
                ParseResult<LocalTime> endTimeParsed = pattern.Parse(value["end"]);
                Value = new Range<LocalTime>()
                {
                    Start = startTimeParsed.GetValueOrThrow(),
                    End = endTimeParsed.GetValueOrThrow()
                };
            }
            else
            {
                throw new Exception("A type was passed that hasn't been implemented for the Value in this model.");
            }
        }
    }
}
