using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour { // This generates a ChunkData which is then rendered by ChunkRenderer
	[Header("Objects and references")]
	[SerializeField] private GameObject player;

	[Header("Generation")]
	[SerializeField] private float m_chunkSize;
	[SerializeField] private int generationBoxWidth; // In chunks


	public class ChunkData {
		public GameObject gameObject;
		public bool isReload; // If the chunk is unloaded and reloaded, this will be true so that things like vehicles won't be spawned again
	}
	[HideInInspector] public Dictionary<Vector2Int, ChunkData> chunks { get; private set; } = new Dictionary<Vector2Int, ChunkData>();


	[HideInInspector] public float chunkSize { get; private set; }
	private void Awake() {
		chunkSize = m_chunkSize;

		Generate();
	}

	private void FixedUpdate() {
		Generate();
	}

	private void Generate() {
		// These are 2D coordinates from a top down perspective
		Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(player.transform.position.z / chunkSize), -Mathf.RoundToInt(player.transform.position.x / chunkSize));
		Vector2Int topLeft = new Vector2Int(playerPos.x - Mathf.RoundToInt(generationBoxWidth / 2), playerPos.y + Mathf.RoundToInt(generationBoxWidth / 2));
		Vector2Int bottomRight = topLeft + new Vector2Int(generationBoxWidth, -generationBoxWidth);

		for (int y = topLeft.y; y >= bottomRight.y; y--) {
			for (int x = topLeft.x; x <= bottomRight.x; x++) {
				Vector2Int chunkPos = new Vector2Int(x, y);
				if (!chunks.ContainsKey(chunkPos)) {
					GenerateChunk(chunkPos);
				}
			}
		}
	}

	private void GenerateChunk(Vector2Int chunkPos) {
		chunks[chunkPos] = new ChunkData();
	}
}
