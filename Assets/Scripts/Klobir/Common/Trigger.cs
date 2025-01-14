using UnityEngine;
using UnityEngine.Events;

namespace Klobir.Common
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onTriggerEnter;
        [Space]
        [SerializeField] private UnityEvent _onTriggerStay;
        [Space]
        [SerializeField] private UnityEvent _onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(nameof(Tags.Player)))
            {
                _onTriggerEnter.Invoke();
                Destroy(gameObject);
            } 
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(nameof(Tags.Player)))
            {
                _onTriggerStay.Invoke();
                Destroy(gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(nameof(Tags.Player)))
            {
                _onTriggerExit.Invoke();
                Destroy(gameObject);
            }   
        }
    }
}
