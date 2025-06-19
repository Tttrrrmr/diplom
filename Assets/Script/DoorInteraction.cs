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

    public bool isAdminRoom = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.material.color;

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
            int role = PlayerSession.RoleId;
            Debug.Log("Роль игрока: " + role);
            Debug.Log($"Токен: {PlayerSession.AccessToken}");

            uiOpen = true;

            if (isAdminRoom && role != 1)
            {
                WarningUI warning = FindObjectOfType<WarningUI>();
                if (warning != null)
                {
                    warning.Show("У вас недостаточно прав для входа в этот кабинет.", () =>
                    {
                        uiOpen = false;
                    });
                }
                else
                {
                    Debug.LogWarning("WarningUI не найден в сцене");
                    uiOpen = false;
                }

                return;
            }

            EnterRoomUI enterUI = FindObjectOfType<EnterRoomUI>();
            if (enterUI != null)
            {
                enterUI.Show(sceneToLoad, roomName);
                enterUI.SetOnClose(() => uiOpen = false);
            }
            else
            {
                Debug.LogWarning("EnterRoomUI не найден в сцене");
                uiOpen = false;
            }
        }
    }
}



