using Microsoft.Recognizers.Text.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
