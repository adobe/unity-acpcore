# Adobe Experience Platform - Core plugin for unity apps

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
    - [Initialization](#initialization)
    - [Core methods](#core-methods)
    - [Lifecycle methods](#lifecycle-methods)
    - [Signals methods](#signals-methods)
- [Running Tests](#running-tests)
- [Sample App](#sample-app)
- [Contributing](#contributing)
- [Licensing](#licensing)

## Prerequisites

The `Unity Hub` application is required for development and testing. Inside of `Unity Hub`, you will be required to download the current version of the `Unity` app.

[Download the Unity Hub](http://unity3d.com/unity/download). The free version works for development and testing, but a Unity Pro license is required for distribution. See [Distribution](#distribution) below for details.

#### FOLDER STRUCTURE
Plugins for a Unity project use the following folder structure:

`{Project}/Assets/Plugins/{Platform}`

## Installation
- Download [ACPCore-0.0.1-Unity.zip](https://github.com/adobe/unity-acpcore/tree/master/bin/ACPCore-0.0.1-Unity.zip) 
- Unzip `ACPCore-0.0.1-Unity.zip`
- Import `ACPCore.unitypackage` via Assets-Import Package
- 
## Usage

### [Core](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core)

#### Initialization
##### Initialize by registering the extensions and calling the start function fo core
```
using com.adobe.marketing.mobile;
using AOT;

public class MainScript : MonoBehaviour
{
    [MonoPInvokeCallback(typeof(AdobeStartCallback))]
    public static void HandleStartAdobeCallback()
    {   
        ACPCore.ConfigureWithAppID("1423ae38-8385-8963-8693-28375403491d"); 
    }

    // Start is called before the first frame update
    void Start()
    {   
        if (Application.platform == RuntimePlatform.Android) {
            ACPCore.SetApplication();
        }
        
        ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
        ACPCore.SetWrapperType();
        ACPIdentity.registerExtension();
        ACPLifecycle.registerExtension();
        ACPSignal.registerExtension();
        ACPCore.Start(HandleStartAdobeCallback);
    }
}
```

#### Core methods

##### Getting Core version:
```cs
ACPCore.ExtensionVersion();
```

##### Updating the SDK configuration:
```cs
var dict = new Dictionary<string, object>();
dict.Add("newConfigKey", "newConfigValue");
ACPCore.UpdateConfiguration(dict);
```

##### Controlling the log level of the SDK:
```cs
ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.ERROR);
ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.WARNING);
ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.DEBUG);
ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
```

##### Getting the current privacy status:
```cs
[MonoPInvokeCallback(typeof(AdobePrivacyStatusCallback))]
public static void HandleAdobePrivacyStatusCallback(int status)
{
    print("Privacy status is : " + ((ACPCore.ACPMobilePrivacyStatus)status).ToString());
}

ACPCore.GetPrivacyStatus(HandleAdobePrivacyStatusCallback);
```

##### Setting the privacy status:
```cs
ACPCore.SetPrivacyStatus(ACPCore.ACPMobilePrivacyStatus.OPT_IN);
ACPCore.SetPrivacyStatus(ACPCore.ACPMobilePrivacyStatus.OPT_OUT);
ACPCore.SetPrivacyStatus(ACPCore.ACPMobilePrivacyStatus.UNKNOWN);
```

##### Getting the SDK identities:
```cs
[MonoPInvokeCallback(typeof(AdobeIdentitiesCallback))]
public static void HandleGetIdentitiesAdobeCallback(string ids)
{
    if (ids is string)
    {
        print("Ids are : " + ids);
    }
}

ACPCore.GetSdkIdentities(HandleGetIdentitiesAdobeCallback);
```

##### Dispatching an Event Hub event:
```cs
[MonoPInvokeCallback(typeof(AdobeExtensionErrorCallback))]
public static void HandleAdobeExtensionErrorCallback(string errorName, int errorCode)
{
    print("Error is : " + errorName);
}

var dict = new Dictionary<string, object>();
dict.Add("key", "value");
ACPCore.DispatchEvent(new ACPExtensionEvent("eventName", "eventType", "eventSource", dict), HandleAdobeExtensionErrorCallback);
```

##### Dispatching an Event Hub event with callback:
```cs
[MonoPInvokeCallback(typeof(AdobeExtensionErrorCallback))]
public static void HandleAdobeExtensionErrorCallback(string errorName, int errorCode)
{
    print("Error is : " + errorName);
}

[MonoPInvokeCallback(typeof(AdobeEventCallback))]
public static void HandleAdobeEventCallback(string eventName, string eventType, string eventSource, string jsonEventData)
{
    print("Event is : " + eventName);
}

var dict = new Dictionary<string, object>();
dict.Add("key", "value");
ACPCore.DispatchEventWithResponseCallback(new ACPExtensionEvent("eventname", "eventType", "eventSource", dict), HandleAdobeEventCallback, HandleAdobeExtensionErrorCallback);
```

##### Dispatching an Event Hub response event:
```cs
[MonoPInvokeCallback(typeof(AdobeExtensionErrorCallback))]
public static void HandleAdobeExtensionErrorCallback(string errorName, int errorCode)
{
    print("Error is : " + errorName);
}

var dictResponse = new Dictionary<string, object>();
dictResponse.Add("eventDataKeyRes", "eventDataValueRes");
var dictRequest = new Dictionary<string, object>();
dictRequest.Add("eventDataKeyReq", "eventDataValueReq");
ACPCore.DispatchResponseEvent(new ACPExtensionEvent("responseEventName", "eventType", "eventSource", dictResponse), new ACPExtensionEvent("requestEventName", "eventType", "eventSource", dictRequest), HandleAdobeExtensionErrorCallback);
```

##### Downloading the Rules
```cs
ACPCore.DownloadRules();
```

##### Setting the advertising identifier:
```cs
ACPCore.SetAdvertisingIdentifier("AdId");
```

##### Calling track action
```cs
var contextData = new Dictionary<string, string>();
contextData.Add("key", "value");
ACPCore.TrackAction("action", contextData);
```

##### Calling track state
```cs
var dict = new Dictionary<string, string>();
dict.Add("key", "value");
ACPCore.TrackState("state", dict);
```

### [Identity](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core/identity)

##### Getting Identity version:
```cs
string identityVersion = ACPIdentity.ExtensionVersion();
```

##### Sync Identifier:
```cs
ACPIdentity.SyncIdentifier("idType1", "idValue1", ACPIdentity.ACPAuthenticationState.AUTHENTICATED);
```

##### Sync Identifiers:
```cs
Dictionary<string, string> ids = new Dictionary<string, string>();
ids.Add("idsType1", "idValue1");
ids.Add("idsType2", "idValue2");
ids.Add("idsType3", "idValue3");
ACPIdentity.SyncIdentifiers(ids);
```
##### Sync Identifiers with Authentication State:
```cs
Dictionary<string, string> ids = new Dictionary<string, string>();
ids.Add("idsType1", "idValue1");
ids.Add("idsType2", "idValue2");
ids.Add("idsType3", "idValue3");
ACPIdentity.SyncIdentifiers(ids, ACPIdentity.ACPAuthenticationState.AUTHENTICATED);
ACPIdentity.SyncIdentifiers(ids, ACPIdentity.ACPAuthenticationState.LOGGED_OUT);
ACPIdentity.SyncIdentifiers(ids, ACPIdentity.ACPAuthenticationState.UNKNOWN);
```

##### Append visitor data to a URL:
```cs
[MonoPInvokeCallback(typeof(AdobeIdentityAppendToUrlCallback))]
public static void HandleAdobeIdentityAppendToUrlCallback(string url)
{
    print("Url is : " + url);
}
ACPIdentity.AppendToUrl("https://www.adobe.com", HandleAdobeIdentityAppendToUrlCallback);
```

##### Get visitor data as URL query parameter string:
```cs
[MonoPInvokeCallback(typeof(AdobeGetUrlVariables))]
public static void HandleAdobeGetUrlVariables(string urlVariables)
{
    print("Url variables are : " + urlVariables);
}
ACPIdentity.GetUrlVariables(HandleAdobeGetUrlVariables);
```

##### Get Identifiers:
```cs
[MonoPInvokeCallback(typeof(AdobeGetIdentifiersCallback))]
public static void HandleAdobeGetIdentifiersCallback(string visitorIds)
{
    print("Ids is : " + visitorIds);
    _result = "Ids is : " + visitorIds;
}
ACPIdentity.GetIdentifiers(HandleAdobeGetIdentifiersCallback);
```

##### Get Experience Cloud IDs:
```cs
[MonoPInvokeCallback(typeof(AdobeGetExperienceCloudIdCallback))]
public static void HandleAdobeGetExperienceCloudIdCallback(string cloudId)
{
    print("ECID is : " + cloudId);
}
ACPIdentity.GetExperienceCloudId(HandleAdobeGetExperienceCloudIdCallback);
```

### [Lifecycle](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core/lifecycle)
##### Starting and pausing a lifecycle event

```csharp
private void OnApplicationPause(bool pauseStatus)
{
  if (pauseStatus)
  {
    ACPCore.LifecyclePause();
  }
  else
  {
    var cdata = new Dictionary<string, string>();
    cdata.Add("launch.data", "added");
    ACPCore.LifecycleStart(cdata);
  }
}
```

##### Getting Lifecycle version:
```cs
string lifecycleVersion = ACPLifecycle.ExtensionVersion();
 ```

### [Signal](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core/signals)

##### Getting Signal version:
```cs
string signalVersion = ACPSignal.ExtensionVersion();
```

## Running Tests
1. Open the demo app in unity.
2. Open the test runner from `Window -> General -> TestRunner`.
3. Click on the `PlayMode` tab.
4. Connect an Android device. As we run the tests on a device in play mode.
5. Click `Run all in player (Android)` to run the tests.

## Sample App
Sample App is located in the *unity-acpcore/ACPCore/Assets/Demo*.
To build demo app for specific platform follow the below instructions.

###### Android
1. Make sure you have an Android device connected.
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `Android` from the __Platform__ window
1. If `Android` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
1. You will be asked to provide a location to save the build. Make a new directory at *unity-acpcore/ACPCore/Builds* (this folder is in the .gitignore file)
1. Name build whatever you want and press __Save__
1. `Unity` will build an `apk` file and automatically deploy it to the connected device

###### iOS
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `iOS` from the __Platform__ window
1. If `iOS` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
1. You will be asked to provide a location to save the build. Make a new directory at *unity-acpcore/ACPCore/Builds* (this folder is in the .gitignore file)
1. Name build whatever you want and press __Save__
1. `Unity` will create and open an `Xcode` project
1. From the Xcode project run the app on a simulator.
1. If you get an error `Symbol not found: _OBJC_CLASS_$_WKWebView`. Select the Unity-iPhone target -> Go to Build Phases tab -> Add `Webkit.Framework` to `Link Binary with Libraries`.

## Additional Cordova Plugins

Below is a list of additional Unity plugins from the AEP SDK suite:

| Extension | GitHub | Unity Package |
|-----------|--------|-----|
| Adobe Analytics | https://github.com/adobe/unity-acpanalytics | [ACPAnalytics](https://github.com/adobe/unity-acpanalytics/raw/master/bin/ACPAnalytics-0.0.1-Unity.zip)
| Project Griffon (Beta) | https://github.com/adobe/unity-acpgriffon | [ACPGriffon](https://github.com/adobe/unity-acpgriffon/raw/master/bin/ACPGriffon-0.0.1-Unity.zip)

## Contributing
Looking to contribute to this project? Please review our [Contributing guidelines](.github/CONTRIBUTING.md) prior to opening a pull request.

We look forward to working with you!

## Licensing
This project is licensed under the Apache V2 License. See [LICENSE](LICENSE) for more information.
