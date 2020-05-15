using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHUDController : MonoBehaviour
{
    [System.Serializable]
    public struct StoryTrigger
    {
        public Collider trigger;
        public StoryScriptable story;
    }
    
    public StoryTrigger[] storyTriggers;
            
    private void Start()
    {
    }
}