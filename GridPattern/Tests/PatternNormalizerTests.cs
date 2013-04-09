using System.Collections.Generic;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PatternNormalizerTests
    {
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

        private PatternNormalizer patternNormalizer;
        private List<List<string>> data;

        [SetUp]
        public void SetUp()
        {
            var patternParser = new PatternParser();
            data = patternParser.Parse(Pattern);
            patternNormalizer = new PatternNormalizer();
        }

        [Test]
        public void TransferDownPatternTest()
        {
            var result = patternNormalizer.TransferDownPattern(data);

            Assert.That(result[0][0], Is.EqualTo("B1;S1"));
            Assert.That(result[1][0], Is.EqualTo("B1;S1"));
            Assert.That(result[15][0], Is.EqualTo("B1;S1"));
            Assert.That(result[8][1], Is.EqualTo("-10"));
            Assert.That(result[15][1], Is.EqualTo("-10"));
            Assert.That(result[0][8], Is.EqualTo("N"));
        }
    }
}