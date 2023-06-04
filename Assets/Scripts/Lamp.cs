using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{

    [SerializeField] private GameObject _errorMessage;

    private void Start()
    {
        Client.IsConnectEvent += (isConnect) =>
        {
            GetComponent<RawImage>().color = isConnect ? Color.green : Color.red;
            _errorMessage.SetActive(!isConnect);
        };
    }
}
