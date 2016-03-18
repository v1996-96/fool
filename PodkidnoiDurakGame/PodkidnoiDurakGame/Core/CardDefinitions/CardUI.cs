using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.CardDefinitions
{
    struct CardUI
    {
        public CardType CardType { get; set; }

        public CardSuit CardSuit { get; set; }

        public CardPosition CardPosition { get; set; }

        public int Index { get; set; }
    }
}
