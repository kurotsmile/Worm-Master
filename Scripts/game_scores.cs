using UnityEngine;
using UnityEngine.UI;

public class game_scores : MonoBehaviour
{
    private int scores;
    public Text txt_scores;

    public void add_scores(int val)
    {
        this.scores += val;
        this.txt_scores.text = "Scores:" + this.scores;
    }

    public int get_scores()
    {
        return this.scores;
    }

    public void rest_scores()
    {
        this.scores = 0;
        this.txt_scores.text = "Scores:" + this.scores;
    }
}
