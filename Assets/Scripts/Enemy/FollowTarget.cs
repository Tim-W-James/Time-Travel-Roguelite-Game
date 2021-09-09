using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float speed = 3f;
    public float maxDist = 50f;
    public float minDist = 2f;

    private Transform target;
    private GameObject player;
    private bool isPlayerHidden = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
    }

    void Update()
    {
        if (GetComponent<EnemyHP>().hp == GetComponent<EnemyHP>().maxHP)
            isPlayerHidden = !player.GetComponent<PlayerController>().flashLight.activeSelf;
        else
            isPlayerHidden = false;

        if (Vector2.Distance(transform.position, target.position) > minDist && Vector2.Distance(transform.position, target.position) < maxDist && !isPlayerHidden)
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
