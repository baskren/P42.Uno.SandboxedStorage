﻿//-----------------------------------------------------------------------
// <copyright file="StorageDeleteOption.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Refactored for cross platform .NetStandard library structure in 2020 by 42ndParallel.

namespace P42.Uno.SandboxedStorage
{
    /// <summary>
    /// Specifies whether a deleted item is moved to the Recycle Bin or permanently deleted.
    /// </summary>
    public enum StorageDeleteOption
    {
        /// <summary>
        /// Specifies the default behavior.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Permanently deletes the item.
        /// The item is not moved to the Recycle Bin.
        /// </summary>
        PermanentDelete = 1,
    }
}