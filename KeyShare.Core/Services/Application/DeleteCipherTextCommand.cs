using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Contracts.Services.Db;

namespace KeyShare.Core.Services.Application;
public class DeleteCipherTextCommand: IDeleteCipherTextCommand
{
    private readonly IKeyRepository _keyRepository;
    public DeleteCipherTextCommand(IKeyRepository keyRepository)
    {
        _keyRepository = keyRepository;
    }
    public void Execute(int keyID)
    {
        _keyRepository.DeleteKey(keyID);
    }
}

