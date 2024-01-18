using Client.Services;
using Client.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client.Systems
{
	public class ScoreCounterSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsCustomInject<SceneService> _sceneService;
		private CounterView _counterView;
		private float _timer;
		private int _counterValue;

		void IEcsInitSystem.Init(IEcsSystems systems)
		{
			_counterView = _sceneService.Value.CounterView;
			_counterView.SetText("0");
		}

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			if (_sceneService.Value.GameOver)
			{
				return;
			}

			if ((_timer += Time.deltaTime) < 1)
			{
				return;
			}

			_timer = 0;
			_counterValue++;
			_counterView.SetText(_counterValue.ToString());
		}
	}
}
