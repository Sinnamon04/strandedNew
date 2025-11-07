using UnityEngine;

namespace Platformers
{
    // Item Scriptable Object that holds all item data
    [CreateAssetMenu(menuName = "Scriptable Object/Item")]
    public class Item : ScriptableObject
    {
      [Header("Gameplay")]
      public Vector2 vector = new Vector2(5,4);
      public ItemType itemType;
      public AttackType attackType;
      public ActionType actionType;
      public EquipmentType equipmentType;

      // bonuses used for equippable items that improve player stats
      public int defenseBonus;
      public int attackBonus;
      public int strengthBonus;
      public int healthBonus;
      public int staminaBonus;
      public GameObject weaponPrefab;

      [Header("Only UI")]
      public bool stackable = true;
      public string ID = System.Guid.NewGuid().ToString();
    

      [Header("UI + Gameplay")]
      public Sprite image;
      public GameObject itemModel;
      public string itemName;
      public int price;

    }
    // Enum for different item types
    public enum ItemType
    {
        Mine,
        Weapon,
        Armor,
        Consumable,
        Object
    }
    // Enum for different equipment types
    public enum AttackType
    {
        Melee,
        Range,
        Misc,
    }
    // Enum for different action types
    public enum ActionType
    {
        Dig,
        Attack,
        Protect,
        Consumable
    }
}
