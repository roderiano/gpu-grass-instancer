using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GrassInstancer : MonoBehaviour
{
    public int numberOfPoints = 1; // Variável para controlar o número de pontos mapeados entre cada par de vértices

    void OnDrawGizmos()
    {
        // Obtém o componente MeshFilter do GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("O GameObject não possui um MeshFilter ou Mesh não atribuída.");
            return;
        }

        // Obtém os vértices da malha
        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        int[] triangles = meshFilter.sharedMesh.triangles;

        // Desenha gizmos em cada ponto da face
        Gizmos.color = Color.green;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int j = 0; j <= numberOfPoints; j++)
            {
                float t = j / (float)(numberOfPoints + 1);

                // Interpolação entre os vértices horizontais
                Vector3 horizontalPoint1 = Vector3.Lerp(vertices[triangles[i]], vertices[triangles[i + 1]], t);
                Vector3 horizontalPoint2 = Vector3.Lerp(vertices[triangles[i + 1]], vertices[triangles[i + 2]], t);
                Vector3 horizontalPointBetween = Vector3.Lerp(horizontalPoint1, horizontalPoint2, t);
                Gizmos.DrawSphere(transform.TransformPoint(horizontalPointBetween), 0.1f);

                // Interpolação entre os vértices verticais
                Vector3 verticalPoint1 = Vector3.Lerp(vertices[triangles[i]], vertices[triangles[i + 2]], t);
                Vector3 verticalPointBetween = Vector3.Lerp(horizontalPointBetween, verticalPoint1, t);
                Gizmos.DrawSphere(transform.TransformPoint(verticalPointBetween), 0.1f);
            }
        }
    }
}
