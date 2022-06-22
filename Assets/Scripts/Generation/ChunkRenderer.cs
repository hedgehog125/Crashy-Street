using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkRenderer : MonoBehaviour {
	[SerializeField] private GameObject groundPrefab;
	[SerializeField] private GameObject grassTilePrefab;

	private WorldGenerator dataScript;

	private int halfChunkSize;

	private void Awake() {
		dataScript = GetComponent<WorldGenerator>();
		halfChunkSize = dataScript.chunkSize / 2;

		Assert.IsNotNull(dataScript);

		Render();
	}

	private void FixedUpdate() {
		Render();
	}

	private void Render() {
		foreach (KeyValuePair<Vector2Int, WorldGenerator.ChunkData> chunkPair in dataScript.chunks) {
			if (chunkPair.Value.gameObject != null) continue;

			Vector3 position = TranslateCoord(chunkPair.Key);
			chunkPair.Value.gameObject = RenderChunk(position, chunkPair.Key, chunkPair.Value);
		}
	}
	private GameObject RenderChunk(Vector3 worldPos, Vector2Int chunkPos, WorldGenerator.ChunkData chunk) {
		GameObject chunkOb = Instantiate(groundPrefab, worldPos, Quaternion.identity);

		for (int i = 0; i < chunk.tiles.Length; i++) {
			WorldGenerator.ChunkData.PlacedTile tile = chunk.tiles[i];
			if (tile == null) continue;

			int x = i % dataScript.chunkSize;
			int y = Mathf.RoundToInt(i / dataScript.chunkSize);

			GameObject tileOb = Instantiate(grassTilePrefab, TranslateCoord(chunkPos, new Vector2Int(x, y)) + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
			tileOb.transform.parent = chunkOb.transform;
		}

		return chunkOb;
	}

	private Vector3 TranslateCoord(Vector2Int xy) {
		return TranslateCoord(xy, new Vector2Int(halfChunkSize, -halfChunkSize));
	}
	private Vector3 TranslateCoord(Vector2Int xy, Vector2Int tileCoord) {
		Vector2Int topLeft = new Vector2Int(
			(xy.x * dataScript.chunkSize) - halfChunkSize,
			(xy.y * dataScript.chunkSize) + halfChunkSize
		); // Still from top down but now scaled to increase the resolution to the level of tiles

		return new Vector3(-(topLeft.y + tileCoord.y), 0, topLeft.x + tileCoord.x);
	}
}
