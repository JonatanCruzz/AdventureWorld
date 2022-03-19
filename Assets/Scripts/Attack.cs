using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") collision.SendMessage("Attacked");
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().anim.SetTrigger("damage");
            collision.GetComponent<Enemy>().damage = true;

            if (transform.position.x > collision.transform.position.x)
            {
                collision.GetComponent<Enemy>().empuje = -3;
                collision.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                collision.GetComponent<Enemy>().empuje = 3;
                collision.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (transform.position.y > collision.transform.position.y)
            {
                collision.GetComponent<Enemy>().empuje = -3;
                collision.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                collision.GetComponent<Enemy>().empuje = 3;
                collision.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

}
