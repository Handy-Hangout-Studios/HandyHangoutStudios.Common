using HandyHangoutStudios.Parsers.Comparers;
using HandyHangoutStudios.Parsers.Models;
using HandyHangoutStudios.Parsers.Resolutions;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Microsoft.Recognizers.Text.DateTime;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HandyHangoutStudios.Parsers.Tests
{
    public class DateTimeV2Model
    {
        [Theory]
        [ClassData(typeof(TestDateDataGenerator))]
        public void TestDateTimeV2ParseToModelConversion(string input, DateTime refTime, DateTimeV2ModelResult expectedResult)
        {
            IEnumerable<ModelResult> recognizerResults = DateTimeRecognizer.RecognizeDateTime(input, Culture.English, refTime: refTime);
            DateTimeV2ModelResult testResult = recognizerResults.Select(item => item.ToDateTimeV2ModelResult()).FirstOrDefault();

            Assert.NotNull(testResult);
            Assert.True(testResult.Text == expectedResult.Text);
            Assert.True(testResult.TypeName == expectedResult.TypeName);
            Assert.True(testResult.Start == expectedResult.Start);
            Assert.True(testResult.End == expectedResult.End);
            Assert.True(testResult.Values.SequenceEqual(expectedResult.Values, new DateTimeV2ValueComparer() ));
        }

        public class TestDateDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[]
                {
                    "I'll go back 04th Jan 2019",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "04th jan 2019",
                        Start = 13,
                        End = 25,
                        TypeName = DateTimeV2Type.Date,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("2019-01-04"),
                                Type = DateTimeV2Type.Date,
                                Value = new LocalDate(2019, 1, 4)
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll go back 03rd Jan 2019.",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "03rd jan 2019",
                        Start = 13,
                        End = 25,
                        TypeName = DateTimeV2Type.Date,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("2019-01-03"),
                                Type = DateTimeV2Type.Date,
                                Value = new LocalDate(2019, 1, 3)
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll go back Oct/2",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "oct/2",
                        Start = 13,
                        End = 17,
                        TypeName = DateTimeV2Type.Date,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("XXXX-10-02"),
                                Type = DateTimeV2Type.Date,
                                Value = new LocalDate(2016, 10, 02)
                            },
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("XXXX-10-02"),
                                Type = DateTimeV2Type.Date,
                                Value = new LocalDate(2017, 10, 02)
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll be out from 4-23 in next month",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "from 4-23 in next month",
                        Start = 12,
                        End = 34,
                        TypeName = DateTimeV2Type.DateRange,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("(2016-12-04,2016-12-23,P19D)"),
                                Type = DateTimeV2Type.DateRange,
                                Value = new Range<LocalDate>()
                                {
                                    Start = new LocalDate(2016, 12, 4),
                                    End = new LocalDate(2016, 12, 23)
                                }
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll be out between 3 and 12 of Sept hahaha",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "between 3 and 12 of sept",
                        Start = 12,
                        End = 35,
                        TypeName = DateTimeV2Type.DateRange,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("(XXXX-09-03,XXXX-09-12,P9D)"),
                                Type = DateTimeV2Type.DateRange,
                                Value = new Range<LocalDate>()
                                {
                                    Start = new LocalDate(2016, 9, 3),
                                    End = new LocalDate(2016, 9, 12)
                                }
                            },
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("(XXXX-09-03,XXXX-09-12,P9D)"),
                                Type = DateTimeV2Type.DateRange,
                                Value = new Range<LocalDate>()
                                {
                                    Start = new LocalDate(2017, 9, 3),
                                    End = new LocalDate(2017, 9, 12)
                                }
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll go back October 14 for 8:00:31am",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "october 14 for 8:00:31am",
                        Start = 13,
                        End = 36,
                        TypeName = DateTimeV2Type.DateTime,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("XXXX-10-14T08:00:31"),
                                Type = DateTimeV2Type.DateTime,
                                Value = new LocalDateTime(2016, 10, 14, 8, 0, 31)
                            },
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("XXXX-10-14T08:00:31"),
                                Type = DateTimeV2Type.DateTime,
                                Value = new LocalDateTime(2017, 10, 14, 8, 0, 31)
                            }
                        }
                    }
                },
                new object[]
                {
                    "I'll go back tomorrow 8:00am",
                    new DateTime(2016, 11, 7, 0, 0, 0),
                    new DateTimeV2ModelResult()
                    {
                        Text = "tomorrow 8:00am",
                        Start = 13,
                        End = 27,
                        TypeName = DateTimeV2Type.DateTime,
                        Values = new List<DateTimeV2Value>()
                        {
                            new DateTimeV2Value()
                            {
                                Timex = new TimexProperty("2016-11-08T08:00"),
                                Type = DateTimeV2Type.DateTime,
                                Value = new LocalDateTime(2016, 11, 8, 8, 0, 0)
                            }
                        }
                    }
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
