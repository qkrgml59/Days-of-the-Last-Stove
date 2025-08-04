using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]

public class ItemSO : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName = "아이템";
    public Sprite itemIcon;
    public ItemType itemType;

    [Header("효과")]
    public int healthEffect = 0;      //체력회복량
    public int hungerEffect = 0;         //배고픔 회복량
    public int tempEffect = 0;           //체온 회복량
    public int infectionEffect = 0;      //감염률 감소량

    [Header("설명")]
    [TextArea(2, 3)]
    public string description = "유용한 아이템 입니다.";

    public enum ItemType
    {
        Food,                 //음식       (배고픔 회복)
        Fuel,                 //연료      (체온 회복)
        Meidicine,            //의약품 (체력 회복)
        cluster               // 억제제
    }
}
