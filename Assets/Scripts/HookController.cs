    using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public Transform holdPoint;
    public float rotationSpeed = 250f;
    public float throwSpeed = 5f;
    public float rotation;
    public float range = 5f;
    public Sprite chainSprite;
    public float chainSizeY = 0.2f;
    public float playerClimbSpeed = 1f;
    public float playerSwingSpeed = 10f;
    public float playerSwingAmount = 0.5f;
    public float playerJumpSpeed = 3f;
    public float playerJumpHeight = 0.8f;

    private Vector3 rotateAround;
    private Vector3 hookStartPos;
    private Vector3 initialPos;
    private Vector3 initialLocalPos;
    private Quaternion initialRotation;
    private Quaternion initialLocalRotation;

    private bool isThrown = false;
    private bool isPlayerClimbing = false;

    private Rigidbody2D rb;
    private Vector3 cameraStartPos;
    private Transform player;
    private Rigidbody2D playerRb;
    private Vector3 playerInitialPos;
    private Animator playerAnimator;


    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        // hold point is the point where the hook is attached and rotates around
        holdPoint = transform.parent;

        player = holdPoint.parent;
        playerInitialPos = player.position;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponent<Animator>();

        cameraStartPos = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
        rotateAround = holdPoint.position;

        initialPos = transform.position;
        initialRotation = transform.rotation;
        initialLocalPos = transform.localPosition;
        initialLocalRotation= transform.localRotation;
        //Debug.Log("hookStartPos: " + initialPos + "hookStartLocalPos" + initialLocalPos);
        CreateChain(rotateAround, transform.position);
        rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;
        RaycastHit2D hit = Physics2D.Raycast(player.position, -player.up, range);
        if (hit)
        {
            Debug.Log("hit: " + hit.collider.gameObject.name);
            StartCoroutine(JumpToPlatform(player.position, hit.collider.gameObject.GetComponent<Platform>(), 0.1f));
        }
    }

    void Update()
    {
        if (gameManager.GetGameState() == GameManager.GameState.GameOver) return;        
            
        
        rotation = transform.rotation.eulerAngles.z;
        playerAnimator.SetBool("isClimbing", isPlayerClimbing);
        if (isThrown && ((Mathf.Abs((player.transform.position - transform.position).magnitude) > range) || Mathf.Abs(transform.position.x) > Camera.main.orthographicSize/2 )) { // if too far away from player
            ResetHook();
        }
        if (isThrown)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, rb.position.y, Camera.main.transform.position.z);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            CreateChain(player.transform.position, transform.position);
        }else
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, player.position.y, Camera.main.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isPlayerClimbing && !isThrown)
        {
            ResetScene(); // teleport player to initial position DO NOT USE THIS IN GAME - WILL BE REMOVED
         
        }
        if (!isThrown && !isPlayerClimbing && Input.GetMouseButton(0)) // hold mouse click to rotate the hook and aim
        {
            RotateHook();

            
        }
        if (!isThrown && !isPlayerClimbing && Input.GetMouseButtonUp(0)) // release mouse click to throw the hook
        {
            ThrowHook();
        }
    }

    

    void CreateChain(Vector3 from, Vector3 to) // creates chain sprites between two points
    {
        Vector3 v = to - from;
        for (int i = 0; i < Mathf.FloorToInt (v.magnitude / chainSizeY); i++)
        {
            Vector3 p = v * i / Mathf.FloorToInt (v.magnitude / chainSizeY) + from;
            GameObject newChain = new GameObject();
            SpriteRenderer chainsRenderer = newChain.AddComponent<SpriteRenderer>();
            chainsRenderer.sprite = chainSprite;
            newChain.transform.position = p;
            newChain.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
            newChain.transform.parent = transform;
        }
    }
    void RotateHook()
    {
        transform.RotateAround(rotateAround, Vector3.forward, rotationSpeed * Time.deltaTime);

        /*
        // -----------Mouse Rotation----------------
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - rotateAround; 
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        angle-=90f;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Smoothly rotate the hook
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        // Move the hook closer to the character (rope part)
        
        transform.position = rotateAround +  transform.up * 1f; // Adjust the distance as needed
        */
    }

    void ThrowHook() // throw the hooks rigidbody
    {
        isThrown = true;
        rb.simulated = true;
        rb.velocity = transform.right * throwSpeed ;
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && isThrown) // if hook hits a platform, transfer player to the platform
        {
            
            Platform platform = collision.gameObject.GetComponent<Platform>();
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            isThrown = false;
            float distance = Mathf.Abs( Vector3.Distance(transform.position, player.transform.position) ); // hook is thrown so the distance is the distance between player and hook
            StartCoroutine(MovePlayer(transform.position, platform, distance / playerClimbSpeed)); // slowly move player to the platform
            
            
            
        }
        
    }
    void TeleportPlayer(Vector3 to) // teleport player to a position
    { 
        isPlayerClimbing = false;
        player.position = to;
        ResetHook();
        /*
        isThrown = false;
        rb.velocity = Vector2.zero;
        rb.rotation = 0f;
        rb.simulated = false;

        rotateAround = holdPoint.position;
        //hookStartPos = rotateAround + Vector3.up * 0.8f;
        /*hookStartPos = rotateAround + hookStartLocalPos;
        transform.localPosition = hookStartLocalPos;
        transform.position = hookStartPos;
        transform.rotation = Quaternion.identity;
        *
        transform.localPosition = initialTransform.position;
        transform.localRotation = initialTransform.rotation;
        CreateChain(rotateAround, transform.position);
        */
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, player.position.y, Camera.main.transform.position.z);
    }

    private void ResetScene() // DO NOT USE THIS IN GAME - WILL BE REMOVED
    { 
        
        player.position = playerInitialPos;
        //hookStartPos = rotateAround + Vector3.up * 0.8f;
        ResetHook();
        
        
        cameraStartPos = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = cameraStartPos;

        
    }
    private void ResetHook() // reset hook next to player
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        isThrown = false;
        rb.velocity = Vector2.zero;
        rb.rotation = 0f; //holdPoint.rotation.z;
        rb.simulated = false;

        //player.position = playerInitialPos;
        rotateAround = holdPoint.position;

        //hookStartPos = rotateAround + Vector3.up * 0.8f;

        transform.rotation = Quaternion.Euler(0,0,0);//holdPoint.rotation;

        transform.localRotation = initialLocalRotation;
        transform.localPosition = initialLocalPos;
        //Debug.Log("resetted, initial pos: " + initialPos + "initialLocalPos: " + initialLocalPos + "pos:" + transform.position + "localpos:" + transform.localPosition );
        CreateChain(rotateAround, transform.position);
    }
    private IEnumerator MovePlayer(Vector3 hit, Platform platform, float time) // move player to the platform over time
    {
        isPlayerClimbing = true;
        transform.parent = null;
        //player.rotation = transform.rotation;
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
        Vector3 startingPos = playerRb.position;

        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            
            playerRb.position = Vector3.Lerp(startingPos, hit , (elapsedTime / time));
            playerRb.position += new Vector2( Mathf.Sin(playerRb.position.y * playerSwingSpeed) * playerSwingAmount, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(JumpToPlatform(hit, platform, playerJumpHeight/playerJumpSpeed)); // overshoot the platform a little and jump on it
        
    }
    
    private IEnumerator JumpToPlatform(Vector3 hit, Platform platform, float time) // for animation purposes
    { 
        Debug.Log("jumping to platform" + platform.gameObject.name);
        player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, 0);
        float elapsedTime = 0;
        Vector3 startingPos = playerRb.position;
        Vector3 to = platform.standPoint.position;
        //Debug.Log("standPoint: " + to);
        Vector3 toOverShoot = (hit + to)/2 + Vector3.up * playerJumpHeight;
        //Debug.Log("overshoot: " + toOverShoot);
        while (elapsedTime < time)
        {
            playerRb.position = Vector3.Lerp(startingPos, toOverShoot, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        startingPos = playerRb.position;
        //Debug.Log("new TO: " + to);
        while (elapsedTime < time)
        {
            playerRb.position = Vector3.Lerp(startingPos, to , (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("endedup at: " + playerRb.position);
        transform.parent = holdPoint;
        foreach (Transform child in transform) // destroy chain sprites
        {
            Destroy(child.gameObject);
        }
        platform.SetCurrent();
        TeleportPlayer(platform.standPoint.position);
    }
}
