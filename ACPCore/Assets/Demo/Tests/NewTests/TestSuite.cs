/*
TestSuite.cs

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

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
                return AssertEqualResult("CoreExtensionVersion", "coreVersion - 1.5.3-UN identityVersion - 1.2.0 lifecycleVersion - 1.0.4 signalVersion - 1.0.2 "); 
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                return AssertEqualResult("CoreExtensionVersion", "coreVersion - 2.9.4-U identityVersion - 2.5.1 lifecycleVersion - 2.2.1 signalVersion - 2.2.0 "); 
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
        }

        private IEnumerator AssertGreaterLengthResult(string gameObjectName, int expectedLength) {
            yield return LoadScene();
            InvokeButtonClick(gameObjectName);
            yield return new WaitForSeconds(1f);
            Assert.Greater(GetActualResult().Length, expectedLength);
        }
    }
}
