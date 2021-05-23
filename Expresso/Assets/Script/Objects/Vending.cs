using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vending : MonoBehaviour
{
    public GameObject m_CoffeePrefab;
    public bool nearVending = false;
    private Vector2 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Vector2(this.transform.position.x, this.transform.position.y - 1);
    }

    void SpawnCoffee()
    {
        Instantiate(m_CoffeePrefab, spawnPos, Quaternion.identity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            nearVending = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            nearVending = false;
        }
    }

    void DispenseCoffee()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(nearVending == true)
            {
                SpawnCoffee();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DispenseCoffee();
    }
}
