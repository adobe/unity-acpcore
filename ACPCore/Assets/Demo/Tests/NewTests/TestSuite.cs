using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class TestSuite
    {
        public static string callbackResultText = "";
    
        [UnityTest]
        public IEnumerator ExtensionVersion()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame
            AsyncOperation async = SceneManager.LoadSceneAsync("Demo/DemoScene");

            while (!async.isDone)
            {
                yield return null;
            }
            var extensionVersionButtonGameObject = GameObject.Find("CoreExtensionVersion");
            var extensionVersionButton = extensionVersionButtonGameObject.GetComponent<Button>();
            extensionVersionButton.onClick.AddListener(ButtonClickedListener);
            extensionVersionButton.onClick.Invoke();
            yield return new WaitForSeconds(0.1f);
            if (Application.platform == RuntimePlatform.Android) {
                string expectedResult = "coreVersion - 1.5.2 identityVersion - 1.2.0 lifecycleVersion - 1.0.4 signalVersion - 1.0.2 ";
                Assert.AreEqual(expectedResult, callbackResultText);
            }            
        }

        [UnityTest]
        public IEnumerator GetPrivacyStatus()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame
            AsyncOperation async = SceneManager.LoadSceneAsync("Demo/DemoScene");

            while (!async.isDone)
            {
                yield return null;
            }
            var privacyStatus = GameObject.Find("GetPrivacyStatus");
            var privacyStatusButton = privacyStatus.GetComponent<Button>();
            privacyStatusButton.onClick.Invoke();
            yield return new WaitForSeconds(1f);
            var callbackResultsGameObject = GameObject.Find("Result");
            var callbackResults = callbackResultsGameObject.GetComponent<Text>();
            string actualResult = callbackResults.text;
            if (Application.platform == RuntimePlatform.Android) {
                string expectedResult = "Privacy status is : OPT_IN";
                Assert.AreEqual(expectedResult, actualResult);
            }            
        }

        // Helper function for button click
        private void ButtonClickedListener()
        {
            var callbackResultsGameObject = GameObject.Find("Result");
            var callbackResults = callbackResultsGameObject.GetComponent<Text>();
            callbackResultText = callbackResults.text;
        }
    }
}
