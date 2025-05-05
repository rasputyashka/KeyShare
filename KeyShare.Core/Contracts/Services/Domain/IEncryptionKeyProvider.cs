using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Contracts.Services.Domain;
public interface IEncryptionKeyProvider
{
    Key GetKey();
}
