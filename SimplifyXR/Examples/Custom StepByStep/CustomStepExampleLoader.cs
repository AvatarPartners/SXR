using System.Collections;
using System.Collections.Generic;
using SimplifyXR;
using UnityEngine;

 //Create a property for each of the new fields in the CustomStepExample class
public interface ILoadCustomStepExampleContent : ILoadStepContent
{
	string StepNumber { get; }
	string StepInstructions { get; }
}

//Example loader class from a CustomStepExample Type Step
public class CustomStepExampleLoader : ILoadCustomStepExampleContent
{
	//A property from the ILoadStepContent interface
	public string StepLabel { get; private set;}
	//Properties from the new custom interface (in this case from ILoadCustomStepExampleContent)
	public string StepNumber { get; private set;}
	public string StepInstructions { get; private set;}

	// Populates the fields of the current passed step
	// Instructions: change the step type to the custom step type created for this class.
	// Fill all of the properties with the values from the step
	public bool LoadStepContent(string step = null)
	{
		CustomStepExample thisStep = null;
		if (string.IsNullOrEmpty(step))
			thisStep = SimplifyXRAccessManager.Instance.LoadTheStep<CustomStepExample>();
		else
			thisStep = SimplifyXRAccessManager.Instance.LoadTheStep<CustomStepExample>(step);
		if (thisStep != null)
		{
			StepLabel = thisStep.StepLabel;
			StepNumber = thisStep.stepNumber;
			StepInstructions = thisStep.stepInstructions;
			return true;
		}
		return false;
	}

	//Define the step type for this loader
	public bool StepTypeCheck()
	{
		return SimplifyXRAccessManager.Instance.GetStepType == typeof(CustomStepExample);
	}
}
