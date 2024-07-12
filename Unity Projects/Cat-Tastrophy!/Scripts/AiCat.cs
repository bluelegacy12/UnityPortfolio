using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCat : MonoBehaviour
{
    public static AiCat aicat;
    public GameObject mouse;
    public CharacterController controller;
    public Vector3 dir;
    public Vector3 movement;
    public Vector3 moveDir;
    public Animator anim;
    public bool isGrabbed = false;
    public GameObject mouth;
    public float turnSmoothTime = 0.1f;
    //need a variable that the smoothdampangle func will alter to create smooth velocity
    float turnSmoothVelocity;
    public float moveTime;
    public float moveDelay = 2;
    public float speed = 6f;
    public float baseSpeed = 6f;
    public GameObject[] speedTokens;
    public GameObject closestToken;
    public Vector3 startLocation;
    public GameObject goal;
    float cooldowndelay = 3;
    float cooldowntime = 3;
    float speeddelay = .1f;
    float speedtime = 1f;
    public float currentSpeed;
    public bool dashing = false;
    public GameObject Player;
    public SphereCollider dashCollider;
    public Rigidbody rb;
    public AudioSource sfx;
    public AudioClip speedDown;
    public AudioClip goldscore;
    public GameObject goldMouse;

    void Start()
    {
        aicat = this;
        Player = GameObject.Find("Player 1");
        startLocation = transform.position;
        speedTokens = GameObject.FindGameObjectsWithTag("SpeedToken");
        dashCollider = transform.GetComponent<SphereCollider>();
        dashCollider.enabled = false;
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < baseSpeed)
        {
            speed = baseSpeed;
        }
        if (speed > 24 && !dashing)
        {
            speed = 24;
        }
        if (!isGrabbed)
        {
            WallCheck();
        }
        speedTokens = GameObject.FindGameObjectsWithTag("SpeedToken");
        if (transform.position.y < -10)
        {
            transform.position = startLocation;
        }
        
        // Dash mechanic
        if (cooldowntime < cooldowndelay)
        {
            cooldowntime += Time.deltaTime;
        }
        if (speedtime < speeddelay)
        {
            speedtime += Time.deltaTime;
        }
        if (dashing && speedtime >= speeddelay)
        {
            speed = currentSpeed;
            dashCollider.enabled = false;
            dashing = false;
        }
    }

    void FixedUpdate()
    {
        if (!GoldMouse.goldMouse.hidden && !isGrabbed && goldMouse.transform.position.y < 10)
        {
            Movement(goldMouse);
            RotateCharacter(goldMouse);
            anim.SetTrigger("!isGrabbed");
        }
        if (!isGrabbed && (Mouse.mouse.isGrabbed || (GoldMouse.goldMouse.isGrabbed && goldMouse.transform.position.y < 10)))
        {
            Movement(Player);
            RotateCharacter(Player);
            anim.SetTrigger("!isGrabbed");
            if (Vector3.Distance(transform.position, Player.transform.position) < 6 && cooldowntime >= cooldowndelay)
            {
                Dash();
            }
        }
        // chase mouse if fast enough
        if(GoldMouse.goldMouse.hidden && ((!isGrabbed && speed > Mouse.mouse.speed) || (!isGrabbed && speedTokens.Length < 1)))
        {
            Movement(mouse);
            RotateCharacter(mouse);
            anim.SetTrigger("!isGrabbed");
        }
        else if(!isGrabbed && speed <= Mouse.mouse.speed && !Mouse.mouse.isGrabbed)
        {
            foreach(GameObject token in speedTokens)
            {
                if (closestToken == null || Vector3.Distance(token.transform.position, transform.position) < (Vector3.Distance(closestToken.transform.position, transform.position)))
                {
                    closestToken = token;
                }
                if (!closestToken.activeSelf && closestToken != null)
                {
                    closestToken = null;
                }
            }
            Movement(closestToken);
            RotateCharacter(closestToken);
            anim.SetTrigger("!isGrabbed");
        }
        if (isGrabbed)
        {
            anim.SetTrigger("isGrabbed");
            Movement(goal);
            RotateCharacter(goal);
        }
    }

    void RotateCharacter(GameObject obj)
    {
        Vector3 facing = obj.transform.position - transform.position;
        if(facing.magnitude < 1f) { return; }
       
        // Rotate toward target
        Quaternion awayRotation = Quaternion.LookRotation(facing);
        Vector3 euler = awayRotation.eulerAngles;
        euler.y -= 0;
        awayRotation = Quaternion.Euler(euler);
       
        // Rotate the game object.
        transform.rotation = Quaternion.Slerp(transform.rotation, awayRotation, 5 * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (dashing && collision.transform.name == "Player 1")
        {
            if (Mouse.mouse.isGrabbed)
            {
                Mouse.mouse.Eject();
            }
            if (GoldMouse.goldMouse.isGrabbed)
            {
                GoldMouse.goldMouse.Eject();
            }
        }
        if (collision.transform.tag == "SpeedToken")
        {
            speed += 3f;
            collision.gameObject.SetActive(false);
        }
        if (collision.transform.name == "Mouse")
        {
            speed -= 6f;
            isGrabbed = true;
        }
        if (collision.transform.name == "GoldMouse")
        {
            speed -= 6f;
            isGrabbed = true;
        }
        if ((collision.gameObject.name == "BlueGoal" || collision.gameObject.name == "RedGoal") && isGrabbed && Mouse.mouse.isGrabbed)
        {
            Mouse.mouse.RestartMouse();
            Mouse.mouse.UpdateScore(collision.gameObject);
            isGrabbed = false;
        }
        if ((collision.gameObject.name == "BlueGoal" || collision.gameObject.name == "RedGoal") && isGrabbed && GoldMouse.goldMouse.isGrabbed)
        {
            GoldMouse.goldMouse.RestartMouse();
            GoldMouse.goldMouse.UpdateScore(collision.gameObject);
            isGrabbed = false;
            sfx.PlayOneShot(goldscore);
            goldMouse.SetActive(false);
        }
    }
    void WallCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Wall")
            {
                transform.position = Vector3.MoveTowards(transform.position, hitCollider.gameObject.transform.position, speed * -Time.deltaTime);
            }
        }
    }
    void Dash()
    {
        dashing = true;
        dashCollider.enabled = true;
        currentSpeed = speed;
        speed *= 2;
        cooldowntime = 0;
        speedtime = 0;
    }
    void Movement(GameObject target)
    {
        dir = target.transform.position - transform.position;
        movement = dir.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }
}
