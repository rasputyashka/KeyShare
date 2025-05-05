namespace KeyShare.Core.Models.Db;
public class DbKey
{
    public int ID
    {
        get; set;
    }

    public int Size
    {
        get; set; 
    }
    public int Threshold
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
