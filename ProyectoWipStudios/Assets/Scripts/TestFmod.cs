using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFmod : MonoBehaviour
{
    FMOD.Studio.EventInstance PlayMySound;

    private void Awake()
    {
        PlayMySound = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Select item/ui_select");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("triggered sound");
            PlayMySound.start();
        }
    }
}
