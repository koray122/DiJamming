using UnityEngine;
using TMPro; // TextMeshPro elemanları için gerekli
using UnityEngine.UI; // UI elemanları için gerekli
using System.Collections; // IEnumerator ve Coroutine'ler için gerekli
using StarterAssets; // FirstPersonController script'inin bulunduğu namespace

public class PlayerController : MonoBehaviour
{
    public Image blackoutImage; // Ekran kararma efekti için UI Image elemanı
    public TextMeshProUGUI gameOverText; // Game Over yazısı için TextMeshPro - Text UI elemanı
    public float blackoutDuration = 10f; // Ekran kararma süresi
    public float freezeDuration = 2f; // Oyuncunun hareket edemeyeceği süre
    public GameObject crosshair; // Crosshair UI elemanı
    public TextMeshProUGUI sphereRadiusText; // Kürenin yarıçapını belirten TextMeshPro - Text UI elemanı
    public GameObject weapon; // Oyuncunun silahı

    private bool isFrozen = false; // Oyuncunun hareket edip edemeyeceğini belirten bayrak
    private FirstPersonController firstPersonController; // FirstPersonController script'ine referans

    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();

        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(false); // Başlangıçta kararma efektini devre dışı bırak
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false); // Başlangıçta Game Over yazısını devre dışı bırak
        }

        if (crosshair != null)
        {
            crosshair.SetActive(true); // Başlangıçta crosshair'i etkinleştir
        }

        if (sphereRadiusText != null)
        {
            sphereRadiusText.gameObject.SetActive(true); // Başlangıçta yarıçapı belirten text'i etkinleştir
        }

        if (weapon != null)
        {
            // Silahın Collider bileşenine sahip olduğundan emin olun
            Collider weaponCollider = weapon.GetComponent<Collider>();
            if (weaponCollider == null)
            {
                weaponCollider = weapon.AddComponent<BoxCollider>(); // Silaha BoxCollider ekleyin
            }

            // Silahın çarpışma olayını dinleyin
            weaponCollider.isTrigger = true; // Silahın Collider'ını trigger olarak ayarlayın
        }
    }

    private void Update()
    {
        if (isFrozen && firstPersonController != null)
        {
            // Oyuncunun hareketini durdur
            firstPersonController.MoveSpeed = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sphere"))
        {
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private void OnWeaponTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sphere"))
        {
            StartCoroutine(HandlePlayerDeath());
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        isFrozen = true; // Oyuncunun hareketini durdur

        yield return new WaitForSeconds(freezeDuration); // Belirtilen süre kadar bekle

        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(true); // Kararma efektini etkinleştir
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true); // Game Over yazısını etkinleştir
        }

        if (crosshair != null)
        {
            crosshair.SetActive(false); // Crosshair'i devre dışı bırak
        }

        if (sphereRadiusText != null)
        {
            sphereRadiusText.gameObject.SetActive(false); // Yarıçapı belirten text'i devre dışı bırak
        }

        yield return new WaitForSeconds(blackoutDuration); // Belirtilen süre kadar bekle

        // Ekran kararma ve Game Over yazısını devre dışı bırak
        if (blackoutImage != null)
        {
            blackoutImage.gameObject.SetActive(false);
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        isFrozen = false; // Oyuncunun hareketini tekrar etkinleştir

        if (firstPersonController != null)
        {
            // Oyuncunun hareket hızını eski haline getir
            firstPersonController.MoveSpeed = 4.0f; // Orijinal hareket hızını buraya yazın
        }

        if (crosshair != null)
        {
            crosshair.SetActive(true); // Crosshair'i tekrar etkinleştir
        }

        if (sphereRadiusText != null)
        {
            sphereRadiusText.gameObject.SetActive(true); // Yarıçapı belirten text'i tekrar etkinleştir
        }
    }
}