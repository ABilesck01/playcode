using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/New Graphic List")]
public class EventGraphicsAsset : ScriptableObject
{
    [System.Serializable]
    public class GraphicData
    {
        public string name;
        public Sprite gfx;
    }

    public List<GraphicData> allGraphics;
}
