using System.Collections.Generic;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PatternDispatcherTests
    {
        [Test]
        public void DispatchTest()
        {
            var pattern = new List<List<string>>
                {
                    new List<string> {"B1;B2", "10", "S1", "20", "BX1", "-30", "SX1", "-10", "N"},
                    new List<string> {"B2", "100", "S2", "-20", "BX2", "-30", "N", "-10", "N"}
                };

            var patternDispatcher = new PatternDispatcher();
            var dispatchedPattern = patternDispatcher.Dispatch(pattern);

            Assert.That(dispatchedPattern.Legs.Count, Is.EqualTo(2));
            Assert.That(dispatchedPattern.Legs[0].Count, Is.EqualTo(4));
            Assert.That(dispatchedPattern.Legs[1].Count, Is.EqualTo(4));
            Assert.That(dispatchedPattern.Legs[0][0], Is.EqualTo(10));
            Assert.That(dispatchedPattern.Legs[0][1], Is.EqualTo(20));
            Assert.That(dispatchedPattern.Legs[0][2], Is.EqualTo(-30));
            Assert.That(dispatchedPattern.Legs[0][3], Is.EqualTo(-10));
            Assert.That(dispatchedPattern.Legs[1][0], Is.EqualTo(100));
            Assert.That(dispatchedPattern.Legs[1][1], Is.EqualTo(-20));
            Assert.That(dispatchedPattern.Legs[1][2], Is.EqualTo(-30));
            Assert.That(dispatchedPattern.Legs[1][3], Is.EqualTo(-10));

            Assert.That(dispatchedPattern.Actions.Count, Is.EqualTo(2));
            Assert.That(dispatchedPattern.Actions[0].Count, Is.EqualTo(5));
            Assert.That(dispatchedPattern.Actions[1].Count, Is.EqualTo(5));
            Assert.That(dispatchedPattern.Actions[0][0], Is.EqualTo("B1;B2"));
            Assert.That(dispatchedPattern.Actions[0][1], Is.EqualTo("S1"));
            Assert.That(dispatchedPattern.Actions[0][2], Is.EqualTo("BX1"));
            Assert.That(dispatchedPattern.Actions[0][3], Is.EqualTo("SX1"));
            Assert.That(dispatchedPattern.Actions[0][4], Is.EqualTo("N"));
            Assert.That(dispatchedPattern.Actions[1][0], Is.EqualTo("B2"));
            Assert.That(dispatchedPattern.Actions[1][1], Is.EqualTo("S2"));
            Assert.That(dispatchedPattern.Actions[1][2], Is.EqualTo("BX2"));
            Assert.That(dispatchedPattern.Actions[1][3], Is.EqualTo("N"));
            Assert.That(dispatchedPattern.Actions[1][4], Is.EqualTo("N"));
        }
    }
}