using Newtonsoft.Json;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;

public class RecorderManager : MonoBehaviour
{
    private GameSessionData frames;
    private ARKitFaceSubsystem faceSubsytem;
    private ARFace face;
    private bool recording;    
    
    void Awake(){
        face = GetComponent<ARFace>();
    }
    
    void flagRecording(){
        recording = !recording;
        if(recording){
            frames = new GameSessionData();
        }
        else{
            if(frames.Count > 0){
                // Serialize and save
                SerializeSave.SimpleWrite(frames, "/file.json");
            }
            frames = null;
        }
    }

    void OnEnable(){
        var faceManager = FindObjectOfType<ARFaceManager>();
        
        if(faceManager != null && faceManager?.subsystem != null){
            faceSubsytem = (ARKitFaceSubsystem)faceManager?.subsystem;
        }
        
        face.updated += OnEnabled;
    }

    void OnEnabled(ARFaceUpdatedEventArgs eventArgs){
        if(!recording) return;
        GameData data =  new GameData(faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp));
        frames.Add(data);
    }
    void OnDisable(){
        face.updated -= OnEnabled;
    }

    
}
