using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EmeWillem
{
    public class Posture : MonoBehaviour
    {
        private BaseAnimatorManager _animatorManager;
        private List<string> _animationNames;

        private int _maximumPosture;
        private int _currentPosture;
        private int _postureRecovery;
        private int _pendingPostureChange;

        public virtual void Init(List<string> animationNames, int maximumPosture, int postureRecovery)
        {
            _animatorManager = GetComponent<BaseAnimatorManager>();

            _animationNames = animationNames;

            _maximumPosture = maximumPosture;
            _postureRecovery = postureRecovery;
            _currentPosture = _maximumPosture;
            _pendingPostureChange = 0;

            StartCoroutine(StaggerCoroutine());
        }

        public virtual void LateTick()
        {
            if (_pendingPostureChange != 0)
            {
                _currentPosture += _pendingPostureChange;

                if (_currentPosture <= 0)
                {
                    if (_animationNames.Count > 1)
                    {
                        int randomIndex = Random.Range(0, _animationNames.Count);
                        _animatorManager.ForceCrossFade(_animationNames[randomIndex]);
                    }
                    else
                    {
                        _animatorManager.ForceCrossFade(_animationNames[0]);
                    }

                    _currentPosture = _maximumPosture;
                }
                else if (_currentPosture > _maximumPosture)
                {
                    _currentPosture = _maximumPosture;
                }

                _pendingPostureChange = 0;
            }
        }

        public virtual void InflictStagger(int amount)
        {
            _pendingPostureChange -= amount;
        }

        private IEnumerator StaggerCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                if (_currentPosture < _maximumPosture)
                {
                    _pendingPostureChange += _postureRecovery;
                }
            }
        }
    }
}