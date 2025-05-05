using System.Numerics;

namespace KeyShare.Core.Models.Db;
public class DbSecretPiece
{
    public int ID
    {
        get; set;
    }

    public BigInteger X
    {
        get; set;
    }

    public BigInteger Y
    {
        get; set;
    }

    public int KeyID
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
