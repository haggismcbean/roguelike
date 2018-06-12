using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	// This allows us to modify how variables with appear in the editor
	[Serializable]
	public class Count {
		public int minimum;
		public int maxiumum;

		// Assignment constructor. This pattern lets us have an initialisation funtion for this class
		public Count (int min, int max) {
			minimum = min;
			maxiumum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);

	//this will be the exit sign
	public GameObject exit;

	//we want to pass in arrays of these tiles and then at a later date choose one (or more) of them
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	//this is used to keep heirarchy clean, we'll child them all to boardHolder so we can collapse them
	private Transform boardHolder;

	//this is used to track all of the places in the game board to check if anything's been spawned there already or not
	private List <Vector3> gridPositions = new List<Vector3>();

	//this 'void' word is just another way of declaring a private function, but it's one that always returns void (ie. undefined)
	void InitializeList() {
		gridPositions.Clear();

		//filling the gridPositions list with each of the grid positions on our game board.
		//NOTE the reason we're going from 1 to columns - 1 instead of 0 to columns is so there is empty space around the border!!
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; x < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup() {
		boardHolder = new GameObject ("Board").transform;

		//laying out the floor and the outer wall tiles
		//we go from -1 to +1 here because we're building an edge on the outer portions (the boundries of the game)
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; x < rows + 1; y++) {
				//Randomized floor tile
				GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];

				//If we're in outer wall, choose outer wall tile
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles[Random.Range (0, outerWallTiles.Length)];
				}

				//we have to 'instantiate' the tiles. We pass: 
				// 1. The tile to instantiate
				// 2. The vector position of the tile (with 0f as we're 2d)
				// 3. The 'rotation' of the tile (with Quaternion.identity because we're in 2d)
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];

		//ensures we don't use the same position twice
		gridPositions.RemoveAt(randomIndex);

		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
		//how many of an object to spawn
		int objectCount = Random.Range(minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	//public function that returns void
	public void SetupScene(int level) {
		BoardSetup();
		InitializeList();
		LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maxiumum);
		LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maxiumum);

		int enemyCount = (int)Math.Log(level, 2f);
		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

		// exit object will always be in upper left corner so we don't use the layout at random thing.
		Instantiate(exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
//
//
//using UnityEngine;
//using System;
//using System.Collections.Generic;       //Allows us to use Lists.
//using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
//
//namespace Completed
//
//{
//
//	public class BoardManager : MonoBehaviour
//	{
//		// Using Serializable allows us to embed a class with sub properties in the inspector.
//		[Serializable]
//		public class Count
//		{
//			public int minimum;             //Minimum value for our Count class.
//			public int maximum;             //Maximum value for our Count class.
//
//
//			//Assignment constructor.
//			public Count (int min, int max)
//			{
//				minimum = min;
//				maximum = max;
//			}
//		}
//
//
//		public int columns = 8;                                         //Number of columns in our game board.
//		public int rows = 8;                                            //Number of rows in our game board.
//		public Count wallCount = new Count (5, 9);                      //Lower and upper limit for our random number of walls per level.
//		public Count foodCount = new Count (1, 5);                      //Lower and upper limit for our random number of food items per level.
//		public GameObject exit;                                         //Prefab to spawn for exit.
//		public GameObject[] floorTiles;                                 //Array of floor prefabs.
//		public GameObject[] wallTiles;                                  //Array of wall prefabs.
//		public GameObject[] foodTiles;                                  //Array of food prefabs.
//		public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
//		public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.
//
//		private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
//		private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.
//
//
//		//Clears our list gridPositions and prepares it to generate a new board.
//		void InitialiseList ()
//		{
//			//Clear our list gridPositions.
//			gridPositions.Clear ();
//
//			//Loop through x axis (columns).
//			for(int x = 1; x < columns-1; x++)
//			{
//				//Within each column, loop through y axis (rows).
//				for(int y = 1; y < rows-1; y++)
//				{
//					//At each index add a new Vector3 to our list with the x and y coordinates of that position.
//					gridPositions.Add (new Vector3(x, y, 0f));
//				}
//			}
//		}
//
//
//		//Sets up the outer walls and floor (background) of the game board.
//		void BoardSetup ()
//		{
//			//Instantiate Board and set boardHolder to its transform.
//			boardHolder = new GameObject ("Board").transform;
//
//			//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
//			for(int x = -1; x < columns + 1; x++)
//			{
//				//Loop along y axis, starting from -1 to place floor or outerwall tiles.
//				for(int y = -1; y < rows + 1; y++)
//				{
//					//Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
//					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
//
//					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
//					if(x == -1 || x == columns || y == -1 || y == rows)
//						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
//
//					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
//					GameObject instance =
//						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
//
//					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
//					instance.transform.SetParent (boardHolder);
//				}
//			}
//		}
//
//
//		//RandomPosition returns a random position from our list gridPositions.
//		Vector3 RandomPosition ()
//		{
//			//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
//			int randomIndex = Random.Range (0, gridPositions.Count);
//
//			//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
//			Vector3 randomPosition = gridPositions[randomIndex];
//
//			//Remove the entry at randomIndex from the list so that it can't be re-used.
//			gridPositions.RemoveAt (randomIndex);
//
//			//Return the randomly selected Vector3 position.
//			return randomPosition;
//		}
//
//
//		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
//		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
//		{
//			//Choose a random number of objects to instantiate within the minimum and maximum limits
//			int objectCount = Random.Range (minimum, maximum+1);
//
//			//Instantiate objects until the randomly chosen limit objectCount is reached
//			for(int i = 0; i < objectCount; i++)
//			{
//				//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
//				Vector3 randomPosition = RandomPosition();
//
//				//Choose a random tile from tileArray and assign it to tileChoice
//				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
//
//				//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
//				Instantiate(tileChoice, randomPosition, Quaternion.identity);
//			}
//		}
//
//
//		//SetupScene initializes our level and calls the previous functions to lay out the game board
//		public void SetupScene (int level)
//		{
//			//Creates the outer walls and floor.
//			BoardSetup ();
//
//			//Reset our list of gridpositions.
//			InitialiseList ();
//
//			//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
//			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
//
//			//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
//			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
//
//			//Determine number of enemies based on current level number, based on a logarithmic progression
//			int enemyCount = (int)Mathf.Log(level, 2f);
//
//			//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
//			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
//
//			//Instantiate the exit tile in the upper right hand corner of our game board
//			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
//		}
//	}
//}
//
