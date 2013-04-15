namespace GridPatternLibrary
{
    public class TradeAction
    {
        public string Action { get; private set; }
        public int Magic { get; private set; }

        public TradeAction(string action, int magic)
        {
            Action = action;
            Magic = magic;
        }
    }
}