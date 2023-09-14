using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{   
    [SerializeField] private List<Image> items;
    [SerializeField] private Sprite unoccupiedBox;
    [SerializeField] private Sprite occupiedBox;
    
    private int capacity;
    private int numberOfItems = 0;
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
        AddToInventory(0);
    }
    
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                // user-item interaction
            }
        }
    }

    public void AddToInventory(int itemIndex)
    {
        Image item = items[itemIndex];
        if (numberOfItems < capacity)
        {
            Transform inventoryBox = transform.GetChild(numberOfItems);
            AddShadow(inventoryBox.gameObject);
            Instantiate(item, inventoryBox);
            numberOfItems += 1;
        }
    }

    public void RemoveFromInventory(int boxIndex)
    {
        if (boxIndex >= 0 && boxIndex < numberOfItems)
        {
            Transform inventoryBox = transform.GetChild(boxIndex);
            Transform item = inventoryBox.GetChild(0);
            Destroy(item.gameObject);
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
