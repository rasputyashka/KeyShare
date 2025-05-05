using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyShare.Contracts.ViewModels;
using KeyShare.Core.Contracts.Services.Application;
using KeyShare.Core.Models.Presentation;

namespace KeyShare.ViewModels;

public partial class DecryptViewModel : ObservableRecipient, INavigationAware
{
    private readonly IRetrieveAllCipherTextsCommand _retrieveAllCipherTextsCommand;
    private readonly IGetKeyAndDecryptCommand _getKeyAndDecryptCommand;
    private readonly IDeleteCipherTextCommand _deleteCipherTextCommand;


    [ObservableProperty]
    private PresentationCipherText? selected;
    public ObservableCollection<PresentationCipherText> Items { get; private set; } = new ObservableCollection<PresentationCipherText>();

    public DecryptViewModel(IRetrieveAllCipherTextsCommand retrieveAllCipherTextsCommand, IGetKeyAndDecryptCommand getKeyAndDecryptCommand, IDeleteCipherTextCommand deleteCipherTextCommand)
    {
        _retrieveAllCipherTextsCommand = retrieveAllCipherTextsCommand;
        _getKeyAndDecryptCommand = getKeyAndDecryptCommand;
        _deleteCipherTextCommand = deleteCipherTextCommand;
    }

    partial void OnSelectedChanged(PresentationCipherText? value)
    {
    }


    [RelayCommand]
    public void OnDecryptClick(PresentationCipherText selectedItem)
    {
        var plainText = _getKeyAndDecryptCommand.Execute(selectedItem.Content, selectedItem.IV, selectedItem.KeyID);
        selectedItem.DisplayContent = plainText;
    }



    public async void OnNavigatedTo(object parameter)
    {
        Items.Clear();

        var data = _retrieveAllCipherTextsCommand.Execute();

        foreach (var item in data)
        {
            item.DisplayContent = BitConverter.ToString(item.Content).Replace("-", string.Empty);
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
