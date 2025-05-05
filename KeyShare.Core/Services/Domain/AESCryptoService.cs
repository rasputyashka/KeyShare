using System.Security.Cryptography;
using System.Text;
using KeyShare.Core.Contracts.Services.Domain;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Services.Domain;

public class AESCryptoService : ICryptoService
{
    public CipherText EncryptText(string content, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(content);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return new CipherText
        {
            Content = cipherBytes,
                IV = aes.IV
        };
    }

    public PlainText DecryptText(CipherText cipherText, byte[] key)
    {
        var iv = cipherText.IV;
        var encrypted = cipherText.Content;

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(encrypted);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);

        var plaintext = reader.ReadToEnd();

        return new PlainText { Content = plaintext };
    }
}