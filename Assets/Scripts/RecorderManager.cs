using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using System;
using TMPro;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class RecorderManager : MonoBehaviour
{   private string blendLog = "";
    public TextMeshProUGUI debugLogger = null;
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
            Debug.Log($"Recording started + {recording.ToString()}");
            frames = new GameSessionData();
        }
        else{
            int frameLength = frames.Length;
            Debug.Log(frameLength.ToString()+" Captured");
            
            Debug.Log("Recording stopped");
            if(frameLength > 0){
                // Serialize and save
                String filename = DateTime.Now.ToString();
                SerializeSave.SimpleWrite(frames, Application.persistentDataPath +"/file-"+filename+".json");
                Debug.Log(frameLength.ToString()+" Captured and saved to"+ Application.persistentDataPath +"/file-"+filename+".json");

            }
            frames = null;

        }
    }

    void OnEnable(){
        var faceManager = FindObjectOfType<ARFaceManager>();
        
        if(faceManager != null && faceManager?.subsystem != null){
            faceSubsytem = (ARKitFaceSubsystem)faceManager?.subsystem;
        }
        UpdateVisibilty();
        face.updated += OnEnabled;
        ARSession.stateChanged += OnSystemStateChanged;
    }

    void OnDisable(){
        face.updated -= OnEnabled;
        ARSession.stateChanged -= OnSystemStateChanged;
    }
    public void SetValues(){
        blendLog = "";
        using (var blendShapes = faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)){
            foreach (var shape in blendShapes){
                Debug.Log($"{shape.blendShapeLocation}: {shape.coefficient}\n");
                blendLog += $"{shape.blendShapeLocation}: {shape.coefficient}\n";
            }
        }
        debugLogger.text = blendLog;
    }
    void UpdateVisibilty(){
        var visible = enabled && recording && (face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);
        SetVisible(visible);
    }
    void SetVisible(bool visible){
        if(debugLogger==null) return;
        debugLogger.enabled = visible;
    }
    void OnEnabled(ARFaceUpdatedEventArgs eventArgs){
        UpdateVisibilty();
        SetValues();
        if(recording){
            using (var blendShapes = faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)){
                GameData data =  new GameData(blendShapes);
                frames.AddGameData(data);
            }
            
        }
    }
    void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs){
        UpdateVisibilty();
    }
}
