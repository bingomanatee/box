﻿using UnityEngine;
using System.Collections;

public class ModTerrain : MonoBehaviour
{
		World world;
		GameObject cameraGO;

		void Start ()
		{
				world = GameObject.FindGameObjectWithTag ("World").GetComponent<World> ();
				cameraGO = GameObject.FindGameObjectWithTag ("MainCamera");
		}

		void Update ()
		{
				if (Input.GetMouseButtonDown (0)) {
						ReplaceBlockCursor (0);
				}
		
				if (Input.GetMouseButtonDown (1)) {
						AddBlockCursor (1);
				}
		}

	#region modification
	
		public void ReplaceBlockCenter (float range, byte block)
		{
				//Replaces the block directly in front of the player
				Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
				RaycastHit hit;
		
				if (Physics.Raycast (ray, out hit)) {
			
						if (hit.distance < range) {
								ReplaceBlockAt (hit, block);
						}
				}
		}
	
		public void AddBlockCenter (float range, byte block)
		{
				//Adds the block specified directly in front of the player  
				Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
				RaycastHit hit;
		
				if (Physics.Raycast (ray, out hit)) {
			
						if (hit.distance < range) {
								AddBlockAt (hit, block);
						}
						Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
				}
		}
	
		public void ReplaceBlockCursor (byte block)
		{
				//Replaces the block specified where the mouse cursor is pointing
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
		
				if (Physics.Raycast (ray, out hit)) {
			
						ReplaceBlockAt (hit, block);
						Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance),
			               Color.green, 2);
				}
		}
	
		public void AddBlockCursor (byte block)
		{
				//Adds the block specified where the mouse cursor is pointing
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
		
				if (Physics.Raycast (ray, out hit)) {
			
						AddBlockAt (hit, block);
						Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance),
			               Color.green, 2);
				}
		
		}
	
		public void ReplaceBlockAt (RaycastHit hit, byte block)
		{
				//removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
				Vector3 position = hit.point;
				position += (hit.normal * -0.5f);
		
				SetBlockAt (position, block);
		}
	
		public void AddBlockAt (RaycastHit hit, byte block)
		{
				//adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
				Vector3 position = hit.point;
				position += (hit.normal * 0.5f);
		
				SetBlockAt (position, block);

		}
	
		public void SetBlockAt (Vector3 position, byte block)
		{
				//sets the specified block at these coordinates
		
				int x = Mathf.RoundToInt (position.x);
				int y = Mathf.RoundToInt (position.y);
				int z = Mathf.RoundToInt (position.z);
		
				SetBlockAt (x, y, z, block);
		}
	
		public void SetBlockAt (int x, int y, int z, byte block)
		{
				//adds the specified block at these coordinates
		
				print ("Adding: " + x + ", " + y + ", " + z);
		
		
				world.data [x, y, z] = block;
				UpdateChunkAt (x, y, z);
				//adds the specified block at these coordinates
		}
	
		//To do: add a way to just flag the chunk for update then it update it in lateupdate
		public void UpdateChunkAt (int x, int y, int z)
		{ 
				//Updates the chunk containing this block
		
				int updateX = Mathf.FloorToInt (x / world.chunkSize);
				int updateY = Mathf.FloorToInt (y / world.chunkSize);
				int updateZ = Mathf.FloorToInt (z / world.chunkSize);
		
				Debug.Log ("Updating: " + updateX + ", " + updateY + ", " + updateZ);
  
				world.chunks [updateX, updateY, updateZ].needsUpdate = true;
  
		}
	
	#endregion
}
