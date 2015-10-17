/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 CameraHeadBob.cs                      *
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

namespace SmartFirstPersonController 
{
    public class CameraHeadBob : MonoBehaviour
    {
        // Fields for parameters

        [Range( 1f, 3f )]
        public float headBobFrequency = 1.5f;

        [Range( .1f, 2f )]
        public float headBobHeight = .35f;

        [Range( .1f, 2f )]
        public float headBobSwayAngle = .5f;

        [Range( .01f, .1f )]
        public float headBobSideMovement = .075f;

        [Range( .1f, 2f )]
        public float bobHeightSpeedMultiplier = .35f;

        [Range( .1f, 2f )]
        public float bobStrideSpeedLengthen = .35f;

        [Range( .1f, 5f )]
        public float jumpLandMove = 2f;

        [Range( 10f, 100f )]
        public float jumpLandTilt = 35f;

        [Range( .1f, 4f )]
        public float springElastic = 1.25f;

        [Range( .1f, 2f )]
        public float springDampen = .77f;


        // Fields for calculation
        private float springPos, springVelocity, headBobFade;
        private Vector3 velocity, velocityChange, prevPosition, prevVelocity;
        private Transform m_Transform = null;
        private FirstPersonController m_fpController = null;

        // Fields for internal access
        internal float headBobCycle { get; private set; }
        internal float xPos { get; private set; }
        internal float yPos { get; private set; }
        internal float xTilt { get; private set; }
        internal float yTilt { get; private set; }


        // Awake
        void Awake()
        {
            m_Transform = transform;
            m_fpController = this.GetComponent<FirstPersonController>();
        }

        // OnEnable
        void OnEnable()
        {
            headBobCycle = 0f;
            xPos = yPos = 0f;
            xTilt = yTilt = 0f;
        }

        // FixedUpdate
        void FixedUpdate()
        {
            velocity = ( m_Transform.position - prevPosition ) / Time.fixedDeltaTime;
            velocityChange = velocity - prevVelocity;
            prevPosition = m_Transform.position;
            prevVelocity = velocity;

            if( !m_fpController.climbing )
                velocity.y = 0f;             

            springVelocity -= velocityChange.y;
            springVelocity -= springPos * springElastic;
            springVelocity *= springDampen;
            springPos += springVelocity * Time.fixedDeltaTime;
            springPos = Mathf.Clamp( springPos, -.32f, .32f );

            if( Mathf.Abs( springVelocity ) < .05f && Mathf.Abs( springPos ) < .05f )            
                springVelocity = springPos = 0f;            

            float flatVelocity = velocity.magnitude;

            if( m_fpController.climbing )
                flatVelocity *= 4f;
            else if( !m_fpController.climbing && !m_fpController.grounded )
                flatVelocity /= 4f;            
             
            float strideLengthen = 1f + flatVelocity * bobStrideSpeedLengthen;
            headBobCycle += ( flatVelocity / strideLengthen ) * ( Time.fixedDeltaTime / headBobFrequency );

            float bobFactor = Mathf.Sin( headBobCycle * Mathf.PI * 2f );
            float bobSwayFactor = Mathf.Sin( headBobCycle * Mathf.PI * 2f + Mathf.PI * .5f );
            bobFactor = 1f - ( bobFactor * .5f + 1f );
            bobFactor *= bobFactor;

            if( velocity.magnitude < .1f )
                headBobFade = Mathf.Lerp( headBobFade, 0f, Time.fixedDeltaTime );            
            else
                headBobFade = Mathf.Lerp( headBobFade, 1f, Time.fixedDeltaTime );            

            float speedHeightFactor = 1f + ( flatVelocity * bobHeightSpeedMultiplier );

            xPos = -headBobSideMovement * bobSwayFactor * headBobFade; 
            yPos = springPos * jumpLandMove + bobFactor * headBobHeight * headBobFade * speedHeightFactor;
            xTilt = springPos * jumpLandTilt;
            yTilt = bobSwayFactor * headBobSwayAngle * headBobFade;                       
        }
    }
}