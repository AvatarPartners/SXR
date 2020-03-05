using System.Collections.Generic;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Action, DirectiveSubCategory.Filter)]
    public class TemplateForFilter : Actions
    {
        string result;

        /* Filters' purpose are to accept data, format the data, and then pass the formatted data on */

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            // KnobKeywords consist of the string label for the Node Editor and the System.Type of the data that can be used
            return new List<KnobKeywords> { new KnobKeywords("ObjectToFilter", typeof(object)) };
        }

        // SendKeywords are how the developer can control what type of data is sent from this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords> { new KnobKeywords("FilteredString", typeof(string)) };
        }

        // Filters are Actions, so the developer overrides the Execute method.
        /* The Execute method is the entry point and first thing that occurs when this Filter is told to execute. */
        public override void Execute()
        {
            GetData();
            SendData(result);

            /* Must call this event when execution of this filter is complete to pass control on to whatever is next to execute. */
            ThisActionCompleted();
        }

        void GetData()
        {
            // Use the GetPassableData method to get the object passed to this Directive at runtime.
            // The appropriate KeywordInUse will also be set at runtime.
            var objectPassed = GetPassableData();
            // It is wise to check if it is null
            if (objectPassed != null)
            {
                // Then check for each keyword from your RecieveKeywords
                if (KeywordInUse == "ObjectToFilter")
                {
                    // Correctly cast the passed object
                    var obj = objectPassed as object;
                    result = obj.ToString();
                }
            }
            else
                // The SimplifyXRDebug messages can be displayed at runtime and in the editor.
                // This is SimplifyXRLog pattern to follow which includes the SimplifyXRDebug message type, the message, and the format arguments for the message.
                SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.AuthorError, "No object to filter on {0}", SimplifyXRDebug.Args(this));
        }

        void SendData(string result)
        {
            // When sending data from this Directive, use the AddPassableData method.
            // You will send a List<string> from your SendKeywords to be used as labels in the Node Editor,
            // and a corresponding List<object>, which is the data you desire to pass along.
            AddPassableData(new List<string> { "FilteredString" }, new List<object> { result });
        }
    }
}