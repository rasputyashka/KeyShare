using Microsoft.Data.Sqlite;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Models.Db;

namespace KeyShare.Core.Services.Db;

public class CipherTextRepository : ICipherTextRepository
{
    private readonly SqliteConnection _connection;

    public CipherTextRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public DbCipherText AddCipherText(byte[] cipherText, byte[] iv, int keyID, string title, string description, string algorithm)
    {
        var now = DateTime.UtcNow.ToString("o");

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO cipher_text (title, algorithm, description, key_id, content, iv, created_at, updated_at)
            VALUES ($title, $algorithm, $description, $keyID, $content, $iv, $createdAt, $updatedAt)
            RETURNING id, title, algorithm, description, key_id, content, iv, created_at, updated_at;
        ";
        cmd.Parameters.AddWithValue("$title", title);
        cmd.Parameters.AddWithValue("$algorithm", algorithm);
        cmd.Parameters.AddWithValue("$description", description);
        cmd.Parameters.AddWithValue("$keyID", keyID);
        cmd.Parameters.AddWithValue("$content", cipherText);
        cmd.Parameters.AddWithValue("$iv", iv);
        cmd.Parameters.AddWithValue("$createdAt", now);
        cmd.Parameters.AddWithValue("$updatedAt", now);

        using var reader = cmd.ExecuteReader();
        reader.Read();

        return new DbCipherText
        {
            ID = reader.GetInt32(0),
            Title = reader.GetString(1),
            Algorithm = reader.GetString(2),
            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
            KeyID = reader.GetInt32(4),
            Content = (byte[])reader[5],
            IV = (byte[])reader[6],
            CreatedAt = DateTime.Parse(reader.GetString(7)),
            UpdatedAt = DateTime.Parse(reader.GetString(8))
        };
    }

    public DbCipherText ReadCipherText(int id)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, title, algorithm, description, key_id, content, iv, created_at, updated_at FROM cipher_text WHERE id = $id;";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            throw new InvalidOperationException("CipherText not found.");

        return new DbCipherText
        {
            ID = reader.GetInt32(0),
            Title = reader.GetString(1),
            Algorithm = reader.GetString(2),
            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
            KeyID = reader.GetInt32(4),
            Content = (byte[])reader[5],
            IV = (byte[])reader[6],
            CreatedAt = DateTime.Parse(reader.GetString(7)),
            UpdatedAt = DateTime.Parse(reader.GetString(8))
        };
    }

    public IEnumerable<DbCipherText> GetAllCipherTexts()
    {
        var result = new List<DbCipherText>();

        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, title, algorithm, description, key_id, content, iv, created_at, updated_at FROM cipher_text;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new DbCipherText
            {
                ID = reader.GetInt32(0),
                Title = reader.GetString(1),
                Algorithm = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                KeyID = reader.GetInt32(4),
                Content = (byte[])reader[5],
                IV = (byte[])reader[6],
                CreatedAt = DateTime.Parse(reader.GetString(7)),
                UpdatedAt = DateTime.Parse(reader.GetString(8))
            });
        }
        return result;
    }
}
