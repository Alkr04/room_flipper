using Controller;
using UnityEngine;

namespace Triggers
{
    public class Exit : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            UIController.DisplayEndPopups();
        }
    }
}