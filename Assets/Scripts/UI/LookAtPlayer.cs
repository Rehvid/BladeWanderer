using UnityEngine;

namespace RehvidGames.UI
{
    public class LookAtPlayer: MonoBehaviour
    {
        [SerializeField] private Transform _mainCamera;

        private void Start()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            }
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _mainCamera.forward);
        }
    }
}