using Framework.Extensions;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayPage : Page<PageModel>
    {
        [SerializeField] private Text _wrongMoveMessage;

        public override void OnEnter()
        {
            base.OnEnter();
            _wrongMoveMessage.gameObject.SetActive(false);
        }

        public void ShowWrongMoveMessage()
        {
            if (_wrongMoveMessage.gameObject.activeSelf)
            {
                return;
            }
            
            _wrongMoveMessage.gameObject.SetActive(true);
            this.WaitForSeconds(3f, () => _wrongMoveMessage.gameObject.SetActive(false));
        }
    }
}