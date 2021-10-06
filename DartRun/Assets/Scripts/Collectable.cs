using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.Instance.Value += 2;
            PlayerController.Instance._starParticle.Play();

            this.gameObject.SetActive(false);
        }
    }
}
