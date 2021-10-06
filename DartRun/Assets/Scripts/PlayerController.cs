using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using TMPro;

public class PlayerController : Singleton<PlayerController>, IEventScripts
{
    Vector3 _startPosition;

    Vector2 _inputStart;
    Vector2 _inputEnd;

    Vector3 _direction;

    [SerializeField] float _turnSpeed = 5f;
    [SerializeField] float _speed = 5f;
    [SerializeField] float PlatformLeftBorder;
    [SerializeField] float PlatformRightBorder;
    [SerializeField] int _value = 5;
    [SerializeField] TextMeshPro _valueText;
    public ParticleSystem _starParticle;

    SplineFollower _splineFollower;
    bool _isLaunched;
    public SplineFollower SplineFollower { get { return _splineFollower; } }

    float scaleVal;
    [SerializeField] float sVal;

    [Header("MiniDart")]
    public Transform AttackFolder;
    public GameObject MiniDart;
    float _lastClickTime;
    [SerializeField] float _doubleClickTime = 0.2f;
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            _value = (int)Mathf.Clamp(_value, 0, float.MaxValue);
            _valueText.text = _value.ToString();
        }
    }
    private void Awake()
    {
        GameManager.Instance.EventPlay += OnPlay;
        GameManager.Instance.EventMenu += OnMenu;
        GameManager.Instance.EventLose += OnLose;
        GameManager.Instance.EventFinish += OnFinish;
        Gate.EventHit += OnGateHit;
    }

    void Start()
    {
        _splineFollower = GetComponent<SplineFollower>();
        _splineFollower.onEndReached += OnEndReached;
        DOTween.Init();
    }
    private void OnEnable()
    {
        transform.localScale = new Vector3(.15f, .15f, .15f);
    }

    void Update()
    {
        Movement();
        PlayerInput();

        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
    }

    void FixedUpdate()
    {
        
    }

    void Movement() 
    {
        if (GameManager.Instance.GameState == GameState.Play && _isLaunched)
        {
            _splineFollower.motion.offset = new Vector2(Mathf.Clamp(_splineFollower.motion.offset.x + _direction.x * Time.deltaTime * _turnSpeed, PlatformLeftBorder, PlatformRightBorder), _splineFollower.motion.offset.y);
        }
    }

    void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            _inputStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - _lastClickTime < _doubleClickTime)
            {
                //double click
                if (_value > 0) { Shoot(); }
            }
            _lastClickTime = Time.time;

            _inputEnd = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _inputStart = Vector2.zero;
            _inputEnd = Vector2.zero;
        }

        if (_inputStart.x - _inputEnd.x < 0) //left
        {
            _direction = Vector3.left;
        }
        else if (_inputStart.x - _inputEnd.x > 0) //right
        {
            _direction = Vector3.right;
        }
        else
        {
            _direction = Vector3.zero;
        }
    }

    public void OnMenu()
    {
        _direction = Vector2.zero;
        transform.localScale = new Vector3(.15f, .15f, .15f);

        _value = 5;
        sVal = 0;
        _valueText.text = _value.ToString();

        _isLaunched = false;
        _splineFollower.follow = false;
        _splineFollower.spline = FindObjectOfType<SplineComputer>();
        _splineFollower.Restart(0);
    }
    IEnumerator InitPlayer()
    {
        yield return new WaitForSeconds(0f);
        _isLaunched = true;
        _splineFollower.spline = LevelManager.Instance.GetCurrentLevel().GetChild(0).GetComponent<SplineComputer>();
        _splineFollower.follow = true;

    }

    public void OnPlay()
    {
        StartCoroutine(InitPlayer());
    }

    public void OnFinish()
    {
       
    }

    public void OnLose()
    {
        _splineFollower.follow = false;
        MyTween.Instance.DoTween_Shake(transform);
    }
    private void OnEndReached(double obj)
    {
        GameManager.Instance.SetFinish();
    }
    void OnGateHit(GateState gateState, int value)
    {
        switch (gateState)
        {
            case GateState.Addition:
                _value += value;
                sVal += .02f;
                break;
            case GateState.Multiplation:
                _value *= value;
                sVal += .02f;
                break;
            case GateState.Divide:
                _value /= value;
                sVal -= .02f;
                break;
            case GateState.Substraction:
                _value -= value;
                sVal -= .02f;
                break;
        }

        _value = (int)Mathf.Clamp(_value, 0, float.MaxValue);
        _valueText.text = _value.ToString();

        DOTween_Scale();

        _starParticle.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MiniDart"))
        {
            _value = _value + 2;
            _valueText.text = _value.ToString();
            sVal += .01f;
            DOTween_Scale();
        }
    }
    public void DOTween_Scale()
    {
        Sequence DCSeq = DOTween.Sequence().SetAutoKill(false);

        DCSeq.Append(transform.DOScale(new Vector3(SplineFollower.motion.baseScale.x + sVal, SplineFollower.motion.baseScale.y + sVal, SplineFollower.motion.baseScale.z + sVal), .6f)).SetEase(Ease.OutSine);
    }
    void Shoot()
    {
        var _miniDart = Instantiate(MiniDart, AttackFolder.position, Quaternion.identity) as GameObject;
        _miniDart.GetComponent<Rigidbody>().AddForce(-transform.forward * 50f, ForceMode.Impulse);
        _miniDart.transform.rotation = transform.rotation;
        Destroy(_miniDart, 10f);

        _value = _value - 2;
        _valueText.text = _value.ToString();
        sVal -= .01f;
    }
}
