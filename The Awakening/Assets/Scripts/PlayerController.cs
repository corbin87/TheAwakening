using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Transform-related variables
    [SerializeField]
    private Rigidbody2D theRB;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Animator myAnim;

    // Status and instance variables
    public static PlayerController instance;
    public string areaTransitionedTo;
    public bool inTransitionArea;
    public bool transitionDone;

    // Stops player at tilemap border
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;
    public bool canMove;

    // Player keeps a reference to the scene's camera for bounds checking
    public CameraController sceneCamera;

    // Awake is called before Start() and before the first frame update
    void Awake()
    {
        // Establish player singleton
        if (instance == null)
        {
            instance = this;
        }

        // Preserves character object through scene transitions
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        transitionDone = true;
        moveSpeed = 8;
        theRB = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Establish player movement behavior
        Movement();

        // Clamp player's position to tilemap dimensions so player can't go offscreen
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    // Tie character to visible screen/tilemap
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        // Get float variable for half player width and height
        float xOffset = (GetComponent<Renderer>().bounds.size.x) / 2;
        float yOffset = (GetComponent<Renderer>().bounds.size.y) / 2;
        // Set bounds, offsetting player box to avoid clipping
        bottomLeftLimit = botLeft + new Vector3(xOffset, yOffset, 0);
        topRightLimit = topRight + new Vector3(-xOffset, -yOffset, 0);
    }

    private void Movement()
    {
        if (transitionDone == true && canMove)
        {
            // Link character movement to axis input
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        else
        {
            theRB.velocity = new Vector2(0,0);
        }

        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        // Set last moved variables to determine which way character sprint faces at end of movement
        if (theRB.velocity.x != 0 || theRB.velocity.y != 0)
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }
}
