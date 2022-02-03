using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ThunderRoad;

namespace Lumiere
{
    public class LumiereItemModule : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            // Load the base class.
            base.OnItemLoaded(item);

            // Is the current level the character selection screen?
            if (string.CompareOrdinal(Level.current.data.id, "CharacterSelection") == 0)
            {
                return;
            }

            // Add the Lumiere component for initialization and set the module.
            item.gameObject.AddComponent<LumiereItem>().ItemModule = this;
        }
    }
}
