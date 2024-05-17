﻿//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]

public class Outline : MonoBehaviour {

  [Serializable]
  private class ListVector3 {
    public List<Vector3> data;
  }

  [SerializeField, Range(1,255)]
  private int stencilIndex;

  [SerializeField,ColorUsage(true,true)]
  private Color outlineColor = Color.white;

  [SerializeField, Range(0f, 20f),Tooltip("For line thickness consistency between this and wireframes, 1.05 is recommended.")]
  private float outlineWidth = 2f;

  //[Header("Optional")]

  //[SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
  //+ "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
  //private bool precomputeOutline;

  [SerializeField, HideInInspector]
  private List<Mesh> bakeKeys = new List<Mesh>();

  [SerializeField, HideInInspector]
  private List<ListVector3> bakeValues = new List<ListVector3>();

  private Renderer _renderer;
  private Material outlineFillMaterial;

  void Awake() {
    ClearData();
    // Cache renderer
    _renderer = GetComponent<Renderer>();

    outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"+stencilIndex));

    outlineFillMaterial.name = "OutlineFill (Instance)";

    Bake();
    // Retrieve or generate smooth normals
    LoadSmoothNormals();

    // Apply material properties immediately
    UpdateMaterialProperties();
  }

  void OnEnable() {
      // Append outline shaders
      var materials = _renderer.sharedMaterials.ToList();

      materials.Add(outlineFillMaterial);

      _renderer.materials = materials.ToArray();
  }

  [ContextMenu("Clear Data")]
  private void ClearData(){
    bakeKeys.Clear();
    bakeValues.Clear();
  }

  void OnValidate() {

    // // Clear cache when baking is disabled or corrupted
    // if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count) {
    //   bakeKeys.Clear();
    //   bakeValues.Clear();
    // }

    // // Generate smooth normals when baking is enabled
    // if (precomputeOutline && bakeKeys.Count == 0) {
    //   Bake();
    // }
  }


  void OnDisable() {
      // Remove outline shaders
      var materials = _renderer.sharedMaterials.ToList();

      materials.Remove(outlineFillMaterial);

      _renderer.materials = materials.ToArray();
    
  }

  void OnDestroy() {

    // Destroy material instances
    Destroy(outlineFillMaterial);
  }

  void Bake() {

    var meshFilter = GetComponent<MeshFilter>();

    // Serialize smooth normals
    var smoothNormals = SmoothNormals(meshFilter.mesh);

    bakeKeys.Add(meshFilter.mesh);
    bakeValues.Add(new ListVector3() { data = smoothNormals });
    
  }

  void LoadSmoothNormals() {

    // Retrieve or generate smooth normals
    var meshFilter = GetComponent<MeshFilter>();

      // Retrieve or generate smooth normals
      var index = bakeKeys.IndexOf(meshFilter.mesh);
      var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.mesh);

      // Store smooth normals in UV3
      meshFilter.mesh.SetUVs(3, smoothNormals);

      // Combine submeshes
      var renderer = meshFilter.GetComponent<Renderer>();

      if (renderer != null) {
        CombineSubmeshes(meshFilter.mesh, renderer.sharedMaterials);
      }
    

      // Clear UV3 on skinned mesh _renderer
      var skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

      if (skinnedMeshRenderer == null)
      return;
      // Clear UV3
      skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

      // Combine submeshes
      CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
    
  }

  List<Vector3> SmoothNormals(Mesh mesh) {

    // Group vertices by location
    var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

    // Copy normals to a new list
    var smoothNormals = new List<Vector3>(mesh.normals);

    if (smoothNormals.Count == 0)
      return smoothNormals;

    // Average normals for grouped vertices
    foreach (var group in groups) {

      // Skip single vertices
      if (group.Count() == 1) {
        continue;
      }

      // Calculate the average normal
      var smoothNormal = Vector3.zero;

      foreach (var pair in group) {
        smoothNormal += smoothNormals[pair.Value];
      }

      smoothNormal.Normalize();

      // Assign smooth normal to each vertex
      foreach (var pair in group) {
        smoothNormals[pair.Value] = smoothNormal;
      }
    }

    return smoothNormals;
  }

  void CombineSubmeshes(Mesh mesh, Material[] materials) {

    // Skip meshes with a single submesh
    if (mesh.subMeshCount == 1) {
      return;
    }

    // Skip if submesh count exceeds material count
    if (mesh.subMeshCount > materials.Length) {
      return;
    }

    // Append combined submesh
    mesh.subMeshCount++;
    mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
  }
  void UpdateMaterialProperties() {
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        outlineFillMaterial.color = outlineColor;
    }
}