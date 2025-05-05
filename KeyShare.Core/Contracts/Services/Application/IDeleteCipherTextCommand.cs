namespace KeyShare.Core.Contracts.Services.Application;
public interface IDeleteCipherTextCommand
{
    void Execute(int keyID);
}
