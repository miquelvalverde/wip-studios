using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    public static FMOD.Studio.EventInstance UIHover;
    public static FMOD.Studio.EventInstance UISelect;

    private void Start()
    {
        LoadAllFMODSounds();
    }

    private void LoadAllFMODSounds()
    {
        UISelect = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Select item/ui_select");
        UIHover = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Hover item/ui_hover");
    }
}
