using UnityEngine;
using System;
using System.Collections.Generic;

public class DictionaryTester : MonoBehaviour {

    private Dictionary<int, Chunk> chunks;

    void Awake() {
        int len = 32; // 1048576

        System.Diagnostics.Stopwatch initTime = System.Diagnostics.Stopwatch.StartNew();
        chunks = new Dictionary<int, Chunk>(len);

        for (int i=0; i<len; i++) {
            chunks.Add(i, new Chunk(8, Vector3.zero));
        }
        initTime.Stop();

        System.Diagnostics.Stopwatch trygetTime = System.Diagnostics.Stopwatch.StartNew();
        for (int n=0; n<len; n++) {
            Chunk chunk;
            chunks.TryGetValue(n, out chunk);
        }
        trygetTime.Stop();

        System.Diagnostics.Stopwatch randrw = System.Diagnostics.Stopwatch.StartNew();
        for (int m=0; m<1024; m++) {
            for (int n=0; n<9; n++) {
                Chunk chunk;
                int key = (int)Mathf.Floor(UnityEngine.Random.value * (len-1));
                chunks.TryGetValue(key, out chunk);
                if (chunk != null) {
                    int x = (int)Mathf.Floor(UnityEngine.Random.value * 7);
                    int y = (int)Mathf.Floor(UnityEngine.Random.value * 7);
                    int z = (int)Mathf.Floor(UnityEngine.Random.value * 7);
                    int v = (int)Mathf.Floor(UnityEngine.Random.value * 2048);
                    chunk.Set(x, y, z, v);
                }
            }
        }
        randrw.Stop();

        Debug.Log("init ms: "+initTime.ElapsedMilliseconds);
        Debug.Log("tryget ms: "+trygetTime.ElapsedMilliseconds);
        Debug.Log("randrw ms: "+randrw.ElapsedMilliseconds);
    }
}
