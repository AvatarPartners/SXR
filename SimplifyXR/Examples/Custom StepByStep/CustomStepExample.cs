using System;
using System.Collections.Generic;
using SimplifyXR;

//How to create a new step content type
// 1) Create a Step derived class with the custom fields. 
// 2) Override the MyTranslationType() and MyContentHolderType() methods from Step.
// 3) Create a StepTranslation derived class which uses your new Step type (see CustomStepExampleTranslation.cs)
// 4) Create a StepContentSceneHolder derived class which uses your new Step type (see CustomStepExampleContentHolder.cs)
// 5) Create an Interface derived from the ILoadStepContent which has properties for the new fields you have created (see CustomStepExampleLoader.cs)
// 6) Create a class which implements the interface created in step 6 (see CustomStepExampleLoader.cs)
// 7) Create an Action similar to the LoadCustomStepExampleContent class to distributed the content to UI elements.

[Serializable]
//This example class shows how to create a custom step content format
public class CustomStepExample : SimplifyXR.Step
{
    //Add the custom fields for the Step here
    public string stepNumber;
    public string stepInstructions;

    //Make sure to override both of these methods. You must create both a custom Translation class
    //and a custom ContentHolder class.
		public override Type MyTranslationType(){return typeof(CustomStepExampleTranslation);}
		public override Type MyContentHolderType(){return typeof(CustomStepExampleContentHolder);}
}
