using SimplifyXR;
[System.Serializable]
// / <summary>
// / An example of how to create a custom StepTranslation 
// / </summary>
public class CustomStepExampleTranslation : StepTranslation
{
	//Change the type of StepInformation to the custom Step type
	public CustomStepExample StepInformation;
	//Change the cast type at the end of the line to the custom Step type
	public override void AssignStepInformation(Step originalStep){StepInformation = originalStep as CustomStepExample;}
}
