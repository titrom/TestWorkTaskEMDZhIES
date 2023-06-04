using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private Toggle _togel;
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _startButton;

    [SerializeField] private AudioSource _music;

    private VideoPlayer _video;


    private void Awake()
    {
        _video = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        Client.IsConnectEvent += (isConnect) =>
        {
            _startButton.interactable = isConnect;
        };

        _togel.onValueChanged.AddListener((isOn) =>
        {
            _video.SetDirectAudioMute(0, !isOn);
        });

        _startButton.onClick.AddListener(() => OnPlayButton());

        _video.loopPointReached += (a) => 
        {
            _music.UnPause();
        };
    }

    private void Update()
    {
        _video.SetDirectAudioVolume(0, _slider.value);
    }


    public void OnPlayButton()
    {
        _music.Pause();
        _video.Play();
    }
}
