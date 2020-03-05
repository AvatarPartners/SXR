using UnityEngine;
using System.Collections.Generic;

namespace SimplifyXR
{
    [DirectiveCategory(false)]
    public class ToggleColorForTesting : Actions
    {
        public GameObject ObjectToChangeColor;
        public Color FirstColor, SecondColor;
        Material material;

        public override List<KnobKeywords> ReceiveKeywords()
        {
			return new List<KnobKeywords> { new KnobKeywords("SendTransformToToggle", typeof(Transform)), new KnobKeywords("SendObjectToToggle", typeof(GameObject)) };
        }

        public override List<KnobKeywords> SendKeywords()
        {
			return new List<KnobKeywords> { new KnobKeywords("Color", typeof(Color)) };
        }

        public override void Execute()
        {
            FindObjectTouched();
            GetAndChangeMaterial();
			SendColor();
            ThisActionCompleted();
        }

        void GetAndChangeMaterial()
        {
            if (ObjectToChangeColor != null)
            {
                if (ObjectToChangeColor.GetComponent<Renderer>() != null)
                {
                    #if UNITY_EDITOR
                    material = ObjectToChangeColor.GetComponent<Renderer>().sharedMaterial;
                    #else
                    material = ObjectToChangeColor.GetComponent<Renderer>().material;
                    #endif
                    if (material.color == FirstColor)
                        material.color = SecondColor;
                    else
                        material.color = FirstColor;
                }
                else
                    SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.AuthorError, "No Renderer on {0}", SimplifyXRDebug.Args(this));
            }
            else
                SimplifyXRDebug.SimplifyXRLog(SimplifyXRDebug.Type.AuthorError, "No object to change color on {0}", SimplifyXRDebug.Args(this));
        }

        void FindObjectTouched()
        {
            var objectPassed = GetPassableData();
            if (objectPassed == null) return;
            
            if (KeywordInUse == "SendObjectToToggle")
                ObjectToChangeColor = objectPassed as GameObject;
        }

        void SendColor()
		{
            if (material != null)
            {
			    AddPassableData(new List<string> { "Color" }, new List<object> { material.color });
            }
		}
    }
}