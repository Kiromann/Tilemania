using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 10f; // Kecepatan jalan player
    [SerializeField] float jumpSpeed = 5f;   // Kekuatan lompatan
    [SerializeField] float climbSpeed = 15f; // Kecepatan naik tangga
    [SerializeField] Vector2 deadKick = new Vector2(10f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;              // "Perintah" arah gerak player
    Rigidbody2D myRigidbody;        // "Mesin fisik" player
    Animator myAnimator;            // "Pengatur animasi" player
    CapsuleCollider2D myCapsuleCollider;   // "Sensor tubuh" player
    BoxCollider2D myBoxCollider;
    float gravityScaleAtStart;      // Menyimpan gravitasi awal

    bool isAlive = true;
    public Image redEffect;
    public TextMeshProUGUI textGameOver;

    void Start()
    {
        // Mengambil komponen yang ada di player.
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();          // Gerakkan player
        PlayerFlip();   // Balik arah hadap
        ClimbLadder();  // Cek tangga
        Dead();
    }

    // Menerima perintah gerak dari keyboard/controller.
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        // Hanya boleh lompat kalau menyentuh tanah.
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            // Memberi dorongan ke atas.
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        // X mengikuti input, Y tetap mengikuti gerakan sebelumnya.
        // Ibaratnya: kita mengatur arah jalan tanpa mengubah lompatan/jatuh.
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed,myRigidbody.linearVelocity.y);

        myRigidbody.linearVelocity = playerVelocity;

        // Kalau X bergerak, nyalakan animasi lari.
        bool isRunning = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRun", isRunning);
    }

    void PlayerFlip()
    {
        // Cek apakah player sedang bergerak kiri/kanan.
        bool hasHorizontalFlip = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;

        if (hasHorizontalFlip)
        {
            // +1 = hadap kanan, -1 = hadap kiri.
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x),1f);
        }
    }

    void ClimbLadder()
    {
        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            // Di tangga, gravitasi dimatikan agar player tidak jatuh.
            myRigidbody.gravityScale = 0f;

            // X tetap, Y mengikuti input naik/turun.
            Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);

            myRigidbody.linearVelocity = climbVelocity;

            // Kalau bergerak vertikal, nyalakan animasi memanjat.
            bool isClimbing = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimb", isClimbing);
        }
        else
        {
            // Kalau keluar dari tangga, kembalikan gravitasi.
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Dead()
    {
        if (myRigidbody.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dead");
            myRigidbody.linearVelocity = deadKick;
            StartCoroutine(DeadEffect());
        }
    }

    private IEnumerator DeadEffect()
    {
        float alfa = 0f;
        
        // Membuat warna merah muncul perlahan
        while (alfa < 0.6f) // 0.6f adalah batas kepekatan warna merah (maksimal 1.0f)
        {
            alfa += Time.deltaTime * 2f; // Angka 2f mengatur kecepatan munculnya
            redEffect.color = new Color(1f, 0f, 0f, alfa);
            yield return null;
        }

        textGameOver.gameObject.SetActive(true);
    }
}