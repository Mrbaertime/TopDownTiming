using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new InputSystem_Actions();
    }

    ////เสริมมาก่อน เอามาใช้ก่อน
    void Start()
    {
        GetComponent<Health>().OnDeath += OnPlayerDeath;
    }


    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // อ่านค่า input
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // เคลื่อนที่
        rb.linearVelocity = moveInput * moveSpeed;
    }

    //เสริมมาก่อน เอามาใช้ก่อน
    void OnPlayerDeath()
    {
        GameManager.Instance.GameOver();
    }
}