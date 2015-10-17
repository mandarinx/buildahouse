/*******************************************************
 * 													   *
 * Asset:		 Smart First Person Controller 		   *
 * Script:		 Ladder.cs                             *
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
    public class Ladder : MonoBehaviour
    {        
        public AudioClip[] footstepSounds;
        internal Transform m_Transform { get; private set; }

        
        // Awake
        void Awake()
        {
            m_Transform = transform;

            Collider col = this.GetComponent<Collider>();
            if( col )
            {
                col.enabled = true;
                col.isTrigger = true;                
            }
        }


        // PlayLadderFootstepSound
        internal void PlayLadderFootstepSound( ref AudioSource m_Audio, float volumeScale )
        {
            int index = Random.Range( 1, footstepSounds.Length );
            m_Audio.clip = footstepSounds[ index ];
            m_Audio.PlayOneShot( m_Audio.clip, volumeScale );
            footstepSounds[ index ] = footstepSounds[ 0 ];
            footstepSounds[ 0 ] = m_Audio.clip;
        }
    }
}