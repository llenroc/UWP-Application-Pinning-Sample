﻿using Mvvm;
using Mvvm.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.ApplicationPinningSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public string IsPinnedToTaskBar()
        {
            var isPinned = Task.Run<bool?>(() => Pinning.IsPinnedToTaskBar()).Result;

            switch (isPinned)
            {
                case true:
                    return "This app is pinned to the task bar.";
                case false:
                    return "This app is not pinned to the task bar.";
                default:
                    return "I cannot determine whether this app is pinned to the task bar.";
            }
        }

        public string IsPinnedToStartMenu()
        {
            var isPinned = Task.Run<bool?>(() => Pinning.IsPinnedToStartMenu()).Result;

            switch (isPinned)
            {
                case true:
                    return "This app is pinned to the start menu.";
                case false:
                    return "This app is not pinned to the start menu.";
                default:
                    return "I cannot determine whether this app is pinned to the start menu.";
            }
        }

        public string HasSecondaryTiles()
        {
            var hasSecondaryTiles = Task.Run<bool>(() => Pinning.HasSecondaryTiles()).Result;
            if (hasSecondaryTiles)
            {
                return "This app has secondary tiles.";
            }

            return "This app has no secondary tiles.";
        }

        public ICommand PinToTaskBarCommand => new DelegateCommand(PinToTaskBar_Executed, PinToTaskBar_CanExecute);

        public ICommand PinToStartMenuCommand => new DelegateCommand(PinToStartMenu_Executed, PinToStartMenu_CanExecute); 

        public ICommand PinSecondaryTileCommand => new DelegateCommand(PinSecondaryTile_Executed);

        private bool PinToTaskBar_CanExecute()
        {
            var isPinned = Task.Run<bool?>(() => Pinning.IsPinnedToTaskBar()).Result;
            return !isPinned ?? false;
        }

        private async void PinToTaskBar_Executed()
        {
            await Pinning.RequestPinToTaskBar();
        }

        private bool PinToStartMenu_CanExecute()
        {
            var isPinned = Task.Run<bool?>(() => Pinning.IsPinnedToStartMenu()).Result;
            return !isPinned ?? false;
        }

        private async void PinToStartMenu_Executed()
        {
            await Pinning.RequestPinToStartMenu();
        }

        private async void PinSecondaryTile_Executed()
        {
            var tileName = await ModalView.InputStringDialogAsync("Pin a new tile.", "Please enter a name for the new secondary tile.", "Go ahead.", "Oops, I changed my mind.");
            if (!string.IsNullOrEmpty(tileName))
            {
                await Pinning.RequestPinSecondaryTile(tileName);
            }
        }

        private async void UnPinSecondaryTile_Executed()
        { }
    }
}
