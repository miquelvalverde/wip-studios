using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KODAKI/Story", order = 1)]
public class StoryScriptable : ScriptableObject
{
    [TextArea]
    public List<string> sentences;
}
