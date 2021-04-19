using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingScore : MonoBehaviour
{
    // Start is called before the first frame update
    float destroyAfter = 2f;
    float time = 0;
    TextMeshPro tmp;

    public void SetScore(int score)
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        tmp.text = score.ToString();
        StartCoroutine(MoveScore());
    }

    IEnumerator MoveScore()
    {
        while (time < destroyAfter)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + .01f, transform.position.z);
            time += Time.deltaTime;
            yield return new WaitForSeconds(.01f);
        }

        Destroy(gameObject);
    }
}
