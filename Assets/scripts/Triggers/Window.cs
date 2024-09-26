using Controller;
using UnityEngine;

namespace Triggers
{
    public class Window : MonoBehaviour
    {
        private Animator _animator;
        public AudioClip screem;
        AudioSource sorce;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            sorce = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            sorce.PlayOneShot(screem);
            _animator.enabled = true;
        }

        private void Update()
        {
            if (_animator.enabled)
            {
                var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                if (animatorStateInfo.normalizedTime >= 1)
                {
                    UIController.ShowMenuPanel(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}