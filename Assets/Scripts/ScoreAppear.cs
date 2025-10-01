using UnityEngine;
using TMPro;

public class ScoreAppear : MonoBehaviour
{
    private TextMeshPro textMesh; 
    private float moveSpeed = 1f;
    private float fadeSpeed = 2f;
    private float lifeTime = 1.5f;
    private Vector3 moveVector;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
            Debug.LogError("Missing TextMeshPro component!");
    }

    public void Setup(int value)
    {
        textMesh.text = value.ToString() + "$";
        textMesh.color = Color.forestGreen;
        moveVector = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f);
        Destroy(gameObject, lifeTime); 
    }

    void Update()
    {
        transform.position += moveVector * moveSpeed * Time.deltaTime;
        moveVector -= new Vector3(0, 1f, 0) * Time.deltaTime;
        Color currentColor = textMesh.color;
        currentColor.a -= fadeSpeed * Time.deltaTime / lifeTime;
        textMesh.color = currentColor;
    }
}