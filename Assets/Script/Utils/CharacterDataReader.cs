using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataReader : JsonReader<CharacterData> {
    public CharacterDataReader(TextAsset json) : base(json) {
        
    }
}