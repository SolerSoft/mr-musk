using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SolerSoft.MRMUSK.Colocation
{
    /// <summary>
    /// The interface for a place to store anchors.
    /// </summary>
    public interface IAnchorStore
    {
        #region Public Methods

        /// <summary>
        /// Loads the stored anchor ID.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that yields the result of the operation.
        /// </returns>
        Task<Guid> LoadAnchorIdAsync();

        /// <summary>
        /// Saves the anchor ID.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that yields the result of the operation.
        /// </returns>
        Task SaveAnchorIdAsync(Guid anchorId);

        #endregion Public Methods
    }
}