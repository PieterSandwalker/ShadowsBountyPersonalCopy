using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System;

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
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(json);
        string encodedText = Convert.ToBase64String(bytesToEncode);
        //File.WriteAllText(Application.dataPath + "/Data/data.txt", json);
        File.WriteAllText(Application.dataPath + "/data.txt", encodedText);
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
            byte[] decodedBytes = Convert.FromBase64String(saveStr);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);
            DataJSON data = JsonUtility.FromJson<DataJSON>(decodedText);
            return data;
        }
        return null;
    }

}
