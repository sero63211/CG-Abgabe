 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{

    private bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f,distanceToStop=2f;

    private Vector3 targetPoint,startPoint;


    public NavMeshAgent agent;

    public float keepChasingTime=5f;
    private float chaseCounter;


    public GameObject bullet;
    public Transform firePoint;

    public float fireRate,waitBetweenShots=2f,timetoShoot=1f;
    private float fireCount,shotWaitCounter,shotTimeCounter;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        shotTimeCounter = timetoShoot;
        shotTimeCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        // if player is in range then chase him 
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                shotTimeCounter = timetoShoot;
                shotWaitCounter = waitBetweenShots;
            }
            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }
            if (agent.remainingDistance < 0.25f)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;

            }
            else
            {
                agent.destination = transform.position;
            }



            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter = keepChasingTime;
            }


            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;


                if (shotWaitCounter <= 0)
                {
                    shotTimeCounter = timetoShoot;
                }
                anim.SetBool("isMoving", true);   
                
            }
            else
            {
                if (PlayerController.instance.gameObject.activeInHierarchy)
                {

                    shotTimeCounter -= Time.deltaTime;



                    if (shotTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;


                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;
                            firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0, 1.2f, 0));

                            //check the angle to the player
                            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);

                                anim.SetTrigger("fireShot");
                            }
                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                            }

                            Instantiate(bullet, firePoint.position, firePoint.rotation);
                        }




                        agent.destination = transform.position;
                    }
                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }
                }
                anim.SetBool("isMoving", false);
            }
        }



    }
}