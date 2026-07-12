using System;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;

namespace WinDroid.Studio;

/// <summary>
/// Minimal initial window shown while the project is in its scaffolding stage.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SelectApkButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        picker.ViewMode = PickerViewMode.List;
        picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
        picker.FileTypeFilter.Add(".apk");

        try
        {
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                SelectedApkPathTextBlock.Text = $"Selected APK: {file.Path}";
            }
            else
            {
                SelectedApkPathTextBlock.Text = "Operation cancelled.";
            }
        }
        catch (Exception ex)
        {
            SelectedApkPathTextBlock.Text = $"Error selecting file: {ex.Message}";
        }
    }
}