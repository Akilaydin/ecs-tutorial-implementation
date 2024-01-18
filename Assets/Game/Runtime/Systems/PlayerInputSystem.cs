using Client.Components;
using Client.Services;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client.Systems
{
	public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsWorldInject _world;
		private EcsPoolInject<UnitComponent> _unitComponentsPool;
		private EcsPoolInject<PlayerTag> _playerTagPool;
		private EcsCustomInject<SceneService> _sceneData;

		private int _playerEntity;

		void IEcsInitSystem.Init(IEcsSystems systems)
		{
			_playerEntity = _world.Value.NewEntity();
			_playerTagPool.Value.Add(_playerEntity);
			
			ref var playerComponent = ref _unitComponentsPool.Value.Add(_playerEntity);

			playerComponent.View = _sceneData.Value.PlayerView;
			
			playerComponent.View.Construct(_playerEntity, _world.Value);
		}
		
		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			var playerMoveSpeed = _sceneData.Value.PlayerMoveSpeed;
			var x = Input.GetAxisRaw("Horizontal");
			var y = Input.GetAxisRaw("Vertical");
			var direction = new Vector3(x, y).normalized;
			var velocity = direction * playerMoveSpeed;

			if (!_unitComponentsPool.Value.Has(_playerEntity))
			{
				return;
			}
			
			ref var playerComponent = ref _unitComponentsPool.Value.Get(_playerEntity);
			playerComponent.Velocity = velocity;
		}
	}
}
