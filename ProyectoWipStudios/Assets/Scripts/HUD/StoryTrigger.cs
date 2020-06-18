using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private StoryScriptable story = null;
        
    private void OnTriggerEnter(Collider other)
    {
        StoryHUDController.instance.StartStory(story);
    }
        
    private void OnTriggerExit(Collider other)
    {
        StoryHUDController.instance.EndStory();
    }
}
