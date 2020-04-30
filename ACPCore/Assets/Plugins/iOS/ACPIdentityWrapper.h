/*
ACPIdentityWrapper.h

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

#ifndef Unity_iPhone_ACPIdentityWrapper_h
#define Unity_iPhone_ACPIdentityWrapper_h

extern "C" {
    const char *acp_Identity_ExtensionVersion();
    void acp_Identity_RegisterExtension();
    void acp_AppendToUrl(const char *url, void (*callback)(const char *url));
    void acp_GetIdentifiers(void (*callback)(const char *ids));
    void acp_GetExperienceCloudId(void (*callback)(const char *cloudId));
    void acp_SyncIdentifier(const char *identifierType, const char *identifier, int authState);
    void acp_SyncIdentifiers(const char *identifiers);
    void acp_SyncIdentifiersWithAuthState(const char *identifiers, int authState);
    void acp_GetUrlVariables(void (*callback)(const char *urlVariables));
}

#endif