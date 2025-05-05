using KeyShare.Core.Models.Db;

namespace KeyShare.Core.Contracts.Services.Db;
public interface IKeyRepository
{

    DbKey AddKey(int threshold, int size);
    DbKey ReadKey(int id);
    IEnumerable<DbKey> GetAllKeys();

    void DeleteKey(int keyID);
}
