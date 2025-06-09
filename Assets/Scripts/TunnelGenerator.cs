using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    public float radius = 5f;
    public float length = 20f;
    public int segments = 32;
    public int rings = 8;

    void Start()
    {
        GenerateTunnel();
    }

    void GenerateTunnel()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = new Vector3[(segments + 1) * (rings + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[segments * rings * 6];
        
        // Crear vértices
        for (int r = 0; r <= rings; r++)
        {
            float v = (float)r / rings;
            float z = Mathf.Lerp(0, length, v);
            
            for (int s = 0; s <= segments; s++)
            {
                float u = (float)s / segments;
                float angle = u * Mathf.PI * 2;
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;
                
                vertices[r * (segments + 1) + s] = new Vector3(x, y, z);
                uv[r * (segments + 1) + s] = new Vector2(u, v);
            }
        }
        
        // Crear triángulos
        int index = 0;
        for (int r = 0; r < rings; r++)
        {
            for (int s = 0; s < segments; s++)
            {
                int current = r * (segments + 1) + s;
                int next = current + segments + 1;
                
                triangles[index++] = current;
                triangles[index++] = next;
                triangles[index++] = current + 1;
                
                triangles[index++] = current + 1;
                triangles[index++] = next;
                triangles[index++] = next + 1;
            }
        }
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        
        // Añadir un material básico
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}