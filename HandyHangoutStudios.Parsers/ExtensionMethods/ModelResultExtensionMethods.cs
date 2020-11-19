//    HandyHanselStudios.Parsers, rerepresents the output of the Microsoft
//    Recognizers using Classes in order to make the usage of them easier
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyHangoutStudios.Parsers
{
    public static class ModelResultExtensionMethods
    {
        public static DateTimeV2ModelResult ToDateTimeV2ModelResult(this ModelResult modelResult)
        {
            return new DateTimeV2ModelResult()
            {
                Text = modelResult.Text,
                TypeName = modelResult.GetDateTimeV2Type(),
                Start = modelResult.Start,
                End = modelResult.End,
                Values = modelResult.GetDateTimeV2Values()
            };
        }

        private static DateTimeV2Type GetDateTimeV2Type(this ModelResult model)
        {
            string modelType = model.TypeName["datetimeV2.".Length..];
            return Enum.Parse<DateTimeV2Type>(modelType, true);
        }

        private static IEnumerable<DateTimeV2Value> GetDateTimeV2Values(this ModelResult model)
        {
            List<Dictionary<string, string>> values = (List<Dictionary<string, string>>) model.Resolution["values"];
            return values.Select(value => new DateTimeV2Value(value)); // timex, type, value
        }
    }
}
