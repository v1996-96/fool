using PodkidnoiDurakGame.Core.PlayerDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core
{
    class GameGateway
    {
        #region Singleton pattern
        private static GameGateway _instance;
        public static GameGateway Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameGateway();

                return _instance;
            }
        }
        private GameGateway() { }
        #endregion

        #region Gateway pipelines (Objects)
        private GameDesktop gameDesktop = GameDesktop.Instance;
        public GameDesktop GameDesktop { get { return gameDesktop; } }
        #endregion

        public void InitializeConnections(ref IPlayer player, ref IPlayer enemy)
        {
            // There we make all connections between game desktop, player and enemy
            player.OnThrow += (card) => gameDesktop.ThrowCard(PlayerType.Player, card);
            player.OnPass += () => gameDesktop.Pass(PlayerType.Player);
            player.OnGetAll += () => gameDesktop.GetAll(PlayerType.Player);

            enemy.OnThrow += (card) => gameDesktop.ThrowCard(PlayerType.Enemy, card);
            enemy.OnPass += () => gameDesktop.Pass(PlayerType.Enemy);
            enemy.OnGetAll += () => gameDesktop.GetAll(PlayerType.Enemy);
        }

        public void GetGamePackages(ref IPlayer someone)
        {
            someone.GamePackage = gameDesktop.GetGameData();
        }
    }
}
