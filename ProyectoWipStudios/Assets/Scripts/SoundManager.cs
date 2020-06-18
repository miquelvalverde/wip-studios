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
    public static FMOD.Studio.EventInstance BearTrap;
    public static FMOD.Studio.EventInstance BoarCharge;
    public static FMOD.Studio.EventInstance BoarFall;
    public static FMOD.Studio.EventInstance BoarHit;
    public static FMOD.Studio.EventInstance ChameleonCamouflage;
    public static FMOD.Studio.EventInstance ChameleonDecamouflage;
    public static FMOD.Studio.EventInstance ChameleonSpit;
    public static FMOD.Studio.EventInstance ChameleonTongue;
    public static FMOD.Studio.EventInstance ChameleonTonguePick;
    public static FMOD.Studio.EventInstance DamBreak;
    public static FMOD.Studio.EventInstance TurbineActive;
    public static FMOD.Studio.EventInstance TurbineBreak;
    public static FMOD.Studio.EventInstance Waterfall;
    public static FMOD.Studio.EventInstance DartShot;
    public static FMOD.Studio.EventInstance ItsThere;
    public static FMOD.Studio.EventInstance BoarOuch;
    public static FMOD.Studio.EventInstance ChameleonOuch;
    public static FMOD.Studio.EventInstance SquirrelOuch;
    public static FMOD.Studio.EventInstance BoarJump;
    public static FMOD.Studio.EventInstance ChameleonJump;
    public static FMOD.Studio.EventInstance SquirrelJump;
    public static FMOD.Studio.EventInstance StepGrass;
    public static FMOD.Studio.EventInstance StepStone;
    public static FMOD.Studio.EventInstance StepWood;
    public static FMOD.Studio.EventInstance RockImpactBoar;
    public static FMOD.Studio.EventInstance RockShatter;
    public static FMOD.Studio.EventInstance Soundtrack;
    public static FMOD.Studio.EventInstance SquirrelClimb;
    public static FMOD.Studio.EventInstance SquirrelGlide;
    public static FMOD.Studio.EventInstance TreeFall;
    public static FMOD.Studio.EventInstance TreeImpact;

    private void Awake()
    {   
        LoadAllFMODSounds();
        DontDestroyOnLoad(this);
    }

    private void LoadAllFMODSounds()
    {
        BearTrap = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Bear Trap/Trap activated/trap");
        BoarCharge = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Boar Specific/Charge/boar_charge");
        BoarFall = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Boar Specific/Fall/boar_fall");
        BoarHit = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Boar Specific/Hit/boar_hit");
        ChameleonCamouflage = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Chameleon Specific/Camouflage/chameleon_camouflage");
        ChameleonDecamouflage = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Chameleon Specific/Decamouflage/chameleon_decamouflage");
        ChameleonSpit = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Chameleon Specific/Spit/chameleon_spit");
        ChameleonTongue = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Chameleon Specific/Tongue/chameleon_tongue");
        ChameleonTonguePick = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Chameleon Specific/Tongue pick/chameleon_tongue_pick");
        DamBreak = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Dam/Dam break/dam_break");
        TurbineActive = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Dam/Turbine active/turbine_active");
        TurbineBreak = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Dam/Turbine break/turbine_break");
        Waterfall = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Dam/Waterfall/waterfall");
        DartShot = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Enemy/Dart shot/dart_shot");
        ItsThere = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Enemy/It's there!/its_there");
        BoarOuch = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Dart impact/Boar ouch/boar_ouch");
        ChameleonOuch = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Dart impact/Chameleon ouch/chameleon_ouch");
        SquirrelOuch = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Dart impact/Squirrel ouch/squirrel_ouch");
        BoarJump = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Jump/Boar jump/boar_jump");
        ChameleonJump = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Jump/Chameleon jump/chameleon_jump");
        SquirrelJump = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Jump/Squirrel jump/squirrel_jump");
        Change = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Transformation/animal_transform");
        StepGrass = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Walk steps/Grass/animal_step_grass");
        StepStone = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Walk steps/Stone/animal_step_stone");
        StepWood = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Player/Walk steps/Wood/animal_step_wood");
        RockImpactBoar = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Rock/Rock impact/rock_impact_boar");
        RockShatter = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Rock/Rock shatter/rock_shatter");
        Music1 = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Soundtrack/Music/Music 1/music1");
        Music2 = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Soundtrack/Music/Music 2/music2");
        Soundtrack = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Soundtrack/Soundtrack/soundtrack");
        SquirrelClimb = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Squirrel Specific/Climb/squirrel_climb");
        SquirrelGlide = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Squirrel Specific/Glide/squirrel_glide");
        TreeFall = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Tree/Falling tree/tree_fall");
        TreeImpact = FMODUnity.RuntimeManager.CreateInstance("event:/Events/Tree/Tree impact/tree_impact");
        UIHover = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Hover item/ui_hover");
        UISelect = FMODUnity.RuntimeManager.CreateInstance("event:/Events/UI/Select item/ui_select");

        StepGrass.setVolume(.2f);
        StepStone.setVolume(.2f);
        StepWood.setVolume(.2f);
    }
}
