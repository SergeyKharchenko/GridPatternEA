using System.Collections.Generic;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PatternNormalizerTests
    {
        private const string Pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1";

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