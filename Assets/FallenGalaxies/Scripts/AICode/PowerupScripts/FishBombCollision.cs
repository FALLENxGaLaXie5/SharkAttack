using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FishBombCollision : MonoBehaviour
{
    [SerializeField] GameObject explosionObject;
    [SerializeField] Transform explosionPoint;
    [Tooltip("The amount of damage the mine can do to the player. The player has 100 hit points.")] [Range(1f, 3)] [SerializeField] int mineDamage = 1;

    AudioSource explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        explosionAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(explosionObject, explosionPoint.position, Quaternion.identity);
            explosionAudio.Play();
            collision.gameObject.GetComponent<IEatable>().Eaten(mineDamage);
            CameraShaker.Instance.ShakeOnce(9f, 4f, .3f, 2f);
            Destroy(transform.parent.gameObject, 0.60f);
        }
    }
}
