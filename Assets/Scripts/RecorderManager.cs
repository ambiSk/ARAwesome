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
    private Flag flag;
    void Awake(){
        flag = new Flag();
        face = GetComponent<ARFace>();
    }
    
    public void flagRecording(){
        flag.Recording = !flag.Recording;
        if(flag.Recording){
            Debug.Log($"Recording started {flag.Recording.ToString()}");
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
        
        bool temp = flag.Recording;
        Debug.Log($"Recording set to {temp.ToString()}");
        face.updated += OnEnabled;
    }

    void OnDisable(){
        face.updated -= OnEnabled;
        // ARSession.stateChanged -= OnSystemStateChanged;
    }
    
    
    
    void OnEnabled(ARFaceUpdatedEventArgs eventArgs){
        bool temp = flag.Recording;
        
        if(temp){
            using (var blendShapes = faceSubsytem.GetBlendShapeCoefficients(face.trackableId, Allocator.Temp)){
                // UpdateVisibilty();
                // SetValues();
                GameData data =  new GameData(blendShapes);
                frames.AddGameData(data);
            }
            
        }
    }
    
}
