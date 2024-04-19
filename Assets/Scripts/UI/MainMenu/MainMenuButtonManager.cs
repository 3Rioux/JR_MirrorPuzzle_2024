using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace UI.MainMenu
{
    public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerUpHandler,IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defaultSprite, _pressedSprite, _hoverSprite;
        [SerializeField] private AudioClip _buttonPressSound,_buttonHoverSound;
        [SerializeField] private AudioSource _audioSource;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            _image.sprite = _pressedSprite;
            _audioSource.PlayOneShot(_buttonPressSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.sprite = _hoverSprite;
            _audioSource.PlayOneShot(_buttonHoverSound);
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            _image.sprite = _defaultSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.sprite = _defaultSprite;
        }
    }
}