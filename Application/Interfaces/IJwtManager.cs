namespace Application.Interfaces
{
    public interface IJwtManager
    {
        string GenerateToken(int id ,string name, string role);
    }
}