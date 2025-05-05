using System.Diagnostics;
using System.Numerics;

namespace KeyShare.Core.Models.Domain;
public class Key
{
    public byte[] Value
    {
        get; set;
    }

    public int Size
    {
        get; set;
    }

    public BigInteger AsLong
    {
        get
        {
            var unsignedBytes = new byte[Value.Length + 1];
            Array.Copy(Value, 0, unsignedBytes, 1, Value.Length);
            Array.Reverse(unsignedBytes);
            var value = new BigInteger(unsignedBytes);
            Debug.WriteLine(BitConverter.ToString(Key.FromLong(value, Size).Value).Replace("-", " "));
            return value;
        }
    }

    public static Key FromLong(BigInteger value, int size)
    {
        if (value < 0)
            throw new ArgumentException("Value must be non-negative");

        var bytes = value.ToByteArray();
        Array.Resize(ref bytes, size);
        Array.Reverse(bytes);
        return new Key { Value = bytes };
    }
}