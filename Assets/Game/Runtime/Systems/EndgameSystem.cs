using Client.Components;
using Client.Services;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Systems
{
	public class EndgameSystem : IEcsRunSystem
	{
		private EcsFilterInject<Inc<CollisionEvent>> _collisionsEventsFilter;
		private EcsPoolInject<CollisionEvent> _collisionsEventsPool;
		private EcsPoolInject<PlayerTag> _playerTagPool;
		private EcsCustomInject<SceneService> _sceneService;
		private EcsFilterInject<Inc<UnitComponent>> _unitComponentFilter;

		void IEcsRunSystem.Run(IEcsSystems systems)
		{
			CheckLoseCondition();
			CheckWinCondition();
		}

		private void CheckLoseCondition()
		{
			foreach (var entity in _collisionsEventsFilter.Value)
			{
				ref var collisionEvt = ref _collisionsEventsPool.Value.Get(entity);
				var collidedEntity = collisionEvt.CollidedEntity;

				if (_playerTagPool.Value.Has(collidedEntity))
				{
					ShowEndGamePopup("Ты проиграл");
				}
			}
		}
		
		private void CheckWinCondition()
		{
			if (Time.timeSinceLevelLoad <= 10)
			{
				return;
			}
			
			ShowEndGamePopup("Ты выиграл");
		}
		
		private void StopAllUnits()
		{
			foreach (var entity in _unitComponentFilter.Value)
			{
				_unitComponentFilter.Pools.Inc1.Del(entity);
			}
		}
		
		private void ShowEndGamePopup(string message)
		{
			_sceneService.Value.GameOver = true;
			StopAllUnits();
			
			var popupWindow = _sceneService.Value.PopupView;

			popupWindow.SetActive(true);
			popupWindow.SetDescription(message);
			popupWindow.SetButtonText("Повторить");
			popupWindow.Button.onClick.RemoveAllListeners();
			popupWindow.Button.onClick.AddListener(RestartGame);
		}
		
		private void RestartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
