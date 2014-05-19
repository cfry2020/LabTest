using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class followleaderBehaviourScript : MonoBehaviour {
	
	private List<Vector3> Path = new List<Vector3>();	
	private Vector3 startPosition;						
	private Vector3 endPosition;							
	public float speed = 1.0f;							
	private float lerpDistanceCovered;						
	private float lerpTotalDistance;						
	private float lerpRemaineder;							
	private Vector3 faceDirection; 						
	
	void Update()
	{
		if (speed <= 0)
		{
			Debug.Log("followleader WARNING: The SPEED needs to be more than 0");
			speed = 1.0f;
		}
		
		
		if (Path.Count >= 1)
		{
			followPath ();
			
		}
		else
		{
			
		}
	}
	
	private void followPath()
	{
		
		lerpDistanceCovered += speed * Time.deltaTime;
		
		
		transform.position = Vector3.Lerp(startPosition, endPosition, lerpDistanceCovered/lerpTotalDistance );
		
		
		transform.forward = new Vector3(endPosition.x -startPosition.x, 0.0f, endPosition.z -startPosition.z) ;
		
		
		if((lerpDistanceCovered/lerpTotalDistance) >= 1 && Path.Count > 1)
			gotoNextWaypoint();
		else if ((lerpDistanceCovered/lerpTotalDistance) >= 1 && Path.Count <= 1)
			StopEnemy();
	}
	
	
	private void gotoNextWaypoint()
	{
		float lastLerpTotalDistance;
		float lerpRemaining;
		
		do
		{
			lastLerpTotalDistance = lerpTotalDistance;
			lerpRemaining = (lerpDistanceCovered/lerpTotalDistance) - 1.0f;
			
			setupLerp();
			
			
			if (lerpRemaining > 0.0f)
			{
				lerpDistanceCovered = lerpFractionToDistance(lastLerpTotalDistance, lerpRemaining);
				transform.position = Vector3.Lerp(startPosition, endPosition, lerpDistanceCovered/lerpTotalDistance );
			}
		} while (lerpRemaining > 1.0f);
	}
	
	
	private void setupLerp()
	{
		startPosition = (Vector3) Path[0];
		Path.RemoveAt(0);
		endPosition = (Vector3) Path[0];
		lerpTotalDistance = Vector3.Distance(startPosition, endPosition);
		lerpDistanceCovered = 0;
	}
	
	
	private float distanceToLerpFraction(float totalDistance, float distance)
	{
		float numberOfSteps = totalDistance / distance;
		float oneStepPecentage = 1 / numberOfSteps;
		return oneStepPecentage;
	}
	private float lerpFractionToDistance(float totalDistance, float lerpFraction)
	{
		float onePecent = totalDistance / 100;
		float distance = lerpFraction * onePecent;
		return distance;
	}
	
	
	public void NewPath(List<Vector3> temp)
	{
		Path = new List<Vector3>();
		Path.AddRange(temp);
		setupLerp();
	}
	
	
	public void StopEnemy()
	{
		Path.Clear();
		startPosition = new Vector3();
		endPosition = new Vector3();
	}
	
	
	public void Death()
	{
		Destroy(this.gameObject);
	}
}
