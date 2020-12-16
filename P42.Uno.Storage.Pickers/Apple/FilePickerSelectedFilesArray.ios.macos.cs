using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P42.Uno.SandboxedStorage
{
    class FilePickerSelectedFilesArray : List<IStorageFile>, IReadOnlyList<IStorageFile>
    {
        public FilePickerSelectedFilesArray(NSUrl[] urls)
        {
            foreach (var url in urls)
                Add(new StorageFile(url, true));
        }

        public FilePickerSelectedFilesArray(NSUrl url)
        {
            Add(new StorageFile(url, true));
        }

    }
}
