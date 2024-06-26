//
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
using static Conditional;

[DisallowMultipleComponent]

public class OutlineEffect : MonoBehaviour {
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    [Serializable]
    private class ListVector3 {
        public List<Vector3> data;
    }
    [SerializeField] private Bacteria bac;

    [SerializeField,GradientUsage(true)]
    private Gradient gradient;
  
    [SerializeField]
    private OnInterval interval;

    [SerializeField, Range(0f, 20f),Tooltip("For line thickness consistency between this and wireframes, 1.05 is recommended.")]
    private float outlineWidth = 2f;

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();
    private Material outlineFillMaterial;
    private Material roundedOutlineMaterial;
    private List<Material> mats = new List<Material>();
    private float t;

    private void AddEffect(){
        if (interval != null)
            interval.enabled = true;
        bac.immuneSystem.stats.conditionals[OUTLINE_FXED] = true;
        foreach(var part in bac.immuneSystem.injector.bodyParts){
            if (!part.outlineEffectable)
                continue;
            var materials = part._renderer.sharedMaterials.ToList();
            if (part.particle) {
                materials.Add(roundedOutlineMaterial);
            }else {
                LoadSmoothNormals(part);
                materials.Add(outlineFillMaterial);
            }
            part._renderer.materials = materials.ToArray();
            mats.Add(part._renderer.materials[part._renderer.materials.Length-1]);
        }
    }

    void Awake() {
        //Bake();
        outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/Outline"));
        roundedOutlineMaterial = Instantiate(Resources.Load<Material>(@"Materials/RoundedOutline"));
    }

    private void CalculateT(){
        if (interval != null){
            t = interval.t;
        }else{
            t = 0;
        }
    }

    void OnEnable() {
        AddEffect();
        UpdateMaterialProperties();
        CalculateT();
        foreach (var mat in mats){
            mat.color = gradient.Evaluate(t);
        }
    }

    void LateUpdate(){
        CalculateT();
        foreach (var mat in mats){
            mat.color = gradient.Evaluate(t);
        }
    }

    void OnDisable() {
        if (interval != null)
            interval.enabled = false;
        bac.immuneSystem.stats.conditionals[OUTLINE_FXED] = false;
        for (int i = 0; i < bac.immuneSystem.injector.bodyParts.Count; i++){
            if (!bac.immuneSystem.injector.bodyParts[i].outlineEffectable)
                continue;
            var materials = bac.immuneSystem.injector.bodyParts[i]._renderer.sharedMaterials.ToList();

            materials.Remove(mats[i]);

            bac.immuneSystem.injector.bodyParts[i]._renderer.materials = materials.ToArray();
        }
        mats.Clear();
        enabled = false;
    }

    void Bake() {
        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh>();

        var meshFilter = GetComponent<MeshFilter>();

        // Serialize smooth normals
        var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

        bakeKeys.Add(meshFilter.sharedMesh);
        bakeValues.Add(new ListVector3() { data = smoothNormals });
    }

    void LoadSmoothNormals(BodyPart part) {
        // Retrieve or generate smooth normals
        var meshFilter = part._renderer.gameObject.GetComponent<MeshFilter>();

        // Retrieve or generate smooth normals
        var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
        var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

        // Store smooth normals in UV3
        meshFilter.sharedMesh.SetUVs(3, smoothNormals);

        if (part._renderer != null) {
        CombineSubmeshes(meshFilter.sharedMesh, part._renderer.sharedMaterials);
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh) {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

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
        foreach (var mat in mats){
            if (mat.HasFloat("_OutlineWidth")) {
                mat.SetFloat("_OutlineWidth", outlineWidth);
            }else {
                mat.SetFloat("_Extension", outlineWidth);
            }
        }
    }
}