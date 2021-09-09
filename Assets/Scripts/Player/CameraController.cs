using UnityEngine;

//manages all movement and properties for camera

public class CameraController : MonoBehaviour
{
    public GameObject camHolder;    //camera holder to allow shakes, etc.
    public GameObject player;       //player game object
    public GameObject crosshair;    //crosshair aiming reticle
    public GameObject mapCrosshair; //crosshair for map

    public float verticalBound;             //verticle limit for camera movement
    public float horizontalBound;           //horizontal limit for camera movement
    public static CameraController instace; 

    public float alph = 0.5f;               //alpha amount of fade from black

    private Vector3 offsetPlayer;           //offset distance between the player and camera
    private Vector3 offsetAim;              //offset distance for aiming
    private Vector3 mouseDistanceToPlayer;  //distance from mouse to player
    private Vector3 target;                 //target for camera to move toward
    private Vector3 tempPos;
    private Vector3 tempPosCross;
    private GameObject tempCrosshair;
    private Texture2D blk;

    private bool isFading = false;      //track if currently fading

    void Start()
    {
        //calculate and store the offset value by getting the distance between the player's position and camera's position.
        offsetPlayer = camHolder.transform.position - player.transform.position;
        offsetAim.z = 0;
        instace = this;

        //set black texture for fade in
        blk = new Texture2D(1, 1);
        blk.SetPixel(0, 0, new Color(0, 0, 0, 0));
        blk.Apply();

        //set crosshair for map
        tempCrosshair = mapCrosshair;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blk);  //add fade to GUI
    }

    void FixedUpdate()
    {
        if (!isFading)  //fade in
        {
            if (alph > 0)
            {
                alph -= Time.deltaTime * .2f;
                if (alph < 0) { alph = 0f; }
                blk.SetPixel(0, 0, new Color(0, 0, 0, alph));
                blk.Apply();
            }
        }

        if (isFading)   //fade out
        {
            if (alph < 1)
            {
                alph += Time.deltaTime * .2f;
                if (alph > 1) { alph = 1f; }
                blk.SetPixel(0, 0, new Color(0, 0, 0, alph));
                blk.Apply();
            }
        }

        if (!PlayerController.instace.isGamePadModeAim) //check for not gamepad mode
        {
            //position crosshair on cursor
            tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempPos.z = 5f;

            crosshair.transform.position = tempPos;

            //set camera offset based on mouse pos and screen size
            offsetAim.x = Input.mousePosition.x;
            offsetAim.y = Input.mousePosition.y;

            offsetAim.x -= Screen.width / 2;
            offsetAim.y -= Screen.height / 2;

            offsetAim.x /= 70;
            offsetAim.y /= 70;

            //ensure offset does not hit vertical bounds
            if (offsetAim.y > (Screen.height / 100) * verticalBound)
                offsetAim.y = (Screen.height / 100) * verticalBound;
            else if (offsetAim.y < -((Screen.height / 100)) * verticalBound)
                offsetAim.y = -((Screen.height / 100) * verticalBound);

            //ensure offset does not hit horizontal bounds
            if (offsetAim.x > (Screen.width / 100) * horizontalBound)
                offsetAim.x = (Screen.width / 100) * horizontalBound;
            else if (offsetAim.x < -((Screen.width / 100) * horizontalBound))
                offsetAim.x = -((Screen.width / 100) * horizontalBound);
        }

        else //game pad aiming
        {
            tempPosCross = camHolder.transform.position;
            if (Mathf.Abs(Input.GetAxis("xAim")) > 0.3 || Mathf.Abs(Input.GetAxis("yAim")) > 0.3) //check if player is aiming
                tempPosCross.z = 5f;     //show crosshair
            else
                tempPosCross.z = -10f;   //hide crosshair
            crosshair.transform.position = tempPosCross; //set crosshair for game pad

            //set offset for joystick pos
            offsetAim.x = Input.GetAxis("xAim") * 5;
            offsetAim.y = Input.GetAxis("yAim") * 5;
        }

        
        //Camera.main.orthographicSize = (float)(Screen.width / (1 * 64) * 0.5);  //set size based on screen size

        target = player.transform.position + offsetPlayer + offsetAim;          //set target based on player and aim offset

        camHolder.transform.position = target;            

        //set crosshair scale
        crosshair.transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }
}