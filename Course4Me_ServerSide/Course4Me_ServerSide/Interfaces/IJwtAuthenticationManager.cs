namespace Course4Me_ServerSide.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
