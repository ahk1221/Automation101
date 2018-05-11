using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using System.Collections;

namespace Automation101
{
    public class PlanterConnectable : StorageConnectable
    {
        public Planter planter;

        private int currentIndex = 0;

        public override IEnumerator OnMoveItem()
        {
            while(true)
            {
                var connectedTo = (StorageConnectable)ConnectedTo;

                if (currentIndex > planter.storageContainer.container.count - 1)
                    currentIndex = 0;

                var inventoryItem = planter.storageContainer.container.ToList()[currentIndex];
                var plantable = inventoryItem.item.GetComponent<Plantable>();

                if (plantable.linkedGrownPlant != null)
                {
                    var grownPlant = plantable.linkedGrownPlant;

                    if (grownPlant.GetComponent<FruitPlant>() != null)
                    {
                        var fruitPlant = grownPlant.GetComponent<FruitPlant>();

                        if (fruitPlant.fruits.Length > 0)
                        {
                            var fruit = fruitPlant.fruits[0];
                            fruit.AddToContainer(connectedTo.container.container);
                        }
                    }
                    else if(grownPlant.GetComponent<GrownPlant>() != null)
                    {
                        var seed = grownPlant.seed;

                        if (seed != null && !seed.isSeedling && seed.pickupable != null && Inventory.Get().HasRoomFor(seed.pickupable) && seed.currentPlanter != null)
                        {
                            seed.currentPlanter.RemoveItem(seed);
                            connectedTo.container.container.AddItem(seed.pickupable);
                        }
                    }
                }

                yield return new WaitForSeconds(itemsPerSecond);
            }
        }
    }
}
