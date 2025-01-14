using Klobir.Core;
using Klobir.Game.Horror;
using Klobir.Game.Jumpscares;
using System.Collections;
using UnityEngine;

namespace Klobir.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class GameService : Singleton<GameService>
    {
        [SerializeField] private AudioSource _soundTrackSource;

        private AudioClip[] _ambientSounds;
        private AudioSource _audioSource;
        private MotionBlur _motionBlur;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _ambientSounds = Resources.LoadAll<AudioClip>("SFX/Ambients");
            _soundTrackSource.loop = true;
            _motionBlur = FindAnyObjectByType<MotionBlur>();
            _motionBlur.enabled = true;
            ChangeHorrorState(0);
            Singleton<JumpscareSystem>.Init();
            StartCoroutine(PlayAmbient());
        }

        protected override void OnInit()
        {

        }

        private void PlayAmbientSound(AudioClip ambientSound)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(ambientSound);
                Debug.Log("Какой то пидорас там чёто делает, ну и хуй с ним");
            }
        }

        private IEnumerator PlayAmbient()
        {
            while (true)
            {
                PlayAmbientSound(_ambientSounds[Random.Range(0, _ambientSounds.Length)]);
                yield return new WaitForSeconds(Random.Range(30, 60));
            }
        }

        public void ChangeHorrorState(int horrorTypeInt)
        {
            var horrorType = (HorrorType)horrorTypeInt;
            _soundTrackSource.Stop();
            switch (horrorType)
            {
                case HorrorType.Start:
                    _soundTrackSource.clip = Resources.Load<AudioClip>("SFX/Music/The Beginning");
                    SetRenderSettings(Color.white, 0.3f, 0.2f);
                    _soundTrackSource.Play();
                    break;
                case HorrorType.Chased:
                    _soundTrackSource.clip = Resources.Load<AudioClip>("SFX/Music/Gathering Darkness");
                    SetRenderSettings(Color.black, 1f, 0.4f);
                    _soundTrackSource.Play();
                    break;
            }
        }

        private void SetRenderSettings(Color ambientColor, float fogIntensity, float blurIntensity)
        {
            RenderSettings.ambientSkyColor = ambientColor;
            RenderSettings.fogDensity = fogIntensity;
            _motionBlur.blurAmount = blurIntensity;
        }
    }
}
