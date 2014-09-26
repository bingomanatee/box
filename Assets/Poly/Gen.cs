using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poly
{
		public class Gen : MonoBehaviour
		{
				public List<Vector3> newVertices = new List<Vector3> ();
				public List<int>newTriangles = new List<int> ();
				public List<Vector2> newUV = new List<Vector2> ();
				Mesh mesh;
				private float tUnit = 0.25f;
				private Vector2 tStone = new Vector2 (0, 0);
				private Vector2 tGrass = new Vector2 (0, 1
				);
				public int squareCount;
				public byte[,] blocks;
				int[] vertOrder = new int[]{0, 1, 3, 1, 2, 3};
				const int Y_DIM = 50;
				int X_DIM = 50;

				void ClearMeshData ()
				{
		
						newVertices = new List<Vector3> ();
						newTriangles = new List<int> ();
						newUV = new List<Vector2> ();
				}

#region colliders
				public List<Vector3> colVertices = new List<Vector3> ();
				public List<int> colTriangles = new List<int> ();
				private int colCount;
				private MeshCollider col;

				void MakeCollTriangles ()
				{
						int offset = colCount * 4;
						foreach (int index in vertOrder)
								colTriangles.Add (offset + index);
						colCount++;
				}

				void ClearColliderData ()
				{
						Debug.Log ("ClearColliderData");
						colVertices.Clear ();
						colTriangles.Clear ();
						colCount = 0;
						squareCount = 0;
				}

				void InitCollMesh ()
				{
			
						Debug.Log ("InitCollMesh");
						Mesh newMesh = new Mesh ();
			
						newMesh.vertices = colVertices.ToArray ();
						newMesh.triangles = colTriangles.ToArray ();
						col.sharedMesh = newMesh;
				}

				void GenCollider (int x, int y)
				{
						//Front
						colVertices.Add (new Vector3 (x, y, 1));
						colVertices.Add (new Vector3 (x + 1, y, 1));
						colVertices.Add (new Vector3 (x + 1, y, 0));
						colVertices.Add (new Vector3 (x, y, 0));
			
						MakeCollTriangles ();
						//Top
						colVertices.Add (new Vector3 (x, y, 1));
						colVertices.Add (new Vector3 (x + 1, y, 1));
						colVertices.Add (new Vector3 (x + 1, y, 0));
						colVertices.Add (new Vector3 (x, y, 0));
			
						MakeCollTriangles ();
			
						//bot
						colVertices.Add (new Vector3 (x, y - 1, 0));
						colVertices.Add (new Vector3 (x + 1, y - 1, 0));
						colVertices.Add (new Vector3 (x + 1, y - 1, 1));
						colVertices.Add (new Vector3 (x, y - 1, 1));
			
						MakeCollTriangles ();
			
						//left
						colVertices.Add (new Vector3 (x, y - 1, 1));
						colVertices.Add (new Vector3 (x, y, 1));
						colVertices.Add (new Vector3 (x, y, 0));
						colVertices.Add (new Vector3 (x, y - 1, 0));
			
						MakeCollTriangles ();
			
						//right
						colVertices.Add (new Vector3 (x + 1, y, 1));
						colVertices.Add (new Vector3 (x + 1, y - 1, 1));
						colVertices.Add (new Vector3 (x + 1, y - 1, 0));
						colVertices.Add (new Vector3 (x + 1, y, 0));
			
						MakeCollTriangles ();
				}


#endregion

		#region generators

				void UpdateMesh ()
				{
						Debug.Log ("UpdateMesh: with " + newVertices.Count + " verts");
						mesh.Clear ();
						mesh.vertices = newVertices.ToArray ();
						mesh.triangles = newTriangles.ToArray ();
						mesh.uv = newUV.ToArray (); // add this line to the code here
						mesh.Optimize ();
						mesh.RecalculateNormals ();
			
				}
		
/**
builds up the vertices and UV definition for the mesh
*/
				void GenSquare (float x, float y, Vector2 textureSpace)
				{
						mesh = GetComponent<MeshFilter> ().mesh;
						float z = 0;

						newVertices.Add (new Vector3 (x, y, z));
						newVertices.Add (new Vector3 (x + 1, y, z));
						newVertices.Add (new Vector3 (x + 1, y - 1, z));
						newVertices.Add (new Vector3 (x, y - 1, z));

						int offset = 4 * squareCount;
						++squareCount;
						foreach (int index in vertOrder)
								newTriangles.Add (offset + index);
			
						newUV.Add (new Vector2 (tUnit * textureSpace.x, tUnit * textureSpace.y + tUnit));
						newUV.Add (new Vector2 (tUnit * textureSpace.x + tUnit, tUnit * textureSpace.y + tUnit));
						newUV.Add (new Vector2 (tUnit * textureSpace.x + tUnit, tUnit * textureSpace.y));
						newUV.Add (new Vector2 (tUnit * textureSpace.x, tUnit * textureSpace.y));
			
				}

/**
 * convert bit data into mesh 
*/
				void BuildMesh ()
				{
						Debug.Log ("Build Mesh");
						for (int px=0; px<X_DIM; px++) {
								for (int py=0; py< Y_DIM; py++) {
										if (blocks [px, py] != 0) { // if !air
						
												// GenCollider here, this will apply it
												// to every block other than air
												GenCollider (px, py);
												if (blocks [px, py] == 1) {
														GenSquare (px, py, tStone);
												} else if (blocks [px, py] == 2) {
														GenSquare (px, py, tGrass);
												}
						
										} // if !air
								}
						}
				}

				/**
* sets the byte definition of the ground
*/
				void GenTerrain ()
				{
						blocks = new byte[X_DIM, Y_DIM];
			
						for (int px=0; px<blocks.GetLength(0); px++) {
								int stone = Noise (px, 0, 80, 15, 1);
								stone += Noise (px, 0, 50, 30, 1);
								stone += Noise (px, 0, 10, 10, 1);
								stone += 75;
				
								int dirt = Noise (px, 0, 100f, 35, 1);
								dirt += Noise (px, 100, 50, 30, 1);
								dirt += 75;
				
				
								for (int py=0; py<blocks.GetLength(1); py++) {
										if (py < stone) {
												blocks [px, py] = 1;
						
												//The next three lines make dirt spots in random places
												if (Noise (px, py, 12, 16, 1) > 10) {
														blocks [px, py] = 2;
							
												}
						
												//The next three lines remove dirt and rock to make caves in certain places
												if (Noise (px, py * 2, 16, 14, 1) > 10) { //Caves
														blocks [px, py] = 0;
							
												}
						
										} else if (py < dirt) {
												blocks [px, py] = 2;
										}
					
					
								}
						}
			
				}
		
		#endregion

		#region loop

				int decTime = 0;
				int groundLevel = 5;

				void Start ()
				{ 
						col = GetComponent<MeshCollider> ();
				}

				void Update ()
				{
						GenTerrain (); // byte definition
						InitCollMesh (); // collision mesh def
						BuildMesh (); // byte data to face data
						UpdateMesh ();
						MoveGround ();
						ClearMeshData ();
						ClearColliderData ();
				}

				void MoveGround ()
				{
						int d = (int)(Time.time * 10);
						if (decTime != d) {
								if (Random.value > 0.5)
										++ groundLevel;
								else
										-- groundLevel;
								decTime = d;
								if (groundLevel != Mathf.Clamp (groundLevel, 0, 10))
										groundLevel = 0;
						}
				}

		#endregion

#region noise

				int Noise (int x, int y, float scale, float mag, float exp)
				{
			
						return (int)(Mathf.Pow ((Mathf.PerlinNoise (x / scale, y / scale) * mag), exp)); 
			
				}

#endregion
		}
}