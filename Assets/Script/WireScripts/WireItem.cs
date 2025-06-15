using UnityEngine;
using UnityEngine.UI;

public class WireItem : MonoBehaviour
{
    public string colorId;
    public Button button;
    private System.Action<WireItem> onClickCallback;

    public void Setup(string id, Color color, System.Action<WireItem> callback)
    {
        colorId = id;
        onClickCallback = callback;
        GetComponent<Image>().color = color;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClickCallback?.Invoke(this));
    }
}
