using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Odometer : MonoBehaviour
{
    [Header("Odometer Settings")]
    [SerializeField] private float _step = 0.5f;

    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Slider Settings")]
    [SerializeField] private Slider _slider;
    [SerializeField] private float _maxValue = 10_000;

    private float nextOdometer;

    private void Start()
    {
        _text.text = "0";
        _slider.value = 0;
        _slider.maxValue= _maxValue;
        Client.GetOdometer += (value) =>
        {
            nextOdometer = value;
        };
    }
    private void LateUpdate()
    {
        
        _slider.value = Mathf.Lerp(_slider.value, nextOdometer, Time.deltaTime * _step);
        _text.text = _slider.value.ToString();
    }

}
