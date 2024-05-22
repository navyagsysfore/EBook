namespace EBook.Services.JWTDetails
{
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class JWTClaimsDetails
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string Key { get; set; }

    }
}
