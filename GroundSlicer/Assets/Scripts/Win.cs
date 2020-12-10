using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] GameObject trailRender;
    [SerializeField] GameObject levelWinPanel;
    Rigidbody rbCharacter;
    Animation anim;
    bool winner;

    // Start is called before the first frame update
    void Start()
    {
        rbCharacter = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Destroy(trailRender);
            GetComponentInParent<MoveController>().enabled = false;
            winner = true;
            anim.Stop("Run");
            anim.Play("Success");
            rbCharacter.velocity = Vector3.zero;
            Time.timeScale = 1f;
            levelWinPanel.SetActive(true);
        }
    }

    public bool GetWin()
    {
        return winner;
    }
}
