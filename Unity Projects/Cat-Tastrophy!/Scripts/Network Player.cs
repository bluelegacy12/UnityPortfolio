using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Mirror;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform mouse;
    public bool mouseFollow = false;
    public Transform cam3;
    public PlayerInput playerInput;
    public static NetworkPlayer networkPlayer;
    Animator anim;
    public Transform cam;
    public CharacterController controller;
    public float baseSpeed = 6f;
    public float speed = 6f;
    //smooth time helps us smooth the turning of our char
    public float turnSmoothTime = 0.1f;
    //need a variable that the smoothdampangle func will alter to create smooth velocity
    float turnSmoothVelocity;
    public float jumpforce = 4f;
    public Rigidbody rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isGrounded = true;
    public bool mouseGrab = false;
    public float groundDistance = 0.4f;
    Vector3 velocity;
    public float gravity = -50f;
    public bool platformCheck;
    public GameObject Platform;
    public float horizontal;
    public float vertical;
    public Vector3 moveDir;
    float cooldowndelay = 3;
    float cooldowntime = 3;
    float speeddelay = .1f;
    float speedtime = 1f;
    float currentSpeed;
    public TMPro.TextMeshProUGUI dashcd;
    public Canvas canvas;
   public Vector3 startLocation;
   public GameObject token1;
   public GameObject token2;
   public GameObject token3;
   public float tokenRespawnDelay = 10f;
   public float tokenTimer1 = 0f;
   public float tokenTimer2 = 0f;
   public float tokenTimer3 = 0f;
   public GameObject[] speedHud;
   public bool dashing = false;
   public SphereCollider dashCollider;
   public bool paused = false;
   public GameObject PauseScreen;
   public bool tokencollected = false;
   public GameObject EndScreen;
   public GameObject instructions;
   public AudioClip startMusic;
   public AudioSource bgm;
   public AudioSource sfx;
   public AudioClip speedUp;
   public AudioClip hit;
   public AudioClip dashaudio;
   public AudioClip speedDown;
   public AudioClip select;
   public AudioClip jumpSound;
   public AudioClip goldscore;
   public GameObject goldMouse;
   public bool assist = true;
   public bool assistDash = false;

    void Awake()
    {
        networkPlayer = this;
    }
    void Start()
    {
        Time.timeScale = 1;
        startLocation = transform.position;
        currentSpeed = speed;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        dashcd = GameObject.Find("DashCD").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        dashCollider = transform.GetComponent<SphereCollider>();
        dashCollider.enabled = false;
        speedHud = new GameObject[7];
        for (int i = 0; i < 7; i++)
        {
            speedHud[i] = GameObject.Find("S" + i);
        }
        // Cursor.lockState = CursorLockMode.Locked; // use this to hide cursor
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (EndScreen.activeSelf == true)
        {
            Time.timeScale = 0;
        }
        if ((Input.GetButtonDown("Submit") || playerInput.actions["Pause"].triggered) && EndScreen.activeSelf == false)
        {
            sfx.PlayOneShot(select);
            if (instructions.activeSelf == true)
            {
                return;
            }
            if (paused)
            {
                Time.timeScale = 1;
                paused = false;
                PauseScreen.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
                PauseScreen.SetActive(true);
            }
        }
        if (speed < baseSpeed)
        {
            speed = baseSpeed;
        }
        if (!dashing)
        {
            if (speed > 24)
            {
                speed = 24;
            }
            if (speed == 24)
            {
                speedHud[6].SetActive(true);
            }
            else
            {
                speedHud[6].SetActive(false);
            }
            if (speed >= 21)
            {
                speedHud[5].SetActive(true);
            }
            else
            {
                speedHud[5].SetActive(false);
                
            }
            if (speed >= 18)
            {
                speedHud[4].SetActive(true);
            }
            else
            {
                speedHud[4].SetActive(false);
                
            }
            if (speed >= 15)
            {
                speedHud[3].SetActive(true);
            }
            else
            {
                speedHud[3].SetActive(false);
                
            }
            if (speed >= 12)
            {
                speedHud[2].SetActive(true);
            }
            else
            {
                speedHud[2].SetActive(false);
                
            }
            if (speed >= 9)
            {
                speedHud[1].SetActive(true);
            }
            else
            {
                speedHud[1].SetActive(false);
                
            }
        }
        if (assist && mouse.transform.parent != transform && goldMouse.transform.parent != transform)
        {
            if (dashing && Vector3.Distance(transform.position, mouse.transform.position) < 5)
            {
                assistDash = true;
                Vector3 dir = mouse.transform.position - transform.position;
                Vector3 movement = dir.normalized * speed * Time.deltaTime;
                controller.Move(movement);
            }
            if (dashing && Vector3.Distance(transform.position, goldMouse.transform.position) < 5)
            {
                assistDash = true;
                Vector3 dir = goldMouse.transform.position - transform.position;
                Vector3 movement = dir.normalized * speed * Time.deltaTime;
                controller.Move(movement);
            }
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (!isGrounded && platformCheck)
        {
            platformCheck = false;
            transform.SetParent(null);
            transform.gameObject.AddComponent<Rigidbody>();
            rb = transform.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (cooldowntime < cooldowndelay)
        {
            cooldowntime += Time.deltaTime;
            string newText = (cooldowndelay - cooldowntime).ToString();
            dashcd.text = newText.Substring(0, 3);
        }
        else
        {
            dashcd.text = "";
        }
        if (speedtime < speeddelay && dashing)
        {
            speedtime += Time.deltaTime;
        }
        else if (dashing)
        {
            speed = currentSpeed;
            if (tokencollected)
            {
                speed += 3;
                tokencollected = false;
                sfx.PlayOneShot(speedUp);
            }
            if (mouseGrab)
            {
                speed -= 6;
                mouseGrab = false;
            }
            dashCollider.enabled = false;
            dashing = false;
            assistDash = false;
        }
        if (Input.GetButtonDown("Fire3") || playerInput.actions["LockOn"].triggered)
        {
            if (mouseFollow)
            {
                cam3.GetComponent<CinemachineFreeLook>().LookAt = transform;
                mouseFollow = false;
            }
            else
            {
                cam3.GetComponent<CinemachineFreeLook>().LookAt = mouse;
                mouseFollow = true;
            }
            
        }
        if (Input.GetButtonDown("Fire1") || playerInput.actions["Dash"].triggered)
        {
            if (cooldowntime < cooldowndelay)
            {
                return;
            }
            dashing = true;
            dashCollider.enabled = true;
            currentSpeed = speed;
            speed = speed * 4;
            speedtime = 0f;
            cooldowndelay = 3;
            cooldowntime = 0;
            sfx.PlayOneShot(dashaudio);
        }
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        // this implements mvmnt
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        // normalized prevents horizontal mvmnt from making char faster
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (move.magnitude >= 0.1f)
        {
            move = move.x * cam.right + move.z * cam.forward;
            move.y = 0;
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg; // the cam part makes the player movement line up with camera
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!assistDash)
            {
                controller.Move(move.normalized * speed * Time.deltaTime);
            }
            anim.SetTrigger("Move");
        }
        // .magnitude is like .length for Vector3
        else if (direction.magnitude >= 0.1f)
        {
            // Atan2 func that returns angle between x-axis and vector starting at 0 on x-axis and ending at x,y
            // the func finds the angle between 0,0 and x,y - assuming the x-axis is the base of the angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // the cam part makes the player movement line up with camera
            // Atan2 returns value in radians, Rad2Degrees converts it to degrees
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!assistDash)
            {
                controller.Move(direction.normalized * speed * Time.deltaTime);
            }
            anim.SetTrigger("Move");
        }
        else
        {
            anim.SetTrigger("Stop");
        }
        Jumping();
        velocity.y += gravity * 2 * Time.deltaTime;
        if (!assistDash)
        {
            controller.Move(velocity * Time.deltaTime);
        }
        if (tokenTimer1 < tokenRespawnDelay && !token1.activeSelf)
        {
            tokenTimer1 += Time.deltaTime;
            if (tokenTimer1 >= tokenRespawnDelay)
            {
                token1.SetActive(true);
                tokenTimer1 = 0f;
            }
        }
        if (tokenTimer2 < tokenRespawnDelay && !token2.activeSelf)
        {
            tokenTimer2 += Time.deltaTime;
            if (tokenTimer2 >= tokenRespawnDelay)
            {
                token2.SetActive(true);
                tokenTimer2 = 0f;
            }
        }
        if (tokenTimer3 < tokenRespawnDelay && !token3.activeSelf)
        {
            tokenTimer3 += Time.deltaTime;
            if (tokenTimer3 >= tokenRespawnDelay)
            {
                token3.SetActive(true);
                tokenTimer3 = 0f;
            }
        }
    }

    void Jumping()
    {
        if ((Input.GetButtonDown("Jump") || playerInput.actions["Jump"].triggered) && isGrounded)
        {
            //rb.velocity = Vector3.up * jumpforce * 2f / -Physics.gravity.y;
            velocity.y = Mathf.Sqrt(jumpforce * -4 * gravity);
            isGrounded = false;
            sfx.PlayOneShot(jumpSound);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Platform = collision.gameObject;
            platformCheck = true;
            Destroy(transform.GetComponent<Rigidbody>());
            transform.SetParent(Platform.transform);
        }
        if (collision.transform.tag == "SpeedToken" || collision.transform.tag == "SpeedTokenHidden")
        {
            if (dashing)
            {
                tokencollected = true;
            }
            speed += 3f;
            sfx.PlayOneShot(speedUp);
            collision.gameObject.SetActive(false);
        }
        if (collision.transform.tag == "Floor")
        {
            isGrounded = true;
        }
        if (dashing && collision.gameObject.name == "Player 2" )
        {
            AiCat.aicat.speed -= 6f;
            speed = currentSpeed;
            dashCollider.enabled = false;
            dashing = false;
            assistDash = false;
            if(AiCat.aicat.isGrabbed)
            {
                if(Mouse.mouse.isGrabbed)
                {
                    Mouse.mouse.Eject();
                }
                if(GoldMouse.goldMouse.isGrabbed)
                {
                    GoldMouse.goldMouse.Eject();
                }
                AiCat.aicat.isGrabbed = false;
            }
            sfx.PlayOneShot(hit);
        }
        if ((collision.gameObject.name == "BlueGoal" || collision.gameObject.name == "RedGoal") && Mouse.mouse.isGrabbed && !AiCat.aicat.isGrabbed)
        {
            Mouse.mouse.RestartMouse();
            Mouse.mouse.UpdateScore(collision.gameObject);
        }
        if ((collision.gameObject.name == "BlueGoal" || collision.gameObject.name == "RedGoal") && GoldMouse.goldMouse.isGrabbed && !AiCat.aicat.isGrabbed)
        {
            GoldMouse.goldMouse.RestartMouse();
            GoldMouse.goldMouse.UpdateScore(collision.gameObject);
            sfx.PlayOneShot(goldscore);
            goldMouse.SetActive(false);
        }
        if (collision.gameObject.name == "Player 2" && AiCat.aicat.dashing)
        {
            speed -= 6f;
            if (Mouse.mouse.isGrabbed && !AiCat.aicat.isGrabbed)
            {
                Mouse.mouse.Eject();
            }
            if (GoldMouse.goldMouse.isGrabbed && !AiCat.aicat.isGrabbed)
            {
                GoldMouse.goldMouse.Eject();
            }
            sfx.PlayOneShot(speedDown);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.SetParent(null);
            transform.gameObject.AddComponent<Rigidbody>();
            rb = transform.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
