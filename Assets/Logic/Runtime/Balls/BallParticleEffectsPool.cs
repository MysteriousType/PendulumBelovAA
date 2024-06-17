namespace Assets.Logic.Runtime.Balls
{
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using System.Collections.Generic;
    using System.Linq;
    using UnityObject = UnityEngine.Object;

    public class BallParticleEffectsPool
    {
        private readonly Dictionary<string, ObjectPool<BallParticleEffect>> PoolByParticleEffectId;

        public BallParticleEffectsPool()
        {
            PoolByParticleEffectId = new Dictionary<string, ObjectPool<BallParticleEffect>>();
        }

        public bool TryGetParticleEffect(string effectId, out BallParticleEffect attackParticleEffect)
        {
            if (PoolByParticleEffectId.ContainsKey(effectId))
            {
                attackParticleEffect = PoolByParticleEffectId[effectId].Pop();
                return attackParticleEffect != null;
            }

            BallParticleEffect attackParticleEffectPrefab = GameContext.PrefabsProvider.BallParticleEffects
                .Where(p => p.gameObject.name == effectId)
                .FirstOrDefault();

            if (attackParticleEffectPrefab == null)
            {
                attackParticleEffect = null;
                return false;
            }

            BallParticleEffect createObject()
            {
                return UnityObject.Instantiate(attackParticleEffectPrefab);
            }

            ObjectPool<BallParticleEffect> pool = new ObjectPool<BallParticleEffect>(createObject);
            PoolByParticleEffectId.Add(effectId, pool);

            attackParticleEffect = pool.Pop();
            return attackParticleEffect != null;
        }
    }
}