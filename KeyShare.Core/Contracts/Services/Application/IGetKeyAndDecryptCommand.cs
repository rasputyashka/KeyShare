namespace KeyShare.Core.Contracts.Services.Application;
public interface IGetKeyAndDecryptCommand
{
    // actually CipherText object is better here
    public string Execute(byte[] cipherText, byte[] iv,  int keyID);
}
