using UnityEngine;
using TMPro; // TextMeshPro elemanlarý için gerekli
using System.Collections; // IEnumerator ve Coroutine'ler için gerekli
using UnityEngine.UI; // UI elemanlarý için gerekli
using UnityEngine.SceneManagement; // Scene yönetimi için gerekli

public class SphereScaler : MonoBehaviour
{
    public float initialRadius = 80f; // Baþlangýç yarýçapý
    public float radiusDecrement = 2f; // Her mermide azalacak yarýçap
    public float shrinkSpeed = 2f; // Küçülme hýzý
    public TextMeshProUGUI sphereRadiusText; // Yarýçapý gösterecek UI elemaný
    public Image winImage; // Kazanma ekraný için UI Image elemaný

    private float currentRadius; // Mevcut yarýçap
    private Renderer sphereRenderer; // Renderer bileþeni
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
        StartCoroutine(RecoverRadius()); // Sürekli büyüme iþlemini baþlat

        if (winImage != null)
        {
            winImage.gameObject.SetActive(false); // Baþlangýçta kazanma ekranýný devre dýþý býrak
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
            winImage.gameObject.SetActive(true); // Kazanma ekranýný etkinleþtir
        }

        // Oyunu durdur
        Time.timeScale = 0f;
    }
}