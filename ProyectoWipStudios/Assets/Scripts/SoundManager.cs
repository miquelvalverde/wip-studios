using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static FMOD.Studio.EventInstance UIHover;
    public static FMOD.Studio.EventInstance UISelect;
    public static FMOD.Studio.EventInstance Music1;
    public static FMOD.Studio.EventInstance Music2;
    public static FMOD.Studio.EventInstance Change;

    private void Awake()
    {   
        LoadAllFMODSounds();
        DontDestroyOnLoad(this);
    }

    private void LoadAllFMODSounds()
    {
        UISelect = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Select item/ui_select");
        UIHover = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Hover item/ui_hover");
        Music1 = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Soundtrack/Music/Music 1/music1");
        Music2 = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Soundtrack/Music/Music 2/music2");
        Change = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Transformation/animal_transform");
    }
}
