using System.Threading.Tasks;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    public class TrashCan : InventoryReceiver
    {
        private int _clearDelayTime;

        override protected void Start()
        {
            base.Start();
            _clearDelayTime = (int)(Curves.Duration * 1000);
            OnAddItem += ClearReceiver;
        }

        private void OnDestroy()
        {
            OnAddItem -= ClearReceiver;
        }

        private async void ClearReceiver()
        {
            await Task.Delay(_clearDelayTime, AsyncCancellation.Token);
            Clear();
        }

    }
}
