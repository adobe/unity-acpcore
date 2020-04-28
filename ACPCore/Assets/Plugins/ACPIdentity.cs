using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.adobe.marketing.mobile
{
    public class ACPIdentity 
    {
        #if UNITY_IPHONE 
		/* ===================================================================
		 * extern declarations for iOS Methods
		 * =================================================================== */
		[DllImport ("__Internal")]
		private static extern System.IntPtr acp_Identity_ExtensionVersion();

        #endif
        
        #if UNITY_ANDROID && !UNITY_EDITOR
		/* ===================================================================
		* Static Helper objects for our JNI access
		* =================================================================== */
        static AndroidJavaClass identity = new AndroidJavaClass("com.adobe.marketing.mobile.Identity");
        #endif

        /*---------------------------------------------------------------------
		* Methods
		*----------------------------------------------------------------------*/
        public static string IdentityExtensionVersion() 
		{
			#if UNITY_IPHONE && !UNITY_EDITOR		
			return Marshal.PtrToStringAnsi(acp_Identity_ExtensionVersion());		
			#elif UNITY_ANDROID && !UNITY_EDITOR 
            AndroidJNI.AttachCurrentThread();
			return identity.CallStatic<string> ("extensionVersion");
			#else
			return "";
			#endif
		}
    }
}

