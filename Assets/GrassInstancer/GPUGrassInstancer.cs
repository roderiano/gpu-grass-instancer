using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Random = System.Random;
using System.Collections.Generic;



public class GPUGrassInstancer : MonoBehaviour {
    public Material material;
    public Mesh mesh;
    public MeshFilter meshFilter;

    GraphicsBuffer commandBuf;
    GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
    const int commandCount = 1;

    void Start()
    {
        commandBuf = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandCount, GraphicsBuffer.IndirectDrawIndexedArgs.size);
        commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[commandCount];
    }

    void OnDestroy()
    {
        commandBuf?.Release();
        commandBuf = null;
    }

    void Update()
    {
        RenderParams renderParams = new RenderParams(material);
        renderParams.worldBounds = new Bounds(Vector3.zero, 10000*Vector3.one); // use tighter bounds for better FOV culling
        renderParams.matProps = new MaterialPropertyBlock();

        foreach (Vector3 vert in meshFilter.mesh.vertices)
        {
            renderParams.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(transform.position + vert));
            for (int i = 0; i < commandCount; i++)
            {
                commandData[i].indexCountPerInstance = mesh.GetIndexCount(0);
                commandData[i].instanceCount = commandCount;
            }

            commandBuf.SetData(commandData);
            Graphics.RenderMeshIndirect(renderParams, mesh, commandBuf, commandCount);
        }
    }
}