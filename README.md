# Adobe Experience Platform - Core plugin for Cordova apps

[![CI](https://github.com/adobe/cordova-acpcore/workflows/CI/badge.svg)](https://github.com/adobe/cordova-acpcore/actions)
[![npm](https://img.shields.io/npm/v/@adobe/cordova-acpcore)](https://www.npmjs.com/package/@adobe/cordova-acpcore)
[![GitHub](https://img.shields.io/github/license/adobe/cordova-acpcore)](https://github.com/adobe/cordova-acpcore/blob/master/LICENSE)

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
- import ACPCore.unitypackage via Assets-Import Package.
- 
## Usage

### [Core](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core)

#### Initialization
##### Initialize by registering the extensions and calling the start function fo core
```
using com.adobe.marketing.mobile;
using using AOT;

public class MainScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        if (Application.platform == RuntimePlatform.Android) {
            ACPCore.SetApplication();
        }
        
        ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
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
var dict = new Dictionary<string, string>();
dict.Add("key", "value");
ACPCore.TrackAction("action", dict);
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
[MonoPInvokeCallback(typeof(AdobeIdentityAppendToUrlCallback))]
public static void HandleAdobeIdentityAppendToUrlCallback(string url)
{
    print("Url is : " + url);
}
ACPIdentity.GetIdentifiers(HandleAdobeIdentityAppendToUrlCallback);
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
> Note: We recommend implementing Lifecycle in native [Android and iOS code](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/mobile-core/lifecycle).

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


## Sample App

## Contributing
Looking to contribute to this project? Please review our [Contributing guidelines](.github/CONTRIBUTING.md) prior to opening a pull request.

We look forward to working with you!

## Licensing
This project is licensed under the Apache V2 License. See [LICENSE](LICENSE) for more information.