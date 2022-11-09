using UnityEngine;
using System;
using UnityEngine.Rendering;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARKit;

[Serializable]
public class BlendShape{
    public string location;
    public float coefficient;
}

[Serializable]
public class GameData 
{
    public List<BlendShape> blendshapes;
    public GameData(NativeArray<ARKitBlendShapeCoefficient> data){
        blendshapes = new List<BlendShape>();
        foreach(ARKitBlendShapeCoefficient blendshape in data){
            // Debug.Log($"{blendshape.blendShapeLocation}: {blendshape.coefficient}\n");
            blendshapes.Add(new BlendShape{location = blendshape.blendShapeLocation.ToString(), coefficient = blendshape.coefficient});
        }
    }
}

[Serializable]
public class GameSessionData{
    public List<GameData> frames;

    public GameSessionData(){
        frames = new List<GameData>();
    }
    public void AddGameData(GameData data){
        Debug.Log("Added");
        frames.Add(data);
    }
    public int Length => frames.Count;
}
public class Flag{
    private bool recording;  

    public Flag(){
        recording=false;
    }  
    
    public bool Recording{
        get { return recording; }   // get method
        set { recording = value; }  // set method
  }
}