using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtons : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    private TMP_Text[] _buttonsText;
    private string[] _currentReplyTags;
    private DualogueStory _dialogueStory;

    [System.Obsolete]
    private void Awake()
    {
        _dialogueStory = FindObjectOfType<DualogueStory>();
        _dialogueStory.ChangedStory += ChangeAnswers;

        _currentReplyTags = new string[_buttons.Length];
        _buttonsText = new TMP_Text[_buttons.Length];

        for (int i = 0; i < _buttons.Length; i++)
        {
            int button = i;
            _buttons[i].onClick.AddListener(() => SendAnswer(button));
            _buttonsText[i] = _buttons[i].gameObject.GetComponentInChildren<TMP_Text>();
        }
    }

    private void ChangeAnswers(DualogueStory.Story story)
    {
        for (int i = 0; i < _buttonsText.Length; i++)
        {
            if (story.Answers.Length <= i)
            {
                _buttonsText[i].text = null;
                _buttons[i].interactable = false;
                continue;
            }

            _buttonsText[i].text = story.Answers[i].Text;
            _currentReplyTags[i] = story.Answers[i].ReposeText;
            _buttons[i].interactable = true;
        }
    }

    private void SendAnswer(int button) => _dialogueStory.ChangeStory(_currentReplyTags[button]);
}

