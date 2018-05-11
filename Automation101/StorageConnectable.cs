using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Automation101
{
    public class StorageConnectable : Connectable
    {
        public int itemsPerSecond = 1;
        public StorageContainer container;

        private float nextItemMovement;

        public override ConnectableType Type => ConnectableType.Storage;

        void Start()
        {
            StartCoroutine(OnMoveItem());
        }

        public virtual IEnumerator OnMoveItem()
        {
            while(true)
            {
                if(ConnectedTo != null && ConnectedTo.Type == ConnectableType.Storage)
                {
                    var connectedTo = (StorageConnectable)ConnectedTo;

                    if (container.container.count > 0)
                    {
                        var item = container.container.ToList()[0];
                        container.container.RemoveItem(item.item, true);
                        connectedTo.container.container.AddItem(item.item);
                    }
                }

                yield return new WaitForSeconds(itemsPerSecond);
            }
        }
    }
}
