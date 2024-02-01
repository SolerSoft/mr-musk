using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class SpatialAnchorSync : RealtimeComponent<SpatialAnchorModel>
{
    #region Public Fields
    public string currentSpatialAnchorModel = "";
    #endregion Public Fields
    #region Private Methods

    private void SpatialAnchorIDChanged(SpatialAnchorModel model, string value)
    {
        UpdateSpatialAnchor();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void UpdateSpatialAnchor()
    {
        currentSpatialAnchorModel = model.spatialAnchorID;
    }

    #endregion Private Methods

    #region Protected Methods

    protected override void OnRealtimeModelReplaced(SpatialAnchorModel previousModel, SpatialAnchorModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.spatialAnchorIDDidChange -= SpatialAnchorIDChanged;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.spatialAnchorID = currentSpatialAnchorModel;
            }
            UpdateSpatialAnchor();
            currentModel.spatialAnchorIDDidChange += SpatialAnchorIDChanged;
        }
    }

    #endregion Protected Methods
    #region Public Methods

    public void SetSpatialAnchorID(string id)
    {
        model.spatialAnchorID = id;
    }

    #endregion Public Methods
}