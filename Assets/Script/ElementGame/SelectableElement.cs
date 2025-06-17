using UnityEngine;
using UnityEngine.UI;

public class SelectableElement : MonoBehaviour
{
    private bool selected = false;
    private string elementName;
    private bool interactionLocked = false; // не даст повторно выбрать после «Сохранить»

    void Start()
    {
        elementName = gameObject.name;
        GetComponent<Button>().onClick.AddListener(ToggleSelect);
    }

    void ToggleSelect()
    {
        if (interactionLocked) return;

        selected = true;

        UIElementManager.Instance.UpdateSelection(elementName, true);

        gameObject.SetActive(false); // скрываем элемент
    }

    public void LockInteraction()
    {
        interactionLocked = true;
    }
}