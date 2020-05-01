/*
ACPIdentityWrapper.mm

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

#import "ACPIdentityWrapper.h"
#import "ACPIdentity.h"
#import "ACPCore.h"
#import "ACPCoreWrapper.h"

static NSString* const ACP_VISITOR_AUTH_STATE_AUTHENTICATED = @"ACP_VISITOR_AUTH_STATE_AUTHENTICATED";
static NSString* const ACP_VISITOR_AUTH_STATE_LOGGED_OUT = @"ACP_VISITOR_AUTH_STATE_LOGGED_OUT";
static NSString* const ACP_VISITOR_AUTH_STATE_UNKNOWN = @"ACP_VISITOR_AUTH_STATE_UNKNOWN";

static NSString* const VISITOR_ID_ID_ORIGIN_KEY = @"idOrigin";
static NSString* const VISITOR_ID_ID_TYPE_KEY = @"idType";
static NSString* const VISITOR_ID_ID_KEY = @"identifier";
static NSString* const VISITOR_ID_AUTH_STATE_KEY = @"authenticationState";

NSDictionary *dictionaryFromVisitorId(ACPMobileVisitorId *visitorId);
NSString *stringFromAuthState(ACPMobileVisitorAuthenticationState authState);
NSDictionary *getDictionaryFromJsonString(const char *jsonString);

const char *acp_Identity_ExtensionVersion() {
   return [[ACPIdentity extensionVersion] cStringUsingEncoding:NSUTF8StringEncoding];
}

void acp_Identity_RegisterExtension() {
    [ACPIdentity registerExtension];
}

void acp_AppendToUrl(const char *url, void (*callback)(const char *url)) {
    NSString *stringUrl = [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
    if (!stringUrl.length) {
        if (callback != nil) {
            callback([@"" cStringUsingEncoding:NSUTF8StringEncoding]);
        }
        return;
    }
    NSURL *nsurl = [NSURL URLWithString:stringUrl];
    [ACPIdentity appendToUrl:nsurl withCallback:^(NSURL * _Nullable urlWithVisitorData) {
        if (callback != nil) {
            callback([urlWithVisitorData.absoluteString cStringUsingEncoding:NSUTF8StringEncoding]);
        }
    }];
}

void acp_GetIdentifiers(void (*callback)(const char *ids)) {
    [ACPIdentity getIdentifiers:^(NSArray<ACPMobileVisitorId *> * _Nullable visitorIDs) {
        NSMutableArray *visitorIDList = [NSMutableArray array];
        for (ACPMobileVisitorId *visitorID in visitorIDs) {
            NSDictionary *visitorIDDict = dictionaryFromVisitorId(visitorID);
            [visitorIDList addObject:visitorIDDict];
        }
        
        NSError *error = nil;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:visitorIDList options:NSJSONWritingPrettyPrinted error:&error];
        NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        NSLog(@"jsonData as string:\n%@", jsonString);
        if (callback != nil) {
            callback([jsonString cStringUsingEncoding:NSUTF8StringEncoding]);
        }
    }];
}

void acp_GetExperienceCloudId(void (*callback)(const char *cloudId)) {
    [ACPIdentity getExperienceCloudId:^(NSString * _Nullable experienceCloudId) {
        callback([experienceCloudId cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

void acp_SyncIdentifier(const char *identifierType, const char *identifier, int authState) {
    NSString *nsIdType = [NSString stringWithCString:identifierType encoding:NSUTF8StringEncoding];
    NSString *nsId = [NSString stringWithCString:identifier encoding:NSUTF8StringEncoding];
    if (!nsIdType.length || !nsId.length) {
        NSLog(@"SyncIdentifier failed as type or id are null");
        return;
    }
    ACPMobileVisitorAuthenticationState authenticationState = (ACPMobileVisitorAuthenticationState)authState;
    [ACPIdentity syncIdentifier:nsIdType identifier:nsId authentication:authenticationState];
}

void acp_SyncIdentifiers(const char *identifiers) {
    NSDictionary *dict = getDictionaryFromJsonString(identifiers);
    if (dict == nil) return;
    [ACPIdentity syncIdentifiers:dict];
}

void acp_SyncIdentifiersWithAuthState(const char *identifiers, int authState) {
    NSDictionary *nsIdentifiers = getDictionaryFromJsonString(identifiers);
    if (nsIdentifiers == nil) return;
    ACPMobileVisitorAuthenticationState authenticationState = ACPMobileVisitorAuthenticationState(authState);
    [ACPIdentity syncIdentifiers:nsIdentifiers authentication:authenticationState];
}

void acp_GetUrlVariables(void (*callback)(const char *urlVariables)){ 
    [ACPIdentity getUrlVariables:^(NSString * _Nullable urlVariables) {
        callback([urlVariables cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

// Helper methods

NSDictionary *dictionaryFromVisitorId(ACPMobileVisitorId *visitorId) {
    NSMutableDictionary *visitorIdDict = [NSMutableDictionary dictionary];
    visitorIdDict[VISITOR_ID_ID_ORIGIN_KEY] = visitorId.idOrigin;
    visitorIdDict[VISITOR_ID_ID_TYPE_KEY] = visitorId.idType;
    visitorIdDict[VISITOR_ID_ID_KEY] = visitorId.identifier;
    visitorIdDict[VISITOR_ID_AUTH_STATE_KEY] = stringFromAuthState(visitorId.authenticationState);
    
    return visitorIdDict;
}

NSString *stringFromAuthState(ACPMobileVisitorAuthenticationState authState) {
    switch (authState) {
        case ACPMobileVisitorAuthenticationStateAuthenticated:
            return ACP_VISITOR_AUTH_STATE_AUTHENTICATED;
        case ACPMobileVisitorAuthenticationStateLoggedOut:
            return ACP_VISITOR_AUTH_STATE_LOGGED_OUT;
        default:
            return ACP_VISITOR_AUTH_STATE_UNKNOWN;
    }
}

NSDictionary *getDictionaryFromJsonString(const char *jsonString) {
    if (!jsonString) {
        return nil;
    }
    
    NSError *error = nil;
    NSString *tempString = [NSString stringWithCString:jsonString encoding:NSUTF8StringEncoding];
    NSData *data = [tempString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data
                                                         options:NSJSONReadingMutableContainers
                                                           error:&error];
    
    return (dict && !error) ? dict : nil;
}
