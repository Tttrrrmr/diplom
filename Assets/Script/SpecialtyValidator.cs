using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialtyValidator : MonoBehaviour
{
    public TMP_Dropdown specialtyDropdown;
    public GameObject alertPanel;
    public TMP_Text alertText;
    public Button okButton;
    public Button regButton; // Это твоя существующая кнопка

    private int validSpecialtyId = 0; // «Программист»

    private void Start()
    {
        alertPanel.SetActive(false);
        okButton.onClick.AddListener(HideAlert);
        regButton.onClick.AddListener(OnRegisterClicked); // Проверка по нажатию на кнопку
    }

    private void OnRegisterClicked()
    {
        int selectedId = specialtyDropdown.value;

        if (selectedId != validSpecialtyId)
        {
            alertText.text = "Для выбранной специальности пока нет учебного пособия.";
            alertPanel.SetActive(true);
        }
        else
        {
            alertPanel.SetActive(false);
            Debug.Log("Специальность подходит. Здесь может быть вызов регистрации через ApiManager.");
        }
    }

    private void HideAlert()
    {
        alertPanel.SetActive(false);
    }
}
