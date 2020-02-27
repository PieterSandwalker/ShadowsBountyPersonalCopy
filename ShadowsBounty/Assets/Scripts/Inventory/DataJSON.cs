using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DataJSON
{

    public int bounty;
    public List<int> itemNum;

    public DataJSON()
    {

    }

    public DataJSON(int bounty)
    {
        this.bounty = bounty;
        itemNum = new List<int>
        {
            0,
            0,
            0
        };
    }

    /**
    * save the data json
    */
    public static void Save(string json)
    {
        File.WriteAllText(Application.dataPath + "/Data/data.txt", json);
    }

    /**
    * save the data json
    */
    public static void Save(DataJSON datajson)
    {
        string json = JsonUtility.ToJson(datajson);
        Save(json);
    }

    /**
     * load json data
     */
    public static DataJSON Load()
    {
        if (File.Exists(Application.dataPath + "/Data/data.txt"))
        {
            string saveStr = File.ReadAllText(Application.dataPath + "/Data/data.txt");
            DataJSON data = JsonUtility.FromJson<DataJSON>(saveStr);
            return data;
        }
        return null;
    }

}
