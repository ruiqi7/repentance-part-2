using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{   
    [SerializeField] private List<GameObject> itemImages;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private Sprite unoccupiedBox;
    [SerializeField] private Sprite occupiedBox;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private DialogueController dialogueController;
    
    private int capacity;
    private int numberOfUniqueItems = 0;
    private int[] inventory;
    private int firstUnoccupiedBox = 0;
    private Dictionary<string, int> itemIndexDict = new Dictionary<string, int>()
    {
        {"Heirloom", 0},
        {"Flashlight", 1},
        {"Eyeballs", 2},
        {"Doll", 3},
        {"Candle", 4},
        {"Flower", 5},
        {"Salt", 6}
    };
    private KeyCode[] keyCodes =
    {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
        KeyCode.Alpha0,
	};

    void Start()
    {
        capacity = transform.childCount;
        inventory = new int[capacity];
        for (int i = 0; i < capacity; i++)
        {
            inventory[i] = -1;
        }
        AddToInventory("Heirloom");
        AddToInventory("Flashlight");
    }
    
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Transform inventoryBox = transform.GetChild(i);
                if (inventoryBox.childCount == 1)
                {
                    return;
                }
                Transform itemImage = inventoryBox.GetChild(1);
                int itemIndex = itemIndexDict[itemImage.name.Substring(0, itemImage.name.Length - "(Clone)".Length)];
                GameObject item = items[itemIndex];
                ItemHandlerInterface itemHandler = item.GetComponent<ItemHandlerInterface>();
                bool itemUsed = itemHandler.InitHandler(dialogueBox, dialogueController);
                if (itemUsed)
                {
                    RemoveFromInventory(i);
                }
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        int itemIndex = itemIndexDict[itemName];
        GameObject itemImage = itemImages[itemIndex];
        if (numberOfUniqueItems < capacity)
        {
            if (Array.Exists(inventory, e => e == itemIndex)) // quantity was 1 or more
            {
                int boxIndex = Array.FindIndex(inventory, e => e == itemIndex);
                Transform inventoryBox = transform.GetChild(boxIndex);
                TextMeshProUGUI quantityText = inventoryBox.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                if (!quantityText.enabled) // quantity was 1
                {
                    quantityText.enabled = true;
                }
                else // quantity was 2 or more
                {
                    int quantity = int.Parse(quantityText.text);
                    quantityText.text = (quantity + 1).ToString();
                }
            }
            else // quantity was 0
            {
                FindFirstUnoccupiedBox();
                inventory[firstUnoccupiedBox] = itemIndex;
                Transform inventoryBox = transform.GetChild(firstUnoccupiedBox);
                AddShadow(inventoryBox.gameObject);
                Instantiate(itemImage, inventoryBox);
                numberOfUniqueItems += 1;
            }
        }
    }

    public bool CheckInventory(string item) {
        int itemIndex = itemIndexDict[item];
        return Array.Exists(inventory, e => e == itemIndex);
    }

    public int GetItemIndex(string item) {
        int index = itemIndexDict[item];
        return Array.FindIndex(inventory, e => e == index);
    }

    public void RemoveFromInventory(int boxIndex)
    {
        if (boxIndex >= 0 && boxIndex < capacity)
        {
            Transform inventoryBox = transform.GetChild(boxIndex);
            TextMeshProUGUI quantityText = inventoryBox.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            int quantity = int.Parse(quantityText.text);
            
            if (!quantityText.enabled) // quantity was 1
            {
                inventory[boxIndex] = -1;
                Transform itemImage = inventoryBox.GetChild(1);
                Destroy(itemImage.gameObject);
                numberOfUniqueItems -= 1;
                RemoveShadow(inventoryBox.gameObject);
            }
            else if (quantity == 2) // quantity was 2
            {
                quantityText.enabled = false;
            }
            else // quantity was 3 or more
            {
                quantityText.text = (quantity - 1).ToString();
            }
        }
    }

    private void FindFirstUnoccupiedBox()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (inventory[i] == -1)
            {
                firstUnoccupiedBox = i;
                break;
            }
        }
    }

    private void AddShadow(GameObject box)
    {
        box.GetComponent<Image>().sprite = occupiedBox;
        RectTransform rt = box.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(120, 100);
        rt.anchoredPosition -= new Vector2(15, 14);
    }

    private void RemoveShadow(GameObject box)
    {
        box.GetComponent<Image>().sprite = unoccupiedBox;
        RectTransform rt = box.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(90, 70);
        rt.anchoredPosition += new Vector2(15, 14);
    }
}
