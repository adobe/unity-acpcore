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
        public static string extensionVersion = "";
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CoreExtensionVersion()
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
            extensionVersionButton.onClick.AddListener(extensionVersionButtonClicked);
            extensionVersionButton.onClick.Invoke();
            yield return new WaitForSeconds(0.1f);
            if (Application.platform == RuntimePlatform.Android) {
                Assert.AreEqual("coreVersion - 1.5.2", extensionVersion);
            }            
        }

        private void extensionVersionButtonClicked()
        {
            var callbackResultsGameObject = GameObject.Find("CallbackResult");
            var callbackResults = callbackResultsGameObject.GetComponent<Text>();
            extensionVersion = callbackResults.text;
        }
    }
}
