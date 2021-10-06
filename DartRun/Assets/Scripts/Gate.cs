using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public enum GateState {Addition, Multiplation, Divide, Substraction }

public class Gate : MonoBehaviour
{
    [SerializeField] GateData _obstacleData;

    [SerializeField]GateState _gateState;

    [SerializeField]int _gateValue;

    [SerializeField] TextMeshPro _gateText;

    [SerializeField] Transform _gate;

    bool _isHit;

    public static event Action<GateState,int> EventHit;

    private void OnEnable()
    {
        _isHit = false;
    }
    private void Start()
    {
        SetObstacle();
    }

    void SetObstacle() 
    {

        switch (_gateState) 
        {
            case GateState.Addition:
                _gateText.text = string.Format("+{0}", _gateValue);
                _gate.GetComponent<MeshRenderer>().material = _obstacleData.BlueMaterial;
                break;
            case GateState.Multiplation:
                _gateText.text = string.Format("X{0}", _gateValue);
                _gate.GetComponent<MeshRenderer>().material = _obstacleData.BlueMaterial;
                break;
            case GateState.Divide:
                _gateText.text = string.Format("/{0}", _gateValue);
                _gate.GetComponent<MeshRenderer>().material = _obstacleData.RedMaterial;
                break;
            case GateState.Substraction:
                _gateText.text = string.Format("-{0}", _gateValue);
                _gate.GetComponent<MeshRenderer>().material = _obstacleData.RedMaterial;
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            if (!_isHit) 
            {
                _isHit = true;


                if(collision.GetContact(0).thisCollider.name == "gate") 
                { 
                    EventHit?.Invoke(_gateState, _gateValue);
                }
            }

        }
    }
}
