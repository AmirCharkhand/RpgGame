using RPG.Domain.Models;

namespace RPG.Application.Services.Contracts;

public interface IHashService
{
    public void CreateHashWithSalt(string text, out byte[] textHash, out byte[] salt);
    public bool VerifyHash(string text, byte[] hashedText, byte[] salt);
    public string CreateJwt(User user);
}