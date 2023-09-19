using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{   
    [SerializeField] private List<Image> itemImages;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private Sprite unoccupiedBox;
    [SerializeField] private Sprite occupiedBox;
    
    private int capacity;
    private int numberOfItems = 0;
    private Dictionary<string, int> itemIndexDict = new Dictionary<string, int>()
    {
        {"Heirloom", 0},
        {"Flashlight", 1},
        {"Eyeballs", 2},
        {"Doll", 3},
        {"Candle", 4},
        {"Flower", 5},
        {"Letter", 6},
        {"Salt", 7}
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
                if (inventoryBox.childCount == 0)
                {
                    return;
                }
                Transform itemImage = inventoryBox.GetChild(0);
                int itemIndex = itemIndexDict[itemImage.name.Substring(0, itemImage.name.Length - "(Clone)".Length)];
                GameObject item = items[itemIndex];
                ItemHandlerInterface itemHandler = item.GetComponent<ItemHandlerInterface>();
                bool itemUsed = itemHandler.HandleBehavior();
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
        Image itemImage = itemImages[itemIndex];
        if (numberOfItems < capacity)
        {
            Transform inventoryBox = transform.GetChild(numberOfItems);
            AddShadow(inventoryBox.gameObject);
            Instantiate(itemImage, inventoryBox);
            numberOfItems += 1;
        }
    }

    public void RemoveFromInventory(int boxIndex)
    {
        if (boxIndex >= 0 && boxIndex < numberOfItems)
        {
            Transform inventoryBox = transform.GetChild(boxIndex);
            Transform itemImage = inventoryBox.GetChild(0);
            Destroy(itemImage.gameObject);
            numberOfItems -= 1;
            RemoveShadow(inventoryBox.gameObject);
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
