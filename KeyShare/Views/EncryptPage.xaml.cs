using KeyShare.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media;

namespace KeyShare.Views;

public sealed partial class EncryptPage : Page
{
    public EncryptViewModel ViewModel
    {
        get;
    }
    public EncryptPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<EncryptViewModel>();
        this.DataContext = ViewModel;

    }

    // this should be in ViewModel, i guess
    private void OnCommitClick(object sender, RoutedEventArgs e)
    {
        ViewModel.ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(ViewModel.Plaintext))
        {
            ViewModel.ErrorMessage = "Открытый текст не может быть пустым.";
            return;
        }

        if (ViewModel.SelectedThreshold > ViewModel.SelectedPieces)
        {
            ViewModel.ErrorMessage = "Размер порога не может превышать количество частей.";
            return;
        }

        ViewModel.EncryptPlaintext();
        ShowToast("Текст успешно зашифрован.");
    }

    // meh, ok?
    private async void ShowToast(string message, string color = "#28a745")
    {
        ToastText.Text = message;
        ToastNotification.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 40, 167, 69)); // Green

        ToastNotification.Visibility = Visibility.Visible;

        var fadeIn = new DoubleAnimation()
        {
            To = 1,
            Duration = new Duration(TimeSpan.FromMilliseconds(300))
        };
        Storyboard.SetTarget(fadeIn, ToastNotification);
        Storyboard.SetTargetProperty(fadeIn, "Opacity");

        var sb = new Storyboard();
        sb.Children.Add(fadeIn);
        sb.Begin();

        await Task.Delay(3000);

        var fadeOut = new DoubleAnimation()
        {
            To = 0,
            Duration = new Duration(TimeSpan.FromMilliseconds(500))
        };
        Storyboard.SetTarget(fadeOut, ToastNotification);
        Storyboard.SetTargetProperty(fadeOut, "Opacity");

        var sb2 = new Storyboard();
        sb2.Children.Add(fadeOut);
        sb2.Completed += (s, e) => ToastNotification.Visibility = Visibility.Collapsed;
        sb2.Begin();
    }
}
