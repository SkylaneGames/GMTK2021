using System;
using System.Collections;
using System.Collections.Generic;
using CoreSystems.Transition.Scripts;
using UnityEngine;

public class WinController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelLoader.Instance.LoadLevel(Level.Menu);
        }
    }
}
