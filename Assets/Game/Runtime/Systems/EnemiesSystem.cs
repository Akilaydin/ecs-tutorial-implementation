using Client.Components;
using Client.Services;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client.Systems
{
	public class EnemiesSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorldInject _world;
		private EcsCustomInject<SceneService> _sceneService;
		private EcsPoolInject<UnitComponent> _unitComponentsPool;
		private EcsPoolInject<LifetimeComponent> _lifetimeComponentsPool;
		private EcsFilterInject<Inc<LifetimeComponent>> _lifetimeFilter;

		private float _spawnInterval;
		private Camera _camera;

		void IEcsInitSystem.Init(IEcsSystems systems)
		{
			_spawnInterval = _sceneService.Value.EnemySpawnInterval;
			_camera = _sceneService.Value.Camera;
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			if (_sceneService.Value.GameOver)
			{
				return;
			}

			CreateEnemy();
			CheckEnemyLifetime();
		}
		
		private void CreateEnemy()
		{
			if ((_spawnInterval -= Time.deltaTime) > 0)
			{
				return;
			}

			_spawnInterval = _sceneService.Value.EnemySpawnInterval;

			var enemyView = _sceneService.Value.GetEnemy();
			var enemyPosition = GetOutOfScreenPosition();
			enemyView.SetPosition(enemyPosition);
			enemyView.RotateTo(_sceneService.Value.PlayerView.transform.position);

			var enemyEntity = _world.Value.NewEntity();
			ref var unitComponent = ref _unitComponentsPool.Value.Add(enemyEntity);
			unitComponent.View = enemyView;
			unitComponent.View.Construct(enemyEntity, _world.Value);
			unitComponent.Velocity = Vector3.up * _sceneService.Value.EnemyMoveSpeed;

			ref var lifetimeComponent = ref _lifetimeComponentsPool.Value.Add(enemyEntity);
			lifetimeComponent.Value = 3;
		}
		
		private void CheckEnemyLifetime()
		{
			foreach (var entity in _lifetimeFilter.Value)
			{
				ref var lifetimeComponent = ref _lifetimeComponentsPool.Value.Get(entity);
				lifetimeComponent.Value -= Time.deltaTime;

				if (lifetimeComponent.Value > 0)
				{
					continue;
				}

				ref var unitComponent = ref _unitComponentsPool.Value.Get(entity);
				_sceneService.Value.ReleaseEnemy(unitComponent.View);
				
				_world.Value.DelEntity(entity);
			}
		}

		private Vector3 GetOutOfScreenPosition()
		{
			var randomX = Random.Range(-1000, 1000);
			var randomY = Random.Range(-1000, 1000);
			var randomPosition = new Vector3(randomX, randomY);
			var randomDirection = (_camera.transform.position - randomPosition).normalized;
			var cameraHeight = _camera.orthographicSize * 2;
			var cameraWith = cameraHeight * _camera.aspect;
			return new Vector3(randomDirection.x * cameraHeight, randomDirection.y * cameraWith);
		}
	}
}
