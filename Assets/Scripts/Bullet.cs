using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �arp�lan nesnenin tag'ini kontrol et
        if (collision.gameObject.CompareTag("sphere"))
        {
            // SphereScaler script'ine eri� ve yar��ap� azalt
            SphereScaler sphereScaler = collision.gameObject.GetComponent<SphereScaler>();
            if (sphereScaler != null)
            {
                sphereScaler.DecreaseRadius(2f); // Yar��ap� 2 birim azalt
                sphereScaler.ChangeColor(Color.red, 0.1f); // Rengi 0.1 saniye boyunca k�rm�z�ya de�i�tir
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
}