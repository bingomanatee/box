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

				void InitCollMesh ()
				{
						Mesh newMesh = new Mesh ();
			
						newMesh.vertices = colVertices.ToArray ();
						newMesh.triangles = colTriangles.ToArray ();
						col.sharedMesh = newMesh;
			
						colVertices.Clear ();
						colTriangles.Clear ();
						colCount = 0;
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

				// Use this for initialization
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

				void BuildMesh ()
				{
						InitCollMesh ();
						for (int px=0; px<blocks.GetLength(0); px++) {
								for (int py=0; py<blocks.GetLength(1); py++) {
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
				// Update is called once per frame
				void UpdateMesh ()
				{
						mesh.Clear ();
						mesh.vertices = newVertices.ToArray ();
						mesh.triangles = newTriangles.ToArray ();
						mesh.uv = newUV.ToArray (); // add this line to the code here
						mesh.Optimize ();
						mesh.RecalculateNormals ();

						squareCount = 0;
						newVertices.Clear ();
						newTriangles.Clear ();
						newUV.Clear ();

			InitCollMesh();
				}

				void GenTerrain ()
				{
						blocks = new byte[10, 10];
			
						for (int px=0; px<blocks.GetLength(0); px++) {
								for (int py=0; py<blocks.GetLength(1); py++) {
										if (py == groundLevel) {
												blocks [px, py] = 2;
										} else if (py < groundLevel) {
												blocks [px, py] = 1;
										}
								}
						}
				}
		
		#endregion

		#region loop

		
				int groundLevel = 5;

				void Start ()
				{ 
						col = GetComponent<MeshCollider> ();
				}

				void Update ()
				{
						GenTerrain ();
						BuildMesh ();
						UpdateMesh ();
						if (Random.value > 0.5)
								++ groundLevel;
						else
								-- groundLevel;

						if (groundLevel != Mathf.Clamp (groundLevel, 0, 10))
								groundLevel = 0;
				}


		#endregion
		}
}