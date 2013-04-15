using System.Collections.Generic;
using GridPatternLibrary;
using GridPatternLibrary.Helpers.Concrete;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ActionParserTests
    {
        private readonly object[] parseData = new object[]
            {
                new object[] {"B1", new List<TradeAction>
                    {
                        new TradeAction("B", 1)
                    }},
                new object[] {"B1;S1", new List<TradeAction>
                    {
                        new TradeAction("B", 1),
                        new TradeAction("S", 1)
                    }},
                new object[] {"BX12;S2;SX1", new List<TradeAction>
                    {
                        new TradeAction("BX", 12),
                        new TradeAction("S", 2),
                        new TradeAction("SX", 1)
                    }},
            };

        [Test, TestCaseSource("parseData")]
        public void ParseTest(string data, List<TradeAction> actions)
        {
            var actionList = ActionParser.Parse(data);

            for (var i = 0; i < actionList.Count; i++)
            {
                Assert.That(actionList[i].Action, Is.EqualTo(actions[i].Action));
                Assert.That(actionList[i].Magic, Is.EqualTo(actions[i].Magic));   
            }            
        }
    }
}