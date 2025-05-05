using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Contracts.Services.Domain;
public interface ICryptoService
{
    CipherText EncryptText(string plainText, byte[] key);
    PlainText DecryptText(CipherText cipherText, byte[] key);
}
