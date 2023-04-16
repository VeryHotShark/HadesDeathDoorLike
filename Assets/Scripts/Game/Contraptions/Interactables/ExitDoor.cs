namespace VHS {
    public class ExitDoor : Interactable {

        private bool _isLocked = false;

        public void SetLocked(bool state) => _isLocked = state;

        public override bool IsInteractable(Player player) => _isLocked == false && base.IsInteractable(player);

        public override void OnInteract(Player player) {
            base.OnInteract(player);
            RoomManager.RequestExitRoom(this);
        }
    }
}
