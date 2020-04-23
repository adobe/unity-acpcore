using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.adobe.marketing.mobile;
using AOT;

public class SceneScript : MonoBehaviour
{
    // Core Buttons
    public Button btnCoreExtensionVersion;
    public Button btnSetApplication;
    public Button btnGetApplication;
    public Button btnSetLogLevel;
    public Button btnGetLogLevel;
    public Button btnDispatchEvent;
    public Button btnDispatchEventWithResponseCallback;
    public Button btnDispatchResponseEvent;
    public Button btnSetPrivacyStatus;
    public Button btnSetAdvertisingIdentifier;
    public Button btnGetSdkIdentities;
    public Button btnGetPrivacyStatus;
    public Button btnDownloadRules;
    public Button btnUpdateConfiguration;
    public Button btnTrackState;
    public Button btnTrackAction;


    // Identity Buttons
    public Button btnIdentityExtensionVersion;

    // Core callbacks
	[MonoPInvokeCallback(typeof(AdobeExtensionErrorCallback))]
	public static void HandleAdobeExtensionErrorCallback(ExtensionError error)
	{
		print("Error is : " + error.errorName);
	}

	[MonoPInvokeCallback(typeof(AdobeEventCallback))]
	public static void HandleAdobeEventCallback(ACPExtensionEvent eventObj)
	{
		print("Event is : " + eventObj.eventName);
	}

    [MonoPInvokeCallback(typeof(AdobePrivacyStatusCallback))]
	public static void HandleAdobePrivacyStatusCallback(ACPCore.ACPMobilePrivacyStatus status)
	{
		print("Privacy status is : " + status.ToString());
	}

    [MonoPInvokeCallback(typeof(AdobeCallback))]
	public static void HandleGetIdentitiesAdobeCallback(object ids)
	{
        if (ids is string) {
            print("Ids are : " + ids);
        }
	}

    [MonoPInvokeCallback(typeof(AdobeCallback))]
	public static void HandleStartAdobeCallback(object ids)
	{
        ACPCore.ConfigureWithAppID("launch-ENf8ed5382efc84d5b81a9be8dcc231be1-development");
	}

    // Start is called before the first frame update
    void Start()
    {
        ACPCore.SetApplication();
        ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
        ACPCore.Start(HandleStartAdobeCallback);

        // Core
        btnCoreExtensionVersion.onClick.AddListener(coreExtensionVersion);
        btnSetApplication.onClick.AddListener(setApplication);
        btnGetApplication.onClick.AddListener(getApplication);
        btnSetLogLevel.onClick.AddListener(setLogLevel);
        btnGetLogLevel.onClick.AddListener(getLogLevel);
        btnDispatchEvent.onClick.AddListener(dispatchEvent);
        btnDispatchEventWithResponseCallback.onClick.AddListener(dispatchEventWithResponseCallback);
        btnDispatchResponseEvent.onClick.AddListener(dispatchResponseEvent);
        btnSetPrivacyStatus.onClick.AddListener(setPrivacyStatus);
        btnSetAdvertisingIdentifier.onClick.AddListener(setAdvertisingIdentifier);
        btnGetSdkIdentities.onClick.AddListener(getSdkIdentities);
        btnGetPrivacyStatus.onClick.AddListener(getPrivacyStatus);
        btnDownloadRules.onClick.AddListener(downloadRules);
        btnUpdateConfiguration.onClick.AddListener(updateConfiguration);
        btnTrackState.onClick.AddListener(trackState);
        btnTrackAction.onClick.AddListener(trackAction);

        // Identity
        btnIdentityExtensionVersion.onClick.AddListener(identityExtensionVersion);
    }

    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            ACPCore.LifecyclePause();
        } else {
            var cdata = new Dictionary<string, string> ();
		    cdata.Add ("launch.data", "added");
            ACPCore.LifecycleStart(cdata);
        }
    }

    // Core Methods
    void coreExtensionVersion()
	{
		string version = ACPCore.ExtensionVersion();
        print (version);
	}

    void setApplication()
	{
        print ("Setting application");
		ACPCore.SetApplication();
	}

    void getApplication()
	{
        print ("Getting application");
		AndroidJavaObject androidApplication = ACPCore.GetApplication();
        print("Application : "+ androidApplication);
	}

    void setLogLevel()
	{
        print ("Setting Log Level");
		ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
	}

    void getLogLevel()
	{
        print ("Getting Log Level");
		ACPCore.ACPMobileLogLevel logLevel = ACPCore.GetLogLevel();
        print("Log level : "+ logLevel);
	}

    void dispatchEvent() {
        print ("Calling Dispatching event");
        var dict = new Dictionary<string, object> ();
		dict.Add ("eventDataKey", "eventDataValue");
        ACPCore.DispatchEvent(new ACPExtensionEvent("eventname", "eventType", "eventSource", dict), HandleAdobeExtensionErrorCallback);
    }

    void dispatchEventWithResponseCallback() {
        print ("Calling dispatchEventWithResponseCallback");
        var dict = new Dictionary<string, object> ();
		dict.Add ("eventDataKey", "eventDataValue");
        ACPCore.DispatchEventWithResponseCallback(new ACPExtensionEvent("eventname", "eventType", "eventSource", dict), HandleAdobeEventCallback, HandleAdobeExtensionErrorCallback);
    }

    void dispatchResponseEvent() {
        print ("Calling dispatchResponseEvent");
        var dictResponse = new Dictionary<string, object> ();
		dictResponse.Add ("eventDataKeyRes", "eventDataValueRes");
        var dictRequest = new Dictionary<string, object> ();
		dictRequest.Add ("eventDataKeyReq", "eventDataValueReq");
        ACPCore.DispatchResponseEvent(new ACPExtensionEvent("responseEventName", "eventType", "eventSource", dictResponse), new ACPExtensionEvent("requestEventName", "eventType", "eventSource", dictRequest), HandleAdobeExtensionErrorCallback);
    }

    void setPrivacyStatus() {
        print ("Calling setPrivacyStatus");
        ACPCore.SetPrivacyStatus(ACPCore.ACPMobilePrivacyStatus.OPT_IN);
    }

    void setAdvertisingIdentifier() {
        print ("Calling setAdvertisingIdentifier");
        ACPCore.SetAdvertisingIdentifier("AdId");
    }

    void getSdkIdentities() {
        print ("Calling getSdkIdentities");
        ACPCore.GetSdkIdentities(HandleGetIdentitiesAdobeCallback);
    }

    void getPrivacyStatus() {
        print ("Calling getPrivacyStatus");
        ACPCore.GetPrivacyStatus(HandleAdobePrivacyStatusCallback);
    }

    void downloadRules() {
        print ("Calling downloadRules");
        ACPCore.DownloadRules();
    }

    void updateConfiguration() {
        print ("Calling updateConfiguration");
        var dict = new Dictionary<string, object> ();
		dict.Add ("config", "updatedvalue");
        ACPCore.UpdateConfiguration(dict);
    }

    void trackState() {
        print ("Calling trackState");
        var dict = new Dictionary<string, string> ();
		dict.Add ("key", "trackState");
        ACPCore.TrackState("trackState", dict);
    }

    void trackAction() {
        print ("Calling trackAction");
        var dict = new Dictionary<string, string> ();
		dict.Add ("key", "trackAction");
        ACPCore.TrackAction("trackAction", dict);
    }

    // Identity Methods
    void identityExtensionVersion()
	{
		string version = ACPIdentity.IdentityExtensionVersion();
        print (version);
	}
}