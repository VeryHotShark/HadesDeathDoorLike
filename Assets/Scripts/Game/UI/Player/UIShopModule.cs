using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS {
    public class UIShopModule : UIModule<GameController> {
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
            Log("DUPA" + shopItem.name);
        }

        private void SpawnShopItems() {
            for (int i = 0; i < _shopItemsCount; i++) {
                UIShopItem shopItem = Instantiate(_shopItemPrefab, transform);
                _shopItems.Add(shopItem);
            }
        }
    }
}
