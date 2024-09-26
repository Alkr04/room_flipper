using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialogue", menuName = "SO/Dialogue", order = 1)]
public class ScenePopup : ScriptableObject
{

    [Serializable]
    public class LevelDialogue
    {
        [TextArea]
        public List<string> dialogueAtStart;
        [TextArea]
        public List<string> dialogueAtEnd;
    }

    public LevelDialogue[] dialogues;

    [TextArea]
    public List<string> dialogueAtMenu;

}