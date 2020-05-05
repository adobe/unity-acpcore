/*
ACPLifecycle.cs

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System;

namespace com.adobe.marketing.mobile
{
    public class ACPLifecycle
    {
        #if UNITY_IPHONE 
		/* ===================================================================
		 * extern declarations for iOS Methods
		 * =================================================================== */
		[DllImport ("__Internal")]
		private static extern System.IntPtr acp_Lifecycle_ExtensionVersion();
        
        [DllImport ("__Internal")]
		private static extern void acp_Lifecycle_RegisterExtension();
        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
		/* ===================================================================
		* Static Helper objects for our JNI access
		* =================================================================== */
        static AndroidJavaClass lifecycle = new AndroidJavaClass("com.adobe.marketing.mobile.Lifecycle");
        #endif

        /*---------------------------------------------------------------------
		* Methods
		*----------------------------------------------------------------------*/
        public static string ExtensionVersion() 
		{
			#if UNITY_IPHONE && !UNITY_EDITOR		
			return Marshal.PtrToStringAnsi(acp_Lifecycle_ExtensionVersion());		
			#elif UNITY_ANDROID && !UNITY_EDITOR 
			return lifecycle.CallStatic<string> ("extensionVersion");
			#else
			return "";
			#endif
		}

		public static void RegisterExtension() {
			#if UNITY_IPHONE && !UNITY_EDITOR	
            acp_Lifecycle_RegisterExtension();	
			#elif UNITY_ANDROID && !UNITY_EDITOR 
			lifecycle.CallStatic("registerExtension");
			#endif
		}
    }
}