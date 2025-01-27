using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter(Collision collision)
    {
        // Çarpýlan nesnenin tag'ini kontrol et
        if (collision.gameObject.CompareTag("Chair"))
        {
            // Bu script'e sahip olan nesneyi yok et
            Destroy(this.gameObject);
        }
    }
}