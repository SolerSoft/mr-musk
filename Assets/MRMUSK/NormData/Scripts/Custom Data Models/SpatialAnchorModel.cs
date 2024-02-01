using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RealtimeModel(createMetaModel:true)]
public partial class SpatialAnchorModel
{
    [RealtimeProperty(1,true,true)]
    private string _spatialAnchorID;
}
