﻿using System;
using System.Threading.Tasks;
using AppKit;

namespace P42.Uno.SandboxedStorage
{
    public partial class FolderOpenPicker : IPickSingleFolderAsync
    {
        public async Task<IStorageFolder> PickSingleFolderAsync()
        {
            using (var panel = new NSOpenPanel
            {
                CanChooseDirectories = true,
                CanChooseFiles = false,
                FloatingPanel = true,
                AllowsMultipleSelection = false,
                ResolvesAliases = true,
            })
            {
                panel.RunModal();

                if (panel.Url is null)
                    return await Task.FromResult<IStorageFolder>(null);

                System.Diagnostics.Debug.WriteLine("panel.Url.Path: " + panel.Url.Path);
                return await Task.FromResult<IStorageFolder>(new StorageFolder(panel.Url, true));
            }
        }

        internal static Task<IStorageFolder> PickSingleFolderAsync(StorageFolder storageFolder, string message = null)
        {
            using (var panel = new NSOpenPanel
            {
                CanCreateDirectories = true,
                CanChooseDirectories = true,
                CanChooseFiles = false,
                FloatingPanel = true,
                AllowsMultipleSelection = false,
                ResolvesAliases = true,
                DirectoryUrl = storageFolder.Url,
                Prompt = "THIS IS THE PROMPT!",
                Title = "TITLE!!!",
            })
            {
                if (!string.IsNullOrWhiteSpace(message))
                    panel.Message = message;

                panel.RunModal();

                if (panel.Url is null)
                    return Task.FromResult<IStorageFolder>(null);

                System.Diagnostics.Debug.WriteLine("panel.Url.Path: " + panel.Url.Path);
                return Task.FromResult<IStorageFolder>(new StorageFolder(panel.Url, true));
            }
        }

    }
}
