using System.Numerics;
using KeyShare.Core.Models.Db;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Contracts.Services.Db;
public interface ISecretPieceRepository
{

    DbSecretPiece AddPiece(BigInteger x, BigInteger y, int keyID);
    IEnumerable<DbSecretPiece> BulkAddPieces(IEnumerable<SecretPiece> pieces, int keyID);
    IEnumerable<DbSecretPiece> GetPiecesByKeyID(int keyID);
    DbSecretPiece ReadPiece(int id);
    IEnumerable<DbSecretPiece> GetAllSecretPieces();
}
