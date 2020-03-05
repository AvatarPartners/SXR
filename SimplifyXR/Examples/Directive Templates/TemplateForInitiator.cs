using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Initiator, DirectiveSubCategory.Other)]
    public class TemplateForInitiator : Initiator
    {
        // Public fields will be displayed in the Inspector when using the Node Editor
        // Tooltips for users when they hover over the field in the Inspector. Surround with the UNITY_EDITOR direcctive
        #if UNITY_EDITOR
        [Tooltip("The Button that will be pressed")]
        #endif
        public Button ButtonToPress;

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            // If no Keywords are to be used (i.e. no data will be received), then return a new List<KnobKeywords>().
            return new List<KnobKeywords>();
        }

        public override List<KnobKeywords> SendKeywords()
        {
            // KnobKeywords consist of the string label for the Node Editor and the System.Type of the data that can be used
            return new List<KnobKeywords>(){new KnobKeywords("ButtonGameObject", typeof(GameObject)), new KnobKeywords("ButtonTitle", typeof(string)),
                   new KnobKeywords("DeleteThis", typeof(string))};
        }

       // Use the protected keyword in front of any Monobehaviour methods
        protected new void Awake()
        {
            // If you use the Awake method in an Initiator, be sure to call the base.Awake method as well.
            base.Awake();
        }
        void MyButtonPressed()
        {
            // If you don't override the Initiate method like further below, then it just calls the Initiate method in the base Initiator class, which is fine.
            Initiate();

            // The SimplifyXRDebug messages can be displayed at runtime and in the editor.
            // This is SimplifyXRLog pattern to follow which includes the SimplifyXRDebug message type, the message, and the format arguments for the message.
            SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.AuthorDebug, "This button {0} has been pressed.", SimplifyXRDebug.Args(ButtonToPress));
        }

        void SendData()
        {
            // You will send a List<string> from your SendKeywords to be used as labels in the Node Editor,
            // and a corresponding List<object>, which is the data you desire to pass along.
            var thisKeywords = new List<string> { "ButtonGameObject", "ButtonTitle","DeleteThis"};
            var thisData = new List<object> { ButtonToPress.gameObject, ButtonToPress.gameObject.GetComponentInChildren<Text>().text,"TestText"};

            // When sending data from this Directive, use the AddPassableData method.
            AddPassableData(thisKeywords, thisData);
            SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.DeveloperDebug, "This button {0} has sent data.", SimplifyXRDebug.Args(ButtonToPress));
        }

        /* The Initiate method is jumping off point for your sequence and how it is initiated, so you need to call this method. */
        // If you override the Initiate method like below, then be sure to call base.Initiate()
        public override void Initiate()
        {
            // Good practice to have any important functions of this Initiator somehow launched from this Initiate method. Other Directives, like
            // Voice Command and Cross Sequence initiators, will call this Initiate method
            SendData();
            base.Initiate();
        }
    }
}