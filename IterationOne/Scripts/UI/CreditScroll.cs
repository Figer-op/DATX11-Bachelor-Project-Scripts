using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    public float ScrollSpeed = 100f;

    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, ScrollSpeed * Time.deltaTime);
    }
}
