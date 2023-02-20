using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public float onCollisionDamage;

    private void Start()
    {
        onCollisionDamage = 5;
        speed = 2f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Calculates the percent of health instead of numbers to trigger boss attack patterns.
        switch(BossManager.instance.currentHealth)
        {
            // First stage when 100% health and greater than 50%.
            case float i when i > BossManager.instance.boss.MaxHealth / 2:
                FirstStage();
                break;
            // Second stage when lower than 50% but not 0%.
            case float i when i <= BossManager.instance.boss.MaxHealth / 2 && i > 0:
                SecondStage();
                break;
            // Destroys the boss when 0%.
            case float i when i <= 0:
                BossManager.instance.bossHPSlider.SetActive(false);
                Destroy(gameObject);
                break;
        }

        BossManager.instance.BossHP();

        // Debugging purposes.
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            BossManager.instance.currentHealth -= 5;
        }
        
    }

    // First stage of boss, triggered at beginning
    private void FirstStage()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Second stage of boss, triggered after % of health has been damaged.
    private void SecondStage()
    {
        speed = 3.5f;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
