using PasswordGenerator;

namespace LRA.Shared.Utils;

public static class StringGenerator
{
    public static string GenerateCode(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }

    public static string GeneratePassword(int length = 16)
    {
        var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: length);
        var password = pwd.Next();
        return password;
    }
}
