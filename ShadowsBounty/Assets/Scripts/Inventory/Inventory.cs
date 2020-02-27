using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasKey;

    Dictionary<string, int> itemList;

    void Start()
    {
        hasKey = false;


        itemList = new Dictionary<string, int>();
        DataJSON data = new DataJSON(100);
        //data.bounty = PlayerPrefs.GetInt("score");
        List<int> list = new List<int>();
        list.Add(0);
        list.Add(2);
        string json = JsonUtility.ToJson(data);
        Debug.Log(Application.dataPath);
        DataJSON.Save(json);
        //DataJSON list2 = JsonUtility.FromJson<DataJSON>(DataJSON.Load());
    }

    public void getKey()
    {
        hasKey = true;
    }

}
