using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorIDSyncTest : MonoBehaviour
{
    [SerializeField] private string _spatialAnchorID = default;
    
    [SerializeField] private string _prevSpatialAnchorID = default;

    private SpatialAnchorSync _spatialAnchorSync;

    private void Awake()
    {
        _spatialAnchorSync = GetComponent<SpatialAnchorSync>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_spatialAnchorID != _prevSpatialAnchorID)
        {
            _spatialAnchorSync.SetSpatialAnchorID(_spatialAnchorID);
            _prevSpatialAnchorID = _spatialAnchorID;
        }   
    }
}
