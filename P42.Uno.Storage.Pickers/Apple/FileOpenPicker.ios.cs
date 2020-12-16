using MobileCoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Windows.UI.Xaml;

namespace P42.Uno.SandboxedStorage
{
    public partial class FileOpenPicker : FrameworkElement, IPickSingleFileAsync, IPickMultipleFilesAsync
    {
        static TaskCompletionSource<IStorageFile> single_tcs;
        static TaskCompletionSource<IReadOnlyList<IStorageFile>> multi_tcs;

        public async Task<IStorageFile> PickSingleFileAsync(string pickerOperationId = null)
        {

            if (!(FileTypeFilter?.ToArray() is string[] fileTypes) || !fileTypes.Any())
                fileTypes = new string[] { UTType.Content, UTType.Item, "public.data" };
            using (var pvc = new UIDocumentPickerViewController(fileTypes?.ToArray() ?? new string[] { UTType.Content, UTType.Item, "public.data" }, UIDocumentPickerMode.Open)
            {
                AllowsMultipleSelection = false,
            })
            {
                single_tcs = new TaskCompletionSource<IStorageFile>();

                pvc.DidPickDocument += Pvc_DidPickDocument;
                pvc.WasCancelled += Pvc_WasCancelled;
                pvc.DidPickDocumentAtUrls += Pvc_DidPickDocumentAtUrls;

                var viewController = FilePickerExtensions.GetActiveViewController();
                viewController.PresentViewController(pvc, true, null);

                return await single_tcs.Task;
            }
        }

        public async Task<IReadOnlyList<IStorageFile>> PickMultipleFilesAsync()
        {
            if (!(FileTypeFilter?.ToArray() is string[] fileTypes) || !fileTypes.Any())
                fileTypes = new string[] { UTType.Content, UTType.Item, "public.data" };
            using (var pvc = new UIDocumentPickerViewController(fileTypes?.ToArray() ?? new string[] { UTType.Content, UTType.Item, "public.data" }, UIDocumentPickerMode.Open)
            {
                AllowsMultipleSelection = true,
            })
            {
                multi_tcs = new TaskCompletionSource<IReadOnlyList<IStorageFile>>();

                pvc.DidPickDocument += Pvc_DidPickDocument;
                pvc.WasCancelled += Pvc_WasCancelled;
                pvc.DidPickDocumentAtUrls += Pvc_DidPickDocumentAtUrls;

                var viewController = FilePickerExtensions.GetActiveViewController();
                viewController.PresentViewController(pvc, true, null);

                return await multi_tcs.Task;
            }
        }

        internal static async Task<IStorageFile> PickSingleFileAsync(StorageFile storageFile, string message = null)
        {
            single_tcs = new TaskCompletionSource<IStorageFile>();

            using (var pvc = new UIDocumentPickerViewController(storageFile.Url, UIDocumentPickerMode.Open)
            {
                AllowsMultipleSelection = false
            })
            {
                pvc.DidPickDocument += Pvc_DidPickDocument;
                pvc.WasCancelled += Pvc_WasCancelled;
                pvc.DidPickDocumentAtUrls += Pvc_DidPickDocumentAtUrls;

                var viewController = FilePickerExtensions.GetActiveViewController();
                viewController.PresentViewController(pvc, true, null);

                return await single_tcs.Task;
            }
        }

        /// <summary>
        /// Callback method called by document picker when file has been picked; this is called
        /// starting from iOS 11.
        /// </summary>
        /// <param name="sender">sender object (document picker)</param>
        /// <param name="args">event args</param>
        private static void Pvc_DidPickDocumentAtUrls(object sender, UIDocumentPickedAtUrlsEventArgs args)
        {
            var count = args.Urls.Count();

            if (count == 0)
                Pvc_WasCancelled(sender, null);
            else
            {
                single_tcs?.TrySetResult(new StorageFile(args.Urls[0]));
                multi_tcs?.TrySetResult(new FilePickerSelectedFilesArray(args.Urls));
            }
        }

        private static void Pvc_WasCancelled(object sender, EventArgs e)
        {
            //single_tcs?.TrySetResult(null);
            //multi_tcs?.TrySetResult(null);
            single_tcs?.TrySetCanceled();
            multi_tcs?.TrySetCanceled();
        }

        /// <summary>
        /// Callback method called by document picker when file has been picked; this is called
        /// up to iOS 10.
        /// </summary>
        /// <param name="sender">sender object (document picker)</param>
        /// <param name="args">event args</param>
        private static void Pvc_DidPickDocument(object sender, UIDocumentPickedEventArgs args)
        {
            try
            {
                /*
                var securityEnabled = args.Url.StartAccessingSecurityScopedResource();
                var doc = new UIDocument(args.Url);

                string filename = doc.LocalizedName;
                string pathname = doc.FileUrl?.Path;

                args.Url.StopAccessingSecurityScopedResource();
                if (!string.IsNullOrWhiteSpace(pathname))
                {
                    // iCloud drive can return null for LocalizedName.
                    if (filename == null && pathname != null)
                        filename = System.IO.Path.GetFileName(pathname);

                    tcs.TrySetResult(new StorageFile(pathname));
                }
                else
                    tcs.TrySetResult(null);
                */
                single_tcs?.TrySetResult(new StorageFile(args.Url));
                multi_tcs?.TrySetResult(new FilePickerSelectedFilesArray(args.Url));
            }
            catch (Exception ex)
            {
                single_tcs?.SetException(ex);
                multi_tcs?.SetException(ex);
            }
        }

    }
}
