namespace KeyShare.Core.Models.Domain;
public class CipherText
{
    public byte[] Content
    {
        get; set;
    }
    public byte[] IV
    {
        get; set;
    }
}
