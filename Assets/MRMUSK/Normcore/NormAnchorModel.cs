using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolerSoft.MRMUSK.Normcore
{
    [RealtimeModel(createMetaModel: true)]
    public partial class NormAnchorModel
    {
        #region Private Fields

        [RealtimeProperty(1, true, true)]
        private string _spatialAnchorID;

        #endregion Private Fields
    }
}