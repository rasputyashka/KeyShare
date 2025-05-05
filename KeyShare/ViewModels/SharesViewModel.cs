using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using KeyShare.Contracts.ViewModels;
using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Models.Presentation;

namespace KeyShare.ViewModels;

public partial class SharesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRetrieveAllPiecesCommand _retrieveAllPiecesCommand;
    
    [ObservableProperty]
    private PresentationSecretPiece? selected;

    public ObservableCollection<PresentationSecretPiece> Items { get; private set; } = new ObservableCollection<PresentationSecretPiece>();

    public SharesViewModel(IRetrieveAllPiecesCommand retrieveAllPiecesCommand)
    {
        _retrieveAllPiecesCommand = retrieveAllPiecesCommand;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Items.Clear();

        var data = _retrieveAllPiecesCommand.Execute();

        foreach (var item in data)
        {
            Items.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (Items.Any())
        {
            Selected ??= Items.First();
        }
    }
}
