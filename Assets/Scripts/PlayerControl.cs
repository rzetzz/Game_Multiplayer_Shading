using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{
    CharacterController cc;
    // Rigidbody rb;
    Animator anim;
    Vector3 moveDirection;
    Vector3 targetRotation;
    PlayerInputManager input;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float rotationSpeed = 2;
    [SerializeField] GameObject cameras;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPosition;
    public FixedString32Bytes playerName;
    bool isJumping;
    bool isGrounded;
    protected bool fallingVelocityBeenSet = false;
    protected float inAirTimer = 0;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector3 yVelocity;
    [SerializeField] float groundedVelocity = -20;
    [SerializeField] float fallStartVelocity = -5;
    bool jumpRequest;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            enabled = false;
            cameras.SetActive(false);
            return;
        }
        else
        {
            // playerName = RelayControl.instance.playerName;
        }
       
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
        // rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        HandleRotation();
        HandleMovement();
    
        HandleGroundCheck();
        if(input.jump)
        {
            AttemptJump();
        }

        anim.SetFloat("MoveAmount",input.moveAmount);
        anim.SetBool("isGrounded", isGrounded);
        if(isGrounded)
        {
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityBeenSet = false;
                yVelocity.y = groundedVelocity;
            }
        }
        else
        {
            if(!isJumping && !fallingVelocityBeenSet)
            {
                fallingVelocityBeenSet = true;
                yVelocity.y = fallStartVelocity;
            }

            inAirTimer += Time.deltaTime;

            yVelocity.y += gravityForce * Time.deltaTime;

            
        }
        cc.Move(yVelocity * Time.deltaTime);
        
    }

    // private void FixedUpdate() 
    // {
    //     HandleMovement();
    //     if(jumpRequest)
    //     {
    //         ApplyJump();
    //         jumpRequest = false;
    //     }
    // }
    // private void HandleMovementRB()
    // {
    //     moveDirection = Camera.main.transform.forward * input.movement.y;
    //     moveDirection = moveDirection + Camera.main.transform.right * input.movement.x;
    //     moveDirection.Normalize();
    //     moveDirection.y = 0;

    //     Vector3 velocity = moveDirection * moveSpeed;
    //     rb.velocity = new Vector3(velocity.x,rb.velocity.y,velocity.z);
    // }
    private void HandleMovement()
    {
        moveDirection = Camera.main.transform.forward * input.movement.y;
        moveDirection = moveDirection + Camera.main.transform.right * input.movement.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        cc.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        targetRotation = Vector3.zero;
        targetRotation = Camera.main.transform.forward * input.movement.y;
        targetRotation = targetRotation + Camera.main.transform.right * input.movement.x;
        targetRotation.Normalize();
        targetRotation.y = 0;

        if(targetRotation == Vector3.zero)
        {
            targetRotation = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotation);
        Quaternion rotationTarget = Quaternion.Slerp(transform.rotation,newRotation,rotationSpeed * Time.deltaTime);
        
        transform.rotation = rotationTarget;

    }
    // private void HandleRotationCC()
    // {
    //     targetRotation = Vector3.zero;
    //     targetRotation = Camera.main.transform.forward * input.movement.y;
    //     targetRotation = targetRotation + Camera.main.transform.right * input.movement.x;
    //     targetRotation.Normalize();
    //     targetRotation.y = 0;

    //     if(targetRotation == Vector3.zero)
    //     {
    //         targetRotation = transform.forward;
    //     }

    //     Quaternion newRotation = Quaternion.LookRotation(targetRotation);
    //     Quaternion rotationTarget = Quaternion.Slerp(transform.rotation,newRotation,rotationSpeed * Time.deltaTime);
    //     transform.rotation = rotationTarget;

    // }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position,groundCheckRadius,groundLayer);
    }
    private void AttemptJump()
    {
        Debug.Log("AttemptJump");
        // if(isJumping)
        // {
        //     return;
        // }
        if(!isGrounded)
        {
            return;
        }
        Debug.Log("Go Jump");
        isJumping = true;
        ApplyJump();
    }

    // public void ApplyJump()
    // {
        
    //     rb.AddForce(Vector3.up * jumpHeight,ForceMode.Impulse);
    //     isGrounded = false;
        
    // }

    public void ApplyJump()
    {
        
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        isGrounded = false;
        
    }

    private void FireBullet()
    {
        if(input.fire)
        {
            ShotBulletServerRPC(spawnPosition.position,transform.rotation);
            

        }
        
    }

    // [ServerRpc]
    // void ShootFireServerRPC(Vector3 position, Quaternion rotation)
    // {
    //     GameObject bullet = Instantiate(bulletPrefab,position,rotation);
    //     bullet.GetComponent<NetworkObject>().Spawn();
    // }

    [ServerRpc]
    void ShotBulletServerRPC(Vector3 position, Quaternion rotation,ServerRpcParams rpcParams = default)
    {
        GameObject bullet = Instantiate(bulletPrefab,position,rotation);

        NetworkObject netObj = bullet.GetComponent<NetworkObject>();

        if(netObj != null)
        {
            netObj.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        }
    }


}
