/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 FootStepSoundsManager.cs              *
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
    public class FootStepSoundsManager : MonoBehaviour
    {
        [System.Serializable]
        public struct MatConcrete
        {
            public AudioClip[] footstepSounds;
            public AudioClip jumpingSound;
            public AudioClip landigSound;
        }

        [System.Serializable]
        public struct MatGravel
        {
            public AudioClip[] footstepSounds;
            public AudioClip jumpingSound;
            public AudioClip landigSound;
        }

        [System.Serializable]
        public struct MatMetal
        {
            public AudioClip[] footstepSounds;
            public AudioClip jumpingSound;
            public AudioClip landigSound;
        }

        [System.Serializable]
        public struct MatWood
        {
            public AudioClip[] footstepSounds;
            public AudioClip jumpingSound;
            public AudioClip landigSound;
        }
                
        [System.Serializable]
        public struct GenericMat
        {
            public AudioClip[] footstepSounds;
            public AudioClip jumpingSound;
            public AudioClip landigSound;
        }
        
        public MatConcrete matConcrete;
        public MatGravel matGravel;
        public MatMetal matMetal;
        public MatWood matWood;
        public GenericMat genericMat;
        

        // PlayJumpingSound
        internal void PlayJumpingSound( string groundTag, ref AudioSource m_Audio, float volumeScale )
        {
            m_Audio.pitch = Time.timeScale;
            switch( groundTag )
            {
                case TagsManager.matConcrete:
                    m_Audio.PlayOneShot( matConcrete.jumpingSound, volumeScale );
                    break;

                case TagsManager.matGravel:
                    m_Audio.PlayOneShot( matGravel.jumpingSound, volumeScale );
                    break;

                case TagsManager.matMetal:
                    m_Audio.PlayOneShot( matMetal.jumpingSound, volumeScale );
                    break;

                case TagsManager.matWood:
                    m_Audio.PlayOneShot( matWood.jumpingSound, volumeScale );
                    break;

                default:
                    m_Audio.PlayOneShot( genericMat.jumpingSound, volumeScale );
                    break;
            }
        }

        // PlayLandingSound
        internal void PlayLandingSound( string groundTag, ref AudioSource m_Audio, float volumeScale)
        {
            m_Audio.pitch = Time.timeScale;
            switch( groundTag )
            {
                case TagsManager.matConcrete:
                    m_Audio.PlayOneShot( matConcrete.landigSound, volumeScale );
                    break;

                case TagsManager.matGravel:
                    m_Audio.PlayOneShot( matGravel.landigSound, volumeScale );
                    break;

                case TagsManager.matMetal:
                    m_Audio.PlayOneShot( matMetal.landigSound, volumeScale );
                    break;

                case TagsManager.matWood:
                    m_Audio.PlayOneShot( matWood.landigSound, volumeScale );
                    break;

                default:
                    m_Audio.PlayOneShot( genericMat.landigSound, volumeScale );
                    break;
            }
        }

        // PlayFootStepAudio
        internal void PlayFootStepSound( string groundTag, ref AudioSource m_Audio, float volumeScale)
        {
            m_Audio.pitch = Time.timeScale;
            switch( groundTag )
            {
                case TagsManager.matConcrete:
                    PlayRandomFootstep( matConcrete.footstepSounds, ref m_Audio, volumeScale );
                    break;

                case TagsManager.matGravel:
                    PlayRandomFootstep( matGravel.footstepSounds, ref m_Audio, volumeScale );
                    break;

                case TagsManager.matMetal:
                    PlayRandomFootstep( matMetal.footstepSounds, ref m_Audio, volumeScale );
                    break;

                case TagsManager.matWood:
                    PlayRandomFootstep( matWood.footstepSounds, ref m_Audio, volumeScale );
                    break;

                default:
                    PlayRandomFootstep( genericMat.footstepSounds, ref m_Audio, volumeScale );
                    break;
            }
        }

        // PlayRandomStepSound
        private void PlayRandomFootstep( AudioClip[] stepSounds, ref AudioSource m_Audio, float volumeScale )
        {
            int index = Random.Range( 1, stepSounds.Length );
            m_Audio.clip = stepSounds[ index ];
            m_Audio.PlayOneShot( m_Audio.clip, volumeScale );
            stepSounds[ index ] = stepSounds[ 0 ];
            stepSounds[ 0 ] = m_Audio.clip;
        }
    }
}