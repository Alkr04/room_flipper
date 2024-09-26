using Controller;
using UnityEngine;

namespace Triggers
{
    public class Window : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _animator.enabled = true;
        }

        private void Update()
        {
            if (_animator.enabled)
            {
                AnimatorStateInfo asi = _animator.GetCurrentAnimatorStateInfo(0);
                if (asi.normalizedTime >= 1)
                {
                    UIController.ShowMenuPanel();
                }
            }
        }
    }
}