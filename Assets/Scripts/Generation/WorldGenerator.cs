using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldGenerator : MonoBehaviour { // This generates a ChunkData which is then rendered by ChunkRenderer
	[Header("Objects and references")]
	[SerializeField] private GameObject player;

	[Header("Generation")]
	[SerializeField] private int m_chunkSize;
	[SerializeField] private int generationBoxWidth; // In chunks


	public class ChunkData {
		public class PlacedTile {

		}

		public GameObject gameObject;
		public bool isReload; // If the chunk is unloaded and reloaded, this will be true so that things like vehicles won't be spawned again
		public bool isFullyGenerated; // Some tiles might have bled over into this chunk without actually generating it

		public PlacedTile[] tiles;

		public ChunkData(int chunkSize) {
			tiles = new PlacedTile[chunkSize * chunkSize];
		}
	}
	[HideInInspector] public Dictionary<Vector2Int, ChunkData> chunks { get; private set; } = new Dictionary<Vector2Int, ChunkData>();


	[HideInInspector] public int chunkSize { get; private set; }
	private void Awake() {
		chunkSize = m_chunkSize;
		Assert.IsTrue(chunkSize % 2 == 0);
		Assert.IsTrue(chunkSize != 0);

		Generate();
	}

	private void FixedUpdate() {
		Generate();
	}

	private void Generate() {
		// These are 2D coordinates from a top down perspective
		Vector2Int playerPos = new Vector2Int(
			Mathf.RoundToInt(player.transform.position.z / chunkSize),
			-Mathf.RoundToInt(player.transform.position.x / chunkSize)
		);
		Vector2Int topLeft = new Vector2Int(
			playerPos.x - Mathf.RoundToInt(generationBoxWidth / 2),
			playerPos.y + Mathf.RoundToInt(generationBoxWidth / 2)
		);
		Vector2Int bottomRight = topLeft + new Vector2Int(generationBoxWidth, -generationBoxWidth);

		for (int y = topLeft.y; y >= bottomRight.y; y--) {
			for (int x = topLeft.x; x <= bottomRight.x; x++) {
				Vector2Int chunkPos = new Vector2Int(x, y);

				if (IsChunkIncomplete(chunkPos)) {
					GenerateChunk(chunkPos);
				}
			}
		}
	}

	private void GenerateChunk(Vector2Int chunkPos) {
		ChunkData chunk = PartlyGenerateChunk(chunkPos);

		PlaceTile(new ChunkData.PlacedTile(), chunkPos, Vector2Int.zero, false);
		PlaceTile(new ChunkData.PlacedTile(), chunkPos, new Vector2Int(1, 0), false);

		chunk.isFullyGenerated = true;
	}

	private bool PlaceTile(ChunkData.PlacedTile tileToPlace, Vector2Int chunkPos, Vector2Int tileCoord, bool replace) {
		chunkPos += new Vector2Int(
			Mathf.FloorToInt(tileCoord.x / chunkSize),
			Mathf.FloorToInt(tileCoord.y / chunkSize)
		);
		tileCoord.x %= chunkSize;
		tileCoord.y %= chunkSize;

		ChunkData chunk = PartlyGenerateChunk(chunkPos);
		int index = (tileCoord.y * chunkSize) + tileCoord.x;
		if (chunk.tiles[index] != null && (! replace)) return false;

		chunk.tiles[index] = tileToPlace;
		return true;
	}
	private ChunkData PartlyGenerateChunk(Vector2Int chunkPos) {
		if (chunks.ContainsKey(chunkPos)) return chunks[chunkPos];

		ChunkData chunk = new ChunkData(chunkSize);
		chunks[chunkPos] = chunk;
		return chunk;
	}

	private bool IsChunkIncomplete(Vector2Int chunkPos) {
		if (! chunks.ContainsKey(chunkPos)) return true;
		if (! chunks[chunkPos].isFullyGenerated) return true;
		return false;
	}
}
