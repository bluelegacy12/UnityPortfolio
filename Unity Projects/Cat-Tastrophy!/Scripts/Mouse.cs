using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
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
    public float speed = 20f;
    public static Mouse mouse;
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
    public AudioClip endgame;
    public AudioSource bgm;

    void Start()
    {
        mouse = this;
        startScale = transform.localScale;
        startLocation = transform.position;
        Players = GameObject.FindGameObjectsWithTag("Player");
        // anim = ThirdPersonMovement.thirdPersonMovement.GetComponent<Animator>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject w in walls)
        {
            w.AddComponent<MeshCollider>();
            w.layer = 3;

        }
    }

    // Update is called once per frame
    void Update()
    {
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
            transform.position = startLocation;
            transform.SetParent(null);
        }
        if (isGrabbed)
        {
            anim.SetTrigger("isGrabbed");
            transform.localRotation = Quaternion.Euler(0, 90, 0);
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
        // move randomly every 2 sec
        if(Vector3.Distance(Player.transform.position, transform.position) > 15 && !isGrabbed)
        {
            if (moveTime < moveDelay)
            {
                moveTime += Time.deltaTime;
            }
            else
            {
                int[] movex = {-3, 3};
                int[] movey = {0, 2};
                int[] movez = {-3, 3};
                int randIntX = Random.Range(0, 1);
                int randIntY = Random.Range(0, 1);
                int randIntZ = Random.Range(0, 1);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + movex[randIntX], transform.position.y + movey[randIntY], transform.position.z + movez[randIntZ]), 3 * Time.deltaTime);
                moveTime = 0;
            }
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
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.SetParent(null);
        transform.gameObject.AddComponent<Rigidbody>();
        transform.position = startLocation;
        Rigidbody rb = transform.gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.localScale = startScale;
        isGrabbed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player && !isGrabbed)
        {
            Destroy(transform.GetComponent<Rigidbody>());
            transform.SetParent(Player.transform);
            isGrabbed = true;
            if (Player == GameObject.Find("Player 1"))
            {
                if (ThirdPersonMovement.thirdPersonMovement.dashing)
                {
                    ThirdPersonMovement.thirdPersonMovement.mouseGrab = true;
                }
                ThirdPersonMovement.thirdPersonMovement.speed -= 6f;
            }
            transform.position = mouth.transform.position;
            sfx.PlayOneShot(grab);
        }
        if (collision.gameObject.tag == "Wall" && !isGrabbed)
        {
            wall = collision.gameObject;
        }
        if (collision.gameObject.name == "BlueGoal")
        {
            RestartMouse();
            int score = int.Parse(bluescore.text);
            score += 1;
            bluescore.text = score.ToString();
        }
        if (collision.gameObject.name == "RedGoal")
        {
            RestartMouse();
            int score = int.Parse(redscore.text);
            score += 1;
            redscore.text = score.ToString();
        }
    }
    public void UpdateScore(GameObject goal)
    {
        if (goal.name == "BlueGoal")
        {
            int score = int.Parse(bluescore.text);
            score += 1;
            bluescore.text = score.ToString();
            sfx.PlayOneShot(playerScore);
        }
        if (goal.name == "RedGoal")
        {
            int score = int.Parse(redscore.text);
            score += 1;
            redscore.text = score.ToString();
            sfx.PlayOneShot(AiScore);
        }
        if (int.Parse(bluescore.text) >= 10)
        {
            EndText.text = "Player 1 Wins!";
            EndText.color = new Color(0, 121, 255, 255);
            EndScreen.SetActive(true); 
            sfx.PlayOneShot(gamewin);
            bgm.clip = endgame;
            bgm.Play();
        }
        if (int.Parse(redscore.text) >= 10)
        {
            EndText.text = "Player 2 Wins!";
            EndText.color = new Color(255, 0, 0, 255);
            EndScreen.SetActive(true);
            sfx.PlayOneShot(gamelose);
            bgm.clip = endgame;
            bgm.Play();
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
