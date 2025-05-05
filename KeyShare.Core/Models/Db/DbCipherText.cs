namespace KeyShare.Core.Models.Db;
public class DbCipherText
{
    public int ID
    {
        get; set;
    }
    public string Title
    {
        get; set;
    }
    public string Algorithm
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }

    public int KeyID
    {
        get; set;
    }

    public byte[] Content
    {
        get; set;
    }

    public byte[] IV
    {
        get; set;
    }

    public DateTime CreatedAt
    {
        get; set;
    }

    public DateTime UpdatedAt
    {
        get; set;
    }
}
