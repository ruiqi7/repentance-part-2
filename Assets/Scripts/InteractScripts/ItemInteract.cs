using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : InteractableInterface
{
    [SerializeField] private string itemName;
    
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = GameObject.Find("InventoryFrame").GetComponent<InventoryController>();
    }

    public override void interact()
    {
        inventoryController.AddToInventory(itemName);
        Destroy(gameObject);
    }
}
