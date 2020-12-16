using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace P42.Uno.SandboxedStorage
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(FileTypeFilter))]
    public partial class FileOpenPicker : DependencyObject
    {
        static Dictionary<string, string> SettingsPaths;

        static FileOpenPicker()
        {
            SettingsPaths = new Dictionary<string, string>();
            //TODO: add store/restore logic!
        }

        #region Properties

        #region FileTypeFilter Property
        public List<string> FileTypeFilter { get; private set; } 
        #endregion FileTypeFilter Property


        #region CommitButtonText Property
        public static readonly DependencyProperty CommitButtonTextProperty = DependencyProperty.Register(
            nameof(CommitButtonText),
            typeof(string),
            typeof(FileOpenPicker),
            new PropertyMetadata(default(string))
        );
        public string CommitButtonText
        {
            get => (string)GetValue(CommitButtonTextProperty);
            set => SetValue(CommitButtonTextProperty, value);
        }
        #endregion CommitButtonText Property


        #region SettingsIdentifier Property
        public static readonly DependencyProperty SettingsIdentifierProperty = DependencyProperty.Register(
            nameof(SettingsIdentifier),
            typeof(string),
            typeof(FileOpenPicker),
            new PropertyMetadata(default(string))
        );
        public string SettingsIdentifier
        {
            get => (string)GetValue(SettingsIdentifierProperty);
            set => SetValue(SettingsIdentifierProperty, value);
        }
        #endregion SettingsIdentifier Property


        #region SuggestedStartLocation Property
        public static readonly DependencyProperty SuggestedStartLocationProperty = DependencyProperty.Register(
            nameof(SuggestedStartLocation),
            typeof(PickerLocationId),
            typeof(FileOpenPicker),
            new PropertyMetadata(default(PickerLocationId))
        );
        public PickerLocationId SuggestedStartLocation
        {
            get => (PickerLocationId)GetValue(SuggestedStartLocationProperty);
            set => SetValue(SuggestedStartLocationProperty, value);
        }
        #endregion SuggestedStartLocation Property


        #region ViewMode Property
        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(
            nameof(ViewMode),
            typeof(PickerViewMode),
            typeof(FileOpenPicker),
            new PropertyMetadata(default(PickerViewMode))
        );
        public PickerViewMode ViewMode
        {
            get => (PickerViewMode)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }
        #endregion ViewMode Property


        #endregion


        #region Construction
        public FileOpenPicker()
        {
            FileTypeFilter = new List<string>();
            CommitButtonText = "Open";
        }
        #endregion

    }
}
