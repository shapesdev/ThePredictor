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
        Debug.Log(mousePosition);

        Matrix4x4 matrix = Matrix4x4.TRS(mousePosition, rotation, scale);

        Graphics.DrawMesh(mesh, matrix, material, 0);
    }
}
