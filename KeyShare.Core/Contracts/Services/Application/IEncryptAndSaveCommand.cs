namespace KeyShare.Core.Contracts.Services.Application;
public interface IEncryptAndSaveCommand
{
    public void Execute(string plaintext, int threshold, int shares, string title, string description);
}