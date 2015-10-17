/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 FirstPersonController.cs              *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/

using UnityEngine;

// TODO:
// - clean up audio integration. Dispatch events/use delegate to separate audio from controller
// - Replace CharacterController with a custom implementation.
    // ? Use raycast downwards to lock to ground
    // ? Depending on how I do the collision with the blocks, use either capsule or custom
    //   raycasts or simple calculations for collision detection.
// - get rid of all the bools, and try to reduce the number of branches
// - put the camera as a child of the controller. That way I don't have to reposition
//   the camera. I only need to add bobbing.

namespace SmartFirstPersonController
{
    //  [RequireComponent( typeof( CameraHeadBob ) )]
    [RequireComponent( typeof( FirstPersonInput ) )]
    //  [RequireComponent( typeof( FootStepSoundsManager ) )]
    [RequireComponent( typeof( CharacterController ) )]
    //  [RequireComponent( typeof( AudioSource ) )]
    public class FirstPersonController : MonoBehaviour
    {
        // Fields for parameters
        public bool canWalk = true;
        public float walkSpeed = 4.25f;
        public float backwardsSpeed = .6f;
        public float sidewaysSpeed = .7f;
        public float inAirSpeed = .35f;

        public bool canRun = true;
        public float runSpeed = 8.75f;

        public bool canCrouch = true;
        public float crouchSpeed = .45f;
        public float crouchHeight = 1.25f;

        public bool canJump = true;
        public float jumpForce = 5f;

        public bool canClimb = true;
        public float climbingSpeed = .8f;

        public bool useHeadBob = true;

        public float gravityMultiplier = 2f;
        public float fallingDistanceToDamage = 3f;
        public float fallingDamageMultiplier = 3.5f;
        public string damageVoidName = "DecrementHealth";

        public float stepInterval = .5f;
        public float soundsVolume = .75f;

        public float lookSensitivity = 2f;
        public float lookSmooth = .3f;
        public float maxLookAngleY = 65f;
        public Vector3 cameraOffset = Vector3.up;


        internal bool grounded { get; private set; }
        internal bool climbing { get; private set; }
        internal bool movement { get; private set; }
        internal bool running  { get; private set; }
        internal bool crouched { get; private set; }

        internal RaycastHit floorHit;
        internal string groundTag { get; private set; }


        // Fields for move calculation
        private bool prevGrounded, jump, jumping, falling, crouching;

        private CharacterController m_Controller = null;
        private CollisionFlags collisionFlags = CollisionFlags.None;
        private Transform m_Transform, cameraTransform;
        private FirstPersonInput m_Input = null;
        private CameraHeadBob m_camHeadBob = null;
        private Vector3 moveDirection, crouchVelVec;
        private float nextStep, nativeCapsuleHeight, crouchVel, fallingStartPos, fallingDist;
        private FootStepSoundsManager m_fssManager = null;
        private AudioSource m_Audio = null;
        private Ladder currentLadder = null;

        // Fields for look calculation
        private float rotationX, rotationY;
        private Quaternion nativeRotation;


        // Awake
        void Awake()
        {
            m_Transform = transform;
            cameraTransform = m_Transform.parent.GetComponentInChildren<Camera>().transform; //Camera

            m_Controller = this.GetComponent<CharacterController>();
            m_Controller.center = Vector3.up;
            nativeCapsuleHeight = m_Controller.height;

            m_Input = this.GetComponent<FirstPersonInput>();
            m_camHeadBob = this.GetComponent<CameraHeadBob>();
            m_fssManager = this.GetComponent<FootStepSoundsManager>();

            m_Audio = this.GetComponent<AudioSource>();
            if (m_Audio != null) {
                m_Audio.playOnAwake = false;
                m_Audio.loop = false;
                m_Audio.spatialBlend = 1f;
            }

            nativeRotation = cameraTransform.localRotation;
            nativeRotation.eulerAngles = new Vector3( 0f, cameraTransform.eulerAngles.y, 0f );
        }


        // Update
        void Update()
        {
            // Cursor lock
            if( Time.timeSinceLevelLoad > .1f )
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if( climbing || crouching || !grounded )
                return;

            if( canJump && canWalk && m_Input.jump && !jump && !jumping && !crouched )
                jump = true;


            if( canCrouch && m_Input.crouch )
            {
                crouching = true;

                if( crouched )
                {
                    if( Physics.SphereCast( m_Transform.position + Vector3.up, m_Controller.radius, Vector3.up, out floorHit, nativeCapsuleHeight * .25f ) )
                    {
                        crouching = false;
                        return;
                    }

                    StartCoroutine( StandUp() );
                }
                else
                    StartCoroutine( SitDown() );
            }
        }


        // FixedUpdate
        void FixedUpdate()
        {
            grounded = m_Controller.isGrounded;
            if( grounded )
            {
                if( falling )
                {
                    falling = false;

                    if( fallingDist > fallingDistanceToDamage )
                    {
                        int damage = Mathf.RoundToInt( fallingDist * fallingDamageMultiplier );
                        this.SendMessage( damageVoidName, damage, SendMessageOptions.DontRequireReceiver );
                    }

                    fallingDist = 0f;
                }
            }
            else
            {
                if( falling )
                {
                    fallingDist = fallingStartPos - m_Transform.position.y;
                }
                else
                {
                    if( !climbing )
                    {
                        falling = true;
                        fallingStartPos = m_Transform.position.y;
                    }
                }
            }


            Movement();
            CameraLook();
            PlayFootStepAudio();

            if( !climbing && !grounded && !jumping && prevGrounded )
                moveDirection.y = 0f;

            prevGrounded = grounded;
        }

        // Movement
        private void Movement()
        {
            float horizontal = m_Input.moveHorizontal * Time.timeScale; // move Left/Right
            float vertical = m_Input.moveVertical * Time.timeScale; // move Forward/Backward

            bool moveForward = ( vertical > 0f );

            vertical *= ( moveForward ? 1f : backwardsSpeed );
            horizontal *= sidewaysSpeed;

            Quaternion screenMovementSpace = Quaternion.Euler( 0f, cameraTransform.eulerAngles.y, 0f );
            Vector3 forwardVector = screenMovementSpace * Vector3.forward * vertical;
            Vector3 rightVector = screenMovementSpace * Vector3.right * horizontal;
            Vector3 moveVector = forwardVector + rightVector;

            if( climbing )
            {
                bool lookUp = cameraTransform.forward.y > -.4f;

                if( moveForward )
                {
                    forwardVector = currentLadder.m_Transform.up * vertical;
                    forwardVector *= lookUp ? 1f : -1f;
                }

                moveVector = forwardVector + rightVector;

                if( grounded )
                {
                    if( moveForward && !lookUp )
                        moveVector += screenMovementSpace * Vector3.forward;
                }
                else
                {
                    if( moveForward && lookUp )
                        moveVector += screenMovementSpace * Vector3.forward;
                }

                moveDirection = moveVector * GetSpeed( moveForward );
            }
            else
            {
                if( grounded )
                {
                    if( Physics.SphereCast( m_Transform.position + m_Controller.center, m_Controller.radius, Vector3.down, out floorHit, m_Controller.height * .5f ) )
                    {
                        groundTag = floorHit.collider.tag;
                    }

                    moveDirection = moveVector * GetSpeed( moveForward );
                    moveDirection.y = -10f;

                    if( jump )
                    {
                        if (m_fssManager != null) {
                            m_fssManager.PlayJumpingSound( groundTag, ref m_Audio, soundsVolume );
                        }
                        jumping = true;
                        jump = false;
                        moveDirection.y = jumpForce;
                    }
                }
                else
                {
                    moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
                }
            }

            if( canWalk )
                collisionFlags = m_Controller.Move( moveDirection * Time.fixedDeltaTime );

            m_Transform.rotation = screenMovementSpace;

            bool accelerated = ( m_Controller.velocity.magnitude > 0f );
            movement = climbing ? accelerated : grounded && accelerated;
        }

        // GetSpeed
        private float GetSpeed( bool inForward )
        {
            running = ( canRun && m_Input.run && !crouched && inForward );
            float speed = running ? runSpeed : walkSpeed;
            speed = crouched ? speed * crouchSpeed : speed;
            speed = ( grounded || ( jumping && !jump ) ) ? speed : speed * inAirSpeed;
            speed = climbing ? speed * climbingSpeed : speed;
            return speed;
        }


        // CameraLook
        private void CameraLook()
        {
            rotationX += m_Input.lookHorizontal * lookSensitivity * Time.timeScale;
            rotationY += m_Input.lookVertical * lookSensitivity * Time.timeScale;

            rotationY = Mathf.Clamp( rotationY, -maxLookAngleY, maxLookAngleY );

            Quaternion xQuaternion = Quaternion.AngleAxis( rotationX + ( useHeadBob ? m_camHeadBob.yTilt : 0f ), Vector3.up );
            Quaternion yQuaternion = Quaternion.AngleAxis( rotationY + ( useHeadBob ? m_camHeadBob.xTilt : 0f ), Vector3.left );

            cameraTransform.rotation = Quaternion.Slerp( cameraTransform.rotation, nativeRotation * xQuaternion * yQuaternion, lookSmooth * ( Time.fixedDeltaTime * 50f ) );

            Vector3 newCameraPosition = Vector3.zero;
            newCameraPosition.x = m_Transform.position.x + m_Controller.center.x + cameraOffset.x + ( useHeadBob ? m_camHeadBob.xPos : 0f ); //xPos
            newCameraPosition.y = m_Transform.position.y + ( m_Controller.center.y * 2f ) + ( cameraOffset.y ) + ( useHeadBob ? m_camHeadBob.yPos : 0f ); //yPos
            newCameraPosition.z = m_Transform.position.z + m_Controller.center.z + cameraOffset.z;
            cameraTransform.position = newCameraPosition;
        }


        // SitDown
        private System.Collections.IEnumerator StandUp()
        {
            Vector3 targetCenter = Vector3.up;

            while( PlayCrouchAnimation( ref targetCenter, ref nativeCapsuleHeight ) )
                yield return null;

            m_Controller.height = nativeCapsuleHeight;
            m_Controller.center = targetCenter;

            crouched = crouching = false;
        }

        // StandUp
        private System.Collections.IEnumerator SitDown()
        {
            Vector3 targetCenter = Vector3.up * ( crouchHeight * .5f );

            while( PlayCrouchAnimation( ref targetCenter, ref crouchHeight ) )
                yield return null;

            m_Controller.height = crouchHeight;
            m_Controller.center = targetCenter;

            crouched = true;
            crouching = false;
        }

        // PlayCrouchAnimation
        private bool PlayCrouchAnimation( ref Vector3 targetCenter, ref float targetHeight )
        {
            m_Controller.height = Mathf.SmoothDamp( m_Controller.height, targetHeight, ref crouchVel, Time.fixedDeltaTime * 5f );
            m_Controller.center = Vector3.SmoothDamp( m_Controller.center, targetCenter, ref crouchVelVec, Time.fixedDeltaTime * 5f );

            const int digits = 3;
            double cMag = System.Math.Round( ( double )m_Controller.center.magnitude, digits );
            double tMag = System.Math.Round( ( double )targetCenter.magnitude, digits );

            return ( cMag != tMag );
        }


        // PlayFootStepAudio
        private void PlayFootStepAudio()
        {
            if( !prevGrounded && grounded )
            {
                if (m_Audio != null && m_fssManager != null) {
                    m_fssManager.PlayLandingSound( groundTag, ref m_Audio, soundsVolume );
                }
                if (useHeadBob) {
                    nextStep = m_camHeadBob.headBobCycle + stepInterval;
                }
                jumping = false;
                moveDirection.y = 0f;
                return;
            }

            if (useHeadBob) {
                if( ( m_camHeadBob.headBobCycle > nextStep ) )
                {
                    nextStep = m_camHeadBob.headBobCycle + stepInterval;

                    if (m_fssManager != null && m_Audio != null) {
                        if( grounded ) {
                            m_fssManager.PlayFootStepSound( groundTag, ref m_Audio, soundsVolume );
                        } else if( climbing ) {
                            currentLadder.PlayLadderFootstepSound( ref m_Audio, soundsVolume );
                        }
                    }
                }
            }
        }


        // PlayerDead
        internal void PlayerDie()
        {
            this.enabled = false;
            m_Controller.height = .1f;
            m_Controller.radius = .1f;
        }


        // OnControllerColliderHit
        void OnControllerColliderHit( ControllerColliderHit hit )
        {
            if( collisionFlags != CollisionFlags.Below )
            {
                Rigidbody hitbody = hit.collider.attachedRigidbody;

                if( hitbody && !hitbody.isKinematic )
                    hitbody.AddForceAtPosition( hit.moveDirection, hit.point, ForceMode.Impulse );
            }
        }

        // OnTriggerEnter
        void OnTriggerEnter( Collider collider )
        {
            if( canClimb && !crouching && collider.tag == TagsManager.Ladder )
            {
                if( crouched )
                {
                    crouching = true;
                    StartCoroutine( StandUp() );
                }

                currentLadder = collider.GetComponent<Ladder>();
                moveDirection = Vector3.zero;
                climbing = true;
            }
        }

        // OnTriggerExit
        void OnTriggerExit( Collider collider )
        {
            if( !crouching && collider.tag == TagsManager.Ladder )
            {
                climbing = false;
                currentLadder = null;
            }
        }
        //
    }
}