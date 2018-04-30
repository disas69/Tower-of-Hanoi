using System.Collections;
using Framework.Extensions;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Event = Framework.Events.Event;

namespace Game.UI
{
    public class StartPage : Page<PageModel>
    {
        private Coroutine _overlayTransitionCoroutine;

        [SerializeField] private float _overlayTransitionSpeed;
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private Text _title;
        [SerializeField] private Text _message;
        [SerializeField] private Event _onTapEvent;

        [UsedImplicitly]
        public void FireEvent()
        {
            _onTapEvent.Fire();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            _message.gameObject.SetActive(false);
            this.WaitForSeconds(1f, () => _message.gameObject.SetActive(true));
        }

        protected override IEnumerator InTransition()
        {
            _overlayTransitionCoroutine = StartCoroutine(ShowOverlay());
            yield return _overlayTransitionCoroutine;
        }

        private IEnumerator ShowOverlay()
        {
            _overlay.gameObject.SetActive(true);
            _overlay.alpha = 1f;

            while (_overlay.alpha > 0f)
            {
                _overlay.alpha -= _overlayTransitionSpeed * 2f * Time.deltaTime;
                yield return null;
            }

            _overlay.alpha = 0f;
            _overlay.gameObject.SetActive(false);
            _overlayTransitionCoroutine = null;
        }

        public override void OnExit()
        {
            _overlay.gameObject.SetActive(false);
            _title.gameObject.SetActive(false);
            _message.gameObject.SetActive(false);
            this.SafeStopCoroutine(_overlayTransitionCoroutine);
            base.OnExit();
        }
    }
}