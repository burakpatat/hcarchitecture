using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform CollectFolder;
    public Transform DartBoardFolder;
    private void OnEnable()
    {
        for (int i = 0; i < CollectFolder.childCount; i++)
        {
            CollectFolder.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < DartBoardFolder.childCount; i++)
        {
            DartBoardFolder.GetChild(i).gameObject.SetActive(true);
        }
    }
}
