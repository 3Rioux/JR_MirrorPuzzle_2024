using UnityEngine;

namespace UI
{
    public class FpsLock : MonoBehaviour
    {

        [SerializeField] private int _fpsLock = 60;
        private void Awake()
        {
            QualitySettings.vSyncCount = 0; // VSync disable -> default value
            Application.targetFrameRate = _fpsLock; // Lock fps to 60
        }
    }
}
