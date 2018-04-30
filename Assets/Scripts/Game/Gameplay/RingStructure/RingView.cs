using Framework.Extensions;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Gameplay.RingStructure
{
    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public class RingView : MonoBehaviour
    {
        private readonly int _isActiveBoolHash = Animator.StringToHash("IsActive");
        private readonly int _blockedAnimationHash = Animator.StringToHash("Blocked");
        private readonly int _finishedAnimationHash = Animator.StringToHash("Finished");

        private Animator _animator;
        private AudioSource _audioSource;
        private int _defaultSortingOrder;
        private bool _innerAnchoredState;

        [SerializeField] private SpriteRenderer _frontSide;
        [SerializeField] private SpriteRenderer _backSide;
        [SerializeField] private int _anchoredStateOrder = -1;
        [SerializeField] private int _unanchoredStateOrder = 10;

        [Header("SFX")] [SerializeField] private AudioClip _interaction1AudioClip;
        [SerializeField] private AudioClip _interaction2AudioClip;
        [SerializeField] private AudioClip _blockedAudioClip;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _defaultSortingOrder = _frontSide.sortingOrder;
        }

        public void SetInteractive(bool value)
        {
            _animator.SetBool(_isActiveBoolHash, value);
            PlayAudioClip(value ? _interaction1AudioClip : _interaction2AudioClip);
        }

        public void SetBlocked()
        {
            _animator.Play(_blockedAnimationHash);
            PlayAudioClip(_blockedAudioClip);
        }

        [UsedImplicitly]
        public void PlayFinish()
        {
            this.WaitForSeconds(Random.value, () => { _animator.Play(_finishedAnimationHash); });
        }

        public void UpdateState(bool isAnchoredToTower)
        {
            if (isAnchoredToTower == _innerAnchoredState)
            {
                return;
            }

            if (isAnchoredToTower)
            {
                _frontSide.sortingOrder = _defaultSortingOrder;
                _backSide.sortingOrder = _anchoredStateOrder;
            }
            else
            {
                _frontSide.sortingOrder = _unanchoredStateOrder;
                _backSide.sortingOrder = _unanchoredStateOrder;
            }

            _innerAnchoredState = isAnchoredToTower;
        }

        private void PlayAudioClip(AudioClip clip)
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _audioSource.PlayOneShot(clip);
        }
    }
}