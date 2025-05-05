using CommunityToolkit.WinUI.UI.Controls;

using KeyShare.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace KeyShare.Views;

public sealed partial class DecryptPage : Page
{
    public DecryptViewModel ViewModel
    {
        get;
    }

    public DecryptPage()
    {
        ViewModel = App.GetService<DecryptViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
