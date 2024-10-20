using UnityEngine;

namespace RehvidGames.UI
{
    public class Billboard: MonoBehaviour
    {
        [SerializeField] private Transform _mainCamera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _mainCamera.forward);
        }
    }
}