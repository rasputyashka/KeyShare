using System.Diagnostics;
using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Contracts.Services.Domain;

namespace KeyShare.Core.Services.Application;
public class EncryptAndSaveCommand : IEncryptAndSaveCommand
{
    private readonly ICryptoService _encryptionService;
    private readonly IEncryptionKeyProvider _encryptionKeyProvider;
    private readonly ISecretService _secretService;
    private readonly ICipherTextRepository _cipherTextRepository;
    private readonly IKeyRepository _keyRepository;
    private readonly ISecretPieceRepository _secretPieceRepository;

    public EncryptAndSaveCommand(
        ICryptoService encryptionService,
        IEncryptionKeyProvider encryptionKeyProvider,
        ICipherTextRepository cipherTextRepository,
        IKeyRepository keyRepository,
        ISecretPieceRepository secretPieceRepository,
        ISecretService secretService
        )
    {
        _encryptionService = encryptionService;
        _encryptionKeyProvider = encryptionKeyProvider;
        _cipherTextRepository = cipherTextRepository;
        _keyRepository = keyRepository;
        _secretPieceRepository = secretPieceRepository;
        _secretService = secretService;

    }

    public void Execute(string plainText, int threshold, int shares, string title, string description)
    {
        var key = _encryptionKeyProvider.GetKey();
        var cipherText = _encryptionService.EncryptText(plainText, key.Value);
        var pieces = _secretService.SplitSecret(key.AsLong, threshold, shares);
        // 256 bits / 8 = 32 bytes
        var keyModel = _keyRepository.AddKey(threshold, 32);
        _cipherTextRepository.AddCipherText(cipherText.Content, cipherText.IV, keyModel.ID, title, description, "AES-CBC-256");
        _secretPieceRepository.BulkAddPieces(pieces, keyModel.ID);
    }
}
