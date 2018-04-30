using Framework.Extensions;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using Framework.Variables;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Event = Framework.Events.Event;

namespace Game.UI
{
    public class ReplayPage : Page<PageModel>
    {
        [SerializeField] private Text _title;
        [SerializeField] private Text _tapMessage;
        [SerializeField] private Text _movesCountMessage;
        [SerializeField] private IntVariable _movesCountVariable;
        [SerializeField] private Event _onTapEvent;

        public override void OnEnter()
        {
            base.OnEnter();

            _movesCountMessage.text = string.Format("You made it in {0} moves", _movesCountVariable.Value);
            
            _title.gameObject.SetActive(false);
            _movesCountMessage.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);
            
            this.WaitForSeconds(1f, () =>
            {
                _title.gameObject.SetActive(true);
                _movesCountMessage.gameObject.SetActive(true);
                _tapMessage.gameObject.SetActive(true);
            });
        }

        public override void OnExit()
        {
            _title.gameObject.SetActive(false);
            _tapMessage.gameObject.SetActive(false);
            _movesCountMessage.gameObject.SetActive(false);
            base.OnExit();
        }

        [UsedImplicitly]
        public void FireEvent()
        {
            _onTapEvent.Fire();
        }
    }
}