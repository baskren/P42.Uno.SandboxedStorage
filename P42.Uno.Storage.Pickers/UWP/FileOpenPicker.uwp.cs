using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace P42.Uno.SandboxedStorage
{
    public partial class FileOpenPicker : FrameworkElement, IPickSingleFileAsync, IPickMultipleFilesAsync
    {
        public async Task<IStorageFile> PickSingleFileAsync(string pickerOperationId = null)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            if (!string.IsNullOrWhiteSpace(CommitButtonText))
                picker.CommitButtonText = CommitButtonText;
            if (FileTypeFilter?.Any() ?? false)
            {
                foreach (var filter in FileTypeFilter)
                    picker.FileTypeFilter.Add(filter);
            }
            else
                picker.FileTypeFilter.Add("*");
            picker.SettingsIdentifier = SettingsIdentifier;
            picker.SuggestedStartLocation = (Windows.Storage.Pickers.PickerLocationId)SuggestedStartLocation;
            picker.ViewMode = (Windows.Storage.Pickers.PickerViewMode)ViewMode;
            if (string.IsNullOrWhiteSpace(pickerOperationId))
                return await picker.PickSingleFileAsync();
            return await picker.PickSingleFileAsync(pickerOperationId);
        }

    }
}