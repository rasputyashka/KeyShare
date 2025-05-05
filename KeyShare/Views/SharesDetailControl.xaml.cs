using KeyShare.Core.Models;
using KeyShare.Core.Models.Presentation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace KeyShare.Views;

public sealed partial class SharesDetailControl : UserControl
{
    public PresentationSecretPiece? MenuItem
    {
        get => GetValue(MenuItemProperty) as PresentationSecretPiece;
        set => SetValue(MenuItemProperty, value);
    }

    public static readonly DependencyProperty MenuItemProperty = DependencyProperty.Register("MenuItem", typeof(PresentationSecretPiece), typeof(SharesDetailControl), new PropertyMetadata(null, OnMenuItemPropertyChanged));

    public SharesDetailControl()
    {
        InitializeComponent();
    }

    private static void OnMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SharesDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
