using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SimplifyXR
{
    //##Instructions: An example LoadContent Example. Most of this class can be duplicated when 
    //creating a custom Step Type. See specific instructions on modifications
    [DirectiveCategory(DirectiveCategories.Action, DirectiveSubCategory.StepByStep)]
    public class LoadCustomStepExampleContent : Actions
    {
        //##Instructions: Create a new field for each of the UI elements the content will be loaded into
        /// <summary>
        /// Text field showing the number of the Step
        /// </summary>
        #if UNITY_EDITOR
        [Tooltip("Text field showing the number of the Step")]
        #endif
        public Text StepNumber;
        /// <summary>
        /// Text field for instruction
        /// </summary>
        #if UNITY_EDITOR
        [Tooltip("Text field for instruction")]
        #endif
        public Text Instruction;
        /// <summary>
        /// The step to load
        /// </summary>
        protected string step;
        /// <summary>
        /// Base Step Loader
        /// </summary>
        protected CustomStepExampleLoader stepLoader;

        public override List<KnobKeywords> ReceiveKeywords()
        {
			return new List<KnobKeywords> { new KnobKeywords("StepLabel", typeof(string)) };
        }

        public override List<KnobKeywords> SendKeywords()
        {
			return new List<KnobKeywords> { new KnobKeywords("StepLabel", typeof(string)) };
        }

        public override void Execute()
        {
            GetPassedStepName();
            LoadContent();
			SendStepName();
            step = null;
            ThisActionCompleted();
        }

        /// <summary>
        /// Get the step to be loaded if sent.
        /// </summary>
        protected void GetPassedStepName()
        {
            var objectPassed = GetPassableData();
            if (objectPassed == null) return;
            
            if (KeywordInUse == "StepLabel")
            {
                var _name = objectPassed as string;
                if (!string.IsNullOrEmpty(_name))
                    step = _name;
            }
        }

        protected void LoadContent()
        {
            //##Instructions: Change this type to the custom Loader
            stepLoader = new CustomStepExampleLoader();
            if (stepLoader.StepTypeCheck())
            {
				if (!string.IsNullOrEmpty(step))
				{
					if (stepLoader.LoadStepContent(step))
                        Debug.LogFormat("Loading content from Step Loader for step: {0}", step);
					else
					{
                        Debug.LogWarningFormat("Could not load content from Step Loader for step: {0}", step);
						return;
					}
				}
				else
				{
					if (stepLoader.LoadStepContent())
                        Debug.Log("Loading content from Step Loader for the current step in the Step Manager.");
					else
					{
                        Debug.LogWarning("Could not load content from Step Loader for the current step in the Step Manager.");
						return;
					}
				}
                DisplayContent();
            }
            else
                Debug.LogFormat("Not using this Step Content Loader Action. No step content will be loaded with {0}.", this);
        }

        /// <summary>
        /// Display the content in the fields
        /// </summary>
        protected void DisplayContent()
        {
            step = stepLoader.StepLabel;

            //##Instructions: Fill all of the UI elements with the values coming from the loader.
            if (StepNumber != null)
            {
                if (!string.IsNullOrEmpty(stepLoader.StepNumber))
                    StepNumber.text = stepLoader.StepNumber;
                else
                    StepNumber.text = "";
            }

            if (Instruction != null)
            {
                if (!string.IsNullOrEmpty(stepLoader.StepInstructions))
                    Instruction.text = stepLoader.StepInstructions;
                else
                    Instruction.text = "";
            }
        }

        /// <summary>
        /// Pass along the loaded step name
        /// </summary>
        protected void SendStepName()
		{
            if (!string.IsNullOrEmpty(step))
            {
			    AddPassableData(new List<string> { "StepLabel" }, new List<object> { step });
            }
		}
    }
}