using System.Numerics;
using KeyShare.Core.Contracts.Services.Domain;
using KeyShare.Core.Models.Domain;

namespace KeyShare.Core.Services.Domain;

public class SecretService: ISecretService
{
    // prime may not be less than key size
    private static readonly BigInteger PRIME = BigInteger.Pow(2, 256) + 297;
    private static readonly Random Random = new();

    public List<SecretPiece> SplitSecret(BigInteger secret, int threhsold, int shares)
    {
        var prime = PRIME;
        if (threhsold > shares)
            throw new ArgumentException("Minimum cannot be greater than the number of shares");

        var poly = new List<BigInteger> { secret };
        for (var i = 1; i < threhsold; i++)
            poly.Add(RandomBigInt(prime - 1));


        var points = new List<SecretPiece>();
        for (var i = 1; i <= shares; i++)
        {
            points.Add( new SecretPiece() { X = i, Y = EvalAt(poly, i, prime) });
        }

        return points;
    }

    public BigInteger RecoverSecret(List<SecretPiece> shares)
    {
        var prime = PRIME;

        if (shares.Count < 1)
            throw new ArgumentException("At least one share is required");

        var x_s = shares.Select(s => s.X).ToList();
        var y_s = shares.Select(s => s.Y).ToList();

        return LagrangeInterpolate(0, x_s, y_s, prime);
    }

    private static BigInteger EvalAt(List<BigInteger> poly, BigInteger x, BigInteger prime)
    {
        BigInteger accum = 0;
        for (var i = poly.Count - 1; i >= 0; i--)
        {
            accum *= x;
            accum += poly[i];
            accum %= prime;
        }
        return accum;
    }

    private static BigInteger LagrangeInterpolate(BigInteger x, List<BigInteger> x_s, List<BigInteger> y_s, BigInteger p)
    {
        var k = x_s.Count;
        BigInteger result = 0;

        for (var i = 0; i < k; i++)
        {
            BigInteger xi = x_s[i], yi = y_s[i];
            BigInteger num = 1, den = 1;

            for (var j = 0; j < k; j++)
            {
                if (i == j) continue;
                BigInteger xj = x_s[j];
                num = (num * (x - xj + p)) % p;
                den = (den * (xi - xj + p)) % p;
            }

            var inv = ModInverse(den, p);
            result = (result + p + (yi * num % p) * inv % p) % p;
        }

        return result;
    }

    private static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        var (gcd, x, _) = ExtendedGcd(a, m);
        if (gcd != 1)
        {
            throw new ArgumentException("Modular inverse does not exist");
        }

        return (x % m + m) % m;
    }

    private static (BigInteger gcd, BigInteger x, BigInteger y) ExtendedGcd(BigInteger a, BigInteger b)
    {
        BigInteger x = 0, lastX = 1;
        BigInteger y = 1, lastY = 0;

        while (b != 0)
        {
            var q = a / b;
            (a, b) = (b, a % b);
            (x, lastX) = (lastX - q * x, x);
            (y, lastY) = (lastY - q * y, y);
        }

        return (a, lastX, lastY);
    }

    private static BigInteger RandomBigInt(BigInteger max)
    {
        var bytes = max.ToByteArray();
        BigInteger result;
        do
        {
            Random.NextBytes(bytes);
            bytes[^1] &= 0x7F; // Ensure non-negative
            result = new BigInteger(bytes);
        } while (result > max);
        return result;
    }
}