using System;
using System.Collections;
using System.Collections.Generic;
using CoreSystems.Transition.Scripts;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelLoader.Instance.LoadLevel(Level.Game);
        }
    }
}
