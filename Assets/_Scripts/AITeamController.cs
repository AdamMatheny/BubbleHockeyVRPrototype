using UnityEngine;
using System.Collections;

public class AITeamController : MonoBehaviour 
{
	[SerializeField] private PlayerMovementScript[] mAIPlayers;

	[SerializeField] private PlayerMovementScript mClosestTeamMate;

	[SerializeField] private GameObject mPuck;


	// Use this for initialization
	void Start () 
	{
		mClosestTeamMate = mAIPlayers[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForPuck();

		//Only move the AI if the puck isn't in play ~Adam
		if (mPuck != null)
		{
			DetermineClosestTeamMate();

			if(mPuck.transform.position.z <= mClosestTeamMate.transform.position.z)
			{
				if(mClosestTeamMate.turnedAround)
				{
					mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.back;
				}
				else
				{
					mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.forward;
				}
			}
			else
			{
				if(!mClosestTeamMate.turnedAround)
				{
					mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.back;
				}
				else
				{
					mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.forward;
				}
			}

			if(Vector3.Distance(mClosestTeamMate.transform.position,mPuck.transform.position)<3f)
			{
				mClosestTeamMate.transform.Rotate(Vector3.up * Time.deltaTime * mClosestTeamMate.turningSpeed);
			}
				
		}
	}


	void CheckForPuck()
	{
		mPuck = GameObject.FindGameObjectWithTag("Puck");
	}

	void DetermineClosestTeamMate()
	{
		foreach (PlayerMovementScript teamMate in mAIPlayers)
		{
			if(Mathf.Abs(teamMate.transform.position.x - mPuck.transform.position.x) < (Mathf.Abs(mClosestTeamMate.transform.position.x - mPuck.transform.position.x)))
			{
				mClosestTeamMate = teamMate;
			}
		}
	}
}
