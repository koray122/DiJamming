using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Çarpýlan nesnenin tag'ini kontrol et
        if (collision.gameObject.CompareTag("sphere"))
        {
            // SphereScaler script'ine eriþ ve yarýçapý azalt
            SphereScaler sphereScaler = collision.gameObject.GetComponent<SphereScaler>();
            if (sphereScaler != null)
            {
                sphereScaler.DecreaseRadius(2f); // Yarýçapý 2 birim azalt
                sphereScaler.ChangeColor(Color.red, 0.1f); // Rengi 0.1 saniye boyunca kýrmýzýya deðiþtir
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }
}