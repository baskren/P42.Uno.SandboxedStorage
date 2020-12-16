﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using P42.Uno.SandboxedStorage;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Syndication;

namespace P42.Uno.SandboxedStorage
{
    class StorageFile : StorageItem, IStorageFile
    {
        #region Properties
        public string ContentType => _file?.ContentType;

        public string FileType => _file?.FileType;

        internal Windows.Storage.StorageFile _file
        {
            get => _item as Windows.Storage.StorageFile;
            set => _item = value;
        }
        #endregion


        #region Construction
        public StorageFile(Windows.Storage.StorageFile file, bool bookmark = false) : base(file, bookmark)
        {
        }
        #endregion


        #region Type Conversion Operators
        public static implicit operator StorageFile(Windows.Storage.StorageFile f)
            => f is null
                ? null
                : new StorageFile(f);

        public static implicit operator Windows.Storage.StorageFile(StorageFile f)
            => f._file;
        #endregion


        #region Methods
        public override bool IsOfType(StorageItemTypes type)
            => type == StorageItemTypes.File;

        public static async Task<IStorageFile> GetFileFromPathAsync(string path)
        {
            await Task.Delay(5).ConfigureAwait(false);

            try
            {
                if (await Windows.Storage.StorageFile.GetFileFromPathAsync(path) is Windows.Storage.StorageFile windowsFile)
                    return new StorageFile(windowsFile);
            }
            catch (Exception)
            {
                if (await RequestAccessAsync<StorageFile>(path) is StorageFile storageFile)
                    return storageFile;
            }
            return null;
        }

        public async Task CopyAndReplaceAsync(IStorageFile iStorageFileToReplace)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                iStorageFileToReplace is StorageFile storageFileToReplace &&
                storageFileToReplace._file is Windows.Storage.StorageFile fileToReplace)
            {
                await TryNativeMethodAsync(this, async (f) => await f._file.CopyAndReplaceAsync(fileToReplace));
            }
        }

        public async Task<IStorageFile> CopyAsync(IStorageFolder iDestinationFolder)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                iDestinationFolder is StorageFolder destinationFolder &&
                destinationFolder?._folder is Windows.Storage.StorageFolder windowsDestinationFolder)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._file.CopyAsync(windowsDestinationFolder));
                if (result is Windows.Storage.StorageFile newFile)
                    return new StorageFile(newFile);
            }
            return null;
        }

        public async Task<IStorageFile> CopyAsync(IStorageFolder iDestinationFolder, string desiredNewName)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                iDestinationFolder is StorageFolder destinationFolder &&
                destinationFolder?._folder is Windows.Storage.StorageFolder windowsDestinationFolder &&
                !string.IsNullOrWhiteSpace(desiredNewName))
            //&& await _file.CopyAsync(windowsDestinationFolder, desiredNewName) is Windows.Storage.StorageFile newFile)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._file.CopyAsync(windowsDestinationFolder, desiredNewName));
                if (result is Windows.Storage.StorageFile newFile)
                    return new StorageFile(newFile);
            }
            return null;
        }

        public override async Task<IStorageFolder> GetParentAsync()
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file is null)
                return null;
            if (_file != null)// && await _file.GetParentAsync() is Windows.Storage.StorageFolder folder)
            {
                var result = await TryNativeMethodAsync(this, async (f) => await f._file.GetParentAsync());
                if (result is Windows.Storage.StorageFolder folder)
                    return new StorageFolder(folder);
            }
            return null;
        }

        public async Task MoveAndReplaceAsync(IStorageFile iFileToReplace)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                iFileToReplace is StorageFile fileToReplace &&
                fileToReplace._file is Windows.Storage.StorageFile windowsFileToReplace)
            {
                await TryNativeMethodAsync(this, async (f) => await f._file.MoveAndReplaceAsync(windowsFileToReplace));
            }
        }

        public async Task MoveAsync(IStorageFolder iDestinationFolder)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                iDestinationFolder is StorageFolder destinationFolder &&
                destinationFolder._folder is Windows.Storage.StorageFolder windowsDestintationFolder)
            {
                await TryNativeMethodAsync(this, async (f) => await f._file.MoveAsync(windowsDestintationFolder));
            }
        }

        public async Task MoveAsync(IStorageFolder iDestinationFolder, string desiredNewName)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null &&
                !string.IsNullOrWhiteSpace(desiredNewName) &&
                iDestinationFolder is StorageFolder destinationFolder &&
                destinationFolder._folder is Windows.Storage.StorageFolder windowsDestintationFolder)
            {
                await TryNativeMethodAsync(this, async (f) => await f._file.MoveAsync(windowsDestintationFolder, desiredNewName));
            }
        }

        public async Task RenameAsync(string desiredName)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null && !string.IsNullOrWhiteSpace(desiredName))
            {
                await TryNativeMethodAsync(this, async (f) => await f._file.RenameAsync(desiredName));
            }
        }

        public async Task RenameAsync(string desiredName, NameCollisionOption option)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (_file != null && !string.IsNullOrWhiteSpace(desiredName))
                await TryNativeMethodAsync(this, async (f) => await f._file.RenameAsync(desiredName, (Windows.Storage.NameCollisionOption)option));
        }
        #endregion


        #region System.IO.File methods
        public async Task AppendAllLinesAsync(IEnumerable<string> lines, System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (await TryGetEncodingAsync() is Windows.Storage.Streams.UnicodeEncoding encoding)
                await TryNativeMethodAsync(this, cancellationToken, async (f, t) => await FileIO.AppendLinesAsync(f._file, lines, encoding).AsTask(t));
        }

        public async Task AppendAllTextAsync(string contents, System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (await TryGetEncodingAsync() is Windows.Storage.Streams.UnicodeEncoding encoding)
                await TryNativeMethodAsync(this, cancellationToken, async (f, t) => await FileIO.AppendTextAsync(_file, contents, encoding).AsTask(t));
        }

        public async Task<byte[]> ReadAllBytesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);

            var result = await TryNativeMethodAsync(this, cancellationToken, async (f,t) => await FileIO.ReadBufferAsync(_file).AsTask(t));
            if (result is Windows.Storage.Streams.IBuffer buffer)
                return buffer?.ToArray();
            return null;
        }

        public async Task<string[]> ReadAllLinesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (await TryNativeMethodAsync(this, async (f) => await f._file.OpenStreamForReadAsync()) is Stream stream)
            {
                var lines = new List<string>();
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        if (reader.ReadLine() is string line)
                            lines.Add(line);
                    }
                }
                stream.Close();
                stream.Dispose();
                return lines.ToArray();
            }

            return null;
        }

        public async Task<string> ReadAllTextAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);

            if (await TryNativeMethodAsync(this, async (f) => await f._file.OpenStreamForReadAsync()) is Stream stream)
            {
                string text = null;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                stream.Close();
                stream.Dispose();
                return text;
            }
            return null;
        }

        public async Task WriteAllBytesAsync(byte[] bytes, System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);
            await TryNativeMethodAsync(this, cancellationToken, async (f, t) => await FileIO.WriteBytesAsync(_file, bytes).AsTask(t));
        }

        public async Task WriteAllLinesAsync(IEnumerable<string> lines, System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);
            await TryNativeMethodAsync(this, cancellationToken, async (f, t) => await FileIO.WriteLinesAsync(_file, lines, Windows.Storage.Streams.UnicodeEncoding.Utf8).AsTask(t));
        }

        public async Task WriteAllTextAsync(string content, System.Threading.CancellationToken cancellationToken = default)
        {
            await Task.Delay(5).ConfigureAwait(false);
            await TryNativeMethodAsync(this, cancellationToken, async (f, t) => await FileIO.WriteTextAsync(_file, content, Windows.Storage.Streams.UnicodeEncoding.Utf8).AsTask(t));
        }

        #endregion


        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <returns></returns>
        async Task<Windows.Storage.Streams.UnicodeEncoding?> TryGetEncodingAsync()
        {
            // Read the BOM
            var bom = new byte[4];
            if ( await TryNativeMethodAsync(this, async (f) => await f._file.OpenStreamForReadAsync()) is Stream stream)
            {
                stream.Read(bom, 0, 4);
                stream.Close();
                stream.Dispose();

                // Analyze the BOM
                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Windows.Storage.Streams.UnicodeEncoding.Utf8;
                if (bom[0] == 0xff && bom[1] == 0xfe) return Windows.Storage.Streams.UnicodeEncoding.Utf16LE;
                if (bom[0] == 0xfe && bom[1] == 0xff) return Windows.Storage.Streams.UnicodeEncoding.Utf16BE;
                // We actually have no idea what the encoding is if we reach this point, so
                // you may wish to return null instead of defaulting to ASCII

                var contentDialog = new ContentDialog
                {
                    Title = "Incompatible Encoding",
                    Content = "The encoding of the file [" + Path + "] is not compatible with Windows UWP Apps.  SUGGESTION: Use a tool like Notepad++ to convert this file to Utf8 or Utf16",
                    CloseButtonText = "OK"
                };

                await contentDialog.ShowAsync();

                return null;
            }
            return null;
        }


    }
}
