using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class Constant : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string _configPath;
    [SerializeField] private string _refVideo;

    [Header("UIText")]
    [SerializeField] private TMP_InputField _addressInputField;
    [SerializeField] private TMP_InputField _portInputField;
    [SerializeField] private TMP_InputField _videoRefInputField;

    public static string RequestPath { get; private set; }


    private string _oldAddress = "";
    private string _oldPort = "";
    private string _oldVideoRef;

    private bool _isChangedAddress = false;
    private bool _isChangedPort = false;
    private bool _isChangedVideoRef = false;

    private void Awake()
    {
        RequestPath = CreateConstant();

    }

    private void Start()
    {
        _addressInputField.onValueChanged.AddListener((v) => ChangedValueInputText(v,  _oldAddress,out _isChangedAddress));
        _portInputField.onValueChanged.AddListener((v) => ChangedValueInputText(v, _oldPort, out _isChangedPort));
    }

    private void ChangedValueInputText(string value, string old, out bool isChanged)
    {
        isChanged = value != old;

    }

    private string CreateConstant()
    {
        var configData = File.ReadAllLines(_configPath);

        var addressServer = configData.First(x => x.Contains("server_address")).Replace("server_address: ", "");

        _addressInputField.text = addressServer;

        var port = configData.First(x => x.Contains("port")).Replace("port: ", "");
        _portInputField.text = port;

        _videoRefInputField.text = _refVideo;


        return $"ws://{addressServer}:{port}/ws";
    }

    public void UpdateConstant()
    {
        if (_isChangedAddress || _isChangedPort)
        {
            var port = Convert.ToInt32(_portInputField.text) > 0 ? _portInputField.text : "1";
            File.WriteAllText(_configPath, $"server_address: {_addressInputField.text}\nport: {port}");
            RequestPath = CreateConstant();
            GetComponent<Client>().IsConnect = false;
        }
    }
    public void UpdateStartValue()
    {
        _oldAddress = _addressInputField.text;
        _oldPort = _portInputField.text;
        _oldVideoRef = _videoRefInputField.text;

        _isChangedAddress = false;
        _isChangedPort = false;
        _isChangedVideoRef = false;
    }
}
