using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Resurses : MonoBehaviour
{
    private int wood     = 0;
    private int water    = 0;
    private int stone    = 0;

    private int woodBuf  = 0;
    private int waterBuf = 0;
    private int stoneBuf = 0;

    #region UI
    /*
    [SerializeField] private Text woodUI;
    [SerializeField] private Text waterUI;
    [SerializeField] private Text stoneUI;
    */
    GameObject Res;
    #endregion

    private void Awake()
    {
        TurnManager.ResUpdate += ResUpdate;
        Res = GameObject.FindWithTag("Player1");
    }
    private void ResUpdate()
    {
        Res.transform.Find("WoodFon/Wood").GetComponent<Text>().text   = "<color=#d56a1aff>Wood:" + wood  + "</color><color=lime> + " + woodBuf  + "</color>";
        Res.transform.Find("WaterFon/Water").GetComponent<Text>().text = "<color=aqua>Water:" + water + "</color><color=lime> + " + waterBuf + "</color>";
        Res.transform.Find("StoneFon/Stone").GetComponent<Text>().text = "<color=gray>Stone:" + stone + "</color><color=lime> + " + stoneBuf + "</color>";
        woodBuf = 0; waterBuf = 0 ; stoneBuf = 0 ;

    }

    public void AddWater(int kol)
    {
        water += kol;
        waterBuf += kol;
    }
    public void AddWood(int kol)
    {
        wood += kol;
        woodBuf += kol;
    }
    public void AddStone(int kol)
    {
        stone += kol;
        stoneBuf += kol;
    }
}
