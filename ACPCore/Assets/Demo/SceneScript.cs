/*
Copyright 2020 Adobe
All Rights Reserved.

NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.adobe.marketing.mobile;
using AOT;

public class SceneScript : MonoBehaviour
{
    public Text txtResult;
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
    public Button btnAppendToUrl;
    public Button btnGetIdentifiers;
    public Button btnGetExperienceCloudId;
    public Button btnSyncIdentifier;
    public Button btnSyncIdentifiers;
    public Button btnSyncIdentifiersWithAuthState;
    public Button btnUrlVariables;

    // Core callbacks
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

    [MonoPInvokeCallback(typeof(AdobePrivacyStatusCallback))]
    public static void HandleAdobePrivacyStatusCallback(int status)
    {
        print("Privacy status is : " + ((ACPCore.ACPMobilePrivacyStatus)status).ToString());
    }

    [MonoPInvokeCallback(typeof(AdobeIdentitiesCallback))]
    public static void HandleGetIdentitiesAdobeCallback(string ids)
    {
        if (ids is string)
        {
            print("Ids are : " + ids);
        }
    }

    [MonoPInvokeCallback(typeof(AdobeStartCallback))]
    public static void HandleStartAdobeCallback()
    {   
        ACPCore.ConfigureWithAppID("launch-ENc28aaf2fb6934cff830c8d3ddc5465b1-development"); 
    }

    // Identity Callbacks
    [MonoPInvokeCallback(typeof(AdobeIdentityAppendToUrlCallback))]
    public static void HandleAdobeIdentityAppendToUrlCallback(string url)
    {
        print("Url is : " + url);
    }

    [MonoPInvokeCallback(typeof(AdobeGetIdentifiersCallback))]
    public static void HandleAdobeGetIdentifiersCallback(string visitorIds)
    {
        print("Ids is : " + visitorIds);
    }

    [MonoPInvokeCallback(typeof(AdobeGetExperienceCloudIdCallback))]
    public static void HandleAdobeGetExperienceCloudIdCallback(string cloudId)
    {
        print("ECID is : " + cloudId);
    }

    [MonoPInvokeCallback(typeof(AdobeGetUrlVariables))]
    public static void HandleAdobeGetUrlVariables(string urlVariables)
    {
        print("Url variables are : " + urlVariables);
    }

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
        btnAppendToUrl.onClick.AddListener(appendToUrl);
        btnGetIdentifiers.onClick.AddListener(getIdentifiers);
        btnGetExperienceCloudId.onClick.AddListener(getExperienceCloudId);
        btnSyncIdentifier.onClick.AddListener(syncIdentifier);
        btnSyncIdentifiers.onClick.AddListener(syncIdentifiers);
        btnSyncIdentifiersWithAuthState.onClick.AddListener(syncIdentifiersWithAuthState);
        btnUrlVariables.onClick.AddListener(urlVariables);
    }

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

    // Core Methods
    void coreExtensionVersion()
    {
        string coreVersion = "coreVersion - " + ACPCore.ExtensionVersion();
        string identityVersion = "identityVersion - " + ACPIdentity.ExtensionVersion();
        string lifecycleVersion = "lifecycleVersion - " + ACPLifecycle.ExtensionVersion();
        string signalVersion = "signalVersion - " + ACPSignal.ExtensionVersion();
        print(coreVersion);
        print(identityVersion);
        print(lifecycleVersion);
        print(signalVersion);

        displayResult(coreVersion + identityVersion + lifecycleVersion + signalVersion);
    }

    void setApplication()
    {
        print("Setting application");
        ACPCore.SetApplication();
    }

    void getApplication()
    {
        print("Getting application");
        AndroidJavaObject androidApplication = ACPCore.GetApplication();
        print("Application : " + androidApplication);
    }

    void setLogLevel()
    {
        print("Setting Log Level");
        ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.DEBUG);
    }

    void getLogLevel()
    {
        print("Getting Log Level");
        ACPCore.ACPMobileLogLevel logLevel = ACPCore.GetLogLevel();
        print("Log level : " + logLevel);
    }

    void dispatchEvent()
    {
        print("Calling Dispatching event");
        var dict = new Dictionary<string, object>();
        dict.Add("eventDataKey", "eventDataValue");
        ACPCore.DispatchEvent(new ACPExtensionEvent("eventname", "eventType", "eventSource", dict), HandleAdobeExtensionErrorCallback);
    }

    void dispatchEventWithResponseCallback()
    {
        print("Calling dispatchEventWithResponseCallback");
        var dict = new Dictionary<string, object>();
        dict.Add("eventDataKey", "eventDataValue");
        ACPCore.DispatchEventWithResponseCallback(new ACPExtensionEvent("eventname", "eventType", "eventSource", dict), HandleAdobeEventCallback, HandleAdobeExtensionErrorCallback);
    }

    void dispatchResponseEvent()
    {
        print("Calling dispatchResponseEvent");
        var dictResponse = new Dictionary<string, object>();
        dictResponse.Add("eventDataKeyRes", "eventDataValueRes");
        var dictRequest = new Dictionary<string, object>();
        dictRequest.Add("eventDataKeyReq", "eventDataValueReq");
        ACPCore.DispatchResponseEvent(new ACPExtensionEvent("responseEventName", "eventType", "eventSource", dictResponse), new ACPExtensionEvent("requestEventName", "eventType", "eventSource", dictRequest), HandleAdobeExtensionErrorCallback);
    }

    void setPrivacyStatus()
    {
        print("Calling setPrivacyStatus");
        ACPCore.SetPrivacyStatus(ACPCore.ACPMobilePrivacyStatus.OPT_IN);
    }

    void setAdvertisingIdentifier()
    {
        print("Calling setAdvertisingIdentifier");
        ACPCore.SetAdvertisingIdentifier("AdId");
    }

    void getSdkIdentities()
    {
        print("Calling getSdkIdentities");
        ACPCore.GetSdkIdentities(HandleGetIdentitiesAdobeCallback);
    }

    void getPrivacyStatus()
    {
        print("Calling getPrivacyStatus");
        ACPCore.GetPrivacyStatus(HandleAdobePrivacyStatusCallback);
    }

    void downloadRules()
    {
        print("Calling downloadRules");
        ACPCore.DownloadRules();
    }

    void updateConfiguration()
    {
        print("Calling updateConfiguration");
        var dict = new Dictionary<string, object>();
        dict.Add("config", "updatedvalue");
        ACPCore.UpdateConfiguration(dict);
    }

    void trackState()
    {
        print("Calling trackState");
        var dict = new Dictionary<string, string>();
        dict.Add("key", "trackState");
        ACPCore.TrackState("trackState", dict);
    }

    void trackAction()
    {
        print("Calling trackAction");
        var dict = new Dictionary<string, string>();
        dict.Add("key", "trackAction");
        ACPCore.TrackAction("trackAction", dict);
    }

    // Identity Methods
    void identityExtensionVersion()
    {
        string version = ACPIdentity.ExtensionVersion();
        print(version);
    }

    void appendToUrl() {
        ACPIdentity.AppendToUrl("visitorId", HandleAdobeIdentityAppendToUrlCallback);
    }

    void getIdentifiers() {
        ACPIdentity.GetIdentifiers(HandleAdobeIdentityAppendToUrlCallback);
    }

    void getExperienceCloudId() {
        ACPIdentity.GetExperienceCloudId(HandleAdobeGetExperienceCloudIdCallback);
    }

    void syncIdentifier() {
        ACPIdentity.SyncIdentifier("idType1", "idValue1", ACPIdentity.ACPAuthenticationState.AUTHENTICATED);
    }

    void syncIdentifiers() {
        Dictionary<string, string> ids = new Dictionary<string, string>();
        ids.Add("idsType1", "idValue1");
        ids.Add("idsType2", "idValue2");
        ids.Add("idsType3", "idValue3");
        ACPIdentity.SyncIdentifiers(ids);
    }

    void syncIdentifiersWithAuthState() {
        Dictionary<string, string> ids = new Dictionary<string, string>();
        ids.Add("idsType1", "idValue1");
        ids.Add("idsType2", "idValue2");
        ids.Add("idsType3", "idValue3");
        ACPIdentity.SyncIdentifiers(ids, ACPIdentity.ACPAuthenticationState.AUTHENTICATED);
    }

    void urlVariables() {
        ACPIdentity.GetUrlVariables(HandleAdobeGetUrlVariables);
    }

    void displayResult(string result) {
        txtResult.text = result;
    }
}