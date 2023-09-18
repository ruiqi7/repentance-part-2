using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : InteractableInterface
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject inventoryFrame;
    
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = inventoryFrame.GetComponent<InventoryController>();
    }

    public override void interact()
    {
        inventoryController.AddToInventory(itemName);
        Destroy(gameObject);
    }
}
