using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGen : MonoBehaviour
{
    //image used for terrain texture
    //https://pxhere.com/en/photo/1095550

    int heightScale = 5;
    float detailScale = 5f;
    public int seed;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        //loop over mesh of plane
        for (int i = 0; i < vertices.Length; i++)
        {
            float noiseBase = Mathf.PerlinNoise((vertices[i].x + this.transform.position.x + seed) / detailScale,
                                              (vertices[i].z + this.transform.position.z + seed) / detailScale);

            //lift vertices
            vertices[i].y = noiseBase * heightScale;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        this.gameObject.AddComponent<MeshCollider>();
    }
}
