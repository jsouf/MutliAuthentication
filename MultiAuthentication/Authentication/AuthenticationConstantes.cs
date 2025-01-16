namespace MultiAuthentication.Authentication
{
    public static class AuthenticationConstantes
    {
        public const string SharedScheme = nameof(SharedScheme);
        public const string ApiKeyAuthenticationScheme = nameof(ApiKeyAuthenticationScheme);
        public const string MultiAuthenticationPolicy = nameof(MultiAuthenticationPolicy);
        // move this to configuration
        public const string SecretKey = "secret";
    }
}
