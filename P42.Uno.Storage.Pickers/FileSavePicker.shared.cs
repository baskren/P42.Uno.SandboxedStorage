using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace P42.Uno.SandboxedStorage
{
    public partial class FileSavePicker : DependencyObject
    {
        static Dictionary<string, string> SettingsPaths;

        static FileSavePicker()
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
            typeof(FileSavePicker),
            new PropertyMetadata(default(string))
        );
        public string CommitButtonText
        {
            get => (string)GetValue(CommitButtonTextProperty);
            set => SetValue(CommitButtonTextProperty, value);
        }
        #endregion CommitButtonText Property


        #region DefaultFileExtension Property
        public static readonly DependencyProperty DefaultFileExtensionProperty = DependencyProperty.Register(
            nameof(DefaultFileExtension),
            typeof(string),
            typeof(FileSavePicker),
            new PropertyMetadata(default(string))
        );
        public string DefaultFileExtension
        {
            get => (string)GetValue(DefaultFileExtensionProperty);
            set => SetValue(DefaultFileExtensionProperty, value);
        }
        #endregion DefaultFileExtension Property


        #region FileTypeChoices Property
        public static readonly DependencyProperty FileTypeChoicesProperty = DependencyProperty.Register(
            nameof(FileTypeChoices),
            typeof(IDictionary<string, IList<string>>),
            typeof(FileSavePicker),
            new PropertyMetadata(default(IDictionary<string, IList<string>>))
        );
        public IDictionary<string, IList<string>> FileTypeChoices
        {
            get => (IDictionary<string, IList<string>>)GetValue(FileTypeChoicesProperty);
            set => SetValue(FileTypeChoicesProperty, value);
        }
        #endregion FileTypeChoices Property



        #region SettingsIdentifier Property
        public static readonly DependencyProperty SettingsIdentifierProperty = DependencyProperty.Register(
            nameof(SettingsIdentifier),
            typeof(string),
            typeof(FileSavePicker),
            new PropertyMetadata(default(string))
        );
        public string SettingsIdentifier
        {
            get => (string)GetValue(SettingsIdentifierProperty);
            set => SetValue(SettingsIdentifierProperty, value);
        }
        #endregion SettingsIdentifier Property


        #region SuggestedFileName Property
        public static readonly DependencyProperty SuggestedFileNameProperty = DependencyProperty.Register(
            nameof(SuggestedFileName),
            typeof(string),
            typeof(FileSavePicker),
            new PropertyMetadata(default(string))
        );
        public string SuggestedFileName
        {
            get => (string)GetValue(SuggestedFileNameProperty);
            set => SetValue(SuggestedFileNameProperty, value);
        }
        #endregion SuggestedFileName Property


        #region SuggestedSaveFile Property
        public static readonly DependencyProperty SuggestedSaveFileProperty = DependencyProperty.Register(
            nameof(SuggestedSaveFile),
            typeof(IStorageFile),
            typeof(FileSavePicker),
            new PropertyMetadata(default(IStorageFile))
        );
        public IStorageFile SuggestedSaveFile
        {
            get => (IStorageFile)GetValue(SuggestedSaveFileProperty);
            set => SetValue(SuggestedSaveFileProperty, value);
        }
        #endregion SuggestedSaveFile Property


        #region SuggestedStartLocation Property
        public static readonly DependencyProperty SuggestedStartLocationProperty = DependencyProperty.Register(
            nameof(SuggestedStartLocation),
            typeof(PickerLocationId),
            typeof(FileSavePicker),
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
            typeof(FileSavePicker),
            new PropertyMetadata(default(PickerViewMode))
        );
        public PickerViewMode ViewMode
        {
            get => (PickerViewMode)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }
        #endregion ViewMode Property


        #endregion


    }
}
