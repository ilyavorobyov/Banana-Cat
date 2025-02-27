using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class LoginRelatedButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _loginPanel;
        [SerializeField] private Button _loginButton;
        [SerializeField] private UIElementsAnimation _uIElementsAnimation;

        private void OnEnable()
        {
            YandexGame.GetDataEvent += OnCheckLogin;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= OnCheckLogin;
        }

        private void OnApplicationFocus(bool state)
        {
            OnCheckLogin();
        }

        public void OnLoginButtonClick()
        {
            _uIElementsAnimation.Appear(_loginPanel.gameObject);
        }

        public void OnCloseLoginPanelButtonClick()
        {
            _uIElementsAnimation.Disappear(_loginPanel.gameObject);
        }

        public void OnLoginButtonPanelClick()
        {
            YandexGame.AuthDialog();
        }

        private void OnCheckLogin()
        {
            if (YandexGame.auth)
            {
                _loginPanel.gameObject.SetActive(false);
                _loginButton.gameObject.SetActive(false);
            }
        }
    }
}