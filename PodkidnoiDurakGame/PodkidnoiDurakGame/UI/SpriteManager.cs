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

        List<CardSprite> _playerCards = new List<CardSprite> { };
        List<CardSprite> _enemyCards = new List<CardSprite> { };
        List<CardPairSprite> _descCards = new List<CardPairSprite> { };
        List<CardSprite> _deck = new List<CardSprite> { };
        Sprite _trump;

        public SpriteManager(Game game)
            :base(game){}

        public SpriteManager(Game game, ref IPlayer player)
            : base(game)
        {
            _gamePackage = player.GamePackage;
        }


        #region Converters
        private CardSprite ConvertCardToSprite(Card card, Texture2D sprite)
        {
            return new CardSprite(
                    sprite,
                    Vector2.Zero,
                    new Point(SpriteFrameWidth, SpriteFrameHeight),
                    new Point((int)card.CardSuit, (int)card.CardType),
                    0,
                    0.7f);
        }

        private List<CardSprite> ConvertCardsToSprite(List<Card> cardList, Texture2D spriteFront)
        {
            List<CardSprite> spriteList = new List<CardSprite> { };
            foreach (var card in cardList)
                spriteList.Add(ConvertCardToSprite(card, spriteFront));
            return spriteList;
        }

        private List<CardSprite> ConvertDeckToSprite(List<Card> cardList, Texture2D spriteFront, Texture2D spriteBack)
        {
            List<CardSprite> spriteList = new List<CardSprite> { };
            for (var i = 0; i < cardList.Count; i++)
                spriteList.Add(ConvertCardToSprite(cardList[i], (i == 0) ? spriteFront : spriteBack));
            return spriteList;
        }

        private List<CardPairSprite> ConvertDescToSprite(List<CardPair> cardList, Texture2D spriteFront)
        {
            List<CardPairSprite> _spriteList = new List<CardPairSprite> { };
            foreach (var cardPair in cardList)
                _spriteList.Add(new CardPairSprite(
                        ConvertCardToSprite(cardPair.LowerCard, spriteFront),
                        (cardPair.UpperCard == null) ? null : ConvertCardToSprite(cardPair.UpperCard.Value, spriteFront)
                        ));
            return _spriteList;
        }
        #endregion


        #region Position, size and z-index setters
        private void PlayerCardsPosition()
        {

        }
        #endregion

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _spriteList = this.Game.Content.Load<Texture2D>(@"sprite1");
            _backSprite = this.Game.Content.Load<Texture2D>(@"backSprite");

            _playerCards = ConvertCardsToSprite(_gamePackage.PlayerCards, _spriteList);
            _enemyCards = ConvertCardsToSprite(_gamePackage.EnemyCards, _backSprite);
            _deck = ConvertDeckToSprite(_gamePackage.Deck, _spriteList, _backSprite);
            _descCards = ConvertDescToSprite(_gamePackage.DescPairs, _spriteList);

            // There we also shall bind events to cards

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update cards state (for tracking mouse events)
            _playerCards.ForEach((sprite) => sprite.Update(gameTime));
            _enemyCards.ForEach((sprite) => sprite.Update(gameTime));
            _deck.ForEach((sprite) => sprite.Update(gameTime));
            _descCards.ForEach((spritePair) =>
            {
                spritePair.LowerCard.Update(gameTime);
                if (spritePair.UpperCard != null)
                    spritePair.UpperCard.Update(gameTime);
            });


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // Draw card lists
            _playerCards.ForEach((sprite) => sprite.Draw(gameTime, spriteBatch));
            _enemyCards.ForEach((sprite) => sprite.Draw(gameTime, spriteBatch));
            _deck.ForEach((sprite) => sprite.Draw(gameTime, spriteBatch));
            _descCards.ForEach((spritePair) =>
            {
                spritePair.LowerCard.Draw(gameTime, spriteBatch);
                if (spritePair.UpperCard != null)
                    spritePair.UpperCard.Draw(gameTime, spriteBatch);
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
