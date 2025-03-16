using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{
    //The Mesh creator create some Mesh directed on the Z axys (blue one in unity

    public string meshName;
    public string ObjectName = "MeshCreator";

    [Tooltip("The value is for each angle from the forward axe, if you enter 25 the angle displayed will be 50°")]
    public float angleHorizon;
    public float angleVerticalBottom;
    public float angleVerticalTop;
    public float distance;
    public Color meshColor;
    public bool ShowMesh = false;
    private Mesh mesh;

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangle = 8;
        int numVertices = numTriangle * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

		
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(-angleVerticalBottom, 0f, 0f) * Quaternion.Euler(0, -angleHorizon/2, 0f) *  (Vector3.forward * distance);
        Vector3 bottomRight = Quaternion.Euler(-angleVerticalBottom, 0f, 0f) *  Quaternion.Euler(0, angleHorizon / 2, 0f) * (Vector3.forward * distance);

        Vector3 topCenter = Vector3.zero;
        Vector3 topLeft = Quaternion.Euler(-angleVerticalTop, 0f, 0f) * Quaternion.Euler(-0, -angleHorizon / 2, 0f) * (Vector3.forward * distance);
        Vector3 topRight = Quaternion.Euler(-angleVerticalTop, 0f, 0f) *Quaternion.Euler(-0, angleHorizon / 2, 0f) * (Vector3.forward * distance);
        

        /*
		Vector3 bottomCenter = Vector3.zero;
		Vector3 bottomLeft = Quaternion.Euler(-angleVerticalBottom, -angleHorizon / 2, 0f) * (Vector3.forward * distance);
		Vector3 bottomRight = Quaternion.Euler(-angleVerticalBottom, angleHorizon / 2, 0f) * (Vector3.forward * distance);

		Vector3 topCenter = Vector3.zero;
		Vector3 topLeft = Quaternion.Euler(-angleVerticalTop, -angleHorizon / 2, 0f) * (Vector3.forward * distance);
		Vector3 topRight = Quaternion.Euler(-angleVerticalTop, angleHorizon / 2, 0f) * (Vector3.forward * distance);
        */

		int vert = 0;

        //Left 
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Right
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        //FarSide
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft;

        //Top
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight;

        //Bottom
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomLeft;

        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        SetAllMeshInfos();
    }

    private void Start()
    {
        mesh = CreateWedgeMesh();
        SetAllMeshInfos();
    }

    private void OnDrawGizmos()
    {
        if (mesh && ShowMesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }


    private void SetAllMeshInfos()
    {
        mesh.name = meshName;
        GetComponent<MeshFilter>().mesh = mesh;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;

        if (ObjectName != null && ObjectName != "MeshCreator")
        {
            gameObject.name = ObjectName;
        }
    }
}

