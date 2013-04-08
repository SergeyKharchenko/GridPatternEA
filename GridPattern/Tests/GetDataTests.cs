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
        public void GetDataTest()
        {
            var fileHelperMock = new Mock<IFileHelper>();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"experts\libraries\pattern.csv");
            fileHelperMock.Setup(helper => helper.ReadFile(filePath)).Returns(Pattern);
            Connector.FileHelper = fileHelperMock.Object;

            var result = Connector.GetData("pattern.csv", null, null);

            Assert.That(result, Is.True);
            fileHelperMock.Verify(helper => helper.ReadFile(filePath), Times.Once());
        }
    }
}
