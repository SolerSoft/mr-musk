using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class SpatialAnchorSync : RealtimeComponent<SpatialAnchorModel>
{
    private string currentSpatialAnchorModel = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void SpatialAnchorIDChanged(SpatialAnchorModel model, string value)
    {
        UpdateSpatialAnchor();
    }

    private void UpdateSpatialAnchor()
    {
        currentSpatialAnchorModel = model.spatialAnchorID;
    }

    public void SetSpatialAnchorID(string id)
    {
        model.spatialAnchorID = id;
    }
}
