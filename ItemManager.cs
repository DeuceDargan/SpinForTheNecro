using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemObject> itemsList = new List<ItemObject>();
    private List<ItemObject> itemsOnsale = new List<ItemObject>();
    private List<ItemObject> itemsObtained = new List<ItemObject>();

    public bool hasWhetStone;

    // Start is called before the first frame update
    void Start()
    {
        itemsOnsale = itemsList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<ItemObject> ItemsToSale()
    {
        return itemsOnsale;
    }

    public List<ItemObject> ItemsObtained()
    {
        return itemsObtained;
    }

    public void RemoveFromSale(ItemObject item)
    {
        itemsOnsale.Remove(item);
    }

    public void AddToPouch(ItemObject item)
    {
        itemsObtained.Add(item);
    }
}
