using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider, _sfxSlider, _ambientSlider;

        public void MusicVolume()
        {
            AudioManager.Instance.MusicVolume(_musicSlider.value);
        }
        
        public void SFXVolume()
        {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }
        
        public void AmbientVolume()
        {
            AudioManager.Instance.AmbientVolume(_ambientSlider.value);
        }
    }
}
