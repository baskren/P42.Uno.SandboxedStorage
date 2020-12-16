using AppKit;
using MobileCoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace P42.Uno.SandboxedStorage
{
    public partial class FileOpenPicker : FrameworkElement, IPickSingleFileAsync, IPickMultipleFilesAsync
    {
        public async Task<IStorageFile> PickSingleFileAsync(string pickerOperationId = null)
        {
            using (var panel = new NSOpenPanel
            {
                CanChooseDirectories = false,
                CanChooseFiles = true,
                FloatingPanel = true,
                AllowsMultipleSelection = false,
                ResolvesAliases = true,
            })
            {
                if (!(FileTypeFilter?.ToArray() is string[] fileTypes) || !fileTypes.Any())
                    fileTypes = new string[] { UTType.Content, UTType.Item, "public.data" };
                panel.RunModal(fileTypes);

                if (panel.Url is null)
                    return await Task.FromResult<IStorageFile>(null);

                return await Task.FromResult<IStorageFile>(new StorageFile(panel.Url, true));
            }
        }

        public async Task<IReadOnlyList<IStorageFile>> PickMultipleFilesAsync()
        {
            using (var panel = new NSOpenPanel
            {
                CanChooseDirectories = false,
                CanChooseFiles = true,
                FloatingPanel = true,
                AllowsMultipleSelection = true,
                ResolvesAliases = true,
            })
            {
                if (!(FileTypeFilter?.ToArray() is string[] fileTypes) || !fileTypes.Any())
                    fileTypes = new string[] { UTType.Content, UTType.Item, "public.data" };
                panel.RunModal(fileTypes);

                if (panel.Urls is null && panel.Url is null)
                    return await Task.FromResult<IReadOnlyList<IStorageFile>>(null);

                if (panel.Urls.Length > 0)
                    return await Task.FromResult<IReadOnlyList<IStorageFile>>(new FilePickerSelectedFilesArray(panel.Urls));
                return await Task.FromResult<IReadOnlyList<IStorageFile>>(new FilePickerSelectedFilesArray(panel.Url));
            }
        }

        internal static async Task<IStorageFile> PickSingleFileAsync(StorageFile storageFile, string message = null)
        {
            var folderUrl = storageFile.Url.RemoveLastPathComponent();

            using (var panel = new NSOpenPanel
            {
                CanCreateDirectories = true,
                CanChooseDirectories = false,
                CanChooseFiles = true,
                FloatingPanel = true,
                AllowsMultipleSelection = false,
                ResolvesAliases = true,
                DirectoryUrl = folderUrl,
                Prompt = "THIS IS THE PROMPT!",
                Title = "TITLE!",
            })
            {
                if (!string.IsNullOrWhiteSpace(message))
                    panel.Message = message;

                var utType = storageFile.FileType;

                panel.RunModal(folderUrl.Path, storageFile.Name, new string[] { utType });

                if (panel.Url is null)
                    return await Task.FromResult<IStorageFile>(null);

                System.Diagnostics.Debug.WriteLine("panel.Url.Path: " + panel.Url.Path);
                return await Task.FromResult<IStorageFile>(new StorageFile(panel.Url, true));
            }
        }

    }
}
