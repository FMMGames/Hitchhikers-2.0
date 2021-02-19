using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CarController myCar;
    [SerializeField] Animator anim;
    [SerializeField] GameObject pistol, rifle;
    [SerializeField] LayerMask racers;

    [SerializeField] int type;
    [SerializeField] int moneyReward;
    [SerializeField] float detectionRange;


    private void OnEnable()
    {
        type = Random.Range(0, 100);

        if(type <= 50)
            anim.SetBool("Melee", true);
        else
        {
            if(type <= 25)
            {
                anim.SetBool("Rifle", true);
                rifle.SetActive(true);
            }
            else
            {
                anim.SetBool("Pistol", true);
                pistol.SetActive(true);
            }
        }     
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, detectionRange, racers))
        {
            Collider[] racersNearby = Physics.OverlapSphere(transform.position, detectionRange, racers);

            transform.LookAt(new Vector3(racersNearby[0].transform.position.x, transform.position.y, racersNearby[0].transform.position.z));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Racer>() && GameManager.instance.currentGameState == GameState.InGame)
        {
            GameManager.instance.EarnKillScore(other.GetComponent<Racer>().racerIndex);
            GameManager.instance.EarnMoney(moneyReward);
            myCar.CoinFX();
            myCar.hostingEnemy = false;
            Destroy(gameObject);
        }
    }
}
