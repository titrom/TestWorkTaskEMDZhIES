using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    private AudioSource _source;

    [SerializeField] private Toggle _togel;

    [SerializeField] private Slider _slider;


    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _togel.onValueChanged.AddListener((isOn) => 
        { 
            _source.mute = !isOn;
        });
    }

    private void Update()
    {
        _source.volume = _slider.value;
    }
}
