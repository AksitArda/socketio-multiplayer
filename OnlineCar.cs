using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineCar : MonoBehaviour
{
    NetworkManager networkManager;
    Gamemanager gameManager;

    public TextMesh usernameText;
    void Start()
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<Gamemanager>();
    }

    
    void Update()
    {
        OnlineToLocal();
    }
    
    void OnlineToLocal()
    {
        try
        {
            if (networkManager.username != networkManager.serverPackage.username)
            {
                usernameText.gameObject.SetActive(true);
                usernameText.text = networkManager.serverPackage.username;
                transform.position = Vector3.Lerp(transform.position, new Vector3(networkManager.serverPackage.xPosition, networkManager.serverPackage.yPosition, networkManager.serverPackage.zPosition), 10 * Time.deltaTime);
                transform.rotation = Quaternion.Euler(networkManager.serverPackage.xRotation, networkManager.serverPackage.yRotation, networkManager.serverPackage.zRotation);
            }
        }
        catch
        {
            Debug.Log("Server Error");
        }
    }
}