using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{
		private List<Vector3> newVertices = new List<Vector3> ();
		private List<int> newTriangles = new List<int> ();
		private List<Vector2> newUV = new List<Vector2> ();
		private float tUnit = 0.25f;
		private Vector2 tStone = new Vector2 (1, 0);
		private Vector2 tGrass = new Vector2 (0, 1);
		private Vector2 tGrassTop = new Vector2 (1, 1);
		private Vector2 tWhite = new Vector2 (3, 0);
		private Mesh mesh;
		private MeshCollider col;
		private int faceCount = 0;
		public GameObject worldGO;
		public int chunkSize = 16;
		private World world;
		public int chunkX;
		public int chunkY;
		public int chunkZ;

		int faceOffset { get { return faceCount * 4; } }

		byte Block (int x, int y, int z)
		{
				return world.Block (x + chunkX, y + chunkY, z + chunkZ);
		}
	
		Vector2 TextureAt (int x, int y, int z)
		{
				return TextureAt (x, y, z, false);
		}
	
		Vector2 TextureAt (int x, int y, int z, bool isTop)
		{
				if (Block (x, y, z) == 1) {
						return tStone;
				} else if (Block (x, y, z) == 2) {
						return isTop ? tGrassTop : tGrass;
				} else {
						return tWhite;
				}
		}

	#region cube drawing
		void CubeTop (int x, int y, int z, byte block)
		{
		
				newVertices.Add (new Vector3 (x, y, z + 1));
				newVertices.Add (new Vector3 (x + 1, y, z + 1));
				newVertices.Add (new Vector3 (x + 1, y, z));
				newVertices.Add (new Vector3 (x, y, z));
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4¬
		
				Vector2 texturePos = TextureAt (x, y, z, true);
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}

		void CubeNorth (int x, int y, int z, byte block)
		{
		
				//CubeNorth
				newVertices.Add (new Vector3 (x + 1, y - 1, z + 1));
				newVertices.Add (new Vector3 (x + 1, y, z + 1));
				newVertices.Add (new Vector3 (x, y, z + 1));
				newVertices.Add (new Vector3 (x, y - 1, z + 1));
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4
		
		
				Vector2 texturePos = TextureAt (x, y, z);
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}

		void CubeSouth (int x, int y, int z, byte block)
		{
				//CubeSouth
				newVertices.Add (new Vector3 (x, y - 1, z));
				newVertices.Add (new Vector3 (x, y, z));
				newVertices.Add (new Vector3 (x + 1, y, z));
				newVertices.Add (new Vector3 (x + 1, y - 1, z));
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4
		
		
				Vector2 texturePos = TextureAt (x, y, z);
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}

		void CubeEast (int x, int y, int z, byte block)
		{
		
				//CubeEast
				newVertices.Add (new Vector3 (x + 1, y - 1, z));
				newVertices.Add (new Vector3 (x + 1, y, z));
				newVertices.Add (new Vector3 (x + 1, y, z + 1));
				newVertices.Add (new Vector3 (x + 1, y - 1, z + 1));
		
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4
		
				Vector2 texturePos;
		
				texturePos = tStone;
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}

		void CubeWest (int x, int y, int z, byte block)
		{
				//CubeWest
				newVertices.Add (new Vector3 (x, y - 1, z + 1));
				newVertices.Add (new Vector3 (x, y, z + 1));
				newVertices.Add (new Vector3 (x, y, z));
				newVertices.Add (new Vector3 (x, y - 1, z));
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4
		
		
				Vector2 texturePos = TextureAt (x, y, z);
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}

		void CubeBot (int x, int y, int z, byte block)
		{
				//CubeBot
				newVertices.Add (new Vector3 (x, y - 1, z));
				newVertices.Add (new Vector3 (x + 1, y - 1, z));
				newVertices.Add (new Vector3 (x + 1, y - 1, z + 1));
				newVertices.Add (new Vector3 (x, y - 1, z + 1));
		
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 1); //2
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset); //1
				newTriangles.Add (faceOffset + 2); //3
				newTriangles.Add (faceOffset + 3); //4
		
		
				Vector2 texturePos = TextureAt (x, y, z);
		
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
				newUV.Add (new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
				newUV.Add (new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
				faceCount++; // Add this line
		}
	
	
#endregion

#region generation

		void GenerateMesh ()
		{
		
				for (int x=0; x<chunkSize; x++) {
						for (int y=0; y<chunkSize; y++) {
								for (int z=0; z<chunkSize; z++) {
										//This code will run for every block in the chunk
					
										if (Block (x, y, z) != 0) {
												//If the block is solid
						
												if (Block (x, y + 1, z) == 0) {
														//Block above is air
														CubeTop (x, y, z, Block (x, y, z));
														Debug.Log ("drawing CubeTop " + x + "," + y + "," + z);
												}
						
												if (Block (x, y - 1, z) == 0) {
														//Block below is air
														CubeBot (x, y, z, Block (x, y, z));							
														Debug.Log ("drawing CubeBot " + x + "," + y + "," + z);
												}

						
												if (Block (x + 1, y, z) == 0) {
														//Block east is air
														CubeEast (x, y, z, Block (x, y, z));							
														Debug.Log ("drawing CubeEast " + x + "," + y + "," + z);
												}
						
												if (Block (x - 1, y, z) == 0) {
														//Block west is air
														CubeWest (x, y, z, Block (x, y, z));							
														Debug.Log ("drawing CubeWest " + x + "," + y + "," + z);
												}
						
												if (Block (x, y, z + 1) == 0) {
														//Block north is air
														CubeNorth (x, y, z, Block (x, y, z));							
														Debug.Log ("drawing CubeNorth " + x + "," + y + "," + z);
												}
						
												if (Block (x, y, z - 1) == 0) {
														//Block south is air
														CubeSouth (x, y, z, Block (x, y, z));							
														Debug.Log ("drawing CubeSouth " + x + "," + y + "," + z);
												}
						
										} else {
												Debug.Log ("Block is air!");

					
										}
								}
						}
		
				}
		}
	
	#endregion


#region loop

		void Start ()
		{
				world = worldGO.GetComponent ("World") as World;
				mesh = GetComponent<MeshFilter> ().mesh;
				col = GetComponent<MeshCollider> ();
		
		}

		void Update ()
		{
				GenerateMesh ();
				UpdateMesh ();
		}
	
		void UpdateMesh ()
		{
				mesh.Clear ();
				mesh.vertices = newVertices.ToArray ();
				mesh.uv = newUV.ToArray ();
				mesh.triangles = newTriangles.ToArray ();
				mesh.Optimize ();
				mesh.RecalculateNormals ();
		
				col.sharedMesh = null;
				col.sharedMesh = mesh;
		
				newVertices.Clear ();
				newUV.Clear ();
				newTriangles.Clear ();
		
				faceCount = 0; //Fixed: Added this thanks to a bug pointed out by ratnushock!
		
		}
#endregion
}
