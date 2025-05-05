using System.Numerics;

namespace KeyShare.Core.Models.Domain;
public class Secret
{
    public BigInteger Value { get; set; }
    public int Threshold { get; set; }
}
