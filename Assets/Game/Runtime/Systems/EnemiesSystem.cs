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

		private float _spawnInterval;
		private Camera _camera;

		void IEcsInitSystem.Init(IEcsSystems systems)
		{
			_spawnInterval = _sceneService.Value.EnemySpawnInterval;
			_camera = _sceneService.Value.Camera;
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			CreateEnemy();
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
			unitComponent.Velocity = Vector3.up * _sceneService.Value.EnemyMoveSpeed;
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
