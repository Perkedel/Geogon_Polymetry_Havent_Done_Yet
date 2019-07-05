using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class SQLiteTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string conn = "URI=file:" + Application.dataPath + "/SQL/PickAndPlaceDatabase.s3db.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();





        string sqlQuery = "SELECT value,name, randomSequence " + "FROM PlaceSequence";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();


        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            string name = reader.GetString(1);
            int rand = reader.GetInt32(2);

            Debug.Log("value= " + value + "  name =" + name + "  random =" + rand);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public int valueing;
    public string naming;
    public int randing;

    public void setValueing(string value)
    {
        valueing = int.Parse(value); //forum unity
    }
    public void setNaming(string name)
    {
        naming = name;
    }

    public void InsertScoring()
    {
        randing = UnityEngine.Random.Range(0,1000000); ;
        string conn = "URI=file:" + Application.dataPath + "/SQL/PickAndPlaceDatabase.s3db.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "Insert into PlaceSequence (value, name, randomSequence)" + "Values (" + valueing +",'"+naming+"',"+randing+");";
        dbcmd.CommandText = sqlQuery;
        
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            //
            //int value = reader.GetInt32(0);
            //string name = reader.GetString(1);
            //int rand = reader.GetInt32(2);

            //Debug.Log("value= " + value + "  name =" + name + "  random =" + rand);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
