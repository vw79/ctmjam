using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;

[FirestoreData]

public struct userID{
    [FirestoreProperty]
    public string Id {get;set;}
}

[System.Serializable]

public class Skins{

    public string skinName;
    public bool IsOwned = false;

    public bool IsEquipped = false;
}

public class DBManager : MonoBehaviour
{
    [Header("DeviceID")]
    private string DeviceID;

    [Header("Firebase")]
    private FirebaseFirestore db;
    public string DBCollectionName = "PlayerDB";
    public string DBDocumentName = "PlayerID";

    [Header("Skin")]
    public Skins[] ListofSkins;



    private void Start()
    { 
        db = FirebaseFirestore.DefaultInstance;
        DeviceID = SystemInfo.deviceUniqueIdentifier;
        checkID().ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                bool isDeviceIDValid = task.Result;
                Debug.Log("Is Device ID valid: " + isDeviceIDValid);
                if(!isDeviceIDValid){
                    AddData();
                }
            }
            else
            {
                Debug.LogError("Error checking device ID: " + task.Exception);
            }
        });
    }

    private async Task<bool> checkID()
    {
        DocumentReference docRef = db.Collection(DBCollectionName).Document(DBDocumentName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ContainsField(DeviceID);
        }
        else
        {
            Debug.Log("PlayerID document does not exist.");
            return false;
        }
    }

    private void AddData(){
        DocumentReference docRef = db.Collection("PlayerDB").Document("PlayerID");
        Dictionary<string, object> docData = new Dictionary<string, object>
        {
                { "ID", DeviceID },
        };
        docRef.SetAsync(docData);

        DocumentReference skinDocRef = db.Collection("SkinDB").Document(DeviceID);
        DocumentReference scoreDocRef = db.Collection("ScoreDB").Document(DeviceID);

        List<Dictionary<string, object>> skinsData = new List<Dictionary<string, object>>();
        foreach (Skins skin in ListofSkins)
        {
            Dictionary<string, object> skinDict = new Dictionary<string, object>
            {
                { "skinName", skin.skinName },
                { "IsOwned", skin.IsOwned },
                { "IsEquipped", skin.IsEquipped }
            };
            skinsData.Add(skinDict);
        }

         // Prepare the data to be added to Firestore
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skins", skinsData }
        };


        Dictionary<string, object> scoreData = new Dictionary<string, object>
        {
            { "highScore", 0 } 
        };

         skinDocRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Successfully added document to skinDB collection.");
            }
            else
            {
                Debug.LogError("Failed to add document to skinDB collection: " + task.Exception);
            }
        });

        scoreDocRef.SetAsync(scoreData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Successfully added document to ScoreDB collection.");
            }
            else
            {
                Debug.LogError("Failed to add document to ScoreDB collection: " + task.Exception);
            }
        });
    }

}
