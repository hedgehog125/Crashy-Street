using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkRenderer : MonoBehaviour {
	[SerializeField] private GameObject groundPrefab;

	private WorldGenerator dataScript;

	private void Awake() {
		dataScript = GetComponent<WorldGenerator>();
		Assert.IsNotNull(dataScript);

		Render();
	}

	private void FixedUpdate() {
		Render();
	}

	private void Render() {
		foreach (KeyValuePair<Vector2Int, WorldGenerator.ChunkData> chunkPair in dataScript.chunks) {
			if (chunkPair.Value.gameObject != null) continue;

			Vector3 position = new Vector3(-chunkPair.Key.y, 0, chunkPair.Key.x) * dataScript.chunkSize;
			chunkPair.Value.gameObject = RenderChunk(position, chunkPair.Value);
		}
	}

	private GameObject RenderChunk(Vector3 position, WorldGenerator.ChunkData chunk) {
		return Instantiate(groundPrefab, position, Quaternion.identity);
	}
}
