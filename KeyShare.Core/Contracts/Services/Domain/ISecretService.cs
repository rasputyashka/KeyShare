using System.Numerics;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Contracts.Services.Domain;
public interface ISecretService
{
    List<SecretPiece> SplitSecret(BigInteger secret, int threshold, int shares);
    BigInteger RecoverSecret (List<SecretPiece> pieces);
}
