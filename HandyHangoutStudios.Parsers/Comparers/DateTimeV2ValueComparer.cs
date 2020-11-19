using HandyHangoutStudios.Parsers.Resolutions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyHangoutStudios.Parsers.Comparers
{
    public class DateTimeV2ValueComparer : IEqualityComparer<DateTimeV2Value>
    {
        public bool Equals(DateTimeV2Value x, DateTimeV2Value y)
        {
            return x.Timex.ToString().Equals(y.Timex.ToString()) &&
                   x.Type.Equals(y.Type) &&
                   x.Value.Equals(y.Value);
        }

        public int GetHashCode([DisallowNull] DateTimeV2Value obj)
        {
            return obj.GetHashCode();
        }
    }
}
