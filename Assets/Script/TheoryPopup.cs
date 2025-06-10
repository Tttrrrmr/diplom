using UnityEngine;
using UnityEngine.UI;

public class TheoryPopup : MonoBehaviour
{
    public GameObject theoryPanel;
    public Button theoryButton;
    public Button closeButton;

    void Start()
    {
        theoryButton.onClick.AddListener(ShowTheory);
        closeButton.onClick.AddListener(HideTheory);
    }

    void ShowTheory()
    {
        theoryPanel.SetActive(true);
    }

    void HideTheory()
    {
        theoryPanel.SetActive(false);
    }
}