using System.Runtime.InteropServices;
using UnityEngine;

public class ReactCommunicate : PersistentSingleton<ReactCommunicate>
{   
    [DllImport("__Internal")]
    private static extern void Ready ();

    private void Start() {
        Ready();
    }

    private FakeAPIData APIData;
    public void SetData(string jsonData) {
       APIData = JsonUtility.FromJson<FakeAPIData>(jsonData);
       GameManager.Instance.SetDebugLog(@$"
       id: {APIData.id}
       dataNum: {APIData.dataNum}
       ");
    }

    public FakeAPIData GetData() {
        return APIData;
    }
}

public class FakeAPIData
{
    public string id;
    public int dataNum;
}
