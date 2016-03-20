using PodkidnoiDurakGame.Core.GameDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.PlayerDefinitions
{
    interface IPlayer
    {
        GamePackage GamePackage { get; set; }
        void TakeTheBaton();
        void Throw(Card card);
        void Pass();
        void GetAll();
        event Action<Card> OnThrow;
        event Action OnPass;
        event Action OnGetAll;
    }
}
