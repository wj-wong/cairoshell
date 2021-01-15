﻿using System.Threading;
using System.Threading.Tasks;
using CairoDesktop.Common;
using System.Windows.Input;
using ManagedShell.Common.Helpers;
using Microsoft.Extensions.Hosting;

namespace CairoDesktop.SupportingClasses
{
    public sealed class ShellHotKeyService : BackgroundService
    {
        private readonly DesktopManager _desktopManager;

        public ShellHotKeyService(DesktopManager desktopManager)
        {
            _desktopManager = desktopManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task serviceStart = new Task(RegisterSystemHotkeys);

            if (!stoppingToken.IsCancellationRequested)
            {
                serviceStart.Start();
            }

            return serviceStart;
        }

        private void RegisterSystemHotkeys()
        {
            if (EnvironmentHelper.IsAppRunningAsShell)
            {
                CairoApplication.Current.Dispatcher.Invoke(() =>
                {
                    new HotKey(Key.R, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinRCommand);
                    new HotKey(Key.D, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinDCommand);
                    new HotKey(Key.OemComma, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinDCommand);
                    new HotKey(Key.E, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinECommand);
                    new HotKey(Key.I, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinICommand);
                    new HotKey(Key.Pause, HotKeyModifier.Win | HotKeyModifier.NoRepeat, OnWinPauseCommand);
                });
            }
        }

        private void OnWinDCommand(HotKey obj)
        {
            _desktopManager.ToggleOverlay();
        }
        
        private void OnWinRCommand(HotKey cmd)
        {
            ShellHelper.ShowRunDialog(Localization.DisplayString.sRun_Title, Localization.DisplayString.sRun_Info);
        }

        private void OnWinECommand(HotKey cmd)
        {
            FolderHelper.OpenLocation("::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
        }

        private void OnWinICommand(HotKey cmd)
        {
            ShellHelper.StartProcess("control.exe");
        }

        private void OnWinPauseCommand(HotKey cmd)
        {
            ShellHelper.StartProcess("system.cpl");
        }

        // TODO: Add window management related HotKeys
        // Win + [up]
        // Win + [down]
        // Win + [left]
        // Win + [right]
    }
}