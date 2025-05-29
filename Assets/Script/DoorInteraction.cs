using TMPro;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public string roomName = "Неизвестный кабинет";
    public string sceneToLoad = "MainScene";

    private SpriteRenderer sr;
    private Color originalColor;
    private TextMeshPro label;
    private bool playerInside = false;
    private bool uiOpen = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.material.color;

        // ищем подпись
        TextMeshPro[] texts = GetComponentsInChildren<TextMeshPro>(true);
        foreach (var txt in texts)
        {
            if (txt.gameObject.name.ToLower().Contains("label"))
            {
                label = txt;
                label.text = roomName;
                label.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            sr.material.color = Color.yellow;

            if (label != null)
                label.transform.parent.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            sr.material.color = originalColor;

            if (label != null)
                label.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && !uiOpen && Input.GetKeyDown(KeyCode.E))
        {
            EnterRoomUI ui = FindObjectOfType<EnterRoomUI>();
            if (ui != null)
            {
                ui.Show(sceneToLoad, roomName);
                ui.OnClose += () => { uiOpen = false; }; // Подписка на закрытие
                uiOpen = true;
            }
        }
    }
}
