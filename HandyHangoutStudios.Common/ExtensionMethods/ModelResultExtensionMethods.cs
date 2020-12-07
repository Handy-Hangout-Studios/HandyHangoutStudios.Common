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
using HandyHangoutStudios.Parsers.Resolutions;
using Microsoft.Recognizers.Text;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyHangoutStudios.Parsers
{
    public static class ModelResultExtensionMethods
    {
        public static DateTimeV2ModelResult ToDateTimeV2ModelResult(this ModelResult modelResult, DateTimeZone timeZone)
        {
            return new DateTimeV2ModelResult()
            {
                Text = modelResult.Text,
                TypeName = modelResult.GetDateTimeV2Type(),
                Start = modelResult.Start,
                End = modelResult.End,
                Values = modelResult.GetDateTimeV2Values(timeZone)
            };
        }

        private static DateTimeV2Type GetDateTimeV2Type(this ModelResult model)
        {
            string modelType = model.TypeName["datetimeV2.".Length..];
            return Enum.Parse<DateTimeV2Type>(modelType, true);
        }

        private static IEnumerable<DateTimeV2Value> GetDateTimeV2Values(this ModelResult model, DateTimeZone timeZone)
        {
            List<Dictionary<string, string>> values = (List<Dictionary<string, string>>) model.Resolution["values"];
            return values.Select(value => new DateTimeV2Value(value, timeZone)); // timex, type, value
        }
    }
}
