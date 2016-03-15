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
        event Action<Card> OnThrow;
        event Action OnPass;
        event Action OnGetAll;
    }
}
