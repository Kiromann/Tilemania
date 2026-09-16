using UnityEngine; // Menggunakan fitur Unity

public class EnemyMovement : MonoBehaviour // Membuat script pergerakan musuh
{
    [SerializeField] float moveSpeed = 1f; // Kecepatan dan arah gerak musuh
    Rigidbody2D myRigidbody; // Menyimpan komponen Rigidbody2D musuh

    // Dipanggil sekali saat game dimulai
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>(); // Mengambil Rigidbody2D dari musuh
    }

    // Dipanggil setiap frame
    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(moveSpeed, 0f); // Membuat musuh bergerak horizontal
    }

    // Dipanggil saat musuh keluar dari trigger
    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed; // Membalik arah gerak musuh
        FlipEnemy(); // Membalik arah tampilan musuh
    }

    // Membalik tampilan musuh sesuai arah geraknya
    void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.linearVelocity.x)), 1f); // Membalik sprite kiri/kanan
    }
}