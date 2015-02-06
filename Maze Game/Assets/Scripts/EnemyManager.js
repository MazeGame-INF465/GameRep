//var playerHealth : PlayerHealth;       // Reference to the player's heatlh.
var enemy : GameObject;                // The enemy prefab to be spawned.
var spawnTime : float = 10f;            // How long between each spawn.
var spawnPoints : Transform[];         // An array of the spawn points this enemy can spawn from.


function Start (){
    
    // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
    InvokeRepeating ("Spawn", spawnTime, spawnTime);
}


function Spawn (){

    /*// If the player has no health left, exit
    if(playerHealth.currentHealth <= 0f){
        return;
    }*/

    // Find a random index between zero and one less than the number of spawn points.
    var spawnPointIndex : int = Random.Range (0, spawnPoints.Length);

    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
    Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
}