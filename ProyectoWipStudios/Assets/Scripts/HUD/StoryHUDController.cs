using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryHUDController : AMonoBehaivourWithInputs
{
    public static StoryHUDController instance;

    protected override void Awake()
    {
        base.Awake();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        ResetStory();
    }

    protected override void SetControls()
    {
        controls.UI.SkipDialog.performed += _ => Next();
    }

    [SerializeField] private Text textPanel;
    private bool isTellingStory = false;
    private int currentMessage = 0;
    private StoryScriptable currentStory;
        
    public void StartStory(StoryScriptable story)
    {
        if (isTellingStory)
            return;
            
        isTellingStory = true;
        currentMessage = 0;
        currentStory = story;
        this.gameObject.SetActive(true);
        Next();
    }
        
    private void Next()
    {
        if (currentMessage < currentStory.sentences.Count)
        {
            textPanel.text = currentStory.sentences[currentMessage];
            currentMessage++;
        }
        else
        {
            ResetStory();
        }
    }

    public void ResetStory()
    {
        textPanel.text = string.Empty;
        this.gameObject.SetActive(false);
        isTellingStory = false;
    }
}