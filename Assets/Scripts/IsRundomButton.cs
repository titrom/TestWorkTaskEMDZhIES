using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsRundomButton : MonoBehaviour
{

    private Toggle _toggle;


    private bool _isRundom;
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    void Start()
    {
        Client.IsConnectEvent += (isConnect) =>
        {
            _toggle.interactable = isConnect;
        };
        Client.GetRandomStatus += (status) =>
        {
            _isRundom = status;
        };
    }

    private void LateUpdate()
    {
        _toggle.isOn = _isRundom;
    }

}
