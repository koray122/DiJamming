using UnityEngine;
using TMPro; // TextMeshPro elemanları için gerekli

public class Raycast : MonoBehaviour
{
    public float rayDistance = 100f; // Raycast mesafesi
    public Camera playerCamera; // Oyuncu kamerası
    public GameObject bulletPrefab; // Mermi prefab'ı
    public Transform gunPivot; // Silahın pivot noktası
    public Transform raycastOrigin; // Raycast'in çıkış noktası
    public float bulletSpeed = 20f; // Mermi hızı
    public float fireRate = 0.1f; // Ateş etme hızı (saniye cinsinden)
    public TextMeshProUGUI hitObjectNameText; // Çarpılan nesnenin ismini gösterecek TextMeshPro - Text UI elemanı
    public TextMeshProUGUI sphereRadiusText; // Kürenin yarıçapını gösterecek TextMeshPro - Text UI elemanı
    public TextMeshProUGUI bulletCountText; // Mermi sayısını gösterecek TextMeshPro - Text UI elemanı

    private float nextFireTime = 0f; // Bir sonraki ateş etme zamanı
    private GameObject targetedBullet; // Hedeflenen mermi
    private string hitObjectName = ""; // Çarpılan nesnenin ismi
    private SphereScaler sphereScaler; // SphereScaler script'ine referans
    private int bulletCount = 0; // Mermi sayısı

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Eğer atanmadıysa ana kamerayı kullan
        }

        if (raycastOrigin == null)
        {
            Debug.LogError("Raycast Origin atanmadı!"); // Raycast Origin atanmadıysa hata mesajı ver
        }

        // SphereScaler script'ine referans al
        GameObject sphere = GameObject.FindGameObjectWithTag("sphere");
        if (sphere != null)
        {
            sphereScaler = sphere.GetComponent<SphereScaler>();
            if (sphereScaler != null)
            {
                sphereScaler.sphereRadiusText = sphereRadiusText; // SphereScaler script'ine Text referansını ver
            }
        }

        // Mermi sayısını güncelle
        UpdateBulletCountText();
    }

    // Update is called once per frame
    void Update()
    {
        if (raycastOrigin == null)
        {
            Debug.LogError("Raycast Origin atanmadı!"); // Raycast Origin atanmadıysa hata mesajı ver
            return;
        }

        // Raycast Origin noktasından ileriye doğru bir ray oluştur
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;

        // Raycast gönder ve bir nesneye çarpıp çarpmadığını kontrol et
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            hitObjectName = hit.collider.gameObject.name; // Çarpılan nesnenin ismini al

            // Eğer çarpılan nesne bir mermi ise
            if (hit.collider.CompareTag("bullet"))
            {
                targetedBullet = hit.collider.gameObject;

                // E tuşuna basıldığında mermiyi sahneden kaldır ve mermi sayısını artır
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(targetedBullet);
                    bulletCount++;
                    UpdateBulletCountText();
                }

                // "Al [E]" mesajını göster
                if (hitObjectNameText != null)
                {
                    hitObjectNameText.text = "TAKE [E]";
                }
            }
            else if (hit.collider.CompareTag("FDoor"))
            {
                // F tuşuna basıldığında buzdolabı kapısını aç/kapa
                if (Input.GetKeyDown(KeyCode.F))
                {
                    FridgeDoorController doorController = hit.collider.GetComponent<FridgeDoorController>();
                    if (doorController != null)
                    {
                        doorController.ToggleDoor();
                    }
                }

                // "Aç [F]" mesajını göster
                if (hitObjectNameText != null)
                {
                    hitObjectNameText.text = "OPEN [F]";
                }
            }
            else if (hit.collider.CompareTag("interactable"))
            {
                // "Tut [F]" mesajını göster
                if (hitObjectNameText != null)
                {
                    hitObjectNameText.text = "HOLD [F]";
                }
            }
            else if (hit.collider.CompareTag("b"))
            {
                // "E" mesajını göster
                if (hitObjectNameText != null)
                {
                    hitObjectNameText.text = "E";
                }

                // E tuşuna basıldığında "Fiş yazdırılıyor..." mesajını göster
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitObjectNameText.text = "Fiş yazdırılıyor...";
                }
            }
            else
            {
                targetedBullet = null;

                // Çarpılan nesnenin ismini gösterme
                if (hitObjectNameText != null)
                {
                    hitObjectNameText.text = "";
                }
            }
        }
        else
        {
            hitObjectName = ""; // Eğer raycast hiçbir şeye çarpmadıysa ismi boş yap

            // Text UI elemanını temizle
            if (hitObjectNameText != null)
            {
                hitObjectNameText.text = "";
            }
        }

        // Raycast'i kırmızı renkte çizdir
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        // Sol tıklama olayını kontrol et ve mermi sayısına göre ateş et
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && bulletCount > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // Bir sonraki ateş etme zamanını güncelle
            bulletCount--; // Mermi sayısını azalt
            UpdateBulletCountText();
        }
    }

    void Shoot()
    {
        // Mermi prefab'ının null olup olmadığını kontrol et
        if (bulletPrefab != null)
        {
            Debug.Log("Shooting bullet..."); // Debug log ekle

            // Mermiyi silahın pivot noktasından instantiate et ve x ekseninde 75 derece döndür
            GameObject bullet = Instantiate(bulletPrefab, gunPivot.position, gunPivot.rotation * Quaternion.Euler(75, 0, 0));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Mermiyi ileriye doğru hareket ettir
            rb.linearVelocity = gunPivot.forward * bulletSpeed;

            // Merminin tag'ini "bullet" olarak ayarla
            bullet.tag = "bullet";

            // Mermiyi 1 saniye sonra yok et
            Destroy(bullet, 1f);
        }
        else
        {
            Debug.LogWarning("Bullet prefab is not assigned.");
        }
    }

    private void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "BULLET : " + bulletCount;
        }
    }
}