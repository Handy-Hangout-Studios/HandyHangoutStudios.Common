using HandyHangoutStudios.Parsers.Resolutions;
using System.Collections.Generic;

namespace HandyHangoutStudios.Parsers.Models
{
    public class DateTimeV2ModelResult
    {
        public string Text { get; init; }
        public DateTimeV2Type TypeName { get; init; }
        public IEnumerable<DateTimeV2Value> Values { get; init; }
        public int Start { get; init; }
        public int End { get; init; }
    }
}
