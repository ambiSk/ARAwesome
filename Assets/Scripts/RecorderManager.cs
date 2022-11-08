
using System.Collections.Generic;
using TMPro;
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
    // Unity Functions
    // - Awake: Get ARFace object
    // - OnEnable : Get Face Manager -> Get ARKit Subsytem -> UpdateVisibility -> OnUpdated
    // - OnDisable : call OnUpdated

    // Custom Functions
    // - SetVisible : Set visibility of TMPRo
    // - SetValues : Set values on TMPro
    // - UpdateVisibilty : [Boolean]Check for faces at each frame, calls SetVisible based on boolean
    // - OnUpdated : Calls UpdateVisibilty
    // - OnSystemStateChanged: Handle ARSessions Event args

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
            }
            frames = null;
        }
    }

    void OnEnable(){
        var faceManager = FindObjectOfType<ARFaceManager>();
        
        if(faceManager != null && faceManager?.subsystem != null){
            faceSubsytem = (ARKitFaceSubsystem)faceManager?.subsystem;
        }
        // UpdateEnabilty();
        face.updated += OnEnabled;
        // ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnEnabled(ARFaceUpdatedEventArgs eventArgs){
        if(!recording) return;
        GameData data =  new GameData(faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp));
        frames.Add(data);
    }
    void OnDisable(){
        
        face.updated -= OnEnabled;
        // ARSession.stateChanged -= OnSystemStateChanged;
    }

    // void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs){
    //     UpdateVisibilty();
    // }
    
}
