using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public PlayerMovementScript[] players;
    public GameObject assignedPlayer;
    public bool inProcessOfMoving;
    public float speed, turnSpeed;
	
    void Start()
    {
        players = GameObject.FindObjectsOfType<PlayerMovementScript>();
    }
	
	void Update () {
        if(assignedPlayer == null || !assignedPlayer.GetComponent<PlayerMovementScript>().activePlayer)
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.activePlayer)
                {
                    assignedPlayer = player.gameObject;
                    inProcessOfMoving = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "BluePlayer1")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "BluePlayer2")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "BluePlayer3")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "BluePlayer4")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "BluePlayer5")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }

        if (inProcessOfMoving)
        {
            MoveToPlayer(assignedPlayer.transform);
        }
	}

    void MoveToPlayer(Transform player)
    {
        transform.parent = player;
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, 0), step);

        float stepR = turnSpeed * Time.deltaTime;

        float angle = Mathf.LerpAngle(transform.localEulerAngles.y, 0, stepR);
        transform.localEulerAngles = new Vector3(0, angle, 0);

        if ((transform.localPosition.x == 0 && transform.localPosition.z == 0) && (transform.localEulerAngles.y == 0 || transform.localEulerAngles.y == 360))
            inProcessOfMoving = false;
    }
}