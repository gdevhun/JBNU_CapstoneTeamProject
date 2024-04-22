using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoints : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<Enemy>().movepoint_num == 1)
            {
                collision.gameObject.GetComponent<Enemy>().movepoint_num = 5;
            }
            else if (collision.gameObject.GetComponent<Enemy>().movepoint_num == 2)
            {
                collision.gameObject.GetComponent<Enemy>().movepoint_num = 3;
            }
            else if (collision.gameObject.GetComponent<Enemy>().movepoint_num == 5)
            {
                collision.gameObject.GetComponent<Enemy>().movepoint_num = 6;
            }
            else if (collision.gameObject.GetComponent<Enemy>().movepoint_num == 3)
            {
                collision.gameObject.GetComponent<Enemy>().movepoint_num = 8;
            }
            else if (collision.gameObject.GetComponent<Enemy>().movepoint_num == 4)
            {
                collision.gameObject.GetComponent<Enemy>().movepoint_num = 7;
            }

        }
    }
}
