using System;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;

namespace VHS {
    public class UIShopItem : ChildBehaviour<UIShopModule> {
        
        [SerializeField] private BetterImage _image;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        
        public EventHandler<UIShopItem> OnSelected = delegate{  };
        
        private ShopItemSO _shopItemSO;
        private BetterButton _betterButton;

        public ShopItemSO Item => _shopItemSO;

        private void Awake() => _betterButton = GetComponent<BetterButton>();
        protected override void Enable() => _betterButton.onClick.AddListener(OnClick);
        protected override void Disable() => _betterButton.onClick.RemoveListener(OnClick);

        private void OnClick() {
            // Log(_shopItemSO.name);
            OnSelected(this, this);
        }

        public void SetShopItemSO(ShopItemSO shopItemSo) {
            _shopItemSO = shopItemSo;
            _name.SetText(_shopItemSO.Name);
            _image.sprite = _shopItemSO.Icon;
            _description.SetText(_shopItemSO.Description);
        }
    }
}
