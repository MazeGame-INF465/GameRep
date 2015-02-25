using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Purpose: demonstrate script interface to interact with terrain trees

// Steps: Attach this to main(parent/top) Player gameObject, or adjust myTransform to your hierarchy,
// define Inspector values for harvestTreeDistance, respawnTimer
// Setup a prefab tree with CAPSULE collider, how to at bottom of
// http://docs.unity3d.com/Documentation/Components/terrain-Trees.html
// paint terrain tree 

// Assign the non-collider prefab version of the tree to felledTree
// press Play, left click on terrain tree

// Harvested terrain tree info is passed to a manager object QM_ResourceManager for respawn management,
// you'll need that too or you could comment out any functionality related to manager 

// Note: this is not a demo of modifying terrainData permanently - there's enough risk involved with that
// such that you shouldn't try it unless you know what you are doing.


public class QM_HarvestTerrainTree : MonoBehaviour {
	
	// Player, Range
	public int harvestTreeDistance;        // Set [Inspector] min. distance from Player to Tree for your scale?
	public bool rotatePlayer = true;    // Should we rotate the player to face the Tree? 
	private Transform myTransform;        // Player transform for cache
	
	// Terrains, Hit
	private Terrain terrain;            // Derived from hit...GetComponent<Terrain>
	private RaycastHit hit;                // For hit. methods
	private string lastTerrain;            // To avoid reassignment/GetComponent on every Terrain click
	
	// Tree, GameManager
	public GameObject felledTree;        // Prefab to spawn at terrain tree loc for TIIIIIIMBER!
	private QM_ResourceManager rMgr;    // Resource manager script
	public float respawnTimer;            // Duration of terrain tree respawn timer
	
	void Start () {
		
		if (harvestTreeDistance <= 0) {
			Debug.Log ("harvestTreeDistance unset in Inspector, using value: 6");
			harvestTreeDistance = 6;
		}
		
		if (respawnTimer <= 0) {
			Debug.Log ("respawnTimer unset in Inspector, using quick test value: 15");
			respawnTimer = 15;
		}
		
		myTransform = transform;
		lastTerrain = null;
		rMgr = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<QM_ResourceManager> ();
		
	}
	
	
	void Update () {
		
		if (Input.GetMouseButtonUp (0)) {
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 30f)) {
				
				// Did we click a Terrain?
				if(hit.collider.gameObject.GetComponent<Terrain>() == null)
					return;
				
				// Did we click on the same Terrain as last time (or very first time?)
				if(lastTerrain == null || lastTerrain != hit.collider.name) {
					terrain = hit.collider.gameObject.GetComponent<Terrain>();
					lastTerrain = terrain.name;
				}
				
				// Was it the terrain or a terrain tree, based on SampleHeight()
				float groundHeight = terrain.SampleHeight(hit.point);
				
				if(hit.point.y - .2f > groundHeight) {
					
					// It's a terrain tree, check Proximity and Harvest
					if(CheckProximity()) 
						HarvestWood();
					
				}
			}
		}
	}
	
	
	private bool CheckProximity() {
		bool inRange = true;
		float clickDist = Vector3.Distance (myTransform.position, hit.point);
		
		if (clickDist > harvestTreeDistance) {
			Debug.Log ("Out of Range");
			inRange = false;
		}
		
		return inRange;
		
	}
	
	private bool CheckRecentUsage(string _terrainName, int _treeINDEX) {
		bool beenUsed = false;
		
		for (int cnt=0; cnt < rMgr.managedTrees.Count; cnt++) {
			if (rMgr.managedTrees[cnt].terrainName == _terrainName && rMgr.managedTrees [cnt].treeINDEX == _treeINDEX) {
				Debug.Log ("Tree has been used recently");
				beenUsed = true;
			}
		}
		
		return beenUsed;
	}
	
	
	private void HarvestWood() {
		int treeIDX = -1;
		int treeCount = terrain.terrainData.treeInstances.Length;
		float treeDist = harvestTreeDistance;
		Vector3 treePos = new Vector3 (0, 0, 0);
		
		// Notice we are looping through every terrain tree, 
		// which is a caution against a 15,000 tree terrain
		
		for (int cnt=0; cnt < treeCount; cnt++) {
			Vector3 thisTreePos = Vector3.Scale (terrain.terrainData.treeInstances [cnt].position, terrain.terrainData.size) + terrain.transform.position;
			float thisTreeDist = Vector3.Distance (thisTreePos, hit.point);
			
			if (thisTreeDist < treeDist) {
				treeIDX = cnt;
				treeDist = thisTreeDist;
				treePos = thisTreePos;
			}
		}
		
		
		if (treeIDX == -1) {
			Debug.Log ("Out of Range");
			return;
		}
		
		if(!CheckRecentUsage(terrain.name, treeIDX)) {
			
			// Success - all tests passed
			// Place a cube to show the tree, the ResourceManager will remove it after a time
			// Obviously tweak to your liking, just a visual aid to show it worked
			
			GameObject marker = GameObject.CreatePrimitive (PrimitiveType.Cube);
			marker.transform.position = treePos;
			
			// Example of spawning a placed tree at this location, just for demo purposes
			// it will slide through terrain and disappear in 4 seconds
			GameObject fellTree = Instantiate(felledTree,treePos,Quaternion.identity) as GameObject;
			fellTree.gameObject.AddComponent<Rigidbody>();
			
			Destroy(fellTree,4);
			
			// Add this terrain tree and cube to our Resource Manager for demo purposes
			rMgr.AddTerrainTree(terrain.name, treeIDX, Time.time+respawnTimer, marker.transform);
			
			if (rotatePlayer) {
				Vector3 lookRot = new Vector3 (hit.point.x, myTransform.position.y, hit.point.z);
				myTransform.LookAt (lookRot);
			}
			
			// There are too many guesses to be made here about your game mechanics to code the rest
			// For example, you might..
			
			// Start Animation
			// Play Sound Clip
			// Give player an Inventory item
			// Bump lumberjacking skill
			// Random.Range[] spawn a Forest Protector Spirit of Woe
			// etc.
			
		}
	}
}