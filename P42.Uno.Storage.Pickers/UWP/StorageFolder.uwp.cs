﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using P42.Uno.SandboxedStorage;

namespace P42.Uno.SandboxedStorage
{
    class StorageFolder : StorageItem, IStorageFolder
    {
        #region Private Properties
        internal Windows.Storage.StorageFolder _folder
        {
            get => _item as Windows.Storage.StorageFolder;
            set => _item = value;
        }
        #endregion


        #region Construction
        public StorageFolder(Windows.Storage.StorageFolder storageFolder, bool bookmark = false) : base(storageFolder, bookmark) { }
        #endregion


        #region Conversion Operators
        public static implicit operator StorageFolder(Windows.Storage.StorageFolder f)
            => f is null
                ? null
                : new StorageFolder(f);

        public static implicit operator Windows.Storage.StorageFolder(StorageFolder f)
            => f._folder;

        #endregion


        #region Methods

        #region IStorageItem overrides
        public override bool IsOfType(StorageItemTypes type)
            => type == StorageItemTypes.Folder;
        #endregion


        #region Exists
        public async Task<bool> ItemExists(string itemName)
        {
            if (_folder == null)
                return false;
            return await _folder.TryGetItemAsync(itemName) != null;
        }

        public async Task<bool> FileExists(string fileName)
        {
            if (_folder == null)
                return false;
            return await _folder.TryGetItemAsync(fileName) is Windows.Storage.StorageFile;
        }

        public async Task<bool> FolderExists(string folderName)
        {
            if (_folder == null)
                return false;
            return await _folder.TryGetItemAsync(folderName) is Windows.Storage.StorageFolder;
        }
        #endregion


        #region Create
        public async Task<IStorageFile> CreateFileAsync(string desiredName)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null && !string.IsNullOrWhiteSpace(desiredName))
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.CreateFileAsync(desiredName));
                if (result is Windows.Storage.StorageFile file)
                    return new StorageFile(file);
            }
            return null;
        }

        public async Task<IStorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null && !string.IsNullOrWhiteSpace(desiredName))
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.CreateFileAsync(desiredName, (Windows.Storage.CreationCollisionOption)options));
                if (result is Windows.Storage.StorageFile file)
                    return new StorageFile(file);
            }
            return null;
        }

        public async Task<IStorageFolder> CreateFolderAsync(string desiredName)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null && !string.IsNullOrWhiteSpace(desiredName))
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.CreateFolderAsync(desiredName));
                if (result is Windows.Storage.StorageFolder folder)
                    return new StorageFolder(folder);
            }
            return null;
        }

        public async Task<IStorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null && !string.IsNullOrWhiteSpace(desiredName))
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.CreateFolderAsync(desiredName, (Windows.Storage.CreationCollisionOption)options));
                if (result is Windows.Storage.StorageFolder folder)
                    return new StorageFolder(folder);
            }
            return null;
        }
        #endregion


        #region Get
        public static async Task<IStorageFolder> GetFolderFromPathAsync(string path)
        {
            await Task.Delay(5).ConfigureAwait(false);

            try
            {
                if (await Windows.Storage.StorageFolder.GetFolderFromPathAsync(path) is Windows.Storage.StorageFolder windowsFolder)
                    return new StorageFolder(windowsFolder);
            }
            catch (Exception)
            {
                if (await RequestAccessAsync<StorageFolder>(path) is StorageFolder storageFolder)
                    return storageFolder;
            }
            return null;
        }

        public async Task<IStorageFile> GetFileAsync(string filename)
        {
            await Task.Delay(5).ConfigureAwait(false);
            var files = await GetFilesAsync(filename);
            return files?.FirstOrDefault();
        }


        public async Task<IReadOnlyList<IStorageFile>> GetFilesAsync(string pattern = null)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.GetFilesAsync());
                if (result is IReadOnlyList<Windows.Storage.StorageFile> windowsFiles)
                {
                    var files = new List<IStorageFile>();

                    var regex = string.IsNullOrWhiteSpace(pattern)
                        ? null
                        : pattern.WildcardToRegex();

                    foreach (var windowsFile in windowsFiles)
                        if (string.IsNullOrWhiteSpace(regex) || Regex.IsMatch(windowsFile.Name, regex))
                            files.Add(new StorageFile(windowsFile));

                    return files.AsReadOnly();
                }
            }
            return null;
        }

        public async Task<IStorageFolder> GetFolderAsync(string name)
        {
            await Task.Delay(5).ConfigureAwait(false);
            var folders = await GetFoldersAsync(name);
            return folders?.FirstOrDefault();
        }

        public async Task<IReadOnlyList<IStorageFolder>> GetFoldersAsync(string pattern = null)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.GetFoldersAsync());
                if (result is IReadOnlyList<Windows.Storage.StorageFolder> windowsFolders)
                {
                    var folders = new List<IStorageFolder>();

                    var regex = string.IsNullOrWhiteSpace(pattern)
                        ? null
                        : pattern.WildcardToRegex();

                    foreach (var windowsFolder in windowsFolders)
                        if (string.IsNullOrWhiteSpace(regex) || Regex.IsMatch(windowsFolder.Name, regex))
                            folders.Add(new StorageFolder(windowsFolder));

                    return folders.AsReadOnly();
                }
            }
            return null;
        }

        public async Task<IStorageItem> GetItemAsync(string name)
        {
            await Task.Delay(5).ConfigureAwait(false);

            var folders = await GetItemsAsync(name);
            return folders?.FirstOrDefault();
        }

        public async Task<IReadOnlyList<IStorageItem>> GetItemsAsync(string pattern = null)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_folder != null)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._folder.GetItemsAsync());
                if (result is IReadOnlyList<Windows.Storage.IStorageItem> windowsItems)
                {
                    var items = new List<IStorageItem>();

                    var regex = string.IsNullOrWhiteSpace(pattern)
                            ? null
                            : pattern.WildcardToRegex();

                    foreach (var windowsItem in windowsItems)
                    {
                        if (string.IsNullOrWhiteSpace(regex) || Regex.IsMatch(windowsItem.Name, regex))
                        {
                            if (windowsItem is Windows.Storage.StorageFile windowsFile)
                                items.Add(new StorageFile(windowsFile));
                            else if (windowsItem is Windows.Storage.StorageFolder windowsFolder)
                                items.Add(new StorageFolder(windowsFolder));
                        }
                    }

                    return items.AsReadOnly();
                }
            }
            return null;
        }

        public async Task<IStorageItem> TryGetItemAsync(string name)
            => await GetItemAsync(name);
        #endregion


        #endregion
    }
}
