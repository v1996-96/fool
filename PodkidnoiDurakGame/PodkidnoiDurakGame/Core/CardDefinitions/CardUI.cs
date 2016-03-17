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
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((obj is CardUI) || (obj is Card)) ? (this.CardSuit == ((Card)obj).CardSuit && this.CardType == ((Card)obj).CardType) : false;
        }
    }
}
