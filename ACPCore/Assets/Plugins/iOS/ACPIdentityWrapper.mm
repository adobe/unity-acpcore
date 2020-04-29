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

void acp_AppendToUrl(const char *url, void (*callback)(const char *url)) {
    NSString *stringUrl = url ? [NSString stringWithCString:url encoding:NSUTF8StringEncoding] : nil;
    if (stringUrl == nil) {
        callback([@"" cStringUsingEncoding:NSUTF8StringEncoding]);
        return;
    }
    NSURL *nsurl = [NSURL URLWithString:stringUrl];
    [ACPIdentity appendToUrl:nsurl withCallback:^(NSURL * _Nullable urlWithVisitorData) {
        callback([urlWithVisitorData.absoluteString cStringUsingEncoding:NSUTF8StringEncoding]);
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
        callback([jsonString cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

void acp_GetExperienceCloudIdCallback(void (*callback)(const char *cloudId)) {
    [ACPIdentity getExperienceCloudId:^(NSString * _Nullable experienceCloudId) {
        callback([experienceCloudId cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

void acp_SyncIdentifier(const char *identifierType, const char *identifier, int authState) {
    NSString *nsIdType = [NSString stringWithCString:identifierType encoding:NSUTF8StringEncoding];
    NSString *nsId = [NSString stringWithCString:identifier encoding:NSUTF8StringEncoding];
    ACPMobileVisitorAuthenticationState authenticationState = (ACPMobileVisitorAuthenticationState)authState;
    [ACPIdentity syncIdentifier:nsIdType identifier:nsId authentication:authenticationState];
}

void acp_SyncIdentifiers(const char *identifiers) {
    NSDictionary *dict = getDictionaryFromJsonString(identifiers);
    [ACPIdentity syncIdentifiers:dict];
}

void acp_SyncIdentifiersWithAuthState(const char *identifiers, int authState) {
    NSDictionary *nsIdentifiers = getDictionaryFromJsonString(identifiers);
    ACPMobileVisitorAuthenticationState authenticationState = ACPMobileVisitorAuthenticationState(authState);
    [ACPIdentity syncIdentifiers:nsIdentifiers authentication:authenticationState];
}

void acp_GetUrlVariables(void (*callback)(const char *urlVariables)){ 
    [ACPIdentity getUrlVariables:^(NSString * _Nullable urlVariables) {
        callback([urlVariables cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

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
