using System.Collections.Generic;
using System;

// Include the SimplifyXR namespace
namespace SimplifyXR
{
    /// <summary>
    /// Insert summary of purpose of this Directive and any appropriate remarks.
    /// </summary>

    // The DirectiveCategory and SubCategory attriubte determines how the Directive is displayed in the Node Editor right-click create menu
    [DirectiveCategory(DirectiveCategories.Modifier)]
    public class TemplateForModifier : Modifier
    {
        // This list gets populated in the ConvertDirectiveListToToInterfaceList() method and will contain whatever is hooked up in the Node Editor
        List<ITemplateModifier> objectsToModify = new List<ITemplateModifier>();

        // Override this method to return the Type of the Modifier interface you are using
        public override Type GetModifierType(){return typeof(ITemplateModifier);}

        // Modifiers are Actions, so the developer overrides the Execute method.
        /* The Execute method is the entry point and first thing that occurs when this Filter is told to execute. */
        public override void Execute()
        {
            // Call this method first. You will override this methos below.
            ConvertDirectiveListToToInterfaceList();

            // Call the interface modifier methods on each object in the list that will be hooked up in the Node Editor
            foreach(ITemplateModifier mod in objectsToModify)
            {
                if(mod == null)
                    continue;

                mod.DoThisInAction();
            }

            /* Must call this event when execution of this Action is complete to pass control on to whatever is next to execute. */
            ThisActionCompleted();
        }

        // ReceiveKeywords are how the developer can control what type of data is received to this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> ReceiveKeywords()
        {
            return new List<KnobKeywords>();
        }

         // SendKeywords are how the developer can control what type of data is sent from this Directive and how that is labeled in the Node Editor.
        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords>();
        }

        /* Important method to override in order to cast the Directives as objects you're using in this Modifier */
        public override void ConvertDirectiveListToToInterfaceList()
        {
            // Model this method with the code below with the exception of the line that adds the Directives to this Modifier's list
            if(isModifierCast)
                return;

            foreach (Directive dir in objectsToInterfaceWith)
                objectsToModify.Add(dir as ITemplateModifier);     // Modify this line to cast as the interface this Modifier is using

            isModifierCast = true;
        }
    }
}