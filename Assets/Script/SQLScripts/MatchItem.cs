
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchItem : MonoBehaviour
{
    public string Key;
    public bool IsSelected { get; private set; }
    public Button Button;
    public TextMeshProUGUI Text;

    public void Init(string key, string displayText)
    {
        Key = key;
        Text.text = displayText;
        Button.onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        IsSelected = !IsSelected;
        Text.color = IsSelected ? Color.green : Color.white;
    }
}
