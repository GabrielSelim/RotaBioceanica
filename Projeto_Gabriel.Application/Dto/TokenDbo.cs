namespace Projeto_Gabriel.Application.Dto
{
    public class TokenDbo
    {
        public TokenDbo(string accessToken, string refreshToken, bool authenticated, string? created = null, string? expiration = null, string role = null)
        {
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Role = role;
        }

        public bool Authenticated { get; set; }
        public string? Created { get; set; }
        public string? Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}