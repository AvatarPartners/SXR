using System.Collections.Generic;
using UnityEngine;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Condition)]
    public class TemplateForCondition : Conditions
    {
        // Public fields will be displayed in the Inspector when using the Node Editor
        // Tooltips for users when they hover over the field in the Inspector. Surround with the UNITY_EDITOR direcctive
        #if UNITY_EDITOR
        [Tooltip("If the condition starts true")]
        #endif
        public bool StartOn;

        // If you need to select Directives in the Inspector while using the Node Editor, place a [DirectiveSelection] attribute above any field, list or array of Directives.
        [DirectiveSelection]
        public Conditions ConditionToNOT;
        bool switchOff = true;

        /* Conditions rarely pass data, but are used to evaluate if action should continue on or not */

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            // If no Keywords are to be used (i.e. no data will be received), then return a new List<KnobKeywords>().
            return new List<KnobKeywords>();
        }

        // SendKeywords are how the developer can control what type of data is sent from this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords>();
        }

        // Use the protected keyword in front of any Monobehaviour methods
        protected void Start()
        {
            /* This Condition property of each condition is important - if Condition = true, then action keeps executing onto the whatever follows this condition,
               if Condition = false, then action will stop in the chain of actions. */
            /* The Condition property starts and defaults as TRUE */
			Condition = StartOn;
		}

        // Conditions are Actions, so the developer overrides the Execute method.
        /* The Execute method is the entry point and first thing that occurs when this Condition is told to execute. */
        public override void Execute()
		{
            if (!switchOff)
                Condition = !Condition;

            /* Must call this event when execution of this condition is complete to pass control on to whatever is next to execute. */
            ThisConditionFinished();
		}
	}
}