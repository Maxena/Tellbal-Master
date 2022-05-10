namespace Common
{
    public class SiteSettings
    {
        public string SmsApiKey { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
        public PaymentSettings PaymentSettings { get; set; }
    }

    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }

    public class PaymentSettings
    {
        public string PaymentReqUrl { get; set; }
        public string PaymentGateWayUrl { get; set; }
        public string PaymentVerificationUrl { get; set; }
        public string SandBox { get; set; }
        public string WWW { get; set; }
        public string WithExtra { get; set; }
        public string ZarinMerchantId { get; set; }
        public string CallBackUrl { get; set; }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string EncryptKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}
