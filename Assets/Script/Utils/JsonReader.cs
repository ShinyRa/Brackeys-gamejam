using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader<E> {    
    public TextAsset data;
    public E list;

    public JsonReader(TextAsset json) {
        this.data = json;
        this.Read();
    }

    public E GetList() {
        return this.list;
    }

    public void Set(E list) {
        this.list = list;
    }

    private void Read() {
        this.Set(JsonUtility.FromJson<E>(this.data.text));
    }
}