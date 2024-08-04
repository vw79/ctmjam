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

public class DBManager : MonoBehaviour
{
    private string DeviceID;
    private FirebaseFirestore db;
    public string DBCollectionName = "PlayerDB";
    public string DBDocumentName = "PlayerID";

    public TextMeshProUGUI text;
    private bool isExists;

    ListenerRegistration listenerRegistration;

    
    private void Start()
    { 
        db = FirebaseFirestore.DefaultInstance;
        DeviceID = SystemInfo.deviceUniqueIdentifier;
        isExists = checkID().Result;
        Debug.Log("Is Device ID valid: " + isExists);
    }

    private async Task<bool> checkID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;

        // Create a reference to the collection
        CollectionReference collectionRef = db.Collection(DBCollectionName);

        // Query the collection for a document with the matching device ID
        Query query = collectionRef.WhereEqualTo("deviceID", deviceID);

        // Execute the query and get the results
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        // Check if any documents match the query
        if (querySnapshot.Count > 0)
        {
            // Device ID found in Firestore
            return true;
        }
        else
        {
            // Device ID not found in Firestore
            return false;
        }
    }
}
