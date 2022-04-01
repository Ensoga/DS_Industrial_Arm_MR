using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// STL Format
// https://en.wikipedia.org/wiki/STL_(file_format)
// Loads the Binary STL format
public static class StlConverter
{
    private const int MaxVerticesPerMesh = 65535; // Limitation on Unity

    // Default merging settings
    private const bool DefaultMergeVertices = false;
    private const float DefaultMergeAngle = 80; // Degrees
    private const float DefaultMergeTolerance = 1e-6f; // TODO: Include this using nearest neighbor search

    // Stl info
    private const int StlHeaderLength = 80;

    public class NormalIndex
    {
        public Vector3 Normal { get; set; }
        public int Index { get; set; }
    }

    public class IndexCollection
    {
        public List<int> FromIndices { get; set; }
        public int ToIndex { get; set; }
    }

    public static Mesh[] Load(string fileName, float scaleFactor, bool mergeVertices = DefaultMergeVertices, float mergeTolerance = DefaultMergeTolerance, float mergeAngle = DefaultMergeAngle)
    {
        return Load(File.ReadAllBytes(fileName), scaleFactor, mergeVertices, mergeTolerance, mergeAngle);
    }

    private static void AddRange<T>(this ICollection<T> collection, params T[] values)
    {
        for (int i=0; i<values.Length; ++i)
            collection.Add(values[i]);
    }

    public static Mesh[] Load(byte[] bytes, float scaleFactor, bool mergeVertices = DefaultMergeVertices, float mergeTolerance = DefaultMergeTolerance, float mergeAngle = DefaultMergeAngle)
    {
        return mergeVertices ? LoadMerged(bytes, scaleFactor, true, mergeTolerance, mergeAngle) : LoadNormal(bytes, scaleFactor);
    }

    private static Mesh[] LoadMerged(byte[] bytes, float scaleFactor, bool mergeVertices = DefaultMergeVertices, float mergeTolerance = DefaultMergeTolerance, float mergeAngle = DefaultMergeAngle)
    {
        BinaryReader reader = new BinaryReader(new MemoryStream(bytes));

        // Read and throw out the header
        reader.ReadBytes(StlHeaderLength);

        uint trianglesCount = reader.ReadUInt32();
        uint vertexCount = trianglesCount * 3;
        

        // First pass, get all vertices and normals
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < trianglesCount; i++)
        {
            Vector3 n = ReadVector(reader);
            Vector3 v0 = ReadVector(reader) * scaleFactor;
            Vector3 v1 = ReadVector(reader) * scaleFactor;
            Vector3 v2 = ReadVector(reader) * scaleFactor;

            // Attribute byte count
            // assume unused
            reader.ReadUInt16();

            vertices.AddRange(v0, v1, v2);
            normals.AddRange(n, n, n);
        }


        // Second pass, generate vertex to index collections dictionary for merging vertices
        int[] vertexMapper = new int[vertices.Count];
        List<IndexCollection> indexCollections = new List<IndexCollection>();
        
        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = vertices[i];
            Vector3 normal = normals[i];

            indexCollections.Add(new IndexCollection() { ToIndex = i, FromIndices = new List<int>() { i } });
        }

        bool hasMapped = true;
        while(hasMapped)
        {
            hasMapped = false;
            for (int i = 0; i < indexCollections.Count; i++)
            {
                IndexCollection iCollection = indexCollections[i];
                if (!iCollection.FromIndices.Any())
                    continue;

                for (int j = i + 1; j < indexCollections.Count; j++)
                {
                    IndexCollection jCollection = indexCollections[j];
                    if (!jCollection.FromIndices.Any())
                        continue;

                    if (iCollection.FromIndices.Any(iIndex =>
                         jCollection.FromIndices.Any(jIndex =>
                             Vector3.Distance(vertices[iIndex], vertices[jIndex]) <= mergeTolerance &&
                             Vector3.Angle(normals[iIndex], normals[jIndex]) <= mergeAngle
                         )))
                    {
                        iCollection.FromIndices.AddRange(jCollection.FromIndices);
                        jCollection.FromIndices.Clear();
                        hasMapped = true;
                    }
                }
            }
        }

        foreach (IndexCollection indexCollection in indexCollections)
        {
            foreach (int fromIndex in indexCollection.FromIndices)
                vertexMapper[fromIndex] = indexCollection.ToIndex;
        }


        // Final pass, generate mesh
        int[] mergeMapper = new int[vertexCount];
        List<int> mergedTriangles = new List<int>();
        List<Vector3> mergedVertices = new List<Vector3>();
        List<Vector3> mergedNormals = new List<Vector3>();

        for (int i = 0; i < vertexCount; i++)
        {
            if(i == vertexMapper[i])
            {
                mergeMapper[i] = mergedVertices.Count;

                Vector3 vertex = Vector3.zero;
                Vector3 normal = Vector3.zero;
                foreach (int index in indexCollections[i].FromIndices)
                {
                    vertex += vertices[index];
                }

                vertex /= indexCollections[i].FromIndices.Count;

                mergedVertices.Add(vertex);
                mergedNormals.Add(PlaneNormalFromPoints(indexCollections[i].FromIndices.Select(j => normals[j]).ToList()));
            }
            else
            {
                mergeMapper[i] = mergeMapper[vertexMapper[i]];
            }
        }

        for (int i = 0; i < vertexCount; i += 3)
        {
            mergedTriangles.Add(mergeMapper[i + 2]);
            mergedTriangles.Add(mergeMapper[i + 1]);
            mergedTriangles.Add(mergeMapper[i + 0]);
        }

        Mesh[] meshes = new Mesh[Mathf.CeilToInt(trianglesCount * 3.0f / MaxVerticesPerMesh)];
        meshes[0] = ToMesh(mergedVertices, mergedNormals, mergedTriangles);
        return meshes;
    }

    private static Vector3 PlaneNormalFromPoints(List<Vector3> points)
    {
        if(!points.Any())
            return Vector3.zero;
        
        Vector3 pointSum = Vector3.zero;
        foreach(Vector3 point in points)
            pointSum += point;

        if (points.Count <= 2)
            return pointSum.normalized;

        Vector3 centroid = pointSum / points.Count;

        // Calc full 3x3 covariance matrix, excluding symmetries:
        float xx = 0f; float xy = 0f; float xz = 0f; float yy = 0f; float yz = 0f; float zz = 0f;

        foreach (Vector3 point in points)
        {
            Vector3 r = point - centroid;
            xx += r.x * r.x;
            xy += r.x * r.y;
            xz += r.x * r.z;
            yy += r.y * r.y;
            yz += r.y * r.z;
            zz += r.z * r.z;
        }

        float det_x = yy * zz - yz * yz;
        float det_y = xx * zz - xz * xz;
        float det_z = xx * yy - xy * xy;

        float det_max = Mathf.Max(det_x, det_y, det_z);
        if (det_max <= 1e-6f) {
            return pointSum.normalized;
        }

        Vector3 normal = Vector3.zero;

        // Pick path with best conditioning
        if (det_max == det_x)
            normal = new Vector3(det_x, xz * yz - xy * zz, xy * yz - xz * yy).normalized;
        else if (det_max == det_y)
            normal = new Vector3(xz * yz - xy * zz, det_y, xy * xz - yz * xx).normalized;
        else
            normal = new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, det_z).normalized;

        if (Vector3.Dot(centroid, normal) < 0)
            normal *= -1;

        return normal;
    }

    private static Mesh[] LoadNormal(byte[] bytes, float scaleFactor)
    {
        BinaryReader reader = new BinaryReader(new MemoryStream(bytes));

        // Read and throw out the header
        reader.ReadBytes(80);

        uint trianglesCount = reader.ReadUInt32();
        Mesh[] meshes = new Mesh[Mathf.CeilToInt(trianglesCount * 3.0f / MaxVerticesPerMesh)];

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        int currTri = 0;
        int meshIndex = 0;

        for (int i = 0; i < trianglesCount; i++)
        {
            Vector3 n = ReadVector(reader);
            Vector3 v0 = ReadVector(reader) * scaleFactor;
            Vector3 v1 = ReadVector(reader) * scaleFactor;
            Vector3 v2 = ReadVector(reader) * scaleFactor;
            
            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);

            normals.Add(n);
            normals.Add(n);
            normals.Add(n);

            triangles.Add(currTri + 2);
            triangles.Add(currTri + 1);
            triangles.Add(currTri + 0);

            // Add vertices in reverse order because of Unity frame conversion
            currTri += 3;

            // Attribute byte count
            // assume unused
            reader.ReadUInt16();

            if (vertices.Count <= MaxVerticesPerMesh - 1) continue;

            meshes[meshIndex++] = ToMesh(vertices, normals, triangles);

            vertices.Clear();
            normals.Clear();
            triangles.Clear();
            currTri = 0;
        }

        if (vertices.Count > 0)
            meshes[meshIndex] = ToMesh(vertices, normals, triangles);

        return meshes;
    }

    private static Vector3 Round(Vector3 v, int decimals)
    {
        return new Vector3(
            (float)Math.Round(v.x, decimals),
            (float)Math.Round(v.y, decimals),
            (float)Math.Round(v.z, decimals));
    }

    private static NormalIndex GetNormalIndex(Dictionary<Vector3, List<NormalIndex>> vertexNormalIndices, Vector3 vertex, Vector3 normal, float mergeAngle)
    {
        if (!vertexNormalIndices.ContainsKey(vertex))
            return null;

        var normalIndices = vertexNormalIndices[vertex];
        foreach (var normalIndex in normalIndices)
        {
            if (Vector3.Angle(normalIndex.Normal, normal) <= mergeAngle)
            {
                return normalIndex;
            }
        }

        return null;
    }

    private static NormalIndex UpdateThenGetNormalIndex(Dictionary<Vector3, List<NormalIndex>> vertexNormalIndices, Vector3 vertex, Vector3 normal, int index, float mergeAngle)
    {
        if(!vertexNormalIndices.ContainsKey(vertex))
        {
            NormalIndex normalIndex = new NormalIndex() { Index = index, Normal = normal };
            vertexNormalIndices.Add(vertex, new List<NormalIndex>() { normalIndex });
            return normalIndex;
        }

        var normalIndices = vertexNormalIndices[vertex];
        foreach(var normalIndex in normalIndices)
        {
            if(Vector3.Angle(normalIndex.Normal, normal) <= mergeAngle)
            {
                normalIndex.Normal += normal;
                return normalIndex;
            }
        }

        NormalIndex newNormalIndex = new NormalIndex() { Index = index, Normal = normal };
        normalIndices.Add(newNormalIndex);
        return newNormalIndex;
    }

    private static Vector3 ReadVector(BinaryReader br)
    {
        Vector3 v = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        return new Vector3(-v.x, v.z, -v.y); // Have been checked converts the same as collada
    }

    private static Mesh ToMesh(List<Vector3> vertices, List<Vector3> normals, List<int> triangles)
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            normals = normals.ToArray()
        };

        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }
}