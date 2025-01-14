using Klobir.Core;
using UnityEngine;

namespace Klobir.Game.Jumpscares
{
    public class JumpscareSystem : Singleton<JumpscareSystem>
    {
        private AudioSource _audioSource;

        protected override void OnInit()
        {
            DontDestroyOnLoad(this);
            SetupAudioSource();
        }

        private void SetupAudioSource()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.Stop();
            _audioSource.reverbZoneMix = 0;
            _audioSource.rolloffMode = AudioRolloffMode.Custom;
            _audioSource.minDistance = 99999999;
            _audioSource.maxDistance = 99999999;
            _audioSource.playOnAwake = false;
        }

        public void MakeJumpscare(JumpscareObject jumpscareObject)
            => _audioSource.PlayOneShot(jumpscareObject.JumpscareSound);
    }
}
