using System.Security.Cryptography;
using KeyShare.Core.Contracts.Services.Domain;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Services.Domain;
public class AESEncryptionKeyProvider : IEncryptionKeyProvider
{
    public Key GetKey()
    {
        using var aes = Aes.Create();
        // 128, 192 or 256
        aes.KeySize = 256;
        aes.GenerateKey();
        
        return new Key() { Value = aes.Key};
    }
}
