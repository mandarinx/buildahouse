/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 FirstPersonControllerEditor.cs        *
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
using UnityEditor;

namespace SmartFirstPersonController.Inspector
{
    [CustomEditor( typeof( FirstPersonController ) )]
    //[CanEditMultipleObjects]
    public class FirstPersonControllerEditor : Editor
    {
        private SerializedProperty
            canWalkProp, walkSpeedProp, backwardsSpeedProp, sidewaysSpeedProp, inAirSpeedProp,
            canRunProp, runSpeedProp,
            canCrouchProp, crouchSpeedProp, crouchHeightProp,
            canJumpProp, jumpForceProp,
            canClimbProp, climbingSpeedProp,
            useHeadBobProp,
            gravityMultiplierProp, fallingDistanceToDamageProp, fallingDamageMultiplierProp, damageVoidNameProp,
            stepIntervalProp, soundsVolumeProp,
            lookSensitivityProp, lookSmoothProp, maxLookAngleYProp, cameraOffsetProp;   

        
        private GUIContent guiContent = new GUIContent( string.Empty );


        // OnEnable
        void OnEnable()
        {
            canWalkProp = serializedObject.FindProperty( "canWalk" );
            walkSpeedProp = serializedObject.FindProperty( "walkSpeed" );
            backwardsSpeedProp = serializedObject.FindProperty( "backwardsSpeed" );
            sidewaysSpeedProp = serializedObject.FindProperty( "sidewaysSpeed" );
            inAirSpeedProp = serializedObject.FindProperty( "inAirSpeed" );

            canRunProp = serializedObject.FindProperty( "canRun" );
            runSpeedProp = serializedObject.FindProperty( "runSpeed" );

            canCrouchProp = serializedObject.FindProperty( "canCrouch" );
            crouchSpeedProp = serializedObject.FindProperty( "crouchSpeed" );
            crouchHeightProp = serializedObject.FindProperty( "crouchHeight" );

            canJumpProp = serializedObject.FindProperty( "canJump" );
            jumpForceProp = serializedObject.FindProperty( "jumpForce" );

            canClimbProp = serializedObject.FindProperty( "canClimb" );
            climbingSpeedProp = serializedObject.FindProperty( "climbingSpeed" );

            useHeadBobProp = serializedObject.FindProperty( "useHeadBob" );

            gravityMultiplierProp = serializedObject.FindProperty( "gravityMultiplier" );
            fallingDistanceToDamageProp = serializedObject.FindProperty( "fallingDistanceToDamage" );
            fallingDamageMultiplierProp = serializedObject.FindProperty( "fallingDamageMultiplier" );
            damageVoidNameProp = serializedObject.FindProperty( "damageVoidName" );

            stepIntervalProp = serializedObject.FindProperty( "stepInterval" );
            soundsVolumeProp = serializedObject.FindProperty( "soundsVolume" );

            lookSensitivityProp = serializedObject.FindProperty( "lookSensitivity" );
            lookSmoothProp = serializedObject.FindProperty( "lookSmooth" );
            maxLookAngleYProp = serializedObject.FindProperty( "maxLookAngleY" );
            cameraOffsetProp = serializedObject.FindProperty( "cameraOffset" );
        }


        // OnInspectorGUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ShowParameters();
            serializedObject.ApplyModifiedProperties();
        }

        // ShowParameters
        private void ShowParameters()
        {
            ShowBoolField( canWalkProp, "Can Walk" );
            ShowSubSlider( walkSpeedProp, 1f, 7f, "Walk Speed" );
            ShowSubSlider( backwardsSpeedProp, 0f, 1f, "Backwards Speed" );
            ShowSubSlider( sidewaysSpeedProp, 0f, 1f, "Sideways Speed" );
            ShowSubSlider( inAirSpeedProp, 0f, 1f, "In Air Speed" );
            GUI.enabled = true;
            
            ShowBoolField( canRunProp, "Can Run" );
            ShowSubSlider( runSpeedProp, 5f, 15f, "Run Speed" );
            GUI.enabled = true;

            ShowBoolField( canCrouchProp, "Can Crouch" );
            ShowSubSlider( crouchSpeedProp, 0f, 1f, "Crouch Speed" );
            ShowSubSlider( crouchHeightProp, 1f, 1.75f, "Crouch Height" );
            GUI.enabled = true;

            ShowBoolField( canJumpProp, "Can Jump" );
            ShowSubSlider( jumpForceProp, 1f, 10f, "Jump Force" );
            GUI.enabled = true;

            ShowBoolField( canClimbProp, "Can Climb" );
            ShowSubSlider( climbingSpeedProp, 0f, 1f, "Climb Speed" );
            GUI.enabled = true;

            GUILayout.Space( 5 );
            guiContent.text = "Use Head Bob";
            EditorGUILayout.PropertyField( useHeadBobProp, guiContent );

            GUILayout.Space( 5f );
            guiContent.text = "Gravity Multiplier";
            EditorGUILayout.Slider( gravityMultiplierProp, 1f, 5f, guiContent );
            guiContent.text = "Falling Distance toDamage";
            EditorGUILayout.Slider( fallingDistanceToDamageProp, 1f, 5f, guiContent );
            guiContent.text = "Falling Damage Multiplier";
            EditorGUILayout.Slider( fallingDamageMultiplierProp, 1f, 10f, guiContent );
            guiContent.text = "Damage Void Name";
            EditorGUILayout.PropertyField( damageVoidNameProp, guiContent );

            GUILayout.Space( 5f );
            guiContent.text = "Step Interval";
            EditorGUILayout.Slider( stepIntervalProp, .1f, 1.5f, guiContent );
            guiContent.text = "Sounds Volume";
            EditorGUILayout.Slider( soundsVolumeProp, 0f, 1f, guiContent );

            GUILayout.Space( 5f );
            guiContent.text = "Look Sensitivity";
            EditorGUILayout.Slider( lookSensitivityProp, .1f, 4f, guiContent );
            guiContent.text = "Look Smooth";
            EditorGUILayout.Slider( lookSmoothProp, .01f, 1f, guiContent );
            guiContent.text = "Max LookAngle Y";
            EditorGUILayout.Slider( maxLookAngleYProp, 25f, 90f, guiContent );
            guiContent.text = "Camera Offset";
            EditorGUILayout.PropertyField( cameraOffsetProp, guiContent );
        }


        // ShowBoolField
        private void ShowBoolField( SerializedProperty property, string label )
        {
            GUILayout.Space( 5 );
            guiContent.text = label;
            EditorGUILayout.PropertyField( property, guiContent );
            GUI.enabled = property.boolValue;
        }

        // ShowSubSlider
        private void ShowSubSlider( SerializedProperty property, float minVal, float maxVal, string label )
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space( 15f );
            guiContent.text = label;
            EditorGUILayout.Slider( property, minVal, maxVal, guiContent );
            GUILayout.EndHorizontal();
        }
    }
}