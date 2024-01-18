using Client.Components;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client.Systems
{
	public class MovementSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<UnitComponent>> _unitComponentsFilter;
		private EcsPoolInject<UnitComponent> _unitComponentsPool;

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			foreach (var unit in _unitComponentsFilter.Value)
			{
				var unitCmp = _unitComponentsPool.Value.Get(unit);
				var velocity = unitCmp.Velocity;
				var view = unitCmp.View;

				view.UpdateAnimationState(velocity);

				if (velocity == Vector3.zero)
					continue;

				var translation = velocity * Time.deltaTime;
				
				view.SetDirection(velocity);
				view.Move(translation);
			}
		}
	}
}
