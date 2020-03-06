
#if USING_ANALYTICS
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace SimplifyXR
{
    /// <summary>
    /// Logs an analytic message to unity's analytic system
    /// </summary>
	[DirectiveCategory(DirectiveCategories.Action, DirectiveSubCategory.Analytics)]
    public class LogAnalyticData : Actions
    {
        #region Fields
        /// <summary>
        /// The name of the analytic event to log
        /// </summary>
#if UNITY_EDITOR
        [Tooltip("The name of the analytic event to log.")]
#endif
        public string nameOfAnalyticEvent;
        //This will be the data sent to the analytic system
        Dictionary<string, object> passedData;
        #endregion

        #region Overrides
        public override List<KnobKeywords> ReceiveKeywords()
        {
            return new List<KnobKeywords> { (new KnobKeywords("DictionaryOfData", typeof(Dictionary<string, object>))) };
        }

        public override List<KnobKeywords> SendKeywords()
        {
            return new List<KnobKeywords>();
        }

        public override void Execute()
        {
            GetData();
            if (string.IsNullOrEmpty(nameOfAnalyticEvent))
            {
                Debug.LogErrorFormat("The name of the analytic event on sequence {0} is null. Enter an event name to use this action.", gameObject.name);
                return;
            }
            else if (passedData == null || passedData.Count == 0)
            {
                Debug.LogErrorFormat("The data passed to this analytic event on sequence {0} is null. Ensure data is being passed to the action to proceed.", gameObject.name);
                return;
            }
            SendAnalyticData();
            ThisActionCompleted();
        }
        #endregion

        #region Private Methods
        void GetData()
        {
            var objectPassed = GetPassableData();
            if (objectPassed == null) return;

            if (KeywordInUse == "DictionaryOfData")
            {
                passedData = objectPassed as Dictionary<string, object>;
            }
        }

        void SendAnalyticData()
        {
            AnalyticsEvent.Custom(nameOfAnalyticEvent, passedData);
            Debug.LogFormat("The analytic data was logged for the '{0}' event.", nameOfAnalyticEvent);
        }
        #endregion
    }
}
#endif