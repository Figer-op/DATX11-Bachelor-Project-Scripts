using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroCheck : TargetCheck
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            enemy.IsAggroed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            enemy.IsAggroed = false;
        }
    }
}
