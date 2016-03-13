using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core
{
    struct Card
    {
        public CardType CardType { get; set; }

        public CardSuit CardSuit { get; set; }
        
        public bool CanCover(Card covering)
        {
            if (covering.IsTrump && !this.IsTrump)
                return true;

            if (!covering.IsTrump && this.IsTrump)
                return false;

            if ((covering.IsTrump && this.IsTrump) ||
                (!covering.IsTrump && !this.IsTrump))
                return covering.CardType > this.CardType;

            return false;
        }
    }
}
