using Assets.Dasbor.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusBrain : MonoBehaviour, IBrain
{
    [Tooltip("Max force for octopus; random float between this and minSpeed is applied at push rate")] [Range(400f, 1000f)] [SerializeField] float maxSpeed = 400f;
    [Tooltip("Min force for octopus; random float between this and maxSpeed is applied at push rate")] [Range(100f, 399f)] [SerializeField] float minSpeed = 50f;

    [Tooltip("Max rotation up for the octopus")] [Range(60f, 80f)] [SerializeField] float maxRotation = 80f;
    [Tooltip("Min rotation down for the octopus")] [Range(45f, 59f)] [SerializeField] float minRotation = 45f;

    [Tooltip("Max time between applying forces on octopus")] [Range(3f, 10f)] [SerializeField] float maxTime = 4.5f;
    [Tooltip("Min time between applying forces on octopus")] [Range(0.2f, 2.9f)] [SerializeField] float minTime = 1.0f;

    // Start is called before the first frame update
    public Vector3 direction;

    Rigidbody2D rigidBody;
    Animator animator;
    ParticleSystem bubbles;
    int swimHash = Animator.StringToHash("swim");
    int idleHash = Animator.StringToHash("idle");



    float pushTime;
    float timer = 0f;
    float idleTriggerTime = 1f;
    float idleTriggerTimer = 0f;
    public bool idle = true;

    void Start()
    {
        rigidBody = transform.parent.GetComponent<Rigidbody2D>();
        pushTime = Random.Range(minTime, maxTime);
        animator = transform.parent.GetComponentInChildren<Animator>();
        bubbles = transform.parent.GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!idle)
        {
            idleTriggerTimer += Time.deltaTime;
        }
        if (timer >= pushTime)
        {
            Stroke();
        }
        if (idleTriggerTimer >= idleTriggerTime)
        {
            Retract();
        }

        //transform.parent.position = Vector2.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }    

    void Stroke()
    {
        rigidBody.AddForce(transform.parent.up * Random.Range(minSpeed, maxSpeed));
        animator.SetTrigger(swimHash);
        bubbles.Play();
        idle = false;
        timer = 0f;
        idleTriggerTimer = 0f;
        pushTime = Random.Range(minTime, maxTime);
        idleTriggerTime = pushTime - minTime;
    }

    void Retract()
    {
        idleTriggerTimer = 0f;
        rigidBody.AddForce(-transform.parent.up * Random.Range(minSpeed / 10f, minSpeed / 3f));
        animator.SetTrigger(idleHash);
        idle = true;
    }

    public void SetDirection(Vector2 direction)
    {
        //Quaternion rotation = Quaternion.LookRotation(new Vector2(-0.3f,-0.8f));
        //transform.parent.rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z);
        this.direction = direction;
        
        if (direction.x < 0 && transform.parent.localScale.x > 0)
        {
            transform.parent.Rotate(new Vector3(0, 0, 1) * Random.Range(minRotation, maxRotation), Space.World);
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        else// if (direction.x > 0 && transform.parent.localScale.x < 0)
        {
            transform.parent.localScale = new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
            transform.parent.Rotate(new Vector3(0, 0, 1) * -(Random.Range(minRotation, maxRotation)), Space.World);
        }
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
