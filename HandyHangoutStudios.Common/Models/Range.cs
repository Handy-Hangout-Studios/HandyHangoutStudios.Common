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

namespace HandyHangoutStudios.Parsers.Models
{
    /// <summary>
    /// Represents a range of generics
    /// </summary>
    /// <typeparam name="T">A type for the range. Be sure that you are using something that makes sense.</typeparam>
    public class Range<T>
    {
        /// <summary>
        /// Start of the range of generics
        /// </summary>
        public T Start { get; init; }

        /// <summary>
        /// End of the range of generics
        /// </summary>
        public T End { get; init; }

        public override bool Equals(object obj)
        {
            if (obj is Range<T> second)
                return Start.Equals(second.Start) && End.Equals(second.End);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
