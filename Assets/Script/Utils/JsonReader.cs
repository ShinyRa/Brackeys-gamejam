using UnityEngine;

public class JsonReader<E> {    
    public TextAsset data;
    public E parsed;

    public JsonReader(TextAsset json) {
        this.data = json;
        this.Read();
    }

    public E GetParsed() {
        return this.parsed;
    }

    public void Set(E parsed) {
        this.parsed = parsed;
    }

    private void Read() {
        this.Set(JsonUtility.FromJson<E>(this.data.text));
    }
}