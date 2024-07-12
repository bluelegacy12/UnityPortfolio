using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMouse : MonoBehaviour
{
    public static GoldMouse goldMouse;
    public GameObject Player;
    public GameObject[] Players;
    public Vector3 moveDir;
    public Animator anim;
    public bool isGrabbed = false;
    public GameObject mouth;
    public LineRenderer lineRenderer;
    public LineRenderer tempLine;
    public Vector3 movement;
    public float turnSmoothTime = 0.1f;
    //need a variable that the smoothdampangle func will alter to create smooth velocity
    float turnSmoothVelocity;
    public GameObject wall;
    public GameObject[] walls;
    public float moveTime;
    public float moveDelay = 2;
    public Vector3 startLocation;
    public float speed = 25f;
    public GameObject redGoal;
    public GameObject blueGoal;
    public TMPro.TextMeshProUGUI bluescore;
    public TMPro.TextMeshProUGUI redscore;
    public GameObject EndScreen;
    public TMPro.TextMeshProUGUI EndText;
    Vector3 startScale;
    public AudioSource sfx;
    public AudioClip playerScore;
    public AudioClip AiScore;
    public AudioClip gamewin;
    public AudioClip gamelose;
    public AudioClip eject;
    public AudioClip grab;
    public bool hidden = true;
    public GameObject Player1;
    public Renderer pr;

    void Start()
    {
        goldMouse = this;
        startScale = transform.localScale;
        startLocation = transform.position;
        Players = GameObject.FindGameObjectsWithTag("Player");
        // anim = ThirdPersonMovement.thirdPersonMovement.GetComponent<Animator>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        Player1 = GameObject.Find("Player 1");
        //pr = Player1.GetComponentInChildren<Renderer>();
        foreach (GameObject w in walls)
        {
            w.AddComponent<MeshCollider>();
            w.layer = 3;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hidden)
        {
            transform.position = startLocation;
        }
        foreach(GameObject p in Players)
        {
            if (Player == null || Vector3.Distance(p.transform.position, transform.position) < (Vector3.Distance(Player.transform.position, transform.position)))
            {
                Player = p;
                mouth = Player.transform.Find("mouth").gameObject;
            }
        }
        if (transform.position.y < -3)
        {
            RestartMouse();
        }
        if (isGrabbed)
        {
            anim.SetTrigger("isGrabbed");
            transform.localRotation = Quaternion.Euler(0, 90, 0);
            hidden = false;
        }
        if (Input.GetButtonDown("Fire2") && isGrabbed)
        {
            Eject();
        }
    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        // run from player
        if(Vector3.Distance(Player.transform.position, transform.position) <= 15 && !isGrabbed)
        {
            if (Physics.Raycast(transform.position, fwd, out hit, 5))
            {
                WallCheck(hit);
            }
            else
            {
                Movement(Player);
            }
            RotateMouse();
            anim.SetTrigger("!isGrabbed");
        }
    }

    void RotateMouse()
    {
        Vector3 facing = Player.transform.position - transform.position;
        if(facing.magnitude < 1f) { return; }
       
        // Rotate the rotation AWAY from the player...
        Quaternion awayRotation = Quaternion.LookRotation(facing);
        Vector3 euler = awayRotation.eulerAngles;
        euler.y -= 180;
        awayRotation = Quaternion.Euler(euler);
       
        // Rotate the game object.
        transform.rotation = Quaternion.Slerp(transform.rotation, awayRotation, 5 * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

    }
    public void Eject()
    {
        sfx.PlayOneShot(eject);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.SetParent(null);
        transform.gameObject.AddComponent<Rigidbody>();
        transform.Translate(transform.forward * 3, Player.transform);
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        isGrabbed = false;
    }

    public void RestartMouse()
    {
        if (Player.transform.name == "Player 2")
        {
            AiCat.aicat.isGrabbed = false;
        }
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.SetParent(null);
        transform.gameObject.AddComponent<Rigidbody>();
        transform.position = startLocation;
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.localScale = startScale;
        isGrabbed = false;
        hidden = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player && !isGrabbed)
        {
            Destroy(transform.GetComponent<Rigidbody>());
            hidden = false;
            transform.SetParent(Player.transform);
            isGrabbed = true;
            if (Player == GameObject.Find("Player 1"))
            {
                if (ThirdPersonMovement.thirdPersonMovement.dashing)
                {
                    ThirdPersonMovement.thirdPersonMovement.mouseGrab = true;
                }
                ThirdPersonMovement.thirdPersonMovement.speed -= 9f;
            }
            transform.position = mouth.transform.position;
            sfx.PlayOneShot(grab);
        }
        if (collision.gameObject.tag == "Wall" && !isGrabbed)
        {
            wall = collision.gameObject;
        }
    }
    
    public void UpdateScore(GameObject goal)
    {
        if (goal.name == "BlueGoal")
        {
            int score = int.Parse(bluescore.text);
            score += 3;
            bluescore.text = score.ToString();
            sfx.PlayOneShot(playerScore);
            pr.material.SetColor("_Color", Color.yellow);
        }
        if (goal.name == "RedGoal")
        {
            int score = int.Parse(redscore.text);
            score += 3;
            redscore.text = score.ToString();
            sfx.PlayOneShot(AiScore);
        }
        
    }
    void WallCheck(RaycastHit hit)
    {
        Vector3 dir = transform.position - (Player.transform.position + hit.transform.position);
        Vector3 movement = dir.normalized * speed * Time.deltaTime;
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.MovePosition(transform.position + movement);
    }
    void Movement(GameObject target)
    {
        Vector3 dir = transform.position - target.transform.position;
        Vector3 movement = dir.normalized * speed * Time.deltaTime;
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.MovePosition(transform.position + movement);
    }
}
