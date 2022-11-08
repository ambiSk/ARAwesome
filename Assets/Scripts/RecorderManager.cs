using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using System;
[RequireComponent(typeof(ARFace))]
public class RecorderManager : MonoBehaviour
{
    private GameSessionData frames;
    private ARKitFaceSubsystem faceSubsytem;
    private ARFace face;
    private bool recording=false;    
    
    void Awake(){
        face = GetComponent<ARFace>();
    }
    
    public void flagRecording(){
        recording = !recording;
        if(recording){
            frames = new GameSessionData();
        }
        else{
            if(frames.Length > 0){
                // Serialize and save
                String filename = DateTime.Now.ToString();
                SerializeSave.SimpleWrite(frames, Application.persistentDataPath +"/file-"+filename+".json");
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
        // ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnEnabled(ARFaceUpdatedEventArgs eventArgs){
        if(recording){
            GameData data =  new GameData(faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp));
            frames.Add(data);
        }
    }
    void OnDisable(){
        face.updated -= OnEnabled;
        // ARSession.stateChanged -= OnSystemStateChanged;
    }
    // void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs){
    //     if(recording) Debug.Log(frames.Length);
    // }
    
}
