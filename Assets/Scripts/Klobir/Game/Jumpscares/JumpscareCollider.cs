using Klobir.Common;
using Klobir.Core;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace Klobir.Game.Jumpscares
{
    public class JumpscareCollider : MonoBehaviour
    {
        [SerializeField] private JumpscareObject _jumpscareObject;

        [SerializeField] private UnityEvent _jumpscareEvent;

        [SerializeField] private GameObject _objectToActivate;

        private MotionBlur _motionBlur;

        private bool _isJumpscared;

        private void Awake()
        {
            _objectToActivate.SetActive(false);
            _motionBlur = FindAnyObjectByType<MotionBlur>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(nameof(Tags.Player)))
            {
                if (!_isJumpscared)
                {
                    _isJumpscared = true;
                    Singleton<JumpscareSystem>.Instance.MakeJumpscare(_jumpscareObject);
                    _objectToActivate.SetActive(true);
                    _jumpscareEvent.Invoke();
                    _motionBlur.blurAmount = 0.8f;
                    Invoke(nameof(DestroyThis), 2);
                }
            }
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
            _motionBlur.blurAmount = 0.4f;
        }
    }
}
