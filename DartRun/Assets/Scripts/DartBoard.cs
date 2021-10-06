using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DartBoard : MonoBehaviour
{
    [SerializeField] int _boardValue;
    [SerializeField] int _startBoardValue;
    [SerializeField] TextMeshPro _boardText;
    public GameObject _cellBoard;

    private void OnEnable()
    {
        _boardValue = _startBoardValue;
        _boardText.text = _boardValue.ToString();
    }
    private void Start()
    {
        _boardText.text = _boardValue.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MiniDartIns"))
        {
            MyTween.Instance.DoTween_Shake(transform);

            _boardValue -= 2;
            _boardText.text = _boardValue.ToString();
            Destroy(collision.gameObject);

            if (_boardValue <= 0)
            {
                this.gameObject.SetActive(false);
                var cell = Instantiate(_cellBoard, transform.position, Quaternion.identity) as GameObject;
                Destroy(cell, 3f);
            }
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.SetLose();
        }
    }  
}
