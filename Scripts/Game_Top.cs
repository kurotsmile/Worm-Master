using Carrot;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game_Top : MonoBehaviour
{
    public Sprite icon_history_game;
    private int length=0;
    public GameObject prefab_game_highest;
    public GameObject prefab_game_top1;
    public GameObject prefab_game_top2;
    public GameObject prefab_game_rank;
    private float max = 0;
    private Carrot.Carrot carrot;

    void Start()
    {
        this.carrot = this.GetComponent<GameManager>().carrot;
        this.length = PlayerPrefs.GetInt("top_length", 0);
        for (int i = 0; i < this.length; i++)
        {
            if (PlayerPrefs.GetFloat("top_val_" + i) > max) this.max = PlayerPrefs.GetFloat("top_val_" + i);
        }
    }

    public void show_game_top()
    {
        if (length == 0)
        {
            this.carrot.show_msg("Achievement history", "You do not have play rankings");
        }
        else
        {
            Carrot_Box box_game_top = this.carrot.Create_Box();
            box_game_top.set_title("High Score");
            box_game_top.set_icon_white(this.icon_history_game);

            if (this.max > 0)
            {
                GameObject item_top_highest = Instantiate(this.prefab_game_highest);
                item_top_highest.transform.SetParent(box_game_top.area_all_item);
                item_top_highest.transform.localScale = new Vector3(1f, 1f, 0f);
                item_top_highest.GetComponent<Item_box>().txt_p1_scores.text = this.max.ToString();
            }

            for (int i = this.length-1; i >=0; i--)
            {
                if (PlayerPrefs.GetInt("top_type_" + i, 0) == 0)
                {
                    GameObject item_top = Instantiate(this.prefab_game_top1);
                    item_top.transform.SetParent(box_game_top.area_all_item);
                    item_top.transform.localScale = new Vector3(1f, 1f, 0f);
                    item_top.GetComponent<Item_box>().txt_p1_scores.text = PlayerPrefs.GetFloat("top_val_" + i).ToString();
                    item_top.GetComponent<Item_box>().txt_timer.text = PlayerPrefs.GetString("top_time_" + i);
                }
                else
                {
                    GameObject item_top = Instantiate(this.prefab_game_top2);
                    item_top.transform.SetParent(box_game_top.area_all_item);
                    item_top.transform.localScale = new Vector3(1f, 1f, 0f);
                    item_top.GetComponent<Item_box>().txt_p1_scores.text = PlayerPrefs.GetFloat("top_val1_" + i).ToString();
                    item_top.GetComponent<Item_box>().txt_p2_scores.text = PlayerPrefs.GetFloat("top_val2_" + i).ToString();
                    item_top.GetComponent<Item_box>().txt_timer.text = PlayerPrefs.GetString("top_time_" + i);
                }
            }

            this.carrot.game.set_list_button_gamepad_console(box_game_top.UI.get_list_btn());
            this.carrot.game.set_scrollRect_gamepad_consoles(box_game_top.UI.scrollRect);
            this.check_and_upload_scores();
        }
    }

    public void add_top_one_player(float score)
    {
        if (score > 0)
        {
            if (score > this.max) this.max = score;
            PlayerPrefs.SetInt("top_type_" + length, 0);
            PlayerPrefs.SetFloat("top_val_" + length, score);
            PlayerPrefs.SetString("top_time_" + length, DateTime.Now.ToString());
            this.length++;
            PlayerPrefs.SetInt("top_length", this.length);
            this.check_and_upload_scores();
        }
    }

    public void add_top_two_player(float scores1, float scores2)
    {
        if (scores1 > 0 && scores2 > 0)
        {
            if (scores1 > this.max) this.max = scores1;
            PlayerPrefs.SetInt("top_type_" + length, 1);
            PlayerPrefs.SetFloat("top_val1_" + length, scores1);
            PlayerPrefs.SetFloat("top_val2_" + length, scores2);
            PlayerPrefs.SetString("top_time_" + length, DateTime.Now.ToString());
            this.length++;
            PlayerPrefs.SetInt("top_length", this.length);
        }
    }

    public void check_and_upload_scores()
    {
        this.carrot.game.update_scores_player((int)this.max);
    }

    public void show_list_rank()
    {
        this.carrot.game.Show_List_Top_player();
    }
}
