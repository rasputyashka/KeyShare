using KeyShare.Core.Models.Presentation;
using KeyShare.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace KeyShare.Views;

public sealed partial class DecryptDetailControl : UserControl
{

    public PresentationCipherText? MenuItem
    {
        get => GetValue(MenuItemProperty) as PresentationCipherText;
        set => SetValue(MenuItemProperty, value);
    }

    public static readonly DependencyProperty MenuItemProperty = DependencyProperty.Register(nameof(MenuItem), typeof(PresentationCipherText), typeof(DecryptDetailControl), new PropertyMetadata(null, OnMenuItemPropertyChanged));

    public DecryptViewModel ViewModel
    {
        get;
    }

    public DecryptDetailControl()
    {
        ViewModel = App.GetService<DecryptViewModel>();
        InitializeComponent();
    }

    private static void OnMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DecryptDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
