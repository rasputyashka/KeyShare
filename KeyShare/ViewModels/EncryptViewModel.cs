using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using KeyShare.Core.Contracts.Services.Application;

namespace KeyShare.ViewModels;

public partial class EncryptViewModel : ObservableRecipient
{
    private IEncryptAndSaveCommand EncryptAndSaveCommand
    {
        get;
    }

    public EncryptViewModel(
        IEncryptAndSaveCommand encryptAndSaveCommand)
    {
        EncryptAndSaveCommand = encryptAndSaveCommand;

        for (var i = 2; i <= 10; i++)
        {
            ThresholdValues.Add(i);
            PiecesValues.Add(i);
        }

        SelectedThreshold = 2;
        SelectedPieces = 2;
    }

    public void EncryptPlaintext(   )
    {
        EncryptAndSaveCommand.Execute(
            Plaintext,
            SelectedThreshold,
            SelectedPieces,
            Title,
            Description
        );
    }


    public ObservableCollection<int> ThresholdValues { get; } = new();
    public ObservableCollection<int> PiecesValues { get; } = new();

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;


    [ObservableProperty]
    private int selectedThreshold;

    [ObservableProperty]
    private int selectedPieces;

    [ObservableProperty]
    private string plaintext = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;
}