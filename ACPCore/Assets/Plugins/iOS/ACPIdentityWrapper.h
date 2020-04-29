#ifndef Unity_iPhone_ACPIdentityWrapper_h
#define Unity_iPhone_ACPIdentityWrapper_h

extern "C" {
    const char *acp_Identity_ExtensionVersion();
    void acp_AppendToUrl(const char *url, void (*callback)(const char *url));
    void acp_GetIdentifiers(void (*callback)(const char *ids));
    void acp_GetExperienceCloudIdCallback(void (*callback)(const char *cloudId));
    void acp_SyncIdentifier(const char *identifierType, const char *identifier, int authState);
    void acp_SyncIdentifiers(const char *identifiers);
    void acp_SyncIdentifiersWithAuthState(const char *identifiers, int authState);
    void acp_GetUrlVariables(void (*callback)(const char *urlVariables));
}

#endif