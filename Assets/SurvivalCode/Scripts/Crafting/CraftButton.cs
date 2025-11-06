using UnityEngine;

namespace Platformers
{
    public class CraftButton : MonoBehaviour
    {
        
        public CraftingManager craftingManager;

        // allows for crafting manager to be updated when button is pressed
        public void ToggleCraftButton()
        {
            craftingManager.UpdateCrafting();
        }
    }
}
