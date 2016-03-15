using PodkidnoiDurakGame.Core.GameDefinitions;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.ArtificialIntelligence
{
    class AINormal : IPlayer
    {
        public GamePackage GamePackage { get; set; }

        public event Action<Card> OnThrow;

        public event Action OnPass;

        public event Action OnGetAll;
    }
}
