using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GraphicsTest : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    private void OnEnable() {

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable() {

        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView) {
        Draw();
    }


    private void Draw() {
        Quaternion rotation = Random.rotation;
        Vector3 scale = new Vector3(1f, 1, 1f);

        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        mousePosition = ray.origin;
        mousePosition.z = 0;

        Matrix4x4 matrix = Matrix4x4.TRS(mousePosition, rotation, scale);

        Color color = new Color(104, 223, 248, 213);
        var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        material.SetColor("_BaseColor", color);

        Graphics.DrawMesh(mesh, matrix, material, 0);
    }
}
