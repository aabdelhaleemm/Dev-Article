namespace Application.Common.Interfaces
{
    public interface IJwtManager
    {
        string GenerateToken(int id ,string name, string role);
    }
}