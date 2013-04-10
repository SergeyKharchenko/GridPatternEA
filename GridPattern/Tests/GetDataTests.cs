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
