﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverboard : MonoBehaviour
{
	[Header("Flying")]
	[SerializeField] float maxRaycastLen = 0.3f;
	[SerializeField] float flyingHeight = 0.2f;
	[SerializeField] float flyForce = 11.0f;

	[Header("Rotation")]
	[SerializeField] float rotationStr = 1;

	[Header("Moving")]
	[SerializeField] float moveAcl = 1;

	[Header("Refs")]
	[SerializeField] Rigidbody rb;
	[SerializeField] Transform[] raycastPos;
	[SerializeField] Vector3 centreOfMass;

	public bool isTunnOff = true;
	float currForce;
	float currTurn;

	RaycastHit[] raycastHit;
	bool[] isRaycastHit;
	bool pressW;

	Vector3 velocity = Vector3.zero;

	void Awake()
	{
		raycastHit = new RaycastHit[raycastPos.Length];
		isRaycastHit = new bool[raycastPos.Length];
		rb.centerOfMass = centreOfMass;
	}

	void Update()
	{
		bool isSpacePressed = Input.GetKey(KeyCode.Space);
		if (isTunnOff != isSpacePressed)
		{
			
			isTunnOff = isSpacePressed;

			if (isTunnOff)
				EnableRagdoll();
			else
				DisableRagdoll();
		}


		float hor = Input.GetAxisRaw("Horizontal");
		currTurn = hor != 0 ? hor * rotationStr : 0;

		if (isTunnOff)
			return;

		float vert = Input.GetAxisRaw("Vertical");
		currForce = vert != 0 ? vert * moveAcl : 0;

		bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
		bool isWPressed = Input.GetKey(KeyCode.W);
		if (isShiftPressed && isWPressed)
		{
			SpeedUp(vert);
		}
	}

	void FixedUpdate()
	{
		if (currTurn != 0)
			rb.AddRelativeTorque(Vector3.up * currTurn);

		if (isTunnOff)
			return;

		byte hittedRaycast = 0;

		for (byte i = 0; i < raycastPos.Length; ++i)
		{
			isRaycastHit[i] = Physics.Raycast(raycastPos[i].position, Vector3.down, out raycastHit[i], maxRaycastLen, LayerMask.GetMask("Floor"));

			if (isRaycastHit[i])
			{
				++hittedRaycast;

				rb.AddForceAtPosition(Vector3.up * flyForce * (1.0f - (raycastHit[i].distance / flyingHeight)), raycastPos[i].position);
			}

			//ProcessSmog(i);
		}

		if (hittedRaycast != 0)
		{
			//rb.AddForce(Vector3.up * flyForce * (1.0f - (avgLen / flyingHeight)));

			if (currForce != 0)
				rb.AddForce(transform.right * currForce);
		}
	}

	/// <summary>
	/// Enable particle sistem, sounds, and animatios
	/// </summary>

	void EnableRagdoll()
	{

	}

	/// <summary>
	/// Disable particles sistem, sounds, and animations, while SpaceKey is pressed 
	/// </summary>

	void DisableRagdoll()
	{


	}

	/// <summary>
	/// Is active while W and Shift are pressed
	/// </summary>
	/// <param name="vert"></param>
	void SpeedUp(float vert)
	{
        // Force by SpeedUp is * 5 
        currForce = vert != 0 ? vert * moveAcl * 5 : 0;
    }

}