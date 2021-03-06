﻿using System;
using CherryBoom;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float restartLevelDelay = 1.5f;        //Delay time in seconds to restart level.
    public float health = 1;
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    public float walkSpeed = 4f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public bool canDoubleJump;
    private bool isDoubleJumping = false;
    private bool isDoubleJumpingValid = true;
    private bool isJumping = false;

    public float wallSlideSpeedMax = 3f;

    public float wallStickTime = .25f;
    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

    private bool lookingRight = true;

    private Transform sprite;
    private Animator spriteAnimator;
    private bool acceptInput = true;

    public Collider2D playerCollider { get { return controller != null ? controller.coll : null; }}

    private GameController gameController;

    private void Start()
    {
        gameController = GetComponentInParent<GameController>();
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void Awake()
    {
        sprite = transform.Find("Little_Cat_GirlRig");
        spriteAnimator = sprite.GetComponent<Animator>();
    }

    private void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        Vector2 deltav = velocity * Time.deltaTime;

        if (transform.position.x + deltav.x <= -25 && deltav.x < 0)
        {
            velocity.x = 0;
            transform.position = new Vector3(8, transform.position.y);
        }
        if (transform.position.y + deltav.y <= 0 && deltav.y < 0)
        {
            acceptInput = false;
            velocity.x = 0;
            velocity.y = 0;
            transform.position = new Vector3(8, 9);
            gameController.Die();
            spriteAnimator.Play("die");
            Invoke("ResetPlayer", restartLevelDelay);
        }
        else
        {
            controller.Move(ref deltav, directionalInput);
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        if (controller.collisions.below)
        {
            isJumping = false;
            isDoubleJumpingValid = true;
        }
        spriteAnimator.SetBool("jumping", isJumping);
    }

    public void ResetPlayer() {
        acceptInput = true;
        spriteAnimator.Play("idle");
        transform.position = new Vector3(8, 9);
    }

    public void GetHit(float force = 1f) {
        health -= force;
        spriteAnimator.Play("get_hit");
        if (health <= 0) {        
            gameController.Die();
            spriteAnimator.Play("die");
            Invoke("ResetPlayer", restartLevelDelay);
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        if (!acceptInput) return;
        
        if (isJumping) return;

        directionalInput = input;
        if (directionalInput.x > 0)
        {
            lookingRight = true;
        } else if (directionalInput.x < 0)
        {
            lookingRight = false;
        }
        sprite.localScale = new Vector3(lookingRight ? -0.33f : 0.33f, 0.33f, 1);
    }

    public void OnJumpInputDown()
    {
        if (!acceptInput) return;
        
        isJumping = true;
        if (wallSliding)
        {
            directionalInput.x = -wallDirX;
            // if (wallDirX == directionalInput.x)
            // {
            //     velocity.x = -wallDirX * wallJumpClimb.x;
            //     velocity.y = wallJumpClimb.y;
            // }
            // else if (directionalInput.x == 0)
            // {
            //     velocity.x = -wallDirX * wallJumpOff.x;
            //     velocity.y = wallJumpOff.y;
            // }
            // else
            // {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            //}
            isDoubleJumping = false;
            isDoubleJumpingValid = false;
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = false;  
            isDoubleJumpingValid = true;          
        }
        if (canDoubleJump && !controller.collisions.below && !isDoubleJumping && !wallSliding && isDoubleJumpingValid)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = true;
        }
    }

    public void OnJumpInputUp()
    {
        if (!acceptInput) return;

        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            isJumping = false;
            isDoubleJumpingValid = false;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0f)
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;
                if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        var targetVelocityX = directionalInput.x * walkSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
        spriteAnimator.SetFloat("velocityX", velocity.x);
    }

    public void StopInput() {
        acceptInput = false;
        velocity = new Vector3();
    }
}
