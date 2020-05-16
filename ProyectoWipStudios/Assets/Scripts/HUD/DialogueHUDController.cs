using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHUDController : MonoBehaviour
{
    Dictionary<Collider, Queue<string>> stories;
    public Collider[] checkPointTriggers;
    public Queue<string> sentences;
        
    private void Start()
    {
        sentences = new Queue<string>();
    }
}