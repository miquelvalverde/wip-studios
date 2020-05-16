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
    [Tooltip("Time between characters. Less is faster.")]
    [SerializeField] private float typeSpeed = 0.01F;
    private bool isTellingStory = false;
    private bool isTyping = false;
    private int index = 0;
    private StoryScriptable currentStory;
        
    public void StartStory(StoryScriptable story)
    {
        if (isTellingStory)
            return;
            
        isTellingStory = true;
        index = 0;
        currentStory = story;
        gameObject.SetActive(true);
        Next();
    }
        
    private void Next()
    {
        if (index < currentStory.sentences.Count)
        {

            if (isTyping)
            {
                EndType();
            }
            else
            {
                StartCoroutine(TypeRoutine(currentStory.sentences[index]));
                index++;
            }
        }
        else
        {
            ResetStory();
        }
    }

    IEnumerator TypeRoutine(string sentence)
    {
        isTyping = true;

        WaitForSeconds wait = new WaitForSeconds(typeSpeed);
        textPanel.text = string.Empty;
        foreach (char letter in sentence.ToCharArray())
        {
            textPanel.text += letter;
            yield return wait;
        }

        isTyping = false;
    }

    private void EndType()
    {
        StopAllCoroutines();
        textPanel.text = currentStory.sentences[index-1];
        isTyping = false;
    }

    public void ResetStory()
    {
        textPanel.text = string.Empty;
        gameObject.SetActive(false);
        isTellingStory = false;
        isTyping = false;
        index = 0;
        currentStory = null;
    }
}