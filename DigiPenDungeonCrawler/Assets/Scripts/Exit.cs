using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject instructions;
    bool pressed = false;

    void Start()
    {
        instructions.SetActive(false);

        Invoke("SetExit", 3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressed = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            pressed = false;
        }
    }

    void SetExit()
    {
        if (RoomSpawner.GetRoomAt(transform.position) != RoomSpawner.exitRoom)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            instructions.SetActive(true);
        }

        if (other.tag == "Player" && pressed)
        {
            SceneManager.LoadScene(SceneManager.sceneCount + 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            instructions.SetActive(false);
        }
    }
}
