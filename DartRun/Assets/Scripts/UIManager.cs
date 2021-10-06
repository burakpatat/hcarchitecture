using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>, IEventScripts
{
    [SerializeField] Transform _menuPanel;
    [SerializeField] Transform _mainPanel;
    [SerializeField] Transform _winPanel;
    [SerializeField] Transform _losePanel;

    [SerializeField] Transform _playButton;
    [SerializeField] Text _menuLevelCount;
    [SerializeField] Text _mainLevelCount;
    [SerializeField] Text _gemCount;

    public Transform MenuPanel { get { return _menuPanel; } }
    public Transform MainPanel { get { return _mainPanel; } }
    public Transform WinPanel { get { return _winPanel; } }
    public Transform LosePanel { get { return _losePanel; } }
    public Transform PlayButton { get { return _playButton; } }
    public Text MenuLevelCount { get { return _menuLevelCount; } }
    public Text MainLevelCount { get { return _mainLevelCount; } }
    public Text GemCount { get { return _gemCount; } }

    [Header("Debug Menu")]
    public Slider _playerSpeed;

    public Slider _followOffsetX;
    public Slider _followOffsetY;
    public Slider _followOffsetZ;

    public Slider _aimOffsetX;
    public Slider _aimOffsetY;
    public Slider _aimOffsetZ;

    public Toggle _camEnabled;
    public Transform _debugPanel;
    void Start()
    {
        GameManager.Instance.EventMenu += OnMenu;
        GameManager.Instance.EventPlay += OnPlay;
        GameManager.Instance.EventFinish += OnFinish;
        GameManager.Instance.EventLose += OnLose;

        //Debug
        _followOffsetX.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.x;
        _followOffsetY.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.y;
        _followOffsetZ.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.z;

        _aimOffsetX.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.x;
        _aimOffsetY.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.y;
        _aimOffsetZ.value = CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.z;

    }
    public void OnMenu()
    {
        MenuPanel.gameObject.SetActive(true);
        WinPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(false);
        _menuLevelCount.text = (LevelManager.Instance.CurrentLevel + 1).ToString();
    }

    public void OnPlay()
    {
        MenuPanel.gameObject.SetActive(false);
        MainPanel.gameObject.SetActive(true);
        _mainLevelCount.text = (LevelManager.Instance.CurrentLevel + 1).ToString();
    }

    public void OnFinish()
    {
        MainPanel.gameObject.SetActive(false);
        WinPanel.gameObject.SetActive(true);
    }
    public void OnLose()
    {
        MainPanel.gameObject.SetActive(false);
        LosePanel.gameObject.SetActive(true);
    }

    public void OpenDebugPanel()
    {
        MenuPanel.gameObject.SetActive(false);
        _debugPanel.gameObject.SetActive(true);
    }
    public void CloseDebugPanel()
    {
        MenuPanel.gameObject.SetActive(true);
        _debugPanel.gameObject.SetActive(false);
    }
    private void Update()
    {
        //Debug
        PlayerController.Instance.SplineFollower.followSpeed = _playerSpeed.value;

        if (_camEnabled.isOn)
        {
            CameraManager.Instance.SetActiveCamera(CameraManager.Instance._playCam);
        }

        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.x = _followOffsetX.value;
        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.y = _followOffsetY.value;
        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.z = _followOffsetZ.value;

        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.x = _aimOffsetX.value;
        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.y = _aimOffsetY.value;
        CameraManager.Instance._playCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineComposer>().m_TrackedObjectOffset.z = _aimOffsetZ.value;
    }
}
