using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[Header("Game Obj Main")]
	public Carrot.Carrot carrot;
	public GameObject panel_menu;
	public GameObject panel_play;
	public GameObject panel_done;
	public GameObject panel_tip;
	private List<Snake> list_snake;
	public ItemManager item_manager;
	public Animal_Manager animal_manager;
	private static GameManager instance;
	public GameObject[] effect_prefab;
	private bool is_play_one;
	private bool is_play_game=false;
	public SpriteRenderer bk_render_sp;
	public Image[] img_bk_leaf;
	public Sprite[] sp_bk;
	public Sprite[] sp_bk_leaf;

	[Header("Prefab")]
	public GameObject snake_player_one_prefab;
	public GameObject snake_player_two_prefab;

	[Header("Scores")]
	public game_scores[] snake_scores;
	public GameObject panel_scores_two_player;

	[Header("GamePad")]
	public RectTransform[] area_gamepad;
	public GameObject btn_gamepad;
	public RectTransform[] area_gamepad2;
	public GameObject btn_gamepad2;
	private int sel_pos_gamepad = 0;
	private int sel_pos_gamepad2 = 0;

	[Header("Done Game")]
	public GameObject panel_done_one_player;
	public GameObject panel_done_two_player;
	public Text txt_msg_done_one_player;
	public Text txt_msg_done_two_player;
	public Text txt_scores_1;
	public Text txt_scores_2;

	[Header("Sound")]
	public AudioSource[] sound;

	[Header("Setting")]
	public Sprite sp_sound_on;
	public Sprite sp_sound_off;
	public Sprite sp_vibrate_on;
	public Sprite sp_vibrate_off;
	public Sprite sp_list_music_bk;
	public Sprite sp_removeads;

	private Carrot_Gamepad gamepad_player_one = null;
	private Carrot_Gamepad gamepad_player_two = null;

	[Header("Gamepad UI")]
	public List<GameObject> list_btn_main;
	public List<GameObject> list_btn_game_over;
	public List<GameObject> list_btn_tip;
	private KeyCode[] KeyCode_default = new KeyCode[10];

	public void Start()
	{
		this.carrot.Load_Carrot(this.check_exit_app);
		this.carrot.act_after_close_all_box = this.act_after_close_all_box;

		this.carrot.change_sound_click(this.sound[0].clip);

		this.gamepad_player_one = this.carrot.game.create_gamepad("Player_1");
		this.gamepad_player_one.set_gamepad_keydown_left(this.gamepad_keydown_left);
		this.gamepad_player_one.set_gamepad_keydown_right(this.gamepad_keydown_right);
		this.gamepad_player_one.set_gamepad_keydown_down(this.gamepad_keydown_down);
		this.gamepad_player_one.set_gamepad_keydown_up(this.gamepad_keydown_up);
		this.gamepad_player_one.set_gamepad_keydown_select(this.gamepad_keydown_select);
		this.gamepad_player_one.set_gamepad_keydown_start(this.gamepad_keydown_start);


		this.gamepad_player_one.set_gamepad_Joystick_left(this.gamepad_keydown_left);
		this.gamepad_player_one.set_gamepad_Joystick_right(this.gamepad_keydown_right);
		this.gamepad_player_one.set_gamepad_Joystick_down(this.gamepad_keydown_down);
		this.gamepad_player_one.set_gamepad_Joystick_up(this.gamepad_keydown_up);

		KeyCode_default[0] = KeyCode.None;
		KeyCode_default[1] = KeyCode.None;
		KeyCode_default[2] = KeyCode.None;
		KeyCode_default[3] = KeyCode.None;
		KeyCode_default[4] = KeyCode.Joystick2Button0;
		KeyCode_default[5] = KeyCode.None;
		KeyCode_default[6] = KeyCode.None;
		KeyCode_default[7] = KeyCode.None;
		KeyCode_default[8] = KeyCode.None;
		KeyCode_default[9] = KeyCode.None;

		this.gamepad_player_two = this.carrot.game.create_gamepad("Player_2");
		this.gamepad_player_two.set_gamepad_keydown_left(this.gamepad_keydown_left2);
		this.gamepad_player_two.set_gamepad_keydown_right(this.gamepad_keydown_right2);
		this.gamepad_player_two.set_gamepad_keydown_down(this.gamepad_keydown_down2);
		this.gamepad_player_two.set_gamepad_keydown_up(this.gamepad_keydown_up2);
		this.gamepad_player_two.set_gamepad_keydown_select(this.gamepad_keydown_select2);
		this.gamepad_player_two.set_gamepad_keydown_start(this.gamepad_keydown_start2);
		this.gamepad_player_two.set_KeyCode_default(KeyCode_default);

		this.panel_menu.SetActive(true);
		this.panel_play.SetActive(false);
		this.panel_done.SetActive(false);

		this.btn_gamepad.SetActive(false);
		this.btn_gamepad2.SetActive(false);

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		this.carrot.game.load_bk_music(this.GetComponent<AudioSource>());

		this.list_snake = new List<Snake>();
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);

		this.animal_manager.On_Load();
	}

    private void act_after_close_all_box()
    {
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);
    }

    private void check_exit_app()
    {
		if (this.panel_play.activeInHierarchy)
		{
			this.back_menu();
			this.carrot.set_no_check_exit_app();
		} else if (this.panel_done.activeInHierarchy) {
			this.back_menu();
			this.carrot.set_no_check_exit_app();
		}
		else if (this.panel_tip.activeInHierarchy)
		{
			this.btn_close_tip();
			this.carrot.set_no_check_exit_app();
		}
    }

	private void play_game()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.is_play_game = true;
		this.panel_play.SetActive(true);
		this.panel_menu.SetActive(false);
		this.panel_done.SetActive(false);
		this.item_manager.Start_game();
		this.snake_scores[0].rest_scores();
		this.snake_scores[1].rest_scores();
		this.animal_manager.reset();
		this.carrot.game.clear_button_gamepad_console();
	}

	public void btn_play_game_one()
    {
		this.play_sound();
		this.is_play_one = true;
		this.bk_render_sp.sprite = this.sp_bk[0];
		this.img_bk_leaf[0].sprite = this.sp_bk_leaf[0];
		this.img_bk_leaf[1].sprite = this.sp_bk_leaf[0];

		GameObject snake_obj1 = Instantiate(this.snake_player_one_prefab);
		snake_obj1.SetActive(true);
		snake_obj1.transform.localScale = new Vector3(1f, 1f, 1f);
		snake_obj1.GetComponent<Snake>().Start_game(this.snake_scores[0]);
		this.list_snake.Add(snake_obj1.GetComponent<Snake>());

		this.btn_gamepad.SetActive(true);
		this.btn_gamepad2.SetActive(false);

		this.panel_scores_two_player.SetActive(false);
		this.area_gamepad[0].gameObject.SetActive(true);
		this.area_gamepad[1].gameObject.SetActive(true);

		this.area_gamepad2[0].gameObject.SetActive(false);
		this.area_gamepad2[1].gameObject.SetActive(false);
		this.sel_pos_gamepad = PlayerPrefs.GetInt("sel_pos_gamepad", 0);
		this.move_position_game_pad(this.sel_pos_gamepad);

		this.play_game();
	}

	public void btn_play_game_two()
    {
		this.carrot.ads.Destroy_Banner_Ad();
		this.play_sound();
		this.is_play_one = false;
		this.bk_render_sp.sprite = this.sp_bk[1];
		this.img_bk_leaf[0].sprite = this.sp_bk_leaf[1];
		this.img_bk_leaf[1].sprite = this.sp_bk_leaf[1];

		GameObject snake_obj1 = Instantiate(this.snake_player_one_prefab);
		snake_obj1.SetActive(true);
		snake_obj1.transform.localScale = new Vector3(1f, 1f, 1f);
		snake_obj1.GetComponent<Snake>().Start_game(this.snake_scores[0]);
		this.list_snake.Add(snake_obj1.GetComponent<Snake>());

		GameObject snake_obj2 = Instantiate(this.snake_player_two_prefab);
		snake_obj2.SetActive(true);
		snake_obj2.transform.localScale = new Vector3(1f, 1f, 1f);
		snake_obj2.GetComponent<Snake>().Start_game(this.snake_scores[1]);
		this.list_snake.Add(snake_obj2.GetComponent<Snake>());

		this.panel_scores_two_player.SetActive(true);
		this.btn_gamepad.SetActive(true);
		this.btn_gamepad2.SetActive(true);

		this.area_gamepad[0].gameObject.SetActive(true);
		this.area_gamepad[1].gameObject.SetActive(true);

		this.area_gamepad2[0].gameObject.SetActive(true);
		this.area_gamepad2[1].gameObject.SetActive(true);

		this.sel_pos_gamepad = PlayerPrefs.GetInt("sel_pos_gamepad", 0);
		this.move_position_game_pad(this.sel_pos_gamepad);

		this.sel_pos_gamepad2 = PlayerPrefs.GetInt("sel_pos_gamepad2", 0);
		this.move_position_game_pad2(this.sel_pos_gamepad2);

		this.play_game();
	}

	public void btn_game_replay()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.carrot.play_sound_click();
		this.snake_scores[0].rest_scores();
		this.snake_scores[1].rest_scores();
		this.item_manager.clearItems();
		this.list_snake = new List<Snake>();
		if (this.is_play_one)
			this.btn_play_game_one();
		else
			this.btn_play_game_two();
    }

	public void back_menu()
	{
		this.carrot.ads.show_ads_Interstitial();
		this.carrot.play_sound_click();
		this.is_play_game = false;
		this.item_manager.clearItems();

		if (this.list_snake.Count > 0) for(int i = 0; i < this.list_snake.Count; i++) Destroy(this.list_snake[i].gameObject);

		this.list_snake = new List<Snake>();
		this.panel_play.SetActive(false);
		this.panel_done.SetActive(false);
		this.panel_menu.SetActive(true);
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);
		if (this.is_play_one == false) this.carrot.ads.create_banner_ads();
	}

	public static GameManager Instance_Obj() {
		if(instance == null) {
			instance = GameObject.FindGameObjectWithTag(Tags.GameManagerTag).GetComponent<GameManager>();
			if(instance == null) Debug.Log("error: There is no GameManager script in the scene!");
		}
		return instance;
	}

	public delegate void OnSnakeAteItselfHandler();
	public event OnSnakeAteItselfHandler snakeAteItself;

	public delegate void OnSnakeLeftBoardHandler();
	public event OnSnakeLeftBoardHandler snakeLeftBoard;

	public delegate void OnFilledBoardHandler();
	public event OnFilledBoardHandler boardFilled;

	public void onSnakeAteItself() {
		if(snakeAteItself != null) {
			snakeAteItself();
			this.panel_play.gameObject.SetActive(false);
			this.panel_done.gameObject.SetActive(true);
		}
	}

	public void onSnakeLeftBoard() {
		if (this.is_play_game)
		{
			if (snakeLeftBoard != null)
			{
				snakeLeftBoard();
				if (this.is_play_one)
				{
					this.create_effect(this.list_snake[0].head().transform.position,3);
				}
				else
				{
					this.create_effect(this.list_snake[0].head().transform.position,3);
					this.create_effect(this.list_snake[1].head().transform.position,3);
				}
				this.play_sound(3);
				this.act_vibrate();
				this.carrot.delay_function(2f, this.show_gameover);
			}
		}
	}

	private void show_gameover()
    {
		this.panel_play.gameObject.SetActive(false);
		this.panel_done.gameObject.SetActive(true);
		if (this.is_play_one)
		{
			this.panel_done_one_player.SetActive(true);
			this.panel_done_two_player.SetActive(false);
			this.txt_msg_done_one_player.text = "You have ended the game with " + this.snake_scores[0].get_scores() + " points!";
			this.GetComponent<Game_Top>().add_top_one_player(this.snake_scores[0].get_scores());
		}
		else
		{
			this.panel_done_one_player.SetActive(false);
			this.panel_done_two_player.SetActive(true);
			if (this.list_snake[0].snake_scores.get_scores() == this.list_snake[1].snake_scores.get_scores()) this.txt_msg_done_two_player.text = "=";
			if (this.list_snake[0].snake_scores.get_scores() > this.list_snake[1].snake_scores.get_scores()) this.txt_msg_done_two_player.text = ">";
			if (this.list_snake[0].snake_scores.get_scores() < this.list_snake[1].snake_scores.get_scores()) this.txt_msg_done_two_player.text = "<";
			this.txt_scores_1.text = this.list_snake[0].snake_scores.get_scores().ToString();
			this.txt_scores_2.text = this.list_snake[1].snake_scores.get_scores().ToString();
			this.GetComponent<Game_Top>().add_top_two_player(this.snake_scores[0].get_scores(), this.list_snake[1].snake_scores.get_scores());
			this.list_snake[0].kill();
			this.list_snake[1].kill();
		}
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_game_over);
		this.check_and_show_ads();
	}

	public void onBoardFilled() {
		if(boardFilled != null) boardFilled();
	}

	public void move_position_game_pad(int index)
	{
		this.play_sound(0);
		this.area_gamepad[0].gameObject.SetActive(true);
		this.area_gamepad[1].gameObject.SetActive(true);
		this.btn_gamepad.GetComponent<RectTransform>().localPosition = this.area_gamepad[index].localPosition;
		this.area_gamepad[index].gameObject.SetActive(false);
		PlayerPrefs.SetInt("sel_pos_gamepad", index); 
	}

	public void move_position_game_pad2(int index)
	{
		this.play_sound(0);
		this.area_gamepad2[0].gameObject.SetActive(true);
		this.area_gamepad2[1].gameObject.SetActive(true);
		this.btn_gamepad2.GetComponent<RectTransform>().localPosition = this.area_gamepad2[index].localPosition;
		this.area_gamepad2[index].gameObject.SetActive(false);
		PlayerPrefs.SetInt("sel_pos_gamepad2", index);
	}

	private void check_and_show_ads()
	{
		this.carrot.ads.show_ads_Interstitial();
	}

	public void create_effect(Vector3 pos,int type_index)
    {
		GameObject effect_game = Instantiate(this.effect_prefab[type_index]);
		effect_game.transform.SetParent(this.transform);
		effect_game.transform.localPosition = pos;
		effect_game.transform.localScale = new Vector3(1f, 1f, 1f);
		Destroy(effect_game,2f);
	}

	public void play_sound(int index_sound=0)
    {
		if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

	public void show_or_hide_btn_virtual_gamepad()
    {
		if (this.btn_gamepad.activeInHierarchy) this.hide_virtual_gamepad_button(false);
		else this.hide_virtual_gamepad_button(true);
    }

	public void hide_virtual_gamepad_button(bool is_show)
    {
		this.area_gamepad[0].gameObject.SetActive(false);
		this.area_gamepad[1].gameObject.SetActive(false);
		if(sel_pos_gamepad==0) this.area_gamepad[1].gameObject.SetActive(is_show);
		else this.area_gamepad[0].gameObject.SetActive(is_show);
		this.btn_gamepad.SetActive(is_show);
		if (!this.is_play_one)
		{
			this.area_gamepad2[0].gameObject.SetActive(false);
			this.area_gamepad2[1].gameObject.SetActive(false);
			if (sel_pos_gamepad2 == 0) this.area_gamepad2[1].gameObject.SetActive(is_show);
			else this.area_gamepad2[0].gameObject.SetActive(is_show);
			this.btn_gamepad2.SetActive(is_show);
		}
    }

	public void player_one_go_left()
    {
		if(this.is_play_game) this.list_snake[0].go_left();
    }

	public void player_one_go_right()
	{
		if (this.is_play_game) this.list_snake[0].go_right();
	}

	public void player_one_go_up()
	{
		if (this.is_play_game) this.list_snake[0].go_up();
	}

	public void player_one_go_down()
	{
		if (this.is_play_game) this.list_snake[0].go_down();
	}

	public void player_two_go_left()
	{
		if (this.is_play_game) this.list_snake[1].go_left();
	}

	public void player_two_go_right()
	{
		if (this.is_play_game) this.list_snake[1].go_right();
	}

	public void player_two_go_up()
	{
		if (this.is_play_game) this.list_snake[1].go_up();
	}

	public void player_two_go_down()
	{
		if (this.is_play_game) this.list_snake[1].go_down();
	}

	public void btn_show_rate()
    {
		this.carrot.show_rate();
		this.play_sound();
    }
	
	public void btn_show_share()
    {
		this.carrot.show_share();
		this.play_sound();
    }

	public void btn_show_setting()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.play_sound();
		Carrot_Box setting_box=this.carrot.Create_Setting();
		setting_box.update_gamepad_cosonle_control();
	}

	public void btn_show_user_carrot()
    {
		this.play_sound();
		this.carrot.show_login();
    }

	public void show_user_buy_id(string s_id_user, string s_lang)
	{
		this.carrot.user.show_user_by_id(s_id_user, s_lang);
	}

	public void btn_remove_Ads()
    {
		this.play_sound();
		this.carrot.buy_inapp_removeads();
    }

	public void btn_show_list_app_carrot()
    {
		this.play_sound();
		this.carrot.show_list_carrot_app();
    }

	public void btn_show_tip()
    {
		this.play_sound();
		this.panel_tip.SetActive(true);
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_tip);
	}

	public void btn_close_tip()
	{
		this.carrot.game.set_list_button_gamepad_console(this.list_btn_main);
		this.play_sound();
		this.panel_tip.SetActive(false);
	}

	private void act_vibrate()
    {
		this.carrot.play_vibrate();
	}


    #region GamePad Player One
    private void gamepad_keydown_down()
    {
		this.player_one_go_down();
		this.carrot.game.gamepad_keydown_down_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_right()
    {
		this.player_one_go_right();
		this.carrot.game.gamepad_keydown_down_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_left()
    {
		this.player_one_go_left();
		this.carrot.game.gamepad_keydown_up_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_up()
    {
		this.player_one_go_up();
		this.carrot.game.gamepad_keydown_up_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_select()
    {
		Debug.Log("chon");
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_start()
    {
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_y()
    {

	}

    private void gamepad_keydown_x()
    {

    }

    private void gamepad_keydown_a()
    {
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

    private void gamepad_keydown_b()
    {

    }
	#endregion

	#region GamePad Player Two
	private void gamepad_keydown_down2()
	{
		this.player_two_go_up();
		this.carrot.game.gamepad_keydown_down_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_right2()
	{
		this.player_two_go_right();
		this.carrot.game.gamepad_keydown_down_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_left2()
	{
		this.player_two_go_left();
		this.carrot.game.gamepad_keydown_up_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_up2()
	{
		this.player_two_go_down();
		this.carrot.game.gamepad_keydown_up_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_select2()
	{
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_start2()
	{
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_y2()
	{

	}

	private void gamepad_keydown_x2()
	{

	}

	private void gamepad_keydown_a2()
	{
		this.carrot.game.gamepad_keydown_enter_console();
		this.hide_virtual_gamepad_button(false);
	}

	private void gamepad_keydown_b2()
	{

	}
    #endregion
}