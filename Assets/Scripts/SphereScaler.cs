using UnityEngine;
using TMPro; // TextMeshPro elemanlar� i�in gerekli
using System.Collections; // IEnumerator ve Coroutine'ler i�in gerekli
using UnityEngine.UI; // UI elemanlar� i�in gerekli
using UnityEngine.SceneManagement; // Scene y�netimi i�in gerekli

public class SphereScaler : MonoBehaviour
{
    public float initialRadius = 80f; // Ba�lang�� yar��ap�
    public float radiusDecrement = 2f; // Her mermide azalacak yar��ap
    public float shrinkSpeed = 2f; // K���lme h�z�
    public TextMeshProUGUI sphereRadiusText; // Yar��ap� g�sterecek UI eleman�
    public Image winImage; // Kazanma ekran� i�in UI Image eleman�

    private float currentRadius; // Mevcut yar��ap
    private Renderer sphereRenderer; // Renderer bile�eni
    private Color originalColor; // Orijinal renk

    void Start()
    {
        currentRadius = initialRadius;
        transform.localScale = Vector3.one * currentRadius * 2f;

        sphereRenderer = GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            originalColor = sphereRenderer.material.color;
        }

        UpdateRadiusText();
        StartCoroutine(RecoverRadius()); // S�rekli b�y�me i�lemini ba�lat

        if (winImage != null)
        {
            winImage.gameObject.SetActive(false); // Ba�lang��ta kazanma ekran�n� devre d��� b�rak
        }
    }

    public void DecreaseRadius(float amount)
    {
        currentRadius = Mathf.Max(0, currentRadius - amount);
        transform.localScale = Vector3.one * currentRadius * 2f;
        UpdateRadiusText();

        if (currentRadius == 0)
        {
            HandleWin();
        }
    }

    private IEnumerator RecoverRadius()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // Her 3 saniyede bir bekle
            if (currentRadius < initialRadius)
            {
                currentRadius += 1f;
                transform.localScale = Vector3.one * currentRadius * 2f;
                UpdateRadiusText();
            }
        }
    }

    public void ChangeColor(Color newColor, float duration)
    {
        StartCoroutine(ChangeColorCoroutine(newColor, duration));
    }

    private IEnumerator ChangeColorCoroutine(Color newColor, float duration)
    {
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = newColor;
            yield return new WaitForSeconds(duration);
            sphereRenderer.material.color = originalColor;
        }
    }

    private void UpdateRadiusText()
    {
        if (sphereRadiusText != null)
        {
            float radius = transform.localScale.x / 2f;
            sphereRadiusText.text = "RADIUS: " + radius.ToString("F0");
        }
    }

    private void HandleWin()
    {
        if (winImage != null)
        {
            winImage.gameObject.SetActive(true); // Kazanma ekran�n� etkinle�tir
        }

        // Oyunu durdur
        Time.timeScale = 0f;
    }
}