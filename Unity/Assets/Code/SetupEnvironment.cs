using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/*
 * Initializes the pathfinding grid. This should only be run once!
 * 
 * This behaviour should be added to the piece of terrain that we'll
 * be pathfinding through.
 */
namespace Simulation
{
	public class SetupEnvironment : MonoBehaviour
	{
		
		// Use this for initialization
		void Start()
		{
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				Debug.LogError("There's no Mesh Filter attached to this component! " +
				               "This script should be added to the portion of the GameObject that controls the mesh you want agents to move on!");
			}
			var mesh = meshFilter.mesh;

			Vector3 center = gameObject.renderer.bounds.center;
			Vector3 size = gameObject.renderer.bounds.size;

			float height = FindGroundLevel(mesh.normals);

			Vector3 corner = new Vector3(center.x - size.x / 2, height, center.z - size.z / 2);

			var offset = gameObject.transform.position;
			var scale = gameObject.transform.lossyScale;
			Quaternion rotation = gameObject.transform.rotation;

			Vector3[] scaledVertices = mesh.vertices.Select(v => offset + rotation * Vector3.Scale(v, scale)).ToArray();
			Grid g = new Grid(corner, (int)(size.x / Tile.TileSize), (int)(size.z / Tile.TileSize), scaledVertices, mesh.triangles);

			Simulation.Instance.Start();
			Simulation.Instance.SetGrid(g);
		}
		
		// Update is called once per frame
		void Update()
		{
			Simulation.Instance.Update();
		}

		void OnGUI()
		{
			Simulation.Instance.OnGUI();
		}

		private float FindGroundLevel(Vector3[] normals)
		{
			// Dark secret - we just assume that the ground is at height 0. 
			// If this becomes a problem then we may be able to come up with a solution, but it's problematic.
			return 0.0f;
		}
	}
}