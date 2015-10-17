/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 FirstPersonInput.cs                   *
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
    public class FirstPersonInput : MonoBehaviour
    {
        public enum UpdateType
        {
            Update = 0,
            LateUpdate = 2,
            FixedUpdate = 3
        }

        public UpdateType updateType = UpdateType.Update;

        
        // Run
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode runJoykey = KeyCode.JoystickButton8;

        // Crouch
        public KeyCode crouchKey = KeyCode.C;
        public KeyCode crouchJoykey = KeyCode.JoystickButton1;

        // Jump
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode jumpJoykey = KeyCode.JoystickButton0;


        // Update
        void Update()
        {
            if( updateType == UpdateType.Update )
                StandaloneInput();
        }
        // LateUpdate
        void LateUpdate()
        {
            if( updateType == UpdateType.LateUpdate )
                StandaloneInput();
        }
        // FixedUpdate
        void FixedUpdate()
        {
            if( updateType == UpdateType.FixedUpdate )
                StandaloneInput();
        }
        
        // StandaloneInput
        private void StandaloneInput()
        {
            lookHorizontal = Input.GetAxis( "Mouse X" );
            lookVertical = Input.GetAxis( "Mouse Y" );
            moveHorizontal = Input.GetAxis( "Horizontal" );
            moveVertical = Input.GetAxis( "Vertical" );

            run = Input.GetKey( runKey ) || Input.GetKey( runJoykey );
            crouch = Input.GetKeyDown( crouchKey ) || Input.GetKeyDown( crouchJoykey );
            jump = Input.GetKeyDown( jumpKey ) || Input.GetKeyDown( jumpJoykey );
        }


        internal float moveHorizontal { get; private set; }
        internal float moveVertical { get; private set; }
        internal float lookHorizontal { get; private set; }
        internal float lookVertical { get; private set; }

        internal bool run { get; private set; }
        internal bool jump { get; private set; }
        internal bool crouch { get; private set; }
    }
}