using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialtyValidator : MonoBehaviour
{
    public TMP_Dropdown specialtyDropdown;
    public GameObject alertPanel;
    public TMP_Text alertText;
    public Button okButton;
    public Button regButton; // ��� ���� ������������ ������

    private int validSpecialtyId = 0; // ������������

    private void Start()
    {
        alertPanel.SetActive(false);
        okButton.onClick.AddListener(HideAlert);
        regButton.onClick.AddListener(OnRegisterClicked); // �������� �� ������� �� ������
    }

    private void OnRegisterClicked()
    {
        int selectedId = specialtyDropdown.value;

        if (selectedId != validSpecialtyId)
        {
            alertText.text = "��� ��������� ������������� ���� ��� �������� �������.";
            alertPanel.SetActive(true);
        }
        else
        {
            alertPanel.SetActive(false);
            Debug.Log("������������� ��������. ����� ����� ���� ����� ����������� ����� ApiManager.");
        }
    }

    private void HideAlert()
    {
        alertPanel.SetActive(false);
    }
}
