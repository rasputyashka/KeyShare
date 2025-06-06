﻿using CommunityToolkit.WinUI.UI.Controls;

using KeyShare.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace KeyShare.Views;

public sealed partial class SharesPage : Page
{
    public SharesViewModel ViewModel
    {
        get;
    }

    public SharesPage()
    {
        ViewModel = App.GetService<SharesViewModel>();
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
