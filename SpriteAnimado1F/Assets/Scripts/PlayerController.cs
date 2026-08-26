using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;

    [Header("Sensor Settings")]
    [SerializeField] private Transform sensorGroundPosition;
    [SerializeField] private Vector3 sensorSize;

    private float currentJumpDuration;
    private float moveX;
    private bool isOnGround;

    private Rigidbody2D rigidbody2D;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    // 0.007
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal") * speed;

        if (moveX > 0)
        {
            transform.eulerAngles = new Vector2(0.0f, 0.0f);
        }
        else if (moveX < 0)
        {
            transform.eulerAngles = new Vector2(0.0f, 180.0f);
        }
            
        isOnGround = Physics2D.OverlapBox(sensorGroundPosition.position, sensorSize, 0.0f, 
            1 <<  LayerMask.NameToLayer("Ground"));


        if (Input.GetButtonDown("Jump") && isOnGround == true)
        {
            currentJumpDuration = jumpDuration;
        }
        else if (Input.GetButton("Jump") && currentJumpDuration > 0.0f) 
        {
            currentJumpDuration -= Time.deltaTime;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            currentJumpDuration = 0.0f;
        }

        

    }

    // 0.02
    void FixedUpdate()
    {
        rigidbody2D.linearVelocity = new Vector2(moveX, rigidbody2D.linearVelocityY);

        if (currentJumpDuration > 0.0f)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(sensorGroundPosition.position, sensorSize);
    }

}
