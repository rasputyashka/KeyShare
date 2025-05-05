using CommunityToolkit.Mvvm.ComponentModel;


namespace KeyShare.Core.Models.Presentation;

public partial class PresentationCipherText : ObservableObject
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
    public byte[] Content
    {
        get; set;
    }

    public byte[] IV
    {
        get; set;
    }

    [ObservableProperty]
    private string displayContent;

    public int KeyID
    {
        get; set;
    }

    public DateTime CreatedAt
    {
        get; set;
    }

    public int Threshold
    {
        get; set;
    }
}
