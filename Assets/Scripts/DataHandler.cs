using UnityEngine;
using System;
using UnityEngine.Rendering;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARKit;

[Serializable]
public class BlendShape{
    public ARKitBlendShapeLocation location;
    public float coefficient;
}

[Serializable]
public class GameData 
{
    public List<BlendShape> blendshapes;
    public GameData(NativeArray<ARKitBlendShapeCoefficient> data){
        foreach(ARKitBlendShapeCoefficient blendshape in data){
            blendshapes.Add(new BlendShape{location = blendshape.blendShapeLocation, coefficient = blendshape.coefficient});
        }
    }
}

[Serializable]
public class GameSessionData{
    private List<GameData> frames = new List<GameData>();

    public void Add(GameData data){
        frames.Add(data);
    }
    public int Length => frames.Count;
}