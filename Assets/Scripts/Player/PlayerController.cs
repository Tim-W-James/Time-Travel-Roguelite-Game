using EZCameraShake;                    //camera shake
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using XInputDotNetPure;                 //controller rumble
using Random = UnityEngine.Random;      //tells Random to use the Unity Engine random number generator.

public class PlayerController : MonoBehaviour
{

    //public variables    
    public float speedBase = 35;                 //player movement speed
    public float projectileSpeed = 50;  //projectile speed
    public float meleeRate = 0.3f;       //projectile shot rate
    public float fireRate = 0.2f;       //projectile shot rate
    public float shieldDuration = 1.5f;       //projectile shot rate
    public float dashTime = 0.4f;      //dash duration
    public float dashSpeed = 10f;
    public float dashRate = 0.4f;       //dash recharge
    public float timeSlowCooldown = 3;
    public float invulnerableDuration = 2f;
    public int shotInnacuracy = 5;      //shot innacuracy
    public int maxHP = 6;
    public bool isRumbleEnabled = true;

    public GameObject projectile;       //player projectile
    public GameObject healParticle;     //player heal particle
    public GameObject reflectSpell;  //player melee reflect
    public GameObject shieldSpell;  //player rangedburst
    public GameObject afterImageBlink;
    public GameObject dropShadow;
    public GameObject meleeAttack;
    public GameObject flashLight;
    public GameObject blinkGlow;
    public GameObject footprint;
    public Material trail;
    public Material trailRecharge;

    public Text healthText;
    public Text timeSlowText;

    public TimeManager timeManager;     //stores reference to TimeManager

    [HideInInspector]
    public int directionFacing;      //last direction moved. 0 = right, 1 = left, 2 = up, 3 = down
    [HideInInspector]
    public bool isGamePadModeAim = false;  //game pad aim mode
    [HideInInspector]
    public bool canPlayerTakeDamage = true;
    [HideInInspector]
    public bool isTimeSlowed = false;

    //private variables
    private int hp;
    private float speed;
    private float dashFct = 2f;
    private float invulnerableTime = 0.0f;
    private float invulnerableAnimationTime = 0.0f;
    private float nextMelee = 0.0f;
    private float nextFire = 0.0f;  //time before next projecile shot fire
    private float nextReflect = 0.0f;
    private float nextDash = 0.0f;  //time before next dash
    private float currentDash = 0.0f;   //time before current dash runs out
    private float nextTimeSlow = 0.0f;
    private float rumbleLevelL;     //game pad rumble left level
    private float rumbleLevelR;     //game pad rumble right level
    private float pickupRumbleDelay;//delay for rumble
    private float meleeRumbleDelay;
    private float shotRumbleDelay;

    private bool isInvulnerable = false;
    private bool isDashOnCooldown = false;
    private bool isDashActive = false;
    private bool isFreeToMove = true;   //tracks if player is free to move and use abilities
    private bool hasSpawned = true;      //tracks if player spawning cycle is finished
    private bool isColliding = false;
    private bool isFootstepSoundPlaying = false;
    private bool isGamePadModeMove = false; //game pad move mode
    private bool isRumbleOn = false;
    private bool hasPickupRumbleFinished = true;
    private bool hasMeleeRumbleFinished = true;
    private bool hasShotRumbleFinished = true;
    private bool hasTimeAbilityCharge = true;
    private bool isFreeToShoot = true;
    private bool isBlinking = false;
    private bool isVictory = false;

    private Rigidbody2D rb2d;       //store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D rb2dProj;       //store a reference to the Rigidbody2D component required to use 2D Physics.
    private Vector2 moveDirection;      //tracks direction moving
    private Vector3 mousePos;           //tracks mouse pos
    private Vector3 shotDir;            //tracks shot direction
    private GameObject tempProj;
    private GameObject tempMeleeReflect;
    private GameObject tempRangedBurst;
    private GameObject tempBlink;
    private GameObject tempMeleeAttack;
    private ParticleSystem.EmissionModule trailEmission; //tracks after image emitter
    private ParticleSystem.EmissionModule footprintEmission;
    private ParticleSystem.MainModule trailLife; //tracks after image emitter
    private Light2D globalLight;
    public static PlayerController instace;

    void Start()
    {
        //set rigidbody, emitter and animator variables
        rb2d = GetComponent<Rigidbody2D>();
        trailEmission = GetComponent<ParticleSystem>().emission;
        footprintEmission = footprint.GetComponent<ParticleSystem>().emission;
        trailLife = gameObject.GetComponent<ParticleSystem>().main;
        globalLight = GetComponent<Light2D>();
        speed = speedBase;
        hp = maxHP;

        Cursor.visible = false; //disable cursor

        trailEmission.enabled = true;   //enable trail by default
        footprintEmission.enabled = false;   //disable footprint by default
        blinkGlow.SetActive(false); //disable blink glow by default

        instace = this;
    }

    float CalcAimAngle(bool gamePadMode)   //calculates angle player is aiming
    {
        float tempAngle;

        if (gamePadMode)    //game pad uses angle on joystick
        {
            if (Mathf.Abs(Input.GetAxis("xAim")) == 0 && Mathf.Abs(Input.GetAxis("yAim")) == 0)   //check for game pad mode and no joystick input
            {
                //default angle for last direction facing
                if (directionFacing == 1)
                    tempAngle = 180;

                else if (directionFacing == 2)
                    tempAngle = 90;

                else if (directionFacing == 3)
                    tempAngle = -90;

                else
                    tempAngle = 0;
            }

            else
                tempAngle = Mathf.Atan2(Input.GetAxis("yAim"), Input.GetAxis("xAim")) * Mathf.Rad2Deg;
        }

        else //calculates angle between player and mouse
        {
            mousePos = Input.mousePosition;
            mousePos.z = (transform.position.z - Camera.main.transform.position.z);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos = mousePos - transform.position;
            tempAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        }

        return tempAngle;
    }

    void PlayFootstepSound() //plays footstep sound if it isn't already playing
    {
        if (!isFootstepSoundPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Footstep");
            isFootstepSoundPlaying = true;
        }
    }

    void StopFootstepSound()    //stops playing footstep sound
    {
        if (isFootstepSoundPlaying)
        {
            FindObjectOfType<AudioManager>().Stop("Footstep");
            isFootstepSoundPlaying = false;
        }
    }

    Vector3 GetShotDir(int innacuracyRange)
    {
        int tempRange = Random.Range(-innacuracyRange, innacuracyRange + 1);  //set innacuracy based on range
        shotDir = Quaternion.AngleAxis(CalcAimAngle(isGamePadModeAim) + tempRange, Vector3.forward) * Vector3.right;

        return shotDir;
    }

    void FixedUpdate()
    {
        //add forces based on player input
        if (isFreeToMove)   //check if can move
        {
            if (isBlinking && isTimeSlowed && !isColliding) //check if dashing
            {
                Time.timeScale = 1f;
                rb2d.velocity = moveDirection * (1 / Time.timeScale);
                rb2d.AddForce(moveDirection * 10);  //fix force in direction
            }

            else if (isBlinking && !isColliding) //check if dashing
                rb2d.AddForce(moveDirection * 10);  //fix force in direction

            else    //normal movement
            {
                moveDirection = new Vector2(Mathf.Lerp(0, Input.GetAxisRaw("xMove") * speed, 0.8f), Mathf.Lerp(0, Input.GetAxisRaw("yMove") * speed, 0.8f));  //set movement based on x and y axes

                if (isTimeSlowed)
                    rb2d.velocity = moveDirection * (1 / Time.timeScale);

                else
                    rb2d.velocity = moveDirection;

                if (isGamePadModeMove)  //make game pad movement consistent with keyboard
                    rb2d.velocity *= 1.1f;
            }
        }

        else
            rb2d.velocity = new Vector2(0, 0);  //stop movement
    }

    void Update()
    {
        healthText.text = "HP: " + hp;

        if (isGamePadModeAim || isGamePadModeMove)
            isRumbleOn = isRumbleEnabled;
        else
            isRumbleOn = false;

        if (isVictory)
        {
            healthText.text = "YOU WIN";
        }
        else if (hp < 1)
        {
            healthText.text = "GAME OVER";
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Application.Quit();     //exit game
        }

        //coordinate tracking for layering
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        if (Mathf.Abs(Input.GetAxis("xAim")) > 0.1 || Mathf.Abs(Input.GetAxis("yAim")) > 0.1 || Input.GetAxis("rTrigger") == -1) //check for game pad input for aim
            isGamePadModeAim = true;

        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) //check for keyboard input for aim
            isGamePadModeAim = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)) //check for keyboard input for move
            isGamePadModeMove = false;

        else if (isGamePadModeAim) //game pad move mode
            isGamePadModeMove = true;

        //animation determined by player input
        if (isFreeToMove)   //check if player can move
        {
            if (Input.GetAxisRaw("xMove") > 0 && Input.GetAxisRaw("yMove") < 0.99f && Input.GetAxisRaw("yMove") > -0.99f) //set animation, particle and sounds for moving right
            {
                transform.rotation = Quaternion.Euler(0, 0, -10);
                PlayFootstepSound();
                directionFacing = 0;
            }

            else if (Input.GetAxisRaw("xMove") < 0 && Input.GetAxisRaw("yMove") > -0.99f && Input.GetAxisRaw("yMove") < 0.99f)    //set animation, particle and sounds for moving left
            {
                transform.rotation = Quaternion.Euler(0, 0, 10);
                PlayFootstepSound();
                directionFacing = 1;
            }

            else if (Input.GetAxisRaw("yMove") > 0)    //set animation, particle and sounds for moving up
            {
                transform.rotation = Quaternion.Euler(-20, 0, 0);
                PlayFootstepSound();
                directionFacing = 2;
            }
            else if (Input.GetAxisRaw("yMove") < 0)    //set animation, particle and sounds for moving down
            {
                transform.rotation = Quaternion.Euler(20, 0, 0);
                PlayFootstepSound();
                directionFacing = 3;
            }

            else
            {
                StopFootstepSound();
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            StopFootstepSound();
        }

        //melee attack
        if (((Input.GetKey(KeyCode.Mouse0) || Input.GetButtonDown("padX"))) && isFreeToMove && hasSpawned)  //check if input from mouse or game pad right trigger and shot off cooldown and can use abilities
        {
            if (isTimeSlowed && hasTimeAbilityCharge)
            {
                hasTimeAbilityCharge = false;
                timeManager.SlowMotion(0.05f, 0.5f);
                if (isRumbleOn)
                    rumbleLevelR = 0.9f;    //set controller left rumble
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                nextMelee = Time.time + shieldDuration;    //start delay for next shot
                canPlayerTakeDamage = false;

                hasMeleeRumbleFinished = false;
                meleeRumbleDelay = Time.time + shieldDuration; //start shot rumble duration

                tempRangedBurst = Instantiate(shieldSpell, transform.position, Quaternion.Euler(0, 0, 0));

                tempRangedBurst.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }

            else if (Time.time > nextMelee && Time.time > nextFire)
            {
                if (isRumbleOn)
                    rumbleLevelR = 0.6f;    //set controller left rumble
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);
                FindObjectOfType<AudioManager>().Play("ShotFired");

                nextMelee = Time.time + meleeRate;    //start delay for next shot

                hasMeleeRumbleFinished = false;
                meleeRumbleDelay = Time.time + meleeRate; //start shot rumble duration

                tempMeleeAttack = Instantiate(meleeAttack, rb2d.transform.position + (GetShotDir(0) * 1.5f), Quaternion.Euler(0, 0, CalcAimAngle(isGamePadModeAim) + 270), transform); //instantiate projectile
                tempMeleeAttack.transform.Translate(0, 1.2f, 0);
                tempMeleeAttack.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }
        }

        if (Time.time < nextMelee && !isBlinking && shieldSpell != null)
            isFreeToMove = false;

        if (Time.time > meleeRumbleDelay && !hasMeleeRumbleFinished)
        {
            rumbleLevelR = 0f;    //set controller left rumble
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);
            hasMeleeRumbleFinished = true;
            isFreeToMove = true;
            canPlayerTakeDamage = true;
        }

        //shooting projectiles
        if ((Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("rTrigger") < -0.9) && isFreeToMove && hasSpawned && isFreeToShoot && !isTimeSlowed)  //check if input from mouse or game pad right trigger and shot off cooldown and can use abilities
        {
            if (Time.time > nextFire)
            {
                if (isRumbleOn)
                    rumbleLevelR = 0.1f;    //set controller left rumble
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                tempProj = Instantiate(projectile, transform.position, transform.rotation); //instantiate projectile
                rb2dProj = tempProj.GetComponent<Rigidbody2D>();

                FindObjectOfType<AudioManager>().Play("ShotFired");

                rb2dProj.AddForce(GetShotDir(shotInnacuracy) * 1600 * projectileSpeed); //add force to projectile

                nextFire = Time.time + fireRate;    //start delay for next shot

                hasShotRumbleFinished = false;
                shotRumbleDelay = Time.time + 0.05f; //start shot rumble duration
            }
        }

        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetAxis("rTrigger") < -0.9) && isFreeToMove && hasSpawned && isFreeToShoot)
        {
            if (isTimeSlowed && hasTimeAbilityCharge)
            {
                hasTimeAbilityCharge = false;
                timeManager.SlowMotion(0.05f, 0.5f);

                tempMeleeReflect = Instantiate(reflectSpell, rb2d.transform.position + (GetShotDir(0) * 1.5f), Quaternion.Euler(0, 0, CalcAimAngle(isGamePadModeAim) + 270), transform); //instantiate projectile
                tempMeleeReflect.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

                if (isRumbleOn)
                    rumbleLevelR = 0.9f;    //set controller left rumble
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                nextFire = Time.time + fireRate;    //start delay for next shot
                nextReflect = Time.time + fireRate;    //start delay for next shot

                hasShotRumbleFinished = false;
                shotRumbleDelay = Time.time + 0.05f; //start shot rumble duration
            }
        }

        if (tempMeleeReflect != null)
        {
            tempMeleeReflect.transform.position = rb2d.transform.position + (GetShotDir(0) * 1.5f);
            tempMeleeReflect.transform.rotation = Quaternion.Euler(0, 0, CalcAimAngle(isGamePadModeAim) + 270);
        }

        if (Time.time > shotRumbleDelay && !hasShotRumbleFinished)
        {
            rumbleLevelR = 0f;    //set controller left rumble
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);
            hasShotRumbleFinished = true;
        }

        //dashing/evade/dodge
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetAxis("lTrigger") < -0.9) && (Time.time > nextDash) && isFreeToMove && hasSpawned) //check for space or game pad left trigger input and free to use abilities
        {
            isDashActive = true;
            isDashOnCooldown = true;

            if (isRumbleOn)
                rumbleLevelL = 0.3f;    //set controller left rumble
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

            //randomly choose between 2 sound effects and play
            int tempRange = Random.Range(0, 2);
            if (tempRange == 0)
                FindObjectOfType<AudioManager>().Play("PlayerDash1");
            else
                FindObjectOfType<AudioManager>().Play("PlayerDash2");

            if (isTimeSlowed && hasTimeAbilityCharge)
            {
                trailLife.startLifetime = 0.1f;
                GetComponent<ParticleSystem>().Clear(true);
                nextDash = Time.time + dashRate + dashTime;    //start cooldown
                currentDash = Time.time + dashTime; //start current dash duration
                speed *= 3;                         //speed up player

                if (Mathf.Abs(rb2d.velocity.x) < 19 && Mathf.Abs(rb2d.velocity.y) < 19)
                {
                    //dash in last direction moved or direction facing
                    if (directionFacing == 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, 1 * dashSpeed, 0.8f), Mathf.Lerp(0, 0 * dashSpeed, 0.8f));
                    else if (directionFacing == 1)
                        moveDirection = new Vector2(Mathf.Lerp(0, -1 * dashSpeed, 0.8f), Mathf.Lerp(0, 0 * dashSpeed, 0.8f));
                    else if (directionFacing == 2)
                        moveDirection = new Vector2(Mathf.Lerp(0, 0 * dashSpeed, 0.8f), Mathf.Lerp(0, 1 * dashSpeed, 0.8f));
                    else
                        moveDirection = new Vector2(Mathf.Lerp(0, 0 * dashSpeed, 0.8f), Mathf.Lerp(0, -1 * dashSpeed, 0.8f));
                }

                else
                {
                    if (Input.GetAxisRaw("xMove") > 0 && Input.GetAxisRaw("yMove") > 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, 1 * dashSpeed / 2, 0.8f), Mathf.Lerp(0, 1 * dashSpeed / 2, 0.8f));
                    else if (Input.GetAxisRaw("xMove") < 0 && Input.GetAxisRaw("yMove") > 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, -1 * dashSpeed / 2, 0.8f), Mathf.Lerp(0, 1 * dashSpeed / 2, 0.8f));
                    else if (Input.GetAxisRaw("xMove") > 0 && Input.GetAxisRaw("yMove") < 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, 1 * dashSpeed / 2, 0.8f), Mathf.Lerp(0, -1 * dashSpeed / 2, 0.8f));
                    else if (Input.GetAxisRaw("xMove") < 0 && Input.GetAxisRaw("yMove") < 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, -1 * dashSpeed / 2, 0.8f), Mathf.Lerp(0, -1 * dashSpeed / 2, 0.8f));
                    else if (Input.GetAxisRaw("xMove") > 0 && Mathf.Abs(Input.GetAxisRaw("yMove")) < 1)
                        moveDirection = new Vector2(Mathf.Lerp(0, 1 * dashSpeed, 0.8f), Mathf.Lerp(0, 0 * dashSpeed, 0.8f));
                    else if (Input.GetAxisRaw("xMove") < 0 && Mathf.Abs(Input.GetAxisRaw("yMove")) < 1)
                        moveDirection = new Vector2(Mathf.Lerp(0, -1 * dashSpeed, 0.8f), Mathf.Lerp(0, 0 * dashSpeed, 0.8f));
                    else if (Mathf.Abs(Input.GetAxisRaw("xMove")) < 1 && Input.GetAxisRaw("yMove") > 0)
                        moveDirection = new Vector2(Mathf.Lerp(0, 0 * dashSpeed, 0.8f), Mathf.Lerp(0, 1 * dashSpeed, 0.8f));
                    else
                        moveDirection = new Vector2(Mathf.Lerp(0, 0 * dashSpeed, 0.8f), Mathf.Lerp(0, -1 * dashSpeed, 0.8f));
                }

                hasTimeAbilityCharge = false;
                isBlinking = true;
                tempBlink = Instantiate(afterImageBlink, transform.position, Quaternion.Euler(0, 0, 0), transform);
                GetComponent<SpriteRenderer>().enabled = false;
                dropShadow.GetComponent<SpriteRenderer>().enabled = false;
                canPlayerTakeDamage = false;
                blinkGlow.SetActive(true);
                flashLight.SetActive(false);
                isFreeToShoot = false;
            }

            else
            {
                nextDash = Time.time + dashRate + (dashTime * 5);    //start cooldown
                currentDash = Time.time + (dashTime * 5); //start current dash duration
                speed = speedBase * dashFct;
                footprintEmission.enabled = true;
            }

            
        }

        //current dash finished
        if (Time.time > currentDash && isDashActive)    //check if current dash has finished and dash is active
        {
            isDashActive = false;
            rumbleLevelL = 0;               //reset rumble
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

            gameObject.GetComponent<ParticleSystemRenderer>().material = trailRecharge;
            trailLife.startLifetime = 0.1f;

            if (isBlinking)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                dropShadow.GetComponent<SpriteRenderer>().enabled = true;
                tempBlink.GetComponent<ParticleSystem>().Stop();
                GetComponent<ParticleSystem>().Clear(false);
                canPlayerTakeDamage = true;
                blinkGlow.SetActive(false);
                flashLight.SetActive(true);
                isFreeToShoot = true;
                isBlinking = false;
            }
            else
            {
                footprintEmission.enabled = false;
            }

            speed = speedBase;                     //reset speed
        }

        //check if dash cooldown has finished and dash is on cooldown
        if (Time.time > nextDash && isDashOnCooldown)
        {
            trailLife.startLifetime = 0.25f;
            gameObject.GetComponent<ParticleSystemRenderer>().material = trail;
            isDashOnCooldown = false;
        }

        //slow down time ability
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("rBumper")) && !isTimeSlowed && Time.time > nextTimeSlow)
        {
            timeManager.SlowMotion(0.05f, 2f);
            isTimeSlowed = true;

            //reset cooldowns
            nextDash = Time.time;
            currentDash = Time.time;

            //set time cooldown
            nextTimeSlow = Time.time + timeSlowCooldown;
        }

        if (isTimeSlowed)
        {
            globalLight.color = new Color((Time.timeScale), 1, 1, 1);
        }

        //only enable once last burst finished
        if (Time.timeScale == 1 && isTimeSlowed)
        {
            isTimeSlowed = false;
            hasTimeAbilityCharge = true;
            globalLight.color = Color.white;
        }

        //time slow cooldown indicator
        if (Time.time > nextTimeSlow)
        {
            timeSlowText.color = Color.cyan;
        }
        else
        {
            timeSlowText.color = Color.grey;
        }

        if (Input.GetKeyDown(KeyCode.L))                //reload level on L key pressed [DEBUG]
            transform.position = new Vector3(0, 0, 0);  //reset player position to prevent getting stuck in terrain

        if (Input.GetKey("escape")) //check if escape key pressed
            Application.Quit();     //exit game

        if (Time.time > pickupRumbleDelay && !hasPickupRumbleFinished) //reset rumble after delay
        {
            rumbleLevelR = 0; //set rumble levels
            rumbleLevelL = 0;
            hasPickupRumbleFinished = true;
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);
        }

        if (isInvulnerable)
        {
            if (Time.time > invulnerableAnimationTime)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                dropShadow.GetComponent<SpriteRenderer>().enabled = false;
                invulnerableAnimationTime = Time.time + 0.1f;
            }

            else
            {
                GetComponent<SpriteRenderer>().enabled = true;
                dropShadow.GetComponent<SpriteRenderer>().enabled = true;
            }

            if (Time.time > invulnerableTime)
            {
                canPlayerTakeDamage = true;
                isInvulnerable = false;

                rumbleLevelR = 0; //set rumble levels
                rumbleLevelL = 0;
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                GetComponent<SpriteRenderer>().enabled = true;
                dropShadow.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        flashLight.transform.rotation = Quaternion.AngleAxis(CalcAimAngle(isGamePadModeAim) - 90, Vector3.forward);
    }

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp"
        if (other.gameObject.CompareTag("PickUp"))
        {
            GameObject tempParticle = Instantiate(healParticle, transform.position, transform.rotation);    //instantiate particle
            tempParticle.transform.parent = transform;
            Destroy(tempParticle.gameObject, 3);

            other.gameObject.SetActive(false); //remove pickup
            FindObjectOfType<AudioManager>().Play("Pickup");    //play pickup audio
            if (isRumbleOn)
            {
                rumbleLevelR = 0.3f;    //set rumble
                rumbleLevelL = 0.3f;
            }
            pickupRumbleDelay = Time.time + 0.5f;
            if (hasSpawned)  //set controller rumble once spawned
                GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);
            hasPickupRumbleFinished = false;

            hp++;
        }

        if (other.gameObject.CompareTag("Environment")) //display indicator when behind large objects
        {
            flashLight.SetActive(false);
        }

        if (other.gameObject.CompareTag("EnemyProjectile"))   //check collision with enemy projectile
        {
            if (canPlayerTakeDamage)
            {
                //reset cooldowns
                nextDash = Time.time;
                currentDash = Time.time;
                nextTimeSlow = Time.time;

                invulnerableTime = Time.time + invulnerableDuration;
                invulnerableAnimationTime = Time.time + 0.1f;
                canPlayerTakeDamage = false;
                isInvulnerable = true;
                CameraShaker.Instance.ShakeOnce(8f, 4f, .1f, 1f);  //set camera shake
                timeManager.SlowMotion(0.05f, 0.5f);

                if (isRumbleOn)
                {
                    rumbleLevelR = 0.5f;    //set rumble
                    rumbleLevelL = 0.5f;
                }
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                hp--;

                Destroy(other.gameObject);   //destroy projectile
            }
        }

        if (other.gameObject.CompareTag("Enemy"))   //check collision with enemy
        {
            if (canPlayerTakeDamage)
            {
                invulnerableTime = Time.time + invulnerableDuration;
                invulnerableAnimationTime = Time.time + 0.1f;
                canPlayerTakeDamage = false;
                isInvulnerable = true;
                CameraShaker.Instance.ShakeOnce(8f, 4f, .1f, 1f);  //set camera shake
                timeManager.SlowMotion(0.05f, 0.5f);

                if (isRumbleOn)
                {
                    rumbleLevelR = 0.5f;    //set rumble
                    rumbleLevelL = 0.5f;
                }
                if (hasSpawned)  //set controller rumble once spawned
                    GamePad.SetVibration(0, rumbleLevelL, rumbleLevelR);

                hp--;
            }
        }

        if (other.gameObject.CompareTag("Exit"))
        {
            isVictory = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Environment")) //hide indicator when not behind large objects
        {
            flashLight.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Environment")) //check isn't tounching an environmental object to stop player dashing into environment
            isColliding = true;
    }

    void OnCollisionExit2D(Collision2D other)   //revert dashing to normal when not touching environmental object
    {
        if (other.gameObject.CompareTag("Environment"))
            isColliding = false;
    }
}