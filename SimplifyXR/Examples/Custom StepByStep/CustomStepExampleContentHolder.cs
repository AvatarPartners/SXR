using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SimplifyXR;

	/// <summary>
	/// An example of StepContentSceneHolder for a custom StepContent 
	/// </summary>
	public class CustomStepExampleContentHolder : StepContentSceneHolder  {
		//Change the type of this list to the new custom Step Type
		public List<CustomStepExample> MySteps = new List<CustomStepExample>();

		#region Overrides
		//Override the GetAllSteps method. The MySteps list is declared 
		//in the derived class and must be cast here.
		public override List<Step> GetAllSteps(){
			return MySteps.Cast<Step>().ToList();
		}
		//Override the AddStep method and cast the incoming step to the 
		//CustomStep Type before adding it to the MySteps list
		public override void AddStep(Step toAdd){
			MySteps.Add(toAdd as CustomStepExample);
		}
		#endregion

	}
