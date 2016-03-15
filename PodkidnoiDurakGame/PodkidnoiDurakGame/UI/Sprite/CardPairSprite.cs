using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.UI.Sprite
{
    class CardPairSprite
    {
        public CardSprite LowerCard { get; set; }
        public CardSprite UpperCard { get; set; }

        public CardPairSprite(CardSprite lowerCard, CardSprite upperCard)
        {
            LowerCard = lowerCard;
            UpperCard = upperCard;
        }
    }
}
