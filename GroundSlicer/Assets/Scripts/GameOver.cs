using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject deathParticle;
    [SerializeField] GameObject trailParticle;
    [SerializeField] GameObject splashSprite;
    [SerializeField] GameObject playerMesh;
    [SerializeField] float health;
    [SerializeField] float bulletPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health -= bulletPower;
            if (health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        if (!(transform.position.y < 0.5f))
            deathParticle.SetActive(true);
        
        GetComponentInParent<MoveController>().enabled = false;
        trailParticle.SetActive(false);
        splashSprite.SetActive(true);
        gameOverPanel.SetActive(true);
        Destroy(playerMesh);
    }
}
