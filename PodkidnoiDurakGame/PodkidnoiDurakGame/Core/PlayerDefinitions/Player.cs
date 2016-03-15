using PodkidnoiDurakGame.Core.GameDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.PlayerDefinitions
{
    class Player : IPlayer
    {
        public GamePackage GamePackage { get; set; }

        public void Throw(Card card)
        {
            if (OnThrow != null) OnThrow(card);
        }

        public void Pass()
        {
            if (OnPass != null) OnPass();
        }

        public void GetAll()
        {
            if (OnGetAll != null) OnGetAll();
        }

        public event Action<Card> OnThrow;

        public event Action OnPass;

        public event Action OnGetAll;
    }
}
