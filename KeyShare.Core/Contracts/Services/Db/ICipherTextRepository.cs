using KeyShare.Core.Models.Db;

namespace KeyShare.Core.Contracts.Services.Db;
public interface ICipherTextRepository
{
    DbCipherText AddCipherText(byte[] cipherText, byte[] IV, int keyID, string title, string description, string algorithm);
    DbCipherText ReadCipherText(int id);
    IEnumerable<DbCipherText> GetAllCipherTexts();
}