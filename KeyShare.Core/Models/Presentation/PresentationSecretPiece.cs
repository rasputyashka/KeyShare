using System.Numerics;

namespace KeyShare.Core.Models.Presentation;
public class PresentationSecretPiece
{
    public int ID { get; set; }
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
}
