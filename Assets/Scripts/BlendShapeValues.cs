using UnityEngine;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;
using System.Collections.Generic;
using Unity.Collections;
using TMPro;
[RequireComponent(typeof(ARFace))]
public class BlendShapeValues : MonoBehaviour
{
    public TextMeshProUGUI debugLogger = null;
    private ARKitFaceSubsystem faceSubsytem;
    private string blendLog = "";
    private Dictionary<ARKitBlendShapeLocation, int> faceIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();
    private ARFace face;
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
    
    void OnEnable(){
        var faceManager = FindObjectOfType<ARFaceManager>();
        if(faceManager != null && faceManager?.subsystem != null){
            faceSubsytem = (ARKitFaceSubsystem)faceManager?.subsystem;
        }
        UpdateVisibilty();
        face.updated += OnUpdated;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnDisable(){
        face.updated -= OnUpdated;
        ARSession.stateChanged -= OnSystemStateChanged;
    }

    void SetVisible(bool visible){
        if(debugLogger==null) return;
        debugLogger.enabled = visible;
    }
    
    void UpdateVisibilty(){
        var visible = enabled && (face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }

    public void SetValues(){
        blendLog = "";
        using (var blendShapes = faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)){
            foreach (var shape in blendShapes){
                blendLog += $"{shape.blendShapeLocation}: {shape.coefficient}\n";
            }
        }
        debugLogger.text = blendLog;
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        UpdateVisibilty();
        SetValues();
    }

    void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs){
        UpdateVisibilty();
    }
    
}
