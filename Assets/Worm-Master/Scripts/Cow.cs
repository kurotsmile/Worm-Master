using UnityEngine;

public class Cow : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sp_render;
    public int type = 0;
    private float speed = 1f;
    private Direction direction=Direction.UP;
    float timer_change_anim = 0f;
    private Direction[] direction_cow=new Direction[4];

    void Start()
    {
        direction_cow[0]= Direction.UP;
        direction_cow[1]= Direction.DOWN;
        direction_cow[2]= Direction.LEFT;
        direction_cow[3]= Direction.RIGHT;

        direction = this.rand_Direction();
        this.update_cow();
    }

    void Update()
    {
        if(direction==Direction.UP)this.transform.Translate(Vector3.up * this.speed * Time.deltaTime);
        if(direction==Direction.DOWN) this.transform.Translate(Vector3.down * this.speed * Time.deltaTime);
        if(direction==Direction.LEFT) this.transform.Translate(Vector3.left * this.speed * Time.deltaTime);
        if(direction==Direction.RIGHT) this.transform.Translate(Vector3.right * this.speed * Time.deltaTime);

        this.timer_change_anim += 1f * Time.deltaTime;
        if (this.timer_change_anim >= 15f)
        {
            this.change_direction();
            this.timer_change_anim = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == Tags.BoundsTag)
        {
            if (direction == Direction.UP) this.direction = Direction.DOWN;
            else if (direction == Direction.DOWN) this.direction = Direction.UP;
            else if (direction == Direction.RIGHT) this.direction = Direction.LEFT;
            else if (direction == Direction.LEFT) this.direction = Direction.RIGHT;

            this.update_cow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.ItemTag)
        {
            GameObject.Find("Game").GetComponent<GameManager>().item_manager.remove(collision.gameObject);
            GameObject.Find("Game").GetComponent<GameManager>().create_effect(collision.gameObject.transform.localPosition, 1);
            Destroy(collision.gameObject);
        }
    }

    private void change_direction()
    {
        Direction d = this.rand_Direction();
        if (d == this.direction)
        {
            change_direction();
        }
        else
        {
            this.direction = d;
            this.update_cow();
        }
    }

    private Direction rand_Direction()
    {
        int r = Random.Range(0, this.direction_cow.Length);
        return this.direction_cow[r];
    }

    private void update_cow()
    {
        if (this.direction == Direction.UP) this.anim.Play("Cow_up");
        else if (this.direction == Direction.DOWN) this.anim.Play("Cow_down");
        else if(this.direction == Direction.LEFT)
        {
            this.anim.Play("cow_left");
            this.sp_render.flipX = true;
        }
        else if(this.direction == Direction.RIGHT)
        {
            this.anim.Play("cow_left");
            this.sp_render.flipX = false;
        }
    }
}
