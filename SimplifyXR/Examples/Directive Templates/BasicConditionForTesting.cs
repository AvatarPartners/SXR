using System.Collections.Generic;

namespace SimplifyXR
{
    [DirectiveCategory(false)]
    public class BasicConditionForTesting : Conditions , ICanToggle
    {
        public bool StartOn;

        public override List<KnobKeywords> ReceiveKeywords()
        {
            return new List<KnobKeywords>();
        }

        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords>();
        }

        protected void Start()
        {
			Condition = StartOn;
		}

        public override void Execute()
		{
            ThisConditionFinished();
		}

        string IModify<ICanToggle>.ModifyObjectName {get {return "Condition";}}

        public void SetTrue()
        {
            Condition = true;
        }

        public void SetFalse()
        {
            Condition = false;
        }

        public void Toggle()
        {
            Condition = !Condition;
        }
    }
}