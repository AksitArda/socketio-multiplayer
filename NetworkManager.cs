using System;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private SocketIOUnity socket;
    [SerializeField]private float tickrate;
    [SerializeField]private string serverip = "http://localhost:3000/";
    [SerializeField]private GameObject localCar;

    [HideInInspector] public sPackage serverPackage;
    public string username;

    float ticker;
    Gamemanager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager =  GameObject.FindWithTag("GameController").GetComponent<Gamemanager>();

        var uri = new Uri(serverip);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
        {
            {"token", "UNITY"}
        }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        socket.Connect();
        
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
    }
    
    void Update()
    {
        socket.OnUnityThread("updatePosition", (data) =>
        {
            serverPackage = data.GetValue<sPackage>();
        });
    }

    public void Emit(string eventName,string data)
    {
        socket.Emit(eventName, data);
    }

    public void Broadcast(string eventName, object data)
    {
        ticker += Time.deltaTime;

        if (ticker >= (6/tickrate))
        {
            socket.Emit(eventName, data);
            ticker = 0;
        }
    }

    public void MoveCar(Transform transform)
    {
        sPackage spackage = new sPackage
        {
            username = username,

            xPosition = Mathf.Round(transform.position.x * 1000.0f) / 1000.0f,
            yPosition = Mathf.Round(transform.position.y * 1000.0f) / 1000.0f,
            zPosition = Mathf.Round(transform.position.z * 1000.0f) / 1000.0f,

            xRotation = Mathf.Round(transform.rotation.eulerAngles.x * 1000.0f) / 1000.0f,
            yRotation = Mathf.Round(transform.rotation.eulerAngles.y * 1000.0f) / 1000.0f,
            zRotation = Mathf.Round(transform.rotation.eulerAngles.z * 1000.0f) / 1000.0f,
        };

        Broadcast("updatePosition", spackage);
    }
}

public class sPackage
{
    public string username { get; set; }

    public float xPosition { get; set; }
    public float yPosition { get; set; }
    public float zPosition { get; set; }
    public float xRotation { get; set; }
    public float yRotation { get; set; }
    public float zRotation { get; set; }
}