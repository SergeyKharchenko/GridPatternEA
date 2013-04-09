using System.Collections.Generic;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class PatternParserTests
    {
        private static readonly object[] removeSpacesData =
            {
                new object[] {"Hello to Billy", "HellotoBilly"},
                new object[] {"", ""},
                new object[] {"123", "123"}
            };

        [Test, TestCaseSource("removeSpacesData")]
        public void RemoveSpacesTest(string input, string output)
        {
            var patternParser = new PatternParser();

            var result = patternParser.RemoveSpaces(input);

            Assert.That(result, Is.EqualTo(output));
        }

        private static readonly object[] getRowsData =
            {
                new object[] {"Hello\r\nto\r\nBilly", new List<string> {"Hello", "to", "Billy"}},
                new object[] {"", new List<string>()},
                new object[] {"Hello\r\n\r\nworld", new List<string> {"Hello", "world"}},
                new object[] {"123", new List<string> {"123"}},
            };

        [Test, TestCaseSource("getRowsData")]
        public void GetRowsTest(string input, List<string> output)
        {
            var patternParser = new PatternParser();

            var result = patternParser.GetRows(input);

            Assert.That(result, Is.EqualTo(output));
        }

        private static readonly object[] removeFirstRowData =
            {
                new object[] {new List<string> {"Hello", "to", "Billy"}, new List<string> {"to", "Billy"}},
                new object[] {new List<string> {""}, new List<string>()},
                new object[] {new List<string> {"123"}, new List<string>()}
            };

        [Test, TestCaseSource("removeFirstRowData")]
        public void RemoveFirstRowTest(List<string> input, List<string> output)
        {
            var patternParser = new PatternParser();

            var result = patternParser.RemoveTitleRow(input);

            Assert.That(result, Is.EqualTo(output));
        }

        private static readonly object[] getColumnsData =
            {
                new object[]
                    {
                        new List<string> {"Hello,Billy", "to,you", "John,Doe"},
                        new List<string[]>
                            {
                                new[] {"Hello", "Billy"},
                                new[] {"to", "you"},
                                new[] {"John", "Doe"}
                            }
                    },
                new object[]
                    {
                        new List<string> {""},
                        new List<string[]> {new[] {""}}
                    },
                new object[]
                    {
                        new List<string> {"123"},
                        new List<string[]> {new[] {"123"}}
                    }
            };

        [Test, TestCaseSource("getColumnsData")]
        public void GetColumnsTest(List<string> input, List<string[]> output)
        {
            var patternParser = new PatternParser();

            var result = patternParser.GetColumns(input);

            Assert.That(result, Is.EqualTo(output));
        }

        private static readonly object[] removeFirstColumnData =
            {
                new object[]
                    {
                        new List<List<string>>
                            {
                                new List<string> {"Hello", "Billy"},
                                new List<string> {"to", "you"},
                                new List<string> {"John", "Doe"}
                            },
                        new List<List<string>>
                            {
                                new List<string> {"Billy"},
                                new List<string> {"you"},
                                new List<string> {"Doe"}
                            }
                    },
                new object[]
                    {
                        new List<List<string>> {new List<string> {""}},
                        new List<List<string>> {new List<string>()}

                    },
                new object[]
                    {
                        new List<List<string>> {new List<string> {"123"}},
                        new List<List<string>> {new List<string>()}
                    }
            };

        [Test, TestCaseSource("removeFirstColumnData")]
        public void RemoveFirstColumnTest(List<List<string>> input, List<List<string>> output)
        {
            var patternParser = new PatternParser();

            var result = patternParser.RemoveTitleColumn(input);

            Assert.That(result, Is.EqualTo(output));
        }

        private const string Pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,B1X; S1X,20,N,10,N,20,N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; S2X,20,B3X
F,,,,,,,,-20,B3X
G,,,,,,-20,S2X,10,N
H,,,,,,,,-10,N
I,,-10,B2; B1X,30,S1X,20,N,10,B2X
J,,,,,,,,-10,B2X
K,,,,,,-20,N,20,B2X
L,,,,,,,,-20,B2X
M,,,,-30,N,10,S1X,30,B2X
N,,,,,,,,-30,B2X
O,,,,,,-10,N,20,B2X; S1X
P,,,,,,,,-20,B2X; S1X";

        [Test]
        public void ParseOriginalDataTest()
        {
            var patternParser = new PatternParser();

            var columns = patternParser.Parse(Pattern);

            Assert.That(columns.First().First(), Is.EqualTo("B1;S1"));
            Assert.That(columns.First().Last(), Is.EqualTo("N"));
            Assert.That(columns.Last().First(), Is.EqualTo(""));
            Assert.That(columns.Last().Last(), Is.EqualTo("B2X;S1X"));
        }        
    }
}