using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

enum Operation
{
    getCurrentOdometer,
    getRandomStatus
}

public class Client : MonoBehaviour
{
    [SerializeField] private float _getOdometerDelay = 2f;

    public static Action<bool> IsConnectEvent;

    public static Action<float> GetOdometer;

    public static Action<bool> GetRandomStatus;


    private WebSocket _ws;

    private bool _isConnect = true;

    public bool IsConnect
    {
        set
        {
            if (value != _isConnect)
            {
                _isConnect = value;
                if (!_isConnect)
                {

                    StopAllCoroutines();
                    _ws.OnMessage -= OnMessage;
                    StartCoroutine(Reconnect());
                }

            }
        }
    }
    private IEnumerator Reconnect()
    {
        while (!_isConnect)
        {
            _ws.Close();
            yield return new WaitForSeconds(10f);
            OnConnect();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Start()
    {
        OnConnect();
        RequestToServer(Operation.getRandomStatus);
    }


    public void OnConnect()
    {
        _ws = new WebSocket(Constant.RequestPath);
        _ws.OnOpen += (sender, e) =>
        {
            IsConnect = true;
            StopAllCoroutines();
            StartCoroutine(GetOdometerEnumerator());
        };

        _ws.OnMessage += OnMessage;

        _ws.OnError += (s, e) => 
        {
            IsConnect = false;
            print(e.Message);
        };
        _ws.OnClose += (s, e) =>
        {
            IsConnect = false;
            Debug.Log("OnClose");
        };

        _ws.Connect();
    }

    private void Update()
    {
        IsConnect = _ws.IsAlive;
        IsConnectEvent?.Invoke(_isConnect);
    }

    public void UpdateRandomStatus()
    {
        RequestToServer(Operation.getRandomStatus);
    }

    private void RequestToServer(Operation operation)
    {
        _ws.Send(JsonUtility.ToJson(new RequestObject(operation.ToString())));
    }

    private IEnumerator GetOdometerEnumerator()
    {
        while (_ws.IsAlive)
        {
            RequestToServer(Operation.getCurrentOdometer);
            yield return new WaitForSeconds(_getOdometerDelay);
        }
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        var data = JsonUtility.FromJson<OdometerData>(e.Data);
        if (data.operation == "odometer_val")
        {
            Debug.Log("OnMessage");
            GetOdometer.Invoke(data.value);
        }
        else if (data.operation == "randomStatus")
        {
            GetRandomStatus.Invoke(data.status);
            GetOdometer.Invoke(data.odometer);
        }
        else
        {
            GetOdometer.Invoke(data.odometer);
        }
    }

    private class RequestObject
    {
        public string operation;

        public RequestObject(string operation)
        {
            this.operation = operation;
        }
    }

    private void OnDestroy()
    {
        _ws.Close();
    }

    private class OdometerData
    {
        public string operation;
        public float value;
        public float odometer;
        public bool status;
    }
}