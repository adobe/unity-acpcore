/*
ACPCoreWrapper.h

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

#ifndef Unity_iPhone_ACPCoreWrapper_h
#define Unity_iPhone_ACPCoreWrapper_h

extern "C" {
    const char *acp_ExtensionVersion();
    void acp_SetWrapperType();
    void acp_SetLogLevel(int logLevel);
    int acp_GetLogLevel();
    void acp_Start(void (*callback)());
    void acp_ConfigureWithAppID(const char *appId);
    void acp_DispatchEvent(const char *eventName, const char *eventType, const char *eventSource, const char *cData, void (*errorCallback)(const char *errorName, int errorCode));
    void acp_DispatchEventWithResponseCallback(const char *eventName, const char *eventType, const char *eventSource, const char *cData, void (*responseCallback)(const char *resEventName, const char *resEventType, const char *resEventSource, const char *resEventData), void (*errorCallback)(const char *errorName, int errorCode));
    void acp_DispatchResponseEvent(const char *responseEventName, const char *responseEventType, const char *responseEventSource, const char *responseCData, const char *requestEventName, const char *requestEventType, const char *requestEventSource, const char *requestCData, void (*errorCallback)(const char *errorName, int errorCode));
    void acp_SetPrivacyStatus(int privacyStatus);
    void acp_SetAdvertisingIdentifier(const char *adId);
    void acp_GetSdkIdentities(void (*callback)(const char *ids));
    void acp_GetPrivacyStatus(void (*callback)(int status));
    void acp_DownloadRules();
    void acp_UpdateConfiguration(const char *cdataString);
    void acp_TrackState(const char *name, const char *cdataString);
    void acp_TrackAction(const char *name, const char *cdataString);
    void acp_LifecycleStart(const char *cdataString);
    void acp_LifecyclePause();
}

#endif