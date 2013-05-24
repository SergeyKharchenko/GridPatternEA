using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GridPatternLibrary;
using GridPatternLibrary.Helpers.Abstract;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class GetDataTests
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

        [Test]
        public void GetDataCallsVerifyTest()
        {
            var fileHelperMock = new Mock<IFileHelper>();
            Connector.FileHelper = fileHelperMock.Object;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"experts\files\pattern.csv");
            fileHelperMock.Setup(helper => helper.ReadFile(filePath)).Returns(Pattern);

            var patternParserMock = new Mock<IPatternParser>();
            Connector.PatternParser = patternParserMock.Object;
            var parsedPattern = new List<List<string>>
                {
                    new List<string> {"B1", "10", "BX1"}
                };
            patternParserMock.Setup(helper => helper.Parse(Pattern)).Returns(parsedPattern);

            var patternValidatorMock = new Mock<IPatternValidator>();
            Connector.PatternValidator = patternValidatorMock.Object;
            patternValidatorMock.Setup(helper => helper.IsSizeValid(parsedPattern)).Returns(new List<string>());
            patternValidatorMock.Setup(helper => helper.IsPositionsValid(parsedPattern)).Returns(new List<string>());
            patternValidatorMock.Setup(helper => helper.IsSyntaxValid(parsedPattern)).Returns(new List<string>());
            patternValidatorMock.Setup(helper => helper.IsTypesValid(parsedPattern)).Returns(new List<string>());
            patternValidatorMock.Setup(helper => helper.IsActionDuplicateValid(parsedPattern)).Returns(new List<string>());

            var patternNormalizerMock = new Mock<IPatternNormalizer>();
            Connector.PatternNormalizer = patternNormalizerMock.Object;
            var normalizedPattern = new List<List<string>>
                {
                    new List<string> {"B1", "10", "BX1", "20", "S1"}
                };
            patternNormalizerMock.Setup(helper => helper.TransferDownPattern(parsedPattern)).Returns(normalizedPattern);

            patternValidatorMock.Setup(helper => helper.IsCloseActionPositionValid(normalizedPattern)).Returns(new List<string>());

            var patternDispatcherMock = new Mock<IPatternDispatcher>();
            Connector.PatternDispatcher = patternDispatcherMock.Object;
            patternDispatcherMock.Setup(helper => helper.Dispatch(normalizedPattern)).Returns(new DispatchedPattern(true, string.Empty));

            var error = string.Empty;
            var dispatchedPattern = Connector.GetData(Directory.GetCurrentDirectory(), "pattern.csv");

            fileHelperMock.Verify(helper => helper.ReadFile(filePath), Times.Once());
            patternParserMock.Verify(helper => helper.Parse(Pattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsSizeValid(parsedPattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsPositionsValid(parsedPattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsSyntaxValid(parsedPattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsTypesValid(parsedPattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsActionDuplicateValid(parsedPattern), Times.Once());
            patternNormalizerMock.Verify(helper => helper.TransferDownPattern(parsedPattern), Times.Once());
            patternValidatorMock.Verify(helper => helper.IsCloseActionPositionValid(normalizedPattern), Times.Once());
            patternDispatcherMock.Verify(helper => helper.Dispatch(normalizedPattern), Times.Once());

            Assert.That(dispatchedPattern.Success, Is.True);
            Assert.That(error, Is.EqualTo(string.Empty));
        }
    }
}
