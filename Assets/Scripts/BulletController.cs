using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletController : MonoBehaviour
{

    public float moveSpeed,lifeTime;
    public Rigidbody theRB;

    public int damage=1;
    public GameObject impactEffect;

    public bool damageEnemy, damagePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //  Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    //  Destroy the bullet when it hits something
    private void OnTriggerEnter(Collider other)
    {
        // If the bullet hits an enemy
        if (other.gameObject.tag == "Enemy"&& damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        //  If the bullet hits the player
        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }
        Destroy(gameObject);
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed*Time.deltaTime)), transform.rotation);

    }

}
