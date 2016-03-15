using PodkidnoiDurakGame.Core.CardDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodkidnoiDurakGame.Core.GameDefinitions
{
    class GamePackage
    {
        public CardSuit Trump { get; set; }
        public List<Card> Deck { get; set; }
        public List<CardPair> DescPairs { get; set; }
        public List<Card> PlayerCards { get; set; }
        public List<Card> EnemyCards { get; set; }
    }
}
