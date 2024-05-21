using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D theRB;

    public UnitDisplay target;
    public float speed = 10f;
    public int dmg = 1;
    public int bonusDmg = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.GetComponent<UnitDisplay>() != null && other.GetComponent<UnitDisplay>() == target)
        {
            target.Hurt(dmg + bonusDmg);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}
