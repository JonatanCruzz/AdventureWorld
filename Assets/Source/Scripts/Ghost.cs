using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    public GameObject ghost;
    public bool makeGhost = false;
    private Coroutine ghostCoroutine;
    void Start()
    {
    }

    private IEnumerator GhostCoroutine()
    {
        while (makeGhost)
        {
            GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
            Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
            currentGhost.transform.localScale = this.transform.localScale;
            currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
            Destroy(currentGhost, 1f);

            yield return new WaitForSeconds(ghostDelay);

        }
    }


    void FixedUpdate()
    {
        if (makeGhost)
        {
            if (ghostCoroutine == null)
            {
                ghostCoroutine = StartCoroutine(GhostCoroutine());
            }
        }
        else
        {
            if (ghostCoroutine != null)
            {
                StopCoroutine(ghostCoroutine);
                ghostCoroutine = null;
            }
        }
    }
}
