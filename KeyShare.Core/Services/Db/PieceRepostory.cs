using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using Microsoft.Data.Sqlite;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Models.Db;

namespace KeyShare.Core.Services.Db;

public class SecretPieceRepository : ISecretPieceRepository
{
    private readonly SqliteConnection _connection;

    public SecretPieceRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public DbSecretPiece AddPiece(BigInteger x, BigInteger y, int keyID)
    {
        var now = DateTime.UtcNow.ToString("o");
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO secret_piece (x, y, key_id, created_at, updated_at)
            VALUES ($x, $y, $keyID, $createdAt, $updatedAt)
            RETURNING id, x, y, key_id, created_at, updated_at;
        ";
        cmd.Parameters.AddWithValue("$x", x.ToString());
        cmd.Parameters.AddWithValue("$y", y.ToString());
        cmd.Parameters.AddWithValue("$keyID", keyID); // Placeholder if key_id is not passed explicitly
        cmd.Parameters.AddWithValue("$createdAt", now);
        cmd.Parameters.AddWithValue("$updatedAt", now);

        using var reader = cmd.ExecuteReader();
        reader.Read();

        return new DbSecretPiece
        {
            ID = reader.GetInt32(0),
            X = BigInteger.Parse(reader.GetString(1)),
            Y = BigInteger.Parse(reader.GetString(2)),
            KeyID = reader.GetInt32(3),
            CreatedAt = DateTime.Parse(reader.GetString(4)),
            UpdatedAt = DateTime.Parse(reader.GetString(5))
        };
    }

    public IEnumerable<DbSecretPiece> BulkAddPieces(IEnumerable<Models.Domain.SecretPiece> pieces, int keyID)
    {
        var now = DateTime.UtcNow.ToString("o");
        var result = new List<DbSecretPiece>();

        using var cmd = _connection.CreateCommand();

        var values = new List<string>();
        int index = 0;
        foreach (var piece in pieces)
        {
            values.Add("(@x" + index + ", @y" + index + ", @keyID" + index + ", @createdAt" + index + ", @updatedAt" + index + ")");
            cmd.Parameters.AddWithValue("@x" + index, piece.X.ToString());
            cmd.Parameters.AddWithValue("@y" + index, piece.Y.ToString());
            cmd.Parameters.AddWithValue("@keyID" + index, keyID);
            cmd.Parameters.AddWithValue("@createdAt" + index, now);
            cmd.Parameters.AddWithValue("@updatedAt" + index, now);
            index++;
        }

        cmd.CommandText = @"
            INSERT INTO secret_piece (x, y, key_id, created_at, updated_at)
            VALUES " + string.Join(", ", values) + @"
            RETURNING id, x, y, key_id, created_at, updated_at;
        ";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DbSecretPiece
            {
                ID = reader.GetInt32(0),
                X = BigInteger.Parse(reader.GetString(1)),
                Y = BigInteger.Parse(reader.GetString(2)),
                KeyID = reader.GetInt32(3),
                CreatedAt = DateTime.Parse(reader.GetString(4)),
                UpdatedAt = DateTime.Parse(reader.GetString(5))
            });
        }

        return result;
    }

    public IEnumerable<DbSecretPiece> GetPiecesByKeyID(int keyID)
    {
        var result = new List<DbSecretPiece>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, x, y, key_id, created_at, updated_at FROM secret_piece WHERE key_id = $keyID;";
        cmd.Parameters.AddWithValue("$keyID", keyID);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DbSecretPiece
            {
                ID = reader.GetInt32(0),
                X = BigInteger.Parse(reader.GetString(1)),
                Y = BigInteger.Parse(reader.GetString(2)),
                KeyID = reader.GetInt32(3),
                CreatedAt = DateTime.Parse(reader.GetString(4)),
                UpdatedAt = DateTime.Parse(reader.GetString(5))
            });
        }
        return result;
    }

    public DbSecretPiece ReadPiece(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, x, y, key_id, created_at, updated_at FROM secret_piece WHERE id = $id;";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new InvalidOperationException("SecretPiece not found.");

        return new DbSecretPiece
        {
            ID = reader.GetInt32(0),
            X = BigInteger.Parse(reader.GetString(1)),
            Y = BigInteger.Parse(reader.GetString(2)),
            KeyID = reader.GetInt32(3),
            CreatedAt = DateTime.Parse(reader.GetString(4)),
            UpdatedAt = DateTime.Parse(reader.GetString(5))
        };
    }

    public IEnumerable<DbSecretPiece> GetAllSecretPieces()
    {
        var result = new List<DbSecretPiece>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, x, y, key_id, created_at, updated_at FROM secret_piece;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DbSecretPiece
            {
                ID = reader.GetInt32(0),
                X = BigInteger.Parse(reader.GetString(1)),
                Y = BigInteger.Parse(reader.GetString(2)),
                KeyID = reader.GetInt32(3),
                CreatedAt = DateTime.Parse(reader.GetString(4)),
                UpdatedAt = DateTime.Parse(reader.GetString(5))
            });
        }

        return result;
    }
}
