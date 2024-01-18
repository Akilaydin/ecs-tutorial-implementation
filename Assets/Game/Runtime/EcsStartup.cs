#region
using Client.Components;
using Client.Services;
using Client.Systems;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;

using UnityEngine;
#endregion

namespace Client
{
	internal sealed class EcsStartup : MonoBehaviour
	{
		[SerializeField] private SceneService _sceneService;
		
		private EcsWorld _world;
		private IEcsSystems _systems;

		private void Start()
		{
			_world = new EcsWorld();
			_systems = new EcsSystems(_world);
			_systems
				#if UNITY_EDITOR
				.Add(new EcsWorldDebugSystem())
				.Add(new PlayerInputSystem())
				.Add(new MovementSystem())
				.Add(new EnemiesSystem())
				.Add(new ScoreCounterSystem())
				.Add(new EndgameSystem())
				.DelHere<CollisionEvent>()
                #endif
				.Inject(_sceneService)
				.Init();
		}

		private void Update()
		{
			_systems?.Run();
		}

		private void OnDestroy()
		{
			if (_systems != null)
			{
				_systems.Destroy();
				_systems = null;
			}

			if (_world != null)
			{
				_world.Destroy();
				_world = null;
			}
		}
	}
}
