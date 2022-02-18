//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

//using Microsoft.Graphics.Canvas.Effects;

namespace Homeschool.App
{
    /// <summary>
    /// A page that displays the app's wcfSettings.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public string Version
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            this.InitializeComponent();
            Loaded += OnSettingsPageLoaded;

            if (ElementSoundPlayer.State == ElementSoundPlayerState.On)
                soundToggle.IsOn = true;
            if (ElementSoundPlayer.SpatialAudioMode == ElementSpatialAudioMode.On)
                spatialSoundBox.IsChecked = true;

            screenshotModeToggle.IsOn = ViewModel.IsScreenshotMode;
            screenshotFolderLinkContent.Text = ViewModel.ScreenshotStorageFolderPath;
        }

        public SettingsViewModel ViewModel
        {
            get;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //NavigationRootPage.Current.NavigationView.Header = "WcfSettings";
        }

        private void OnSettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = ThemeHelper.RootTheme.ToString();
            (ThemePanel.Children.Cast<RadioButton>()
                .FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;
        }

        private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
            //ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            if (selectedTheme is not null)
            {
                ThemeHelper.RootTheme = App.GetEnum<ApplicationTheme>(selectedTheme);
            }
        }

        private void OnThemeRadioButtonKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Up)
            {
                //NavigationRootPage.Current.PageHeader.Focus(FocusState.Programmatic);
            }
        }
        private void spatialSoundBox_Checked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
            }
        }

        private void soundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                spatialSoundBox.IsEnabled = true;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
            else
            {
                spatialSoundBox.IsEnabled = false;
                spatialSoundBox.IsChecked = false;

                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }
        }

        private void screenshotModeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            ViewModel.IsScreenshotMode = screenshotModeToggle.IsOn;
        }

        private void spatialSoundBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }
        }

        private async void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.Window);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            folderPicker.FileTypeFilter.Add(".png"); // meaningless, but you have to have something
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder == null)
            {
                return;
            }

            ViewModel.ScreenshotStorageFolderPath = folder.Path;
            screenshotFolderLink.Content = ViewModel.ScreenshotStorageFolder;
        }

        private async void screenshotFolderLink_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchFolderAsync(ViewModel.ScreenshotStorageFolder);
        }

        private void OnResetTeachingTipsButtonClick(object sender, RoutedEventArgs e)
        {
            ProtocolActivationClipboardHelper.ShowCopyLinkTeachingTip = true;
        }
    }
}
