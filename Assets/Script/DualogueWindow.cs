using TMPro;
using UnityEngine;

public class DualogueWindow : MonoBehaviour
{
    private TMP_Text _text;
    private DualogueStory _dialogueStory;

    [System.Obsolete]
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _dialogueStory = FindObjectOfType<DualogueStory>();
        _dialogueStory.ChangedStory += ChangeAnswers;
    }

    private void ChangeAnswers(DualogueStory.Story story) => _text.text = story.Text;
} 

