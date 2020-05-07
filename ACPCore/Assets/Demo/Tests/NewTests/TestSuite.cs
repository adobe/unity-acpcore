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
        [UnityTest]
        public IEnumerator Test_ExtensionVersion()
        {
            if (Application.platform == RuntimePlatform.Android) {
                return AssertEqualResult("CoreExtensionVersion", "coreVersion - 1.5.2 identityVersion - 1.2.0 lifecycleVersion - 1.0.4 signalVersion - 1.0.2 "); 
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                return AssertEqualResult("CoreExtensionVersion", "coreVersion - 1.5.2 identityVersion - 1.2.0 lifecycleVersion - 1.0.4 signalVersion - 1.0.2 "); 
            } else {
                return null;
            }
        }

        [UnityTest]
        public IEnumerator Test_GetPrivacyStatus()
        {
            InvokeButtonClick("SetPrivacyStatus");
            return AssertEqualResult("GetPrivacyStatus", "Privacy status is : OPT_IN");      
        }

        [UnityTest]
        public IEnumerator Test_GetLogLevel()
        {
            InvokeButtonClick("SetLogLevel");
            return AssertEqualResult("GetLogLevel", "Log level : VERBOSE");          
        }

        [UnityTest]
        public IEnumerator Test_GetSdkIdentities()
        {
            return AssertGreaterLengthResult("GetSdkIdentities", "Ids are : ".Length);
        }

        [UnityTest]
        public IEnumerator Test_AppendToUrl()
        {
            return AssertGreaterLengthResult("AppendToUrl", "Url is : ".Length);
        }

        [UnityTest]
        public IEnumerator Test_GetIdentifiers()
        {
            InvokeButtonClick("AppendToUrl");
            return AssertGreaterLengthResult("GetIdentifiers", "Ids is : ".Length);
        }

        [UnityTest]
        public IEnumerator Test_GetExperienceCloudId()
        {
            return AssertGreaterLengthResult("GetExperienceCloudId", "ECID is : ".Length);
        }

        [UnityTest]
        public IEnumerator Test_UrlVariables()
        {
            return AssertGreaterLengthResult("UrlVariables", "Url variables are : ".Length);
        }
        
        // Helper functions
        private IEnumerator LoadScene() {
            AsyncOperation async = SceneManager.LoadSceneAsync("Demo/DemoScene");

            while (!async.isDone)
            {
                yield return null;
            }
        }

        private IEnumerator UnLoadScene() {
            yield return SceneManager.UnloadSceneAsync("Demo/DemoScene");
        }

        private void InvokeButtonClick(string gameObjName) {
            var gameObj = GameObject.Find(gameObjName);
            var button = gameObj.GetComponent<Button>();
            button.onClick.Invoke();
        }

        private string GetActualResult()
        {
            var callbackResultsGameObject = GameObject.Find("Result");
            var callbackResults = callbackResultsGameObject.GetComponent<Text>();
            return callbackResults.text;
        }

        private IEnumerator AssertEqualResult(string gameObjectName, string expectedResult) {
            yield return LoadScene();
            InvokeButtonClick(gameObjectName);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(expectedResult, GetActualResult());
            UnLoadScene();
        }

        private IEnumerator AssertGreaterLengthResult(string gameObjectName, int expectedLength) {
            yield return LoadScene();
            InvokeButtonClick(gameObjectName);
            yield return new WaitForSeconds(1f);
            Assert.Greater(GetActualResult().Length, expectedLength);
            UnLoadScene();
        }
    }
}
