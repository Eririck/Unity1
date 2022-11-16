using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
#region Name
	{ 
	
#endregion    public float horizontalMove;
    public float verticalMove;
    public float HorizontalMove;
    private Vector3 PlayerInput;
    
    public CharacterController Player;

    public float PlayerSpeed;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity; 
    public float jumpForce;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

//use initialization
     void Start()
    {
        Player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        PlayerInput = new Vector3(HorizontalMove, 0, verticalMove);
        PlayerInput = Vector3.ClampMagnitude(PlayerInput, 1);
        
        camDiretion();

        movePlayer = PlayerInput.x * camRight + PlayerInput.z * camForward;

        movePlayer = movePlayer * PlayerSpeed;
    

        Player.transform.LookAt(Player.transform.position + movePlayer);

        SetGravity();

        PlayerSkills();

        Player.Move(movePlayer * Time.deltaTime);
        
        Debug.Log(Player.velocity.magnitude);
    }

        //funcion para determinar la posición de la cámara
       void camDiretion ()
        {
            camForward = mainCamera.transform.forward;
            camRight = mainCamera.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward = camForward.normalized;
            camRight = camRight.normalized;

        }

        //función para las habilidades del jugador
        void PlayerSkills ()
    {
        if (Player.isGrounded && Input.GetButtonDown("Jump")) 
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;

        }

    }

        //función para la gravedad
       void SetGravity()
        {
            if (Player.isGrounded)
            {
                fallVelocity = -gravity * Time.deltaTime;
                movePlayer.y = fallVelocity;
            }
            else
            {
                 fallVelocity -= gravity * Time.deltaTime;
                 movePlayer.y = fallVelocity;
            }

            SlideDown();
        }

        public void SlideDown()
        {
            isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= Player.slopeLimit;
            if (isOnSlope)
            {
                 movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
                 movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

                 movePlayer.y += slopeForceDown;
            }

        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            hitNormal = hit.normal;
        }
}