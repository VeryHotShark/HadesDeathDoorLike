using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class BossBarUI : UIModule<GameController> {
        [SerializeField] private Transform _barTransform;
        [SerializeField] private UIFillPoint healthFillPointPrefab;
        
        private List<UIFillPoint> _healthPoints = new();

        public override void MyAwake() {
            base.MyAwake();
            Show(false);
        }

        public override void MyEnable() {
            Boss.OnBossEnable += OnBossEnable;
            Boss.OnBossDisable += OnBossDisable;
            Boss.OnBossHealthChanged += OnBossHealthChanged;
        }

        public override void MyDisable() {
            Boss.OnBossEnable -= OnBossEnable;
            Boss.OnBossDisable -= OnBossDisable;
            Boss.OnBossHealthChanged -= OnBossHealthChanged;
        }

        private void OnBossEnable(Boss boss) {
            SpawnHealthPoints(boss.HitPoints.Max);
            Show(true);
        }

        private void OnBossDisable(Boss boss) => Show(false);

        private void SpawnHealthPoints(int count) {
            for (int i = _barTransform.childCount -1 ; i >= 0; i--) 
                Destroy(_barTransform.GetChild(i).gameObject);
            
            for (int i = 0; i < count; i++) {
                UIFillPoint fillPoint = Instantiate(healthFillPointPrefab, _barTransform);
                fillPoint.Fill(true);
                _healthPoints.Add(fillPoint);
            }
        }
        
        public void OnBossHealthChanged(HitPoints hitPoints) {
            for (int i = 0; i < _healthPoints.Count; i++) {
                UIFillPoint fillPoint = _healthPoints[i];
                fillPoint.Fill(i < hitPoints.Current);
            } 
        }
    }
}
