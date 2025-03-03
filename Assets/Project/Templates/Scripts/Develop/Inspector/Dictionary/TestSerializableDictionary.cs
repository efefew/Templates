using UnityEngine;

public class TestSerializableDictionary : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<string, GameObject> dictionary = new SerializableDictionary<string, GameObject>();
}
