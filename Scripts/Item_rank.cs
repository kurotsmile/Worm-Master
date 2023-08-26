using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_rank : MonoBehaviour
{
    public Text txt_name;
    public Text txt_scores;
    public Image img_avatar;
    public string s_id_user;
    public string s_lang;
    public void click()
    {
        GameObject.Find("Game").GetComponent<GameManager>().show_user_buy_id(this.s_id_user, this.s_lang);
    }
}
