using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueSwitcher : MonoBehaviour
{
    [SerializeField] private string[] _disableTags;
    private DualogueStory _dialogueStory;

    [Obsolete]
    private void Start()
    {
        _dialogueStory = FindObjectOfType<DualogueStory>(true);
        _dialogueStory.ChangedStory += Disable;
    }

    private async void Disable(DualogueStory.Story story)
    {
        if (_disableTags.All(disableTag => story.Tag != disableTag)) return;
        await Task.Delay(1000);
        _dialogueStory.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        _dialogueStory.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
