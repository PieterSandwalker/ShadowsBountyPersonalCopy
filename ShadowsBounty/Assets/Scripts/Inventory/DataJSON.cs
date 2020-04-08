using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DataJSON
{

    public int bounty;
    public int playerRole;
    public List<int> item;
    public List<int> status;
    //public List<int> trapNum;

    public DataJSON()
    {

    }

    public DataJSON(int bounty)
    {
        this.bounty = bounty;
        this.playerRole = 0;
        item = new List<int>
        {
            0,
            0,
            0,
            0
        };
        status = new List<int>
        {
            0,
            0,
            0
        };
        /*trapNum = new List<int>
        {
            0,
            0,
            0,
            0,
            0,
            0
        };*/
    }

    /**
    * save the data json
    */
    public static void Save(string json)
    {
        //File.WriteAllText(Application.dataPath + "/Data/data.txt", json);
        File.WriteAllText(Application.dataPath + "/data.txt", json);
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
        //File.Exists(Application.dataPath + "/Data/data.txt"
        if (File.Exists(Application.dataPath + "/data.txt"))
        {
            string saveStr = File.ReadAllText(Application.dataPath + "/data.txt");
            DataJSON data = JsonUtility.FromJson<DataJSON>(saveStr);
            return data;
        }
        return null;
    }

}
