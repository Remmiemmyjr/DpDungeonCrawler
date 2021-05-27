using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: Brackeys
// DESC: Manages enemy states and
// pathfinding logic
// DATE MODIFIED: 5/15/2021
//
// CREDITS: Brackeys 2d pathfinding
// tutorial and A* pathfinder
// package by Aron Granberg
// ===============================

public class EnemyAI : MonoBehaviour
{
    enum States {Idle, Following, StartAttacking, Attacking, Home};

    States state = States.Idle;

    enum Direction {North, South, East, West};

    public float speed = 2f;
    public float nextWaypointDistance = 1f;
    public float followDistance = 2.5f;

    public float attackRadius = 1;
    private float attackTime = 1f;
    private float attackCounter = 1f;

    public BoxCollider2D North;
    public BoxCollider2D South;
    public BoxCollider2D East;
    public BoxCollider2D West;

    Transform target;
    Seeker seeker;
    Path path;
    Vector3 home;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [HideInInspector]
    public Rigidbody2D rb;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        home = transform.position;
    }

    void Update()
    {
        StateMachine();

        if (state == States.Following)
        {
            MoveTowards(target.position);
        }

        if(state == States.Home && rb.position.x == home.x)
        {
            state = States.Idle;
        }

        #region pathfinding
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        if (state != States.Attacking)
        {
            Vector2 movement = direction * speed;
            rb.velocity = movement;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        #endregion

    }

    Direction GetDirection(Vector3 dir)
    {
        // moving towards lower right
        if(dir.x >= dir.y)
        {
           if(dir.x > -dir.y)
            {
                return Direction.East;
            }
            else
            {
                return Direction.South;
            }
        }
        // moving towards upper left
        else
        {
            if (dir.x > -dir.y)
            {
                return Direction.North;
            }
            else
            {
                return Direction.West;
            }
        }
    }

    //float nextUpdateTime = 0;
    //void UpdatePath()
    //{

    //    //if (Time.time > nextUpdateTime)
    //    //{
    //    //    if (seeker.IsDone())
    //    //    {
    //    //        nextUpdateTime = Time.time + 0.5f;

    //    //        seeker.StartPath(rb.position, target.position, OnPathComplete);
    //    //    }
    //    //}
    //}

    void MoveTowards(Vector2 targetObj)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetObj, speed * Time.deltaTime);
    }

    void HomePath()
    {
        MoveTowards(home);
        //transform.position = Vector2.MoveTowards(transform.position, home, speed * Time.deltaTime);

        //if (Time.time > nextUpdateTime)
        //{
        //    if (seeker.IsDone())
        //    {
        //        nextUpdateTime = Time.time + 0.5f;

        //        seeker.StartPath(rb.position, home, OnPathComplete);
        //    }
        //}

        //state = States.Idle;
    }

    void Idle()
    {
        rb.velocity = new Vector2();
    }

    //void OnPathComplete(Path p)
    //{
    //    if (!p.error)
    //    {
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    States lastState;
    void StateMachine()
    {
        if (state != lastState)
        {
            Debug.Log(state);
            lastState = state;
        }

        switch (state)
        {
            case States.Idle:
                if (Vector2.Distance(rb.position, target.position) < followDistance && Vector2.Distance(rb.position, target.position) > attackRadius)
                {
                    state = States.Following;
                    Debug.Log("following");
                }
                break;


            case States.Following:
                //UpdatePath();
                if (RoomSpawner.GetRoomAt(target.position) != RoomSpawner.GetRoomAt(this.transform.position))
                {
                    state = States.Home;
                    //path.vectorPath.Clear();
                    // temp, will have "return" movement
                    rb.velocity = new Vector2();
                    Debug.Log("not following");
                }
                if (Vector2.Distance(rb.position, target.position) < attackRadius)
                {
                    state = States.StartAttacking; 
                }
                break;


            case States.StartAttacking:
                var attackDir = GetDirection(target.position - transform.position);
                rb.velocity = new Vector2();
                switch (attackDir)
                {
                    case Direction.North:
                        North.gameObject.SetActive(true);
                        break;
                    case Direction.South:
                        South.gameObject.SetActive(true);
                        break;
                    case Direction.East:
                        East.gameObject.SetActive(true);
                        break;
                    case Direction.West:
                        West.gameObject.SetActive(true);
                        break;
                }
                attackCounter = attackTime;
                state = States.Attacking;
                break;


            case States.Attacking:
                attackCounter -= Time.deltaTime;
                if (attackCounter <= 0)
                {
                    DisableHitbox();
                }
                // do animation
                break;


            case States.Home:
                HomePath();
                break;
        }
    }


    void DisableHitbox()
    {
        North.gameObject.SetActive(false);

        South.gameObject.SetActive(false);

        East.gameObject.SetActive(false);

        West.gameObject.SetActive(false);

        state = States.Following;

        attackCounter = attackTime;
    }

}
