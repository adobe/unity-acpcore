using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.adobe.marketing.mobile
{
	public delegate void AdobeExtensionErrorCallback(ExtensionError error);
	public delegate void AdobeEventCallback(ACPExtensionEvent eventObj);
	public delegate void AdobePrivacyStatusCallback(ACPCore.ACPMobilePrivacyStatus privacyStatus);
	public delegate void AdobeCallback(object value);

	#if UNITY_ANDROID
	class ExtensionErrorCallback: AndroidJavaProxy
	{
		AdobeExtensionErrorCallback redirectedDelegate;
		public ExtensionErrorCallback (AdobeExtensionErrorCallback callback): base("com.adobe.marketing.mobile.ExtensionErrorCallback"){
			redirectedDelegate = callback;
		}

		void error(AndroidJavaObject error)
		{
			string errorName = error.Call<string>("getErrorName");
			int errorCode = error.Call<int>("getErrorCode");

			redirectedDelegate (new ExtensionError(errorName, errorCode));
		}
	}

	class EventCallback: AndroidJavaProxy
	{
		AdobeEventCallback redirectedDelegate;
		public EventCallback (AdobeEventCallback callback): base("com.adobe.marketing.mobile.AdobeCallback") {
			redirectedDelegate = callback;
		}

		void call(AndroidJavaObject eventObj)
		{
			string eventName = eventObj.Call<string>("getName");
			string eventType = eventObj.Call<string>("getType");
			string eventSource = eventObj.Call<string>("getSource");
			AndroidJavaObject eventDataHashmap = eventObj.Call<AndroidJavaObject>("getEventData");
			Dictionary<string, object> eventData = ACPCore.GetDictionaryFromHashMap(eventDataHashmap);
			redirectedDelegate (new ACPExtensionEvent(eventName, eventType, eventSource, eventData));
		}
	}

	class PrivacyStatusCallback: AndroidJavaProxy
	{
		AdobePrivacyStatusCallback redirectedDelegate;
		public PrivacyStatusCallback (AdobePrivacyStatusCallback callback): base("com.adobe.marketing.mobile.AdobeCallback") {
			redirectedDelegate = callback;
		}

		void call(AndroidJavaObject status)
		{
			ACPCore.ACPMobilePrivacyStatus privacyStatusObject = ACPCore.ACPMobilePrivacyStatusFromInt(status.Call<int>("ordinal"));
			redirectedDelegate (privacyStatusObject);
		}
	}

	class Callback: AndroidJavaProxy
	{
		AdobeCallback redirectedDelegate;
		public Callback (AdobeCallback callback): base("com.adobe.marketing.mobile.AdobeCallback") {
			redirectedDelegate = callback;
		}

		void call(object value)
		{
			redirectedDelegate (value);
		}
	}
	#endif

	public class ExtensionError {
		public string errorName;
		public int errorCode;
		public ExtensionError(string errorParam, int errorCodeParam) {
			errorName = errorParam;
			errorCode = errorCodeParam;
		}
	}
	
	public class ACPExtensionEvent
	{
		public string eventName;
		public string eventType;
		public string eventSource;
		public Dictionary<string, object> eventData;

		public ACPExtensionEvent(string acp_eventName, string acp_eventType, string acp_eventSource, Dictionary<string, object> acp_eventData) {
			eventName = acp_eventName;
			eventType = acp_eventType;
			eventSource = acp_eventSource;
			eventData = acp_eventData;
		}
	}

    public class ACPCore 
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static string CONST_JAVA_CLASS_UNITY_PLAYER = "com.unity3d.player.UnityPlayer";
		#endif

		public enum ACPMobilePrivacyStatus {
			OPT_IN = 0,
			OPT_OUT = 1,
			UNKNOWN = 2
		};

		public enum ACPMobileLogLevel {
			ERROR = 0,
			WARNING = 1,
			DEBUG = 2,
			VERBOSE = 3,
			UNKOWN = -1
		};

        #if UNITY_IPHONE 
		/* ===================================================================
		 * extern declarations for iOS Methods
		 * =================================================================== */
		[DllImport ("__Internal")]
		private static extern System.IntPtr acp_Core_ExtensionVersion();

        #endif
        
        #if UNITY_ANDROID && !UNITY_EDITOR
		/* ===================================================================
		* Static Helper objects for our JNI access
		* =================================================================== */
        static AndroidJavaClass mobileCore = new AndroidJavaClass("com.adobe.marketing.mobile.MobileCore");
        #endif

        /*---------------------------------------------------------------------
		* Core Methods
		*----------------------------------------------------------------------*/
		public static string ExtensionVersion() 
		{
			#if UNITY_IPHONE && !UNITY_EDITOR		
			return Marshal.PtrToStringAnsi(acp_Core_ExtensionVersion());		
			#elif UNITY_ANDROID && !UNITY_EDITOR 
			return mobileCore.CallStatic<string> ("extensionVersion");
			#else
			return "";
			#endif
		}

		public static void SetApplication() 
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (var actClass = new AndroidJavaClass(CONST_JAVA_CLASS_UNITY_PLAYER))
			{
				// get activity
				var activity = actClass.GetStatic<AndroidJavaObject>("currentActivity");
				// get application
				var application = activity.Call<AndroidJavaObject>("getApplication");
				mobileCore.CallStatic("setApplication", application);
			}
			#endif
		}

		public static AndroidJavaObject GetApplication() 
		{
			#if UNITY_ANDROID && !UNITY_EDITOR			
			return mobileCore.CallStatic<AndroidJavaObject> ("getApplication");
			#endif

			return null;
		}

		public static void SetLogLevel(ACPMobileLogLevel logLevel) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (var logLevelVar = new AndroidJavaClass("com.adobe.marketing.mobile.LoggingMode")) 
			{
				var logModelObject = logLevelVar.GetStatic<AndroidJavaObject>(logLevel.ToString());
				mobileCore.CallStatic("setLogLevel", logModelObject);
			}
			#endif
		}

		public static ACPMobileLogLevel GetLogLevel() {
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaObject logLevel = mobileCore.CallStatic<AndroidJavaObject>("getLogLevel")) 
			{
				int level = logLevel.Call<int>("ordinal");
				return ACPMobileLogLevelFromInt(level);
			}
			#endif

			return ACPMobileLogLevel.UNKOWN;
		}

		public static void Start(AdobeCallback callback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("start", new Callback(callback));
			#endif
		}

		public static void ConfigureWithAppID(string appId) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("configureWithAppID", appId);
			#endif
		}

		public static void DispatchEvent(ACPExtensionEvent acpExtensionEvent, AdobeExtensionErrorCallback errorCallback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject eventObj = GetAdobeEventFromACPExtensionEvent(acpExtensionEvent);
			mobileCore.CallStatic<Boolean>("dispatchEvent", eventObj, new ExtensionErrorCallback(errorCallback));
			#endif
		}

		public static void DispatchEventWithResponseCallback(ACPExtensionEvent acpExtensionEvent, AdobeEventCallback responseCallback, AdobeExtensionErrorCallback errorCallback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject eventObj = GetAdobeEventFromACPExtensionEvent(acpExtensionEvent);
			mobileCore.CallStatic<Boolean>("dispatchEventWithResponseCallback", eventObj, new EventCallback(responseCallback), new ExtensionErrorCallback(errorCallback));
			#endif
		}

		public static void DispatchResponseEvent(ACPExtensionEvent responseEvent, ACPExtensionEvent requestEvent, AdobeExtensionErrorCallback errorCallback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject responseEventObject = GetAdobeEventFromACPExtensionEvent(responseEvent);
			AndroidJavaObject requestEventObject = GetAdobeEventFromACPExtensionEvent(requestEvent);
			mobileCore.CallStatic<Boolean>("dispatchResponseEvent", responseEventObject, requestEventObject, new ExtensionErrorCallback(errorCallback));
			#endif
		}

		public static void SetPrivacyStatus(ACPMobilePrivacyStatus privacyStatus) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (var privacyClass = new AndroidJavaClass("com.adobe.marketing.mobile.MobilePrivacyStatus"))
			{
				var privacyStatusObject = privacyClass.GetStatic<AndroidJavaObject>(privacyStatus.ToString());
				mobileCore.CallStatic("setPrivacyStatus", privacyStatusObject);
			}
			#endif
		}

		public static void SetAdvertisingIdentifier(string adId) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("setAdvertisingIdentifier", adId);
			#endif
		}

		public static void GetSdkIdentities(AdobeCallback callback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("getSdkIdentities", new Callback(callback));
			#endif
		}

		public static void GetPrivacyStatus(AdobePrivacyStatusCallback callback) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("getPrivacyStatus", new PrivacyStatusCallback(callback));
			#endif
		}

		public static void DownloadRules() {
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (var logLevelVar = new AndroidJavaClass("com.adobe.marketing.mobile.LoggingMode")) 
			{
				AndroidJavaObject logDebug = logLevelVar.GetStatic<AndroidJavaObject>("DEBUG");
				mobileCore.CallStatic("log", logDebug, "ACPCore", "DownloadRules() cannot be invoked on Android");
			}
			#endif
		}

		public static void UpdateConfiguration(Dictionary<string, object> config) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			var map = GetHashMapFromDictionary(config);
			mobileCore.CallStatic("updateConfiguration", map);
			#endif
		}

		public static void TrackState(string state, Dictionary<string, string> contextDataDict) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject contextData = GetStringHashMapFromDictionary(contextDataDict);
			mobileCore.CallStatic("trackState", state, contextData);
			#endif
		}

		public static void TrackAction(string action, Dictionary<string, string> contextDataDict) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject contextData = GetStringHashMapFromDictionary(contextDataDict);
			mobileCore.CallStatic("trackAction", action, contextData);
			#endif
		}

		public static void LifecycleStart(Dictionary<string, string> additionalContextData) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject contextData = GetStringHashMapFromDictionary(additionalContextData);
			mobileCore.CallStatic("lifecycleStart", contextData);
			#endif
		}

		public static void LifecyclePause() {
			#if UNITY_ANDROID && !UNITY_EDITOR
			mobileCore.CallStatic("lifecyclePause");
			#endif
		}

		/* ===================================================================
		 * Helper Methods
		 * =================================================================== */	
		 private static ACPMobileLogLevel ACPMobileLogLevelFromInt(int logLevel)
		{
			switch (logLevel) 
			{
			case 0:
				return ACPMobileLogLevel.ERROR;				
			case 1:
				return ACPMobileLogLevel.WARNING;				
			case 2:
				return ACPMobileLogLevel.DEBUG;
			case 3:
				return ACPMobileLogLevel.VERBOSE;
			default:
				return ACPMobileLogLevel.UNKOWN;				
			}
		}

		internal static ACPMobilePrivacyStatus ACPMobilePrivacyStatusFromInt(int status)
		{
			switch (status) 
			{
			case 0:
				return ACPMobilePrivacyStatus.OPT_IN;				
			case 1:
				return ACPMobilePrivacyStatus.OPT_OUT;					
			default:
				return ACPMobilePrivacyStatus.UNKNOWN;			
			}
		}

		#if UNITY_ANDROID
		private static AndroidJavaObject GetHashMapFromDictionary(Dictionary<string, object> dict)
		{
			// quick out if nothing in the dict param
			if (dict == null || dict.Count <= 0) 
			{
				return null;
			}
			
			AndroidJavaObject hashMap = new AndroidJavaObject ("java.util.HashMap");
			IntPtr putMethod = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] args = new object[2];
			foreach (KeyValuePair<string, object> kvp in dict)
			{
				using (var key = new AndroidJavaObject("java.lang.String", kvp.Key))
				{
					using (var value = new AndroidJavaObject("java.lang.String", kvp.Value))
					{
						args[0] = key;
						args[1] = value;
						AndroidJNI.CallObjectMethod(hashMap.GetRawObject(), putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
					}
				}
			}
			
			return hashMap;
		}

		private static AndroidJavaObject GetStringHashMapFromDictionary(Dictionary<string, string> dict)
		{
			// quick out if nothing in the dict param
			if (dict == null || dict.Count <= 0) 
			{
				return null;
			}
			
			AndroidJavaObject hashMap = new AndroidJavaObject ("java.util.HashMap");
			IntPtr putMethod = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] args = new object[2];
			foreach (KeyValuePair<string, string> kvp in dict)
			{
				using (var key = new AndroidJavaObject("java.lang.String", kvp.Key))
				{
					using (var value = new AndroidJavaObject("java.lang.String", kvp.Value))
					{
						args[0] = key;
						args[1] = value;
						AndroidJNI.CallObjectMethod(hashMap.GetRawObject(), putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
					}
				}
			}
			
			return hashMap;
		}

		internal static Dictionary<string, object> GetDictionaryFromHashMap(AndroidJavaObject hashmap)
		{
			Dictionary<string, object> dict = new Dictionary<string,object> ();
			AndroidJavaObject entrySet = hashmap.Call<AndroidJavaObject>("entrySet");
			AndroidJavaObject[] array = entrySet.Call<AndroidJavaObject[]> ("toArray");
			foreach (AndroidJavaObject keyValuepair in array) 
			{
				string key = keyValuepair.Call<string> ("getKey");
				string value = keyValuepair.Call<string> ("getValue");
				dict.Add (key, value);
			}

			if (dict == null || dict.Count <= 0) 
			{
				return null;
			}
			return dict;
		}

		private static AndroidJavaObject GetAdobeEventFromACPExtensionEvent(ACPExtensionEvent acpExtensionEvent) {
			if (acpExtensionEvent == null) 
			{
				return null;
			}

			using (AndroidJavaObject eventBuilder = new AndroidJavaObject("com.adobe.marketing.mobile.Event$Builder", acpExtensionEvent.eventName, acpExtensionEvent.eventType, acpExtensionEvent.eventSource)) 
			{
				if (acpExtensionEvent.eventData != null) {
					using (var hashmap = GetHashMapFromDictionary(acpExtensionEvent.eventData)) {
						eventBuilder.Call<AndroidJavaObject>("setEventData", hashmap);
					}
				}
				
				AndroidJavaObject eventObj = eventBuilder.Call<AndroidJavaObject>("build");
				return eventObj;
			}
		}
		#endif
    }
}

