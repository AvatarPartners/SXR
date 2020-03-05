using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Action, DirectiveSubCategory.Other)]
    public class TemplateForActionWithModifier : Actions , ICanToggle, ITemplateModifier  /* Here using the Modifier's interfaces */
    {
        // Public fields will be displayed in the Inspector when using the Node Editor
        // Tooltips for users when they hover over the field in the Inspector. Surround with the UNITY_EDITOR direcctive
        #if UNITY_EDITOR
        [Tooltip("The Text field to change")]
        #endif
        public Text TextFieldToChange;
        public bool UsePassedText;
        public string NewText;
        int counter;

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            // KnobKeywords consist of the string label for the Node Editor and the System.Type of the data that can be used
            return new List<KnobKeywords> { new KnobKeywords("GameObjectForName", typeof(GameObject)), new KnobKeywords("NewText", typeof(string)) };
        }

         // SendKeywords are how the developer can control what type of data is sent from this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> SendKeywords()
        {
            // If no Keywords are to be used (i.e. no data will be passed), then return a new List<KnobKeywords>().
            return new List<KnobKeywords>();
        }

        /* The Execute method is the entry point and first thing that occurs when this Action is told to execute. */
        public override void Execute()
        {
            if (UsePassedText)
                GetNewText();
            if (TextFieldToChange != null && !string.IsNullOrEmpty(NewText))
                TextFieldToChange.text = NewText + " " + (++counter).ToString();

            /* Must call this event when execution of this Action is complete to pass control on to whatever is next to execute. */
            ThisActionCompleted();
        }

        void GetNewText()
        {
            // Use the GetPassableData method to get the object passed to this Directive at runtime.
            // The appropriate KeywordInUse will also be set at runtime.
            var objectPassed = GetPassableData();
            if (objectPassed != null)
            {
                 // Then check for each keyword from your RecieveKeywords and correctly cast the passed object
               if (KeywordInUse == "GameObjectForName")
                {
                    var myObject = objectPassed as GameObject;
                    NewText = myObject.name;
                }
                else if (KeywordInUse == "NewText")
                {
                    NewText = objectPassed as string;
                }
            }
            else
                NewText = "";
        }

        /* Implementing the Modifier's interfaces. The Modifier (e.g. ModifyToggle), when executed, will call these methods. */
        public void DoThisInAction()
        {
            // Developer inserts the desired code to execute on this action when the Modifier calls these methods. 
            Debug.Log("Templated action.");
        }

        #region ICanToggle implementation
        /* This is the description that will appear when the connection knob in the node editor is right clicked */
        string IModify<ICanToggle>.ModifyObjectName {get {return "Descriptive name";}}

        public void SetTrue()
        {
            Debug.Log("Setting True");
        }
        public void SetFalse()
        {
            Debug.Log("Setting False");
        }
        public void Toggle()
        {
            Debug.Log("Setting Toggle");
        }
        #endregion
    }
}