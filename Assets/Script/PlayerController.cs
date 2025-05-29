using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public SpriteRenderer spriteRenderer;
    public Sprite leftSprite;
    public Sprite rightSprite;

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
        {
            transform.Translate(Vector2.right * move * speed * Time.deltaTime);

            if (move > 0 && rightSprite != null)
                spriteRenderer.sprite = rightSprite;
            else if (move < 0 && leftSprite != null)
                spriteRenderer.sprite = leftSprite;
        }
    }
}


