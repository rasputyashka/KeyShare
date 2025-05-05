using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Models.Presentation;

namespace KeyShare.Core.Services.Application;
public class RetrieveAllCipherTexts: IRetrieveAllCipherTextsCommand
{
    private readonly ICipherTextRepository _cipherTextRepository;
    private readonly IKeyRepository _keyRepository;
    public RetrieveAllCipherTexts(ICipherTextRepository cipherTextRepository, IKeyRepository keyRepository)
    {
        _cipherTextRepository = cipherTextRepository;
        _keyRepository = keyRepository;
    }
    public IEnumerable<PresentationCipherText> Execute()
    {
        // i am not going to refactor this...
        var cipherTexts = new List<PresentationCipherText>();
        var dbCipherTexts = _cipherTextRepository.GetAllCipherTexts();
        var dbKeys = _keyRepository.GetAllKeys();
        foreach (var dbCipherText in dbCipherTexts)
        {
            var matchingKey = dbKeys.FirstOrDefault(p => p.ID == dbCipherText.KeyID);
            var threshold = matchingKey.Threshold;
            cipherTexts.Add(
                new PresentationCipherText() { 
                    Algorithm = dbCipherText.Algorithm,
                    Content = dbCipherText.Content,
                    Description = dbCipherText.Description,
                    KeyID = dbCipherText.KeyID,
                    Title = dbCipherText.Title,
                    ID = dbCipherText.ID,
                    CreatedAt = dbCipherText.CreatedAt,
                    Threshold = threshold,
                    IV = dbCipherText.IV,
                }
            );
        }
        return cipherTexts;
    }
}
