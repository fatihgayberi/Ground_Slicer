using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Win win;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject player;
    [SerializeField] GameObject meshEnemy;
    [SerializeField] GameObject deathParticle;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletSpawnSpeed;
    [SerializeField] float fear;
    [SerializeField] float smoothSpeed;
    Animation anim;

    float bulletTimer;
    float firstHigh;
    GameObject newBullet;
    List<Bullet> bullets;

    // Start is called before the first frame update
    void Start()
    {
        win = FindObjectOfType<Win>();
        bullets = new List<Bullet>();
        anim = GetComponent<Animation>();
        StartCoroutine(BulletSpawner());
        firstHigh = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        GameOverControl();
        Atack();
        StartCoroutine(FallDead());
    }

    void Atack()
    {
        if (player != null && !win.GetWin())
        {
            bulletTimer += Time.deltaTime;
            transform.LookAt(player.transform.position);

            if (IsClose(fear))
            {
                if (bulletTimer >= bulletSpawnSpeed)
                {
                    anim.Stop("Run");
                    StartCoroutine(BulletSpawner());
                    bulletTimer = 0;
                }
            }
            else
            {
                anim.Play("Run");
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, smoothSpeed * Time.deltaTime);
            }

            for (int i = 0; i < bullets.Count; ++i)
            {
                bullets[i].BulletPositionUpdate();
            }
        }
        else
        {
            for (int i = 0; i < bullets.Count; ++i)
            {
                Destroy(bullets[i].GetBulletObject());
                bullets.RemoveAt(i);
            }
        }
    }

    bool IsClose(float fear)
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= fear)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator BulletSpawner()
    {
        anim.Play("Shot");
        yield return new WaitForSeconds(0.5f);
        Vector3 bulletPosition = new Vector3(0, 0, 0);
        if (player != null)
        {
            newBullet = Instantiate(bullet, bulletPosition, Quaternion.identity);
            bullets.Add(new Bullet(player.transform.position, transform.position, newBullet, bulletSpeed));
        }
    }

    void GameOverControl()
    {
        if (player == null)
        {
            GetComponent<EnemyAI>().enabled = false;
        }
    }

    IEnumerator FallDead()
    {
        if (transform.position.y < firstHigh)
        {
            anim.Play("Fall");
            yield return new WaitForSeconds(1f);
            meshEnemy.SetActive(false);
            deathParticle.SetActive(true);
            Destroy(gameObject, 2f);
        }
    }
}
