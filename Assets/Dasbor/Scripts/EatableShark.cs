using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EatableShark : MonoBehaviour, IEatable
{
    //[SerializeField] HealthBar healthBar;
    [SerializeField] int maxHealth = 5;
    [SerializeField] int startingHealth = 5;
    [SerializeField] int deathTransitionScene = 0;

    [SerializeField] float pitchReverseSpeed = 0.2f;


    int health = 3;

    SpriteRenderer bodySprite, headSprite;

    int numberOfFlashesWhenHit = 20;
    float timeToFlash = .2f;

    public int currentHealth;

    GameObject livesContainer;

    public bool invulnerable = false;
    public bool shielded = false;

    AudioSource audio;

    public TransitionScenePersistent transitionScript;

    void Start()
    {
        currentHealth = startingHealth;
        bodySprite = transform.parent.GetComponent<SpriteRenderer>();
        headSprite = transform.parent.Find("Head").GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        livesContainer = GameObject.Find("Lives");
        transitionScript = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScenePersistent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Eaten(int damage)
    {
        if (!invulnerable && !shielded)
        {
            currentHealth -= damage;
            GameObject.Find("Lives").transform.GetChild(currentHealth).gameObject.SetActive(false);
            //health -= damage;
            //Destroy(livesContainer.transform.GetChild(livesContainer.transform.childCount - 1).gameObject);
            if (currentHealth <= 0)
            {
                transitionScript.LoadMainMenuNonAsynchronous(2, 1);
                HandleMusicReversal();
                //StartCoroutine(transitionScript.Transition(deathTransitionScene));
                Destroy(gameObject.transform.parent.gameObject);

                // END GAME HERE
            }
            else
            {
                StartCoroutine(TakeDamage(damage));
            }
        }
    }

    void HandleMusicReversal()
    {
        float oldPitch = MusicControl.instance.GetCurrentPitch("Track2");
        MusicControl.instance.LerpToNewPitch("Track2", oldPitch, MusicControl.instance.GetInitialPitch("Track2"), pitchReverseSpeed);
    }

    public void RegenerateHealth(int recoverNumber)
    {
        for (int i = 0; i < recoverNumber; i++)
        {
            if (currentHealth < startingHealth)
            {
                currentHealth += 1;
                livesContainer.transform.GetChild(currentHealth-1).gameObject.SetActive(true);
            }
        }        
    }

    IEnumerator TakeDamage(int damage)
    {
        float damagePerFlash = (float)damage / (float)numberOfFlashesWhenHit;
        float currentTotalDamage = currentHealth + damage;
        SetInvulnerable(true);
        audio.Play();
        for (int x = 0; x < numberOfFlashesWhenHit; x++)
        {
            if (currentHealth >= 1)
            {
                currentTotalDamage -= damagePerFlash;
                float damagePerc = (float)(maxHealth - currentTotalDamage) / (float)maxHealth;
                //healthBar.SetSize(damagePerc);
            }            
            if (x % 2 == 0)
            {
                SwitchColour(Color.red);
            }
            else
            {
                SwitchColour(Color.white);
            }
            yield return new WaitForSeconds(timeToFlash);
        }

        SetInvulnerable(false);
        SwitchColour(Color.white);
    }

    void SwitchColour(Color colour)
    {
        bodySprite.color = colour;
        headSprite.color = colour;
    }

    public bool IsInvulnerable()
    {
        return this.invulnerable;
    }

    public void SetInvulnerable(bool newInvulnerable)
    {
        this.invulnerable = newInvulnerable;
    }

    public bool IsShielded()
    {
        return this.shielded;
    }

    public void SetShielded(bool newShielded)
    {
        this.shielded = newShielded;
    }
}