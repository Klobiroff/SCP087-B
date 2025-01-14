using UnityEngine;
using UnityEngine.Events;

namespace Klobir.Game.Jumpscares
{
    [CreateAssetMenu(fileName = "Jumpscare", menuName = "Klobir/Jumpscare Object", order = 1)]
    public class JumpscareObject : ScriptableObject
    {
        [SerializeField] private AudioClip _jumpscareSound;

        public AudioClip JumpscareSound => _jumpscareSound;
    }
}
