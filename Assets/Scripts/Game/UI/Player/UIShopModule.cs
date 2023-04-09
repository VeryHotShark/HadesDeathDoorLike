using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VHS {
    public class UIShopModule : UIModule<GameController> {
        [SerializeField] private Transform _container;
        [SerializeField] private UIShopItem _shopItemPrefab;
        [SerializeField] private int _shopItemsCount;
        
        private List<UIShopItem> _shopItems = new List<UIShopItem>();

        public override void MyAwake() {
            base.MyAwake();
            SpawnShopItems();
            Show(false);
        }

        public override void MyEnable() {
            foreach (UIShopItem shopItem in _shopItems) 
                shopItem.OnSelected += OnItemSelected;
        }

        public override void MyDisable() {
            foreach (UIShopItem shopItem in _shopItems) 
                shopItem.OnSelected -= OnItemSelected;
        }

        private void OnItemSelected(object sender, UIShopItem shopItem) {
            Show(false);
            GameManager.ResumeGame();
        }

        private void SpawnShopItems() {
            for (int i = _container.childCount -1 ; i >= 0; i--) 
                Destroy(_container.GetChild(i).gameObject);
            
            for (int i = 0; i < _shopItemsCount; i++) {
                UIShopItem shopItem = Instantiate(_shopItemPrefab, _container);
                _shopItems.Add(shopItem);
            }
        }

        public override void OnShow() {
            EventSystem.current.SetSelectedGameObject(_shopItems[0].gameObject);
        }
    }
}
