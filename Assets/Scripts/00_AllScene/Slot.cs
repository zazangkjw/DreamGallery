using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int index;
    public Item item;
    public RawImage slotImage;
    public RawImage backgroundImage;
    public Slot connectedSlot; // 퀵슬롯에 인벤토리 퀵슬롯 연결
    public static int currentIndex; // 퀵슬롯 인덱스(1~10)
    public static Slot selectedSlot;
    public static RawImage cursorImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        connectedSlotUpdate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (selectedSlot == null)
            {
                if (!item.itemName.Equals("Empty"))
                {
                    selectedSlot = this;
                    cursorImage.texture = item.itemImage;
                    cursorImage.gameObject.SetActive(true);
                }
                else
                {
                    
                }
            }
            else
            {
                // 아이템 서로 바꾸기
                Item i = item;
                item = selectedSlot.item;
                selectedSlot.item = i;

                // 아이템 이미지 갱신
                Texture t = slotImage.texture;
                bool a = slotImage.gameObject.activeSelf;
                slotImage.texture = selectedSlot.slotImage.texture;
                slotImage.gameObject.SetActive(selectedSlot.slotImage.gameObject.activeSelf);
                selectedSlot.slotImage.texture = t;
                selectedSlot.slotImage.gameObject.SetActive(a);

                selectedSlot = null;
                cursorImage.texture = null;
                cursorImage.gameObject.SetActive(false);
            }
        }
    }

    public void connectedSlotUpdate()
    {
        if (connectedSlot != null)
        {
            if (item != connectedSlot.item)
            {
                item = connectedSlot.item;
            }

            if (slotImage.texture != connectedSlot.slotImage.texture)
            {
                slotImage.texture = connectedSlot.slotImage.texture;
            }

            if (slotImage.gameObject.activeSelf != connectedSlot.slotImage.gameObject.activeSelf)
            {
                slotImage.gameObject.SetActive(connectedSlot.slotImage.gameObject.activeSelf);
            }
        }
    }
}
