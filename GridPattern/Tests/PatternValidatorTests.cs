using System.Collections.Generic;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class PatternValidatorTests
    {        
        private PatternValidator patternValidator;

        [SetUp]
        public void SetUp()
        {
            patternValidator = new PatternValidator();
        }

        #region IsSizeValidTest

        [Test]
        public void IsSizeValidTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsSizeValid(data);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void IsSizeValidRowCountFailTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsSizeValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo("Pattern row count is invalid"));
        }

        private static readonly object[] isSizeValidRowLengthFailData =
            {
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20
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
P,,,,,,,,-20,BX2; SX1", "A"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
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
P,,,,,,,,-20,BX2; SX1", "B"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "G"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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
P,,,,,,,-20,BX2; SX1", "P"}
            };

        [Test, TestCaseSource("isSizeValidRowLengthFailData")]
        public void IsSizeValidRowLengthFailTest(string pattern, string rowName)
        {
            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsSizeValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(string.Format("Pattern row: {0} is invalid", rowName)));
        }

        #endregion

        #region IsPositionsValidTest

        [Test]
        public void IsPositionsValidTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsPositionsValid(data);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        private static readonly object[] isPositionsValidData =
            {
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,
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
P,,,,,,,,-20,BX2; SX1", "Invalid element position in pattern row A"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,B5,,,,,,-20,N
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
P,,,,,,,,-20,BX2; SX1", "Invalid element position in pattern row B"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid element position in pattern row J"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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
P,,,,,,,S5,-20,BX2; SX1", "Invalid element position in pattern row P"},
            };

        [Test, TestCaseSource("isPositionsValidData")]
        public void IsPositionsValidFailTest(string pattern, string error)
        {
            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsPositionsValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(error));
        }

        #endregion

        #region IsSyntaxValidTest

        [Test]
        public void IsSyntaxValidTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsSyntaxValid(data);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        private static readonly object[] isSyntaxValidFailData =
            {
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,Z,BX1; SX1,20,N,10,N,20
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
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row A: Z"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; SS1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
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
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row A: SS1"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,B,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row F: B"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,NN
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
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row B: NN"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; B3; SX2,20,BX3
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
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row E: B3"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
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
P,,,,,,,,-20-20,BX2; SX1", "Invalid action in pattern row P: -20-20"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
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
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row A: B1S1"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1;S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B100; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row I: B100"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1;S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B-3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B1; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row E: B-3"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1;S1,10,BX1; SX1,20,N,10,N,20,N
B,,,,,,,,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B1; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,B0X
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid action in pattern row N: B0X"}
            };

        [Test, TestCaseSource("isSyntaxValidFailData")]
        public void IsSyntaxValidFailTest(string pattern, string error)
        {
            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsSyntaxValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(error));
        }

        #endregion

        #region IsCloseActionPositionValidTest

        [Test]
        public void IsCloseActionPositionValidTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);
            var patternNormalizer = new PatternNormalizer();
            data = patternNormalizer.TransferDownPattern(data);

            var result = patternValidator.IsCloseActionPositionValid(data);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        private static readonly object[] isCloseActionPositionValidFailData =
            {
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid close action position in pattern row H: BX2"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX3
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid close action position in pattern row I: BX3"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,N
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX4
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Invalid close action position in pattern row K: BX4"}
            };

        [Test, TestCaseSource("isCloseActionPositionValidFailData")]
        public void IsCloseActionPositionValidFailTest(string pattern, string error)
        {
            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);
            var patternNormalizer = new PatternNormalizer();
            data = patternNormalizer.TransferDownPattern(data);

            var result = patternValidator.IsCloseActionPositionValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(error));
        }

        #endregion

        #region IsActionDuplicatePositionValidTest

        [Test]
        public void IsActionDuplicatePositionValidTest()
        {
            const string pattern = @",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
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

            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsActionDuplicateValid(data);

            Assert.That(result.Count, Is.EqualTo(0));
        }

        private static readonly object[] isActionDuplicateValidFailData =
            {
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,B1,10,N,20, N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Duplicate action in pattern row A: B1"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20, N
B,,,,,,,,-20,B1
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; SX1
P,,,,,,,,-20,BX2; SX1", "Duplicate action in pattern row B: B1"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20, N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; B1
P,,,,,,,,-20,BX2; SX1", "Duplicate action in pattern row O: B1"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20, N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,N,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; B2
P,,,,,,,,-20,BX2; SX1", "Duplicate action in pattern row O: B2"},
                new object[] {@",Gate0,Leg1,Gate1,Leg2,Gate2,Leg3,Gate3,Leg4,Gate4
A,B1; S1,10,BX1; SX1,20,N,10,N,20, N
B,,,,,,,,-20,N
C,,,,,,-10,N,30,N
D,,,,,,,,-30,N
E,,,,-20,S2,20,B3; SX2,20,BX3
F,,,,,,,,-20,BX3
G,,,,,,-20,SX2,10,N
H,,,,,,,,-10,BX2
I,,-10,B2; BX1,30,SX1,20,N,10,BX2
J,,,,,,,,-10,BX2
K,,,,,,-20,N,20,BX2
L,,,,,,,,-20,BX2
M,,,,-30,BX1,10,SX1,30,BX2
N,,,,,,,,-30,BX2
O,,,,,,-10,N,20,BX2; N
P,,,,,,,,-20,BX2; SX1", "Duplicate action in pattern row M: BX1"},
            };

        [Test, TestCaseSource("isActionDuplicateValidFailData")]
        public void IsActionDuplicateValidFailTest(string pattern, string error)
        {
            var patternParser = new PatternParser();
            var data = patternParser.Parse(pattern);

            var result = patternValidator.IsActionDuplicateValid(data);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(), Is.EqualTo(error));
        }

        #endregion
    }
}