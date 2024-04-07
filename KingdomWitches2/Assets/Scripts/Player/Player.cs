using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{

    public float maxJumpHeight = 3f;
    public float minJumpHeight = 1.5f;
    public float timeToJumpApex = .4f;
    public float accelerationTimeGrounded = .1f;
    public float accelerationTimeAirborneMultiplier = 2f;

    public float timeInvincible = 2.0f;

    bool invincible;
    bool forceApplied;

    public float moveSpeed = 6f;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    Vector2 movementInput;
    float velocityXSmoothing;

    Direction direction;
    bool facingRight;

    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerController controller;

    int hpCount = 0;
    public GameObject hpPrefab;
    List<GameObject> hp = new List<GameObject>();

    float lastButtonPress;
    //float timeSinceLastPress;

    bool canDash = true;
    bool isDashing;
    float dashPower = 24;
    float dashTime = 0.2f;
    float dashCooldown = 1;

    public static Player instance;

    //PUBLICS

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public void ApplyDamage(int damage)
    {

        if (!invincible)
        {
            
            SubtractHealth(damage);
            SetVelocity(Vector2.up * 8.0f);
            StartCoroutine(SetInvincible());
            if(health > 0) 
            {
                for (int i = 0; i < damage; i++)
                {
                    hp[hpCount+i].GetComponent<RawImage>().color = Color.green;
                }
            }
            hpCount++;

            if (state == State.Died)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

    }

    public IEnumerator addBuff(string buff, int buffStrength, float duration)
    {
        switch(buff)
        {
            case "speed":
                moveSpeed += buffStrength;
                yield return new WaitForSeconds(duration);
                moveSpeed -= buffStrength;
                break;
            case "jump":
                maxJumpVelocity += buffStrength;
                minJumpVelocity += buffStrength;
                yield return new WaitForSeconds(duration);
                maxJumpVelocity -= buffStrength;
                minJumpVelocity -= buffStrength;
                break;
            case "damage":
                sowrd sword = gameObject.GetComponentInChildren<sowrd>();
                sword.damage += buffStrength;
                pitchfork pitchfork = gameObject.GetComponentInChildren<pitchfork>();
                pitchfork.damage += buffStrength;
                yield return new WaitForSeconds(duration);
                sword.damage -= buffStrength;
                pitchfork.damage -= buffStrength;
                break;
            case "challenge":
                Transform camera = FindAnyObjectByType<FollowCamera>().transform;
                camera.Rotate(new Vector3(0, 0, 180));
                yield return new WaitForSeconds(duration);
                camera.Rotate(new Vector3(0, 0, 180));
                break;
        }

    }

    //PRIVATES

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        //Initialize Vertical Values
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        for(int i = 0; i < maxHealth; i++) 
        {
            Vector2 tempu = GameObject.Find("PlayerHpBar").GetComponent<RectTransform>().offsetMax;
            hp.Add(Instantiate(hpPrefab, GameObject.Find("PHPBGrid").transform.position , Quaternion.identity, GameObject.Find("PHPBGrid").transform));
            GameObject.Find("PlayerHpBar").GetComponent<RectTransform>().offsetMax = tempu + new Vector2((i * 75), 0);
            hp[i].GetComponent<RawImage>().color = Color.white; 
        }
    }

    private void Update()
    {
        //quitGame();
        GetInput();
        Animation();
        Horizontal();
        Vertical();
        ApplyMovement();



    }

    private void GetInput()
    {

        if (isDashing) return;

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        direction = facingRight ? Direction.RIGHT : Direction.LEFT;

        

        //If moving horizontally
        if (movementInput.x != 0)
        {            
            direction = movementInput.x < 0 ? Direction.RIGHT : Direction.LEFT;
            facingRight = movementInput.x < 0;
            
        }

        if(Input.GetButtonDown("Shift") && canDash)
        {
                StartCoroutine(Dash());
        }
        float verticalAimFactor = movementInput.y;
        if (controller.collisions.below)
        {
            verticalAimFactor = Mathf.Clamp01(verticalAimFactor);
        }

        //If looking vertically
        if (verticalAimFactor != 0)
        {
            direction = verticalAimFactor > 0 ? Direction.UP : Direction.DOWN;
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        gravity = 0;
        float originMS = moveSpeed;
        moveSpeed = 30;
        float originTI = timeInvincible;
        timeInvincible = dashTime;
        SetInvincible();
        yield return new WaitForSeconds(dashTime);
        moveSpeed = originMS;
        timeInvincible = originTI;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

    }

    private void Animation()
    {

        spriteRenderer.flipX = facingRight;
        animator.SetFloat("VelocityX", Mathf.Abs(movementInput.x));
        animator.SetFloat("VelocityY", Mathf.Sign(velocity.y));
        animator.SetFloat("Looking", General.Direction2Vector(direction).y);
        animator.SetBool("Grounded", controller.collisions.below);
    }

    private void Vertical()
    {
        if (forceApplied)
        {
            forceApplied = false;
        }
        else if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;

    }

    private void Horizontal()
    {
        float targetVelocityX = movementInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            accelerationTimeGrounded * (controller.collisions.below ? 1.0f : accelerationTimeAirborneMultiplier));
    }

    private void ApplyMovement()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    private void SetVelocity(Vector2 v)
    {
        velocity = v;
        forceApplied = true;
    }

    private IEnumerator SetInvincible()
    {

        invincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < timeInvincible)
        {

            spriteRenderer.enabled = !spriteRenderer.enabled;
            elapsedTime += .04f;
            yield return new WaitForSeconds(.04f);

        }

        spriteRenderer.enabled = true;
        invincible = false;

    }

/*    private void quitGame()
    {
        if (Input.GetButton("Exit"))
        {
            Application.Quit();
        }
    }*/
}
