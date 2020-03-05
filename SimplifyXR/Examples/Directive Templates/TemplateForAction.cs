using UnityEngine;
using System.Collections.Generic;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Action, DirectiveSubCategory.Other)]
    public class TemplateForAction : Actions
    {
        // Public fields will be displayed in the Inspector when using the Node Editor
        // Tooltips for users when they hover over the field in the Inspector. Surround with the UNITY_EDITOR direcctive
        #if UNITY_EDITOR
        [Tooltip("The GameObject to change color")]
        #endif
        public GameObject ObjectToChangeColor;
        public Color FirstColor, SecondColor;

        // If you need to select Directives in the Inspector while using the Node Editor, place a [DirectiveSelection] attribute above any field, list or array of Directives.
        [DirectiveSelection]
        public List<Actions> ActionsToShow;

        Material material;

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            // KnobKeywords consist of the string label for the Node Editor and the System.Type of the data that can be used
			return new List<KnobKeywords> { new KnobKeywords("SendTransformToToggle", typeof(Transform)), new KnobKeywords("SendObjectToToggle", typeof(GameObject)) };
        }

        // SendKeywords are how the developer can control what type of data is sent from this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> SendKeywords()
        {
            // KnobKeywords consist of the string label for the Node Editor and the System.Type of the data that can be used
			return new List<KnobKeywords> { new KnobKeywords("MyColor", typeof(Color)), new KnobKeywords("MyMaterial", typeof(Material)) };
        }
        
        // Use the protected keyword in front of any Monobehaviour methods
        protected void OnEnable()
        {
        }

        /* The Execute method is the entry point and first thing that occurs when this Action is told to execute. */
        public override void Execute()
        {
            FindObjectTouched();
            GetAndChangeMaterial();
			SendColor();

            /* Must call this event when execution of this Action is complete to pass control on to whatever is next to execute. */
            ThisActionCompleted();
        }

        void GetAndChangeMaterial()
        {
            if (material = ObjectToChangeColor.GetComponent<Renderer>().material)
            {
                if (material.color == FirstColor)
                    material.color = SecondColor;
                else
                    material.color = FirstColor;
            }
        }

        void FindObjectTouched()
        {
            // Use the GetPassableData method to get the object passed to this Directive at runtime.
            // The appropriate KeywordInUse will also be set at runtime.
            var objectPassed = GetPassableData();
            // It is wise to check if it is null
            if (objectPassed != null)
            {
                // Then check for each keyword from your RecieveKeywords and correctly cast the passed object
                if (KeywordInUse == "SendObjectToToggle")
                    ObjectToChangeColor = objectPassed as GameObject;
                else if (KeywordInUse == "SendTransformToToggle")
                {
                    var t = objectPassed as Transform;
                    ObjectToChangeColor.transform.position = t.position;
                }
            }
        }

        void SendColor()
		{
            // When sending data from this Directive, use the AddPassableData method.
            // You will send a List<string> from your SendKeywords to be used as labels in the Node Editor,
            // and a corresponding List<object>, which is the data you desire to pass along.
			AddPassableData(new List<string> { "MyColor", "MyMaterial" }, new List<object> { material.color, material });
		}
    }
}