using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    
    public Vector2 movement;    
    public bool fire;
    public bool jump;
    PlayerInput playerInput;
    public float moveAmount;

    private void Start() 
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() 
    {
        fire = playerInput.actions["Attack"].WasPerformedThisFrame();
        jump = playerInput.actions["Jump"].WasPerformedThisFrame();
        
        MoveManager();
    }

    public void Moving(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }

    public void MoveManager()
    {
        moveAmount = Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
    }

    
}
