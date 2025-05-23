using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Scripts.Game
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMP_Text actionForceText;
        [SerializeField] Toggle toggle;

        public void Setup(Sprite sprite, string actionForce)
        {
            gameObject.SetActive(true);
            image.sprite = sprite;
            actionForceText.text = actionForce;
        }

        public bool IsOn => toggle.isOn;
        
        public void SetIsOn(bool state) => toggle.isOn = state;
        
    }
}