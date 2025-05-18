namespace KeyShare.Core.Contracts.Services.Application;
public interface IGetKeyAndDecryptCommand
{
    public string Execute(byte[] cipherText, byte[] iv,  int keyID);
}
