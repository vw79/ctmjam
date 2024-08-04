using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.UI;

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
    private bool isExists;

    ListenerRegistration listenerRegistration;

    
    private void Start()
    { 
        db = FirebaseFirestore.DefaultInstance;
        DeviceID = SystemInfo.deviceUniqueIdentifier;
        CheckID();
    }

    private void CheckID(){
        listenerRegistration =  db.Collection(DBCollectionName).Document(DBDocumentName).Listen(snapshot =>{
        if (snapshot.Exists)
            {
                Dictionary<string, object> documentData = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in documentData)
                {
                    Debug.Log($"{pair.Key}: {pair.Value}");
                }
            }
            else
            {
                Debug.Log("Document does not exist.");
            }
        
        });
    }
    
    private void OnDestroy() {
        listenerRegistration.Stop();
    }

}
