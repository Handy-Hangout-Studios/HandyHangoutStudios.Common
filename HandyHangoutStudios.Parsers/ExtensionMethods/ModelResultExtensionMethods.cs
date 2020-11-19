using HandyHangoutStudios.Parsers.Models;
using HandyHangoutStudios.Parsers.Resolutions;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
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
