using System.Numerics;
using KeyShare.Core.Contracts.Services.Db;
using Microsoft.Data.Sqlite;
using KeyShare.Core.Models.Db;


namespace KeyShare.Core.Services.Db;

public class KeyRepository : IKeyRepository
{
    private readonly SqliteConnection _connection;

    public KeyRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public DbKey AddKey(int threshold, int size)
    {
        var now = DateTime.UtcNow.ToString("o");
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO key (size, threshold, created_at, updated_at)
                            VALUES ($size, $threshold, $createdAt, $updatedAt) RETURNING id, size, threshold, created_at, updated_at;";
        cmd.Parameters.AddWithValue("$size", size);
        cmd.Parameters.AddWithValue("$threshold", threshold);
        cmd.Parameters.AddWithValue("$createdAt", now);
        cmd.Parameters.AddWithValue("$updatedAt", now);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new DbKey
            {
                ID = reader.GetInt32(0),
                Threshold = reader.GetInt32(1),
                Size = reader.GetInt32(2),
                CreatedAt = DateTime.Parse(reader.GetString(3)),
                UpdatedAt = DateTime.Parse(reader.GetString(4))
            };
        }
        throw new Exception("Failed to insert the key");
    }

    public void DeleteKey(int keyID)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM key WHERE id = $keyID";
        cmd.Parameters.AddWithValue("$keyID", keyID);
        var rows = cmd.ExecuteNonQuery();
        if (rows == 0)
        {
            throw new InvalidOperationException($"No key found with ID {keyID} to delete.");
        }
    }
    public DbKey ReadKey(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM key WHERE id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) throw new Exception("Key not found");

        return new DbKey
        {
            ID = reader.GetInt32(0),
            Size = reader.GetInt32(1),
            Threshold = reader.GetInt32(2),
            CreatedAt = DateTime.Parse(reader.GetString(3)),
            UpdatedAt = DateTime.Parse(reader.GetString(4))
        };
    }
    public IEnumerable<DbKey> GetAllKeys()
    {
        var result = new List<DbKey>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM key;";
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new DbKey
            {
                ID = reader.GetInt32(0),
                Size = reader.GetInt32(1),
                Threshold = reader.GetInt32(2),
                CreatedAt = DateTime.Parse(reader.GetString(3)),
                UpdatedAt = DateTime.Parse(reader.GetString(4)),
            });
        }

        return result;
    }
}