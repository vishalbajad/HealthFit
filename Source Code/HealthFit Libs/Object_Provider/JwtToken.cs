namespace HealthFit.JwtAuthentication.Model
{
    public class JwtToken
    {
        public string? Token { get; set; }
        public string? Expiration { get; set; }
    }
}
