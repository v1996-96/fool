using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PodkidnoiDurakGame.Core;

using ApplicationForms;
using PodkidnoiDurakGame.Core.PlayerDefinitions;
using PodkidnoiDurakGame.UI.Sprite;
using PodkidnoiDurakGame.Core.GameDefinitions;
using PodkidnoiDurakGame.Core.CardDefinitions;

namespace PodkidnoiDurakGame.GameDesk
{
    class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D _spriteList;
        Texture2D _backSprite;
        const int SpriteFrameWidth = 170;
        const int SpriteFrameHeight = 252;

        GamePackage _gamePackage;
        List<CardUI> _cardUIList = new List<CardUI> { };
        List<CardSprite> _cardSpriteList = new List<CardSprite> { };

        // UI is blocked while animation is on screen
        public event Action OnUserBlocked;

        public SpriteManager(Game game)
            :base(game){}

        public void RenewWindowPackage(GamePackage package)
        {
            _gamePackage = package;

            // There we are parsing package and decide which animation to make
        }

        public List<CardUI> ConvertPackageToUI(GamePackage package)
        {
            List<CardUI> cardUIList = new List<CardUI> { };
            int index = 0;
            package.Deck.ForEach((card) => { 
                cardUIList.Add(new CardUI { 
                    CardPosition = CardPosition.Deck,
                    CardSuit = card.CardSuit,
                    CardType = card.CardType,
                    Index = index++
                });
            });

            index = 0;
            package.DescPairs.ForEach((pair) =>
            {
                cardUIList.Add(new CardUI
                {
                    CardPosition = CardPosition.Desc,
                    CardSuit = pair.LowerCard.CardSuit,
                    CardType = pair.LowerCard.CardType,
                    Index = index++
                });

                if (pair.UpperCard != null)
                    cardUIList.Add(new CardUI
                    {
                        CardPosition = CardPosition.Desc,
                        CardSuit = pair.UpperCard.Value.CardSuit,
                        CardType = pair.UpperCard.Value.CardType,
                        Index = index++
                    });
            });

            index = 0;
            package.PlayerCards.ForEach((card) =>
            {
                cardUIList.Add(new CardUI
                {
                    CardPosition = CardPosition.Player,
                    CardSuit = card.CardSuit,
                    CardType = card.CardType,
                    Index = index++
                });
            });

            index = 0;
            package.EnemyCards.ForEach((card) =>
            {
                cardUIList.Add(new CardUI
                {
                    CardPosition = CardPosition.Enemy,
                    CardSuit = card.CardSuit,
                    CardType = card.CardType,
                    Index = index++
                });
            });
            return cardUIList;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _spriteList = this.Game.Content.Load<Texture2D>(@"sprite1");
            _backSprite = this.Game.Content.Load<Texture2D>(@"backSprite");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
