using System;
using UnityEngine;

[CreateAssetMenu]
public class CustomerObject : ScriptableObject
{
    [Serializable]
    public class CharacterDialogue
    {
        [Serializable]
        public class DialogueStatement
        {
            [Serializable]
            public class DialogueOption
            {
                public string text;
                public int value;

                [TextArea]
                public string response;
                public string mood;
            }
            [TextArea]
            public string text;

            public DialogueOption[] options;
        }

        [TextArea, Space]
        public string openingStatement;
        public DialogueStatement[] statements;
    }

    [Serializable]
    public class DesireSettings
    {
        [Serializable]
        public class DesireResponse
        {
            [TextArea]
            public string salesPitch;

            [TextArea, Space]
            public string characterResponse;
            public string mood;

            [TextArea, Space]
            public string selfResponse;
        }
        [Serializable]
        public class DesireValue
        {
            public int value;
            public DesireResponse responses;
        }
        public DesireValue min;
        public DesireValue max;

        public DesireResponse neutralResponses;
    }

    public bool hideSellButton;

    [Space]
    public Sprite backgroundSprite;

    [Space]
    public GameObject characterPrefab;

    [Space]
    public CharacterDialogue dialogue;

    [Space]
    public DesireSettings desireSettings;
}
