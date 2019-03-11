using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Data;
using System.IO.Ports;
using System.IO.Compression;
using UnityEngine;

//https://www.youtube.com/watch?v=of_oLAvWfSI Arduino Interact to Unity
//https://stackoverflow.com/questions/54677130/the-type-or-namespace-name-ports-does-not-exist-in-the-namespace-system-io/54679189#54679189
//Workaround:
//1. Switch Platform away from PC if you are on PC
//2. To any platform
//3. Then go back to PC Switch platform
//4. Serial Port Library will appear!!!

public class ArduinoControlled : MonoBehaviour
{
    public float Speed = 100f;

    public string SerialPortName = "COM10"; //Arduino COM port connects to where?
    public int SerialPortBaudRate = 9600; //baud rate of this serial port

    SerialPort sp;

    [SerializeField] private float amountToMove;

    // Start is called before the first frame update
    void Start()
    {
        sp = new SerialPort(SerialPortName, SerialPortBaudRate);
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        amountToMove = Speed * Time.deltaTime;

        if (sp.IsOpen)
        {
            try
            {
                MoveObject(sp.ReadByte());
                Debug.Log(sp.ReadByte());
            }
            catch (System.Exception)
            {
                Debug.LogError("Error! Serial Port " + SerialPortName + " Not Exists!\n Did you connect properly? Or wrong COM port??");
            }
        }
    }

    void MoveObject(int Direction)
    {
        if(Direction == 7) //up
        {
            //transform.translate(Vector3.forward * amountToMove, Space.World); //Use Rigidbody version instead!
            GetComponent<Rigidbody>().AddForce(Vector3.forward * Speed);
        }
        if (Direction == 6) //left
        {
            GetComponent<Rigidbody>().AddForce(Vector3.left * Speed);
        }
        if (Direction == 5) //right
        {
            GetComponent<Rigidbody>().AddForce(Vector3.right * Speed);
        }
        if (Direction == 4) //down
        {
            GetComponent<Rigidbody>().AddForce(Vector3.back * Speed);
        }
    }
}
