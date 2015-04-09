using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Plane切分凸面体
/// </summary>
public class TestSplit : MonoBehaviour
{
    private Plane plane;

    private Vector3[] vertics;
    private Vector3[] normals;
    private Vector2[] uvs;
    private int[] triangles;
    private Vector3[] positions;
    private bool[] sides;

    private Dictionary<string, int> intersectCaches;
    private Dictionary<int, float> distanceCaches;

    private MeshData mesh_pos;
    private MeshData mesh_neg;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 80, 20), "Split"))
        {
            Split();
        }
    }

	void Split () 
    {
        plane = new Plane(Vector3.up, Vector3.zero);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        vertics = mesh.vertices;
        normals = mesh.normals;
        uvs = mesh.uv;
        triangles = mesh.triangles;

        positions = new Vector3[vertics.Length];
        sides = new bool[vertics.Length];
        for (int i = vertics.Length - 1; i >= 0; i--)
        {
            positions[i] = transform.TransformPoint(vertics[i]);
            sides[i] = plane.GetSide(positions[i]);
        }

        mesh_pos = new MeshData();
        mesh_neg = new MeshData();

        distanceCaches = new Dictionary<int, float>();
        intersectCaches = new Dictionary<string, int>();

        int index1, index2, index3;
        bool side1, side2, side3;
        int loneIndex, togIndex1, togIndex2, newIndex1, newIndex2;
        bool loneSide;
        string key, rkey;
        Vector3 wp = Vector3.zero;
        Vector3 nm = Vector3.zero;
        Vector2 uv = Vector2.zero;
        float lerp;
        List<Vector3> intersect_wps = new List<Vector3>();
        List<Vector3> intersect_nms = new List<Vector3>();
        List<Vector2> intersect_uvs = new List<Vector2>();
        int intersectCount = positions.Length;
        int[] tr1, tr2;
        int tlength = triangles.Length;
        for (int i = 0; i < tlength; i += 3)
        {
            index1 = triangles[i];
            index2 = triangles[i + 1];
            index3 = triangles[i + 2];
            side1 = sides[index1];
            side2 = sides[index2];
            side3 = sides[index3];
            if (!(side1 == side2 && side2 == side3))
            {
                if (side2 == side3)
                {
                    loneSide = side1;
                    loneIndex = index1;
                    togIndex1 = index2;
                    togIndex2 = index3;
                }
                else if (side1 == side3)
                {
                    loneSide = side2;
                    loneIndex = index2;
                    togIndex1 = index3;
                    togIndex2 = index1;
                }
                else
                {
                    loneSide = side3;
                    loneIndex = index3;
                    togIndex1 = index1;
                    togIndex2 = index2;
                }

                key = loneIndex + "_" + togIndex1;
                rkey = togIndex1 + "_" + loneIndex;
                if (!intersectCaches.ContainsKey(key))
                {
                    wp = GetLinePlaneIntersetion(loneIndex, togIndex1, plane);
                    lerp = (positions[loneIndex] - wp).magnitude / (positions[loneIndex] - positions[togIndex1]).magnitude;
                    nm = Vector3.Slerp(normals[loneIndex], normals[togIndex1], lerp);
                    uv = Vector2.Lerp(uvs[loneIndex], uvs[togIndex1], lerp);
                    intersect_wps.Add(wp);
                    intersect_nms.Add(nm);
                    intersect_uvs.Add(uv);
                    intersectCaches[key] = intersectCount;
                    intersectCaches[rkey] = intersectCount;
                    intersectCount++;
                }
                newIndex1 = intersectCaches[key];

                key = loneIndex + "_" + togIndex2;
                rkey = togIndex2 + "_" + loneIndex;
                if (!intersectCaches.ContainsKey(key))
                {
                    wp = GetLinePlaneIntersetion(loneIndex, togIndex2, plane);
                    lerp = (positions[loneIndex] - wp).magnitude / (positions[loneIndex] - positions[togIndex2]).magnitude;
                    nm = Vector3.Slerp(normals[loneIndex], normals[togIndex2], lerp);
                    uv = Vector2.Lerp(uvs[loneIndex], uvs[togIndex2], lerp);
                    intersect_wps.Add(wp);
                    intersect_nms.Add(nm);
                    intersect_uvs.Add(uv);
                    intersectCaches[key] = intersectCount;
                    intersectCaches[rkey] = intersectCount;
                    intersectCount++;
                }
                newIndex2 = intersectCaches[key];

                tr1 = new int[] { loneIndex, newIndex1, newIndex2 };
                tr2 = new int[] { newIndex2, newIndex1, togIndex1, newIndex2, togIndex1, togIndex2 };

                if (loneSide)
                {
                    mesh_pos.triangles.AddRange(tr1);
                    mesh_neg.triangles.AddRange(tr2);
                }
                else
                {
                    mesh_pos.triangles.AddRange(tr2);
                    mesh_neg.triangles.AddRange(tr1);
                }
            }
            else
            {
                tr1 = new int[] { index1, index2, index3 };
                if (side1)
                {
                    mesh_pos.triangles.AddRange(tr1);
                }
                else
                {
                    mesh_neg.triangles.AddRange(tr1);
                }
            }
        }

        // 填充新增的空白面
        List<Vector3> new_positions = new List<Vector3>();
        List<Vector2> new_uvs = new List<Vector2>();
        int cutCount = intersect_wps.Count;
        if (cutCount > 0)
        {
            new_positions.Add(Vector3.Lerp(intersect_wps[0], intersect_wps[(int)cutCount / 2], 0.5f));
            new_positions.AddRange(intersect_wps);

            // 去重
            for (int i = cutCount - 1; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (new_positions[i] == new_positions[j])
                    {
                        new_positions.RemoveAt(i);
                        break;
                    }
                }
            }
            cutCount = new_positions.Count;

            // 排序/同一条直线上的选择较samplePoint远点
            Vector3 samplePoint = new_positions[0];
            List<Vector3> expired = new List<Vector3>();
            new_positions.Sort((Vector3 p1, Vector3 p2) =>
            {
                if (p1 == p2)
                {
                    return 0;
                }
                else if (p1 == samplePoint)
                {
                    return -1;
                }
                else if (p2 == samplePoint)
                {
                    return 1;
                }
                else
                {
                    Vector3 v1 = p1 - samplePoint;
                    Vector3 v2 = p2 - samplePoint;
                    float y1 = Quaternion.LookRotation(v1, plane.normal).eulerAngles.y;
                    float y2 = Quaternion.LookRotation(v2, plane.normal).eulerAngles.y;
                    if (Mathf.Approximately(y1, y2))
                    {
                        expired.Add(v1.magnitude < v2.magnitude ? p1 : p2);
                        return -1;
                    }
                    else
                    {
                        return y1 > y2 ? 1 : -1;
                    }
                }
            });
            foreach (Vector3 v in expired)
            {
                new_positions.Remove(v);
            }
            cutCount = new_positions.Count;

            for (int i = 0; i < cutCount; i++)
            {
                new_uvs.Add(new Vector2(i / cutCount, i / cutCount));
            }
            int count = intersectCount + cutCount - 1;
            for (int i = intersectCount + 1; i < count; i++)
            {
                mesh_pos.triangles.Add(i + 1);
                mesh_pos.triangles.Add(i);
                mesh_pos.triangles.Add(intersectCount);

                mesh_neg.triangles.Add(intersectCount);
                mesh_neg.triangles.Add(i);
                mesh_neg.triangles.Add(i + 1);
            }

            mesh_pos.triangles.Add(intersectCount + 1);
            mesh_pos.triangles.Add(count);
            mesh_pos.triangles.Add(intersectCount);

            mesh_neg.triangles.Add(intersectCount);
            mesh_neg.triangles.Add(count);
            mesh_neg.triangles.Add(intersectCount + 1);
        }

        List<Vector3> all_positions = new List<Vector3>(positions);
        all_positions.AddRange(intersect_wps);
        all_positions.AddRange(new_positions);

        List<Vector3> all_vertices = new List<Vector3>(vertics);
        all_vertices.AddRange(intersect_wps.ConvertAll<Vector3>((Vector3 position) => transform.InverseTransformPoint(position)));
        all_vertices.AddRange(new_positions.ConvertAll<Vector3>((Vector3 position) => transform.InverseTransformPoint(position)));

        List<Vector2> all_uvs = new List<Vector2>(uvs);
        all_uvs.AddRange(intersect_uvs);
        all_uvs.AddRange(new_uvs);

        List<Vector3> all_normals = new List<Vector3>(normals);
        all_normals.AddRange(intersect_nms);

        mesh_pos.positions.AddRange(all_positions);
        mesh_pos.vertices.AddRange(all_vertices);
        mesh_pos.uvs.AddRange(all_uvs);
        mesh_pos.normals.AddRange(all_normals);
        Vector3 new_local_normal = transform.worldToLocalMatrix.MultiplyVector(plane.normal);
        mesh_pos.normals.AddRange(new_positions.ConvertAll<Vector3>((Vector3 position) => -new_local_normal));

        mesh_neg.positions.AddRange(all_positions);
        mesh_neg.vertices.AddRange(all_vertices);
        mesh_neg.uvs.AddRange(all_uvs);
        mesh_neg.normals.AddRange(all_normals);
        mesh_neg.normals.AddRange(new_positions.ConvertAll<Vector3>((Vector3 position) => new_local_normal));

        CreateMeshObject(mesh_pos);
        CreateMeshObject(mesh_neg);
	}

    private Vector3 GetLinePlaneIntersetion(int from, int to, Plane plane)
    {
        Vector3 intersection = Vector3.zero;
        float distance = 0;
        if (!distanceCaches.ContainsKey(from))
        {
            distanceCaches[from] = -plane.GetDistanceToPoint(positions[from]);
        }
        distance = distanceCaches[from];
        Vector3 dir = (positions[from] - positions[to]).normalized;
        float dot = Vector3.Dot(dir, plane.normal);
        if (dot != 0.0f)
        {
            intersection = positions[from] + dir * distance / dot;
        }
        return intersection;
    }

    private void CreateMeshObject(MeshData meshData)
    {
        GameObject go = new GameObject("SubMesh");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = meshData.GetMesh();
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Diffuse"));
        meshRenderer.material = material;
    }
}
