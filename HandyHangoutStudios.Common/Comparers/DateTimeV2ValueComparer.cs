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

using HandyHangoutStudios.Parsers.Resolutions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
