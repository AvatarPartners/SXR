using System.Collections.Generic;

namespace SimplifyXR
{
    [DirectiveCategory(false)]
    public class NOTConditionForTesting : Conditions
    {
        [DirectiveSelection]
        public Conditions ConditionToNOT;

        public override List<KnobKeywords> ReceiveKeywords()
        {
            return new List<KnobKeywords>();
        }

        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords>();
        }

		public override void Execute()
		{
            if (ConditionToNOT != null)
			    ConditionToNOT.Condition = !ConditionToNOT.Condition;
			ThisConditionFinished();
		}
	}
}