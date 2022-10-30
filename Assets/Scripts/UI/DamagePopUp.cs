using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using MEC;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS
{
    public class DamagePopUp : BaseBehaviour, IPoolable
    {
        private TextMeshProUGUI _damageText;
        private void Awake() => _damageText = GetComponentInChildren<TextMeshProUGUI>();

        public void Init(int damage, float duration)
        {
            _damageText.faceColor = Color.red;
            _damageText.SetText(damage.ToString());
            transform.DOMoveY(transform.position.y + 1.0f + (Random.value * 0.5f), duration);
            Timing.CallDelayed(duration, () => PoolManager.Return(this), gameObject);
        }
    }
}
