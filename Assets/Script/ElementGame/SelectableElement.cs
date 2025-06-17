using UnityEngine;
using UnityEngine.UI;

public class SelectableElement : MonoBehaviour
{
    private bool selected = false;
    private string elementName;
    private bool interactionLocked = false; // �� ���� �������� ������� ����� �����������

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

        gameObject.SetActive(false); // �������� �������
    }

    public void LockInteraction()
    {
        interactionLocked = true;
    }
}