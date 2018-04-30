using Game.Gameplay.RingStructure;
using Game.Gameplay.RingStructure.Configuration;
using UnityEngine;
using Event = Framework.Events.Event;

namespace Game.Gameplay
{
    [RequireComponent(typeof(RingInputHandler), typeof(RingContactHandler), typeof(RingView))]
    public class Ring : MonoBehaviour
    {
        private RingInputHandler _inputHandler;
        private RingContactHandler _contactHandler;
        private RingView _ringView;

        private bool _isAnchoredToTower;
        private bool _isTopRing;
        private Vector2 _anchoredPosition;
        private Vector2 _dragPosition;
        private Vector3 _velocity;
        private Tower _currentTower;
        private Tower _nextTower;

        [SerializeField] private int _sizeIndex;
        [SerializeField] private RingSettings _ringSettings;
        [SerializeField] private Event _onWrongMoveEvent;

        public int SizeIndex
        {
            get { return _sizeIndex; }
        }

        private void Start()
        {
            _inputHandler = GetComponent<RingInputHandler>();
            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.Drag += OnDrag;
            _inputHandler.PointerUp += OnPointerUp;

            _contactHandler = GetComponent<RingContactHandler>();
            _contactHandler.Entered += OnTowerEntered;
            _contactHandler.Exited += OnTowerExited;

            _ringView = GetComponent<RingView>();
            _ringView.SetInteractive(false);
            _isAnchoredToTower = true;
        }

        public void AttachTo(Tower tower, bool isCountable)
        {
            tower.Add(this, isCountable);
            _currentTower = tower;
            _anchoredPosition = tower.GetTopPosition();
        }

        private void Update()
        {
            UpdatePosition();
            UpdateView();
        }

        private void UpdatePosition()
        {
            Vector2 targetPosition;

            if (_inputHandler.IsDragging && _isTopRing)
            {
                targetPosition = _dragPosition;

                if (_isAnchoredToTower)
                {
                    targetPosition.x = _anchoredPosition.x;

                    if (targetPosition.y < _anchoredPosition.y)
                    {
                        targetPosition.y = _anchoredPosition.y;
                    }
                }
            }
            else
            {
                targetPosition = _anchoredPosition;

                if (!_isAnchoredToTower)
                {
                    var anchorPosition = _currentTower.Anchor.transform.position;
                    if (Vector2.Distance(transform.position, anchorPosition) > 1f)
                    {
                        targetPosition = anchorPosition;
                    }
                    else
                    {
                        _isAnchoredToTower = true;
                    }
                }
            }

            var newPosition = Vector3.SmoothDamp(transform.position, targetPosition,
                ref _velocity, _ringSettings.MoveSmoothing);

            newPosition.z = 0f;
            transform.position = newPosition;
        }

        private void UpdateView()
        {
            _ringView.UpdateState(_isAnchoredToTower);
        }

        private void OnPointerDown(Vector2 position)
        {
            _isTopRing = _currentTower.GetTopRingSizeIndex() == SizeIndex;
            _dragPosition = position;

            if (_isTopRing)
            {
                _ringView.SetInteractive(true);
            }
            else
            {
                _ringView.SetBlocked();
            }
        }

        private void OnDrag(Vector2 position)
        {
            _dragPosition = position;
        }

        private void OnPointerUp()
        {
            if (_nextTower != null)
            {
                var topSizeIndex = _nextTower.GetTopRingSizeIndex();
                if (topSizeIndex < _sizeIndex)
                {
                    _currentTower.Remove();
                    AttachTo(_nextTower, true);
                }
                else
                {
                    _onWrongMoveEvent.Fire();
                }

                _nextTower = null;
            }

            if (_isTopRing)
            {
                _ringView.SetInteractive(false);
            }
        }

        private void OnTowerEntered(Tower tower)
        {
            if (tower != _currentTower)
            {
                _nextTower = tower;
            }
        }

        private void OnTowerExited(Tower tower)
        {
            if (tower == _currentTower)
            {
                _isAnchoredToTower = false;
            }
            else if (tower == _nextTower)
            {
                _nextTower = null;
            }
        }

        private void OnDestroy()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.Drag -= OnDrag;
            _inputHandler.PointerUp -= OnPointerUp;

            _contactHandler.Entered -= OnTowerEntered;
            _contactHandler.Exited -= OnTowerExited;
        }
    }
}