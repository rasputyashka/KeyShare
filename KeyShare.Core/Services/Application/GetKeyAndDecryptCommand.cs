using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Contracts.Services.Domain;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Services.Application;
public class GetKeyAndDecryptCommand: IGetKeyAndDecryptCommand
{
    private readonly ISecretPieceRepository _secretPieceRepository;
    private readonly ICryptoService _cryptoService;
    private readonly ISecretService _secretService;
    public GetKeyAndDecryptCommand(ISecretPieceRepository secretPieceRepository, ICryptoService cryptoService, ISecretService secretService)
    {
        _secretPieceRepository = secretPieceRepository;
        _cryptoService = cryptoService;
        _secretService = secretService;

    }
    public string Execute(byte[] cipherText, byte[] iv, int keyID)
    {
        var pieces = new List<SecretPiece>();
        var dbPieces = _secretPieceRepository.GetPiecesByKeyID(keyID);
        foreach (var dbPiece in dbPieces) {
            pieces.Add(
                new SecretPiece()
                {
                    X = dbPiece.X,
                    Y = dbPiece.Y,
                }
            );
        }
        var secret = _secretService.RecoverSecret(pieces);
        // 256 / 8 = 32
        var key = Key.FromLong(secret, 32);
        var plainText = _cryptoService.DecryptText(new CipherText() { Content = cipherText, IV = iv }, key.Value);
        return plainText.Content;
        
    }
}
