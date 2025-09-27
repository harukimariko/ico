using System.Collections.Generic;
using UnityEngine;

public class DropParticle : MonoBehaviour
{ 
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> triggeredParticles = new List<ParticleSystem.Particle>();
    [SerializeField] private float _addHp = 10.0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        Debug.Log("OnParticleTriggerが使えないよ！");

        // トリガーに触れたパーティクルを取得
        int numTriggered = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, triggeredParticles);

        // List の要素は struct（値型）なので一度コピーして戻す
        for (int i = 0; i < numTriggered; i++)
        {
            ParticleSystem.Particle p = triggeredParticles[i]; // コピー
            p.startColor = Color.red;                          // コピーを変更
            triggeredParticles[i] = p;                         // List に戻す

            GetParticle();
        }

        // 変更をパーティクルシステムに反映
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, triggeredParticles);
    }

    private void GetParticle()
    {
        Debug.Log("HP回復！");
        GameManager._instance.SetHp(GameManager._instance._hp + _addHp);
    }
}
