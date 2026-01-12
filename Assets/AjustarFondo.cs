using UnityEngine;

public class AjustarFondo : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float anchoSprite = sr.bounds.size.x;

        float anchoCamara = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;

        float escala = anchoCamara / anchoSprite;
        transform.localScale = new Vector3(escala, escala, 1f);
    }
}
