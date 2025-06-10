using UnityEngine;

public class MouseHoverLabel : MonoBehaviour
{
    public GameObject labelObject;

    private void Start()
    {
        if (labelObject != null)
            labelObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (labelObject != null)
            labelObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (labelObject != null)
            labelObject.SetActive(false);
    }
}
