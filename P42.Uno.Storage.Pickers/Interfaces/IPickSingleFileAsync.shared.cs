﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P42.Uno.SandboxedStorage
{
    interface IPickSingleFileAsync
    {
        Task<IStorageFile> PickSingleFileAsync(string pickerOperationId = null);
    }
}
