using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Sprites")]
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {


        rb.velocity = moveInput.sqrMagnitude > 1f ? moveInput.normalized * moveSpeed : moveInput * moveSpeed;
    }

    void UpdateSpriteDirection()
    {
        if (moveInput.y > 0)        // Haut d'abord
            sr.sprite = upSprite;
        else if (moveInput.y < 0)   // Bas ensuite
            sr.sprite = downSprite;
        else if (moveInput.x > 0)   // Droite si pas vertical
            sr.sprite = rightSprite;
        else if (moveInput.x < 0)   // Gauche si pas vertical
            sr.sprite = leftSprite;
    }
}
