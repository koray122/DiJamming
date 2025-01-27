using UnityEngine;

public class PickUp : MonoBehaviour
{
    bool isHolding = false; // Nesnenin tutulup tutulmadığını belirten bayrak

    [SerializeField] float throwForce = 50f; // Fırlatma kuvveti
    [SerializeField] float maxDistance = 3f; // Maksimum tutma mesafesi
    [SerializeField] Camera mainCamera; // Ana kamera referansı
    [SerializeField] RenderTexture renderTexture; // Render Texture referansı

    float distance; // Nesne ile oyuncu arasındaki mesafe
    TempParent tempParent; // Geçici ebeveyn referansı
    Rigidbody rb; // Nesnenin Rigidbody bileşeni
    Vector3 objectPos; // Nesnenin pozisyonu

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Nesnenin Rigidbody bileşenini al
        tempParent = TempParent.Instance; // TempParent singleton referansını al

        if (mainCamera == null || renderTexture == null)
        {
            Debug.LogError("Ana kamera veya Render Texture atanmadı!"); // Kamera veya Render Texture atanmadıysa hata mesajı ver
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isHolding)
            {
                Throw(); // F tuşuna tekrar basıldığında nesneyi fırlat
            }
            else
            {
                TryPickUp(); // F tuşuna basıldığında nesneyi almaya çalış
            }
        }

        if (isHolding)
        {
            Hold(); // Nesne tutuluyorsa Hold fonksiyonunu çağır
        }
    }

    private void TryPickUp()
    {
        if (mainCamera == null || renderTexture == null)
        {
            Debug.LogError("Kamera veya Render Texture eksik!"); // Kamera veya Render Texture eksikse hata mesajı ver
            return;
        }

        // Render Texture'un ekran ortasını hesapla
        Vector3 renderTextureCenter = new Vector3(renderTexture.width / 2, renderTexture.height / 2, 0);

        // Ray oluştur
        Ray ray = mainCamera.ScreenPointToRay(renderTextureCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                distance = Vector3.Distance(transform.position, tempParent.transform.position);
                if (distance <= maxDistance)
                {
                    isHolding = true; // Nesne tutuluyor olarak işaretle
                    rb.useGravity = false; // Yerçekimini devre dışı bırak
                    rb.detectCollisions = true; // Çarpışma algılamayı etkinleştir

                    transform.SetParent(tempParent.transform); // Nesneyi geçici ebeveynin altına taşı
                }
            }
        }
    }

    private void Hold()
    {
        distance = Vector3.Distance(transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop(); // Mesafe maksimum mesafeyi aştıysa nesneyi bırak
        }

        rb.linearVelocity = Vector3.zero; // Nesnenin hızını sıfırla
        rb.angularVelocity = Vector3.zero; // Nesnenin açısal hızını sıfırla
    }

    private void Throw()
    {
        if (isHolding)
        {
            isHolding = false; // Nesne tutulmuyor olarak işaretle
            objectPos = transform.position; // Nesnenin pozisyonunu kaydet
            transform.SetParent(null); // Nesneyi ebeveyninden ayır
            transform.position = objectPos; // Nesnenin pozisyonunu geri yükle
            rb.useGravity = true; // Yerçekimini etkinleştir

            // Nesneyi ileri doğru fırlat
            rb.AddForce(tempParent.transform.forward * throwForce);
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false; // Nesne tutulmuyor olarak işaretle
            objectPos = transform.position; // Nesnenin pozisyonunu kaydet
            transform.SetParent(null); // Nesneyi ebeveyninden ayır
            transform.position = objectPos; // Nesnenin pozisyonunu geri yükle
            rb.useGravity = true; // Yerçekimini etkinleştir
        }
    }
}