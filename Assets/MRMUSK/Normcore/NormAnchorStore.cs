using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Normal.Realtime;
using SolerSoft.MRMUSK.Colocation;
using UnityEngine;

namespace SolerSoft.MRMUSK.Normcore
{
    public class NormAnchorStore : RealtimeComponent<NormAnchorModel>, IAnchorStore
    {
        #region Unity Inspector Variables

        [SerializeField]
        [Tooltip("Whether the anchor ID should persist across sessions.")]
        private bool _persistAcrossSessions;

        #endregion Unity Inspector Variables

        #region Private Methods

        private void NotifyAnchorIDChanged()
        { }

        private void SpatialAnchorIDChanged(NormAnchorModel model, string value)
        {
            NotifyAnchorIDChanged();
        }

        // Start is called before the first frame update
        private void Start()
        {
            model.destroyWhenLastClientLeaves = !_persistAcrossSessions;
        }

        #endregion Private Methods

        #region Protected Methods

        protected override void OnRealtimeModelReplaced(NormAnchorModel previousModel, NormAnchorModel currentModel)
        {
            if (previousModel != null)
            {
                previousModel.destroyWhenLastClientLeaves = true;
                previousModel.spatialAnchorIDDidChange -= SpatialAnchorIDChanged;
            }

            if (currentModel != null)
            {
                currentModel.destroyWhenLastClientLeaves = !_persistAcrossSessions;
                if (currentModel.isFreshModel)
                {
                    // currentModel.spatialAnchorID = currentSpatialAnchorModel;
                }
                else
                {
                    NotifyAnchorIDChanged();
                }
                currentModel.spatialAnchorIDDidChange += SpatialAnchorIDChanged;
            }
        }

        #endregion Protected Methods
        #region Public Methods

        /// <inheritdoc />
        Task<Guid> IAnchorStore.LoadAnchorIdAsync()
        {
            return Task.FromResult(Guid.Parse(model.spatialAnchorID));
        }

        /// <inheritdoc />
        Task IAnchorStore.SaveAnchorIdAsync(Guid anchorId)
        {
            model.spatialAnchorID = anchorId.ToString();
            return Task.CompletedTask;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets Whether the anchor ID should persist across sessions.
        /// </summary>
        public bool PersistAcrossSessions
        {
            get => _persistAcrossSessions;
            set
            {
                _persistAcrossSessions = value;
                model.destroyWhenLastClientLeaves = !_persistAcrossSessions;
            }
        }

        #endregion Public Properties
    }
}