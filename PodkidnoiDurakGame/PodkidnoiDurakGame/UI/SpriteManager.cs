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
using PodkidnoiDurakGame.Core.GameDefinitions;
using PodkidnoiDurakGame.Core.CardDefinitions;

namespace PodkidnoiDurakGame.GameDesk
{
    class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D _spriteList;
        Texture2D _backSprite;
        int _countPlayerCards = 0;
        int _countEnemyCards = 0;

        const int SpriteFrameWidth = 170;
        const int SpriteFrameHeight = 252;
        const int FrameOffset = 5;
        const int MaxCardTypeIndex = 9;
        const int MaxCardSuitIndex = 4;

        public bool ShowCards { get; set; }
        private bool SpritesAreAnimated
        {
            get
            {
                bool anim = false;
                _cardSpriteList.ForEach((s) => { if (s.IsAnimated) anim = true; });
                return anim;
            }
        }

        List<CardUI> _cardUIList;
        List<CardSprite> _cardSpriteList = new List<CardSprite> { };


        #region Position calculator
        // player cards
        const float MaxCardOffset = 10;
        const float CardResizeIndex = 0.515873f;
        // window
        const float VerticalWindowPadding = 15;
        const float HorizontalWindowPadding = 15;
        const float WindowHeight = 599;
        const float WindowWidth = 960;
        // desc cards
        const float DescTopOffset = 82;
        const float DescBottomOffset = 82;
        const float DescVerticalCardOffset = 15;
        const float DescHorizintalCardOffset = 15;
        const float DescCardOffset = 20;
        // deck cards
        const float DeckCardsRightOffset = 20;
        const float DeckCardOffset = 1;
        // trump card
        const float TrumpCardTopOffset = 25;
        const float TrumpCardToDeck = 40;
        // z-index
        const float PlayerCardsMinZIndex = 0f;
        const float PlayerCardsMaxZIndex = 1f;
        const float DescLowerCardZIndex = 0f;
        const float DescUpperCardZIndex = 1f;
        const float TrumpCardZIndex = 0f;
        const float DeckCardsMinZIndex = 0.1f;
        const float DeckCardsMaxZIndex = 1f;

        private float CurrentGameCardWidth
        {
            get { return CardResizeIndex * SpriteFrameWidth; }
        }
        private float CurrentGameCardHeight
        {
            get { return CardResizeIndex * SpriteFrameHeight; }
        }

        private void CalculateNewSpriteState(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {
            switch (ui.CardPosition)
            {
                case CardPosition.Player:
                    CalculateSpriteState_Enemy(ui, out position, out texture, out rotation, out zIndex);
                    break;
                case CardPosition.Enemy:
                    CalculateSpriteState_Player(ui, out position, out texture, out rotation, out zIndex);
                    break;
                case CardPosition.Desc:
                    CalculateSpriteState_Desc(ui, out position, out texture, out rotation, out zIndex);
                    break;
                case CardPosition.Deck:
                    CalculateSpriteState_Deck(ui, out position, out texture, out rotation, out zIndex);
                    break;
                case CardPosition.OutOfDesc:
                    CalculateSpriteState_OutOfDesc(ui, out position, out texture, out rotation, out zIndex);
                    break;
                default:
                    CalculateSpriteState_OutOfDesc(ui, out position, out texture, out rotation, out zIndex);
                    break;
            }
        }

        private void CalculateSpriteState_Enemy(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {
            var blockWidth = (_countEnemyCards - 1) * MaxCardOffset + _countEnemyCards * CurrentGameCardWidth;
            var cardsSpace = WindowWidth - 2*HorizontalWindowPadding;
            var y = VerticalWindowPadding;
            var x = HorizontalWindowPadding;

            if (blockWidth < cardsSpace)
            {
                x = (WindowWidth - blockWidth) / 2 + ui.Index * (CurrentGameCardWidth + MaxCardOffset);
            }
            else
            {
                var cardOffset = MaxCardOffset - (cardsSpace - blockWidth)/(_countEnemyCards-1);
                x = HorizontalWindowPadding + ui.Index * (CurrentGameCardWidth + cardOffset);
            }

            position = new Vector2(x, y);
            texture = _backSprite;
            rotation = 0;
            zIndex = PlayerCardsMinZIndex + ui.Index * ((PlayerCardsMaxZIndex - PlayerCardsMinZIndex) / _countEnemyCards);
        }

        private void CalculateSpriteState_Player(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {
            var blockWidth = (_countPlayerCards - 1) * MaxCardOffset + _countPlayerCards * CurrentGameCardWidth;
            var cardsSpace = WindowWidth - 2 * HorizontalWindowPadding;
            var y = VerticalWindowPadding + 2 * CurrentGameCardHeight + DescTopOffset + DescVerticalCardOffset + DescBottomOffset;
            var x = HorizontalWindowPadding;

            if (blockWidth < cardsSpace)
            {
                x = (WindowWidth - blockWidth) / 2 + ui.Index * (CurrentGameCardWidth + MaxCardOffset);
            }
            else
            {
                var cardOffset = MaxCardOffset - (cardsSpace - blockWidth) / (_countPlayerCards - 1);
                x = HorizontalWindowPadding + ui.Index * (CurrentGameCardWidth + cardOffset);
            }

            position = new Vector2(x, y);
            texture = _spriteList;
            rotation = 0;
            zIndex = PlayerCardsMinZIndex + ui.Index * ((PlayerCardsMaxZIndex - PlayerCardsMinZIndex) / _countPlayerCards);
        }

        private void CalculateSpriteState_Desc(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {   
            var y = VerticalWindowPadding + CurrentGameCardHeight + DescTopOffset;
            y += ((ui.Index+1) % 2 == 0) ? DescVerticalCardOffset : 0;

            var x = ((ui.Index + 1) % 2 == 0) ? DescHorizintalCardOffset : 0;
            x += (((ui.Index + 1) / 2) - ((ui.Index + 1) % 2)) * CurrentGameCardWidth;

            position = new Vector2(x, y);
            texture = _spriteList;
            rotation = 0;
            zIndex = ((ui.Index + 1) % 2 == 0) ? DescUpperCardZIndex : DescLowerCardZIndex;
        }

        private void CalculateSpriteState_Deck(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {
            float x = 0;
            float y = 0;

            if (ui.Index == 0)
            {
                x = WindowWidth - HorizontalWindowPadding - DeckCardsRightOffset;
                y = VerticalWindowPadding + CurrentGameCardHeight + DescTopOffset + TrumpCardTopOffset;
                zIndex = TrumpCardZIndex;
                rotation = MathHelper.Pi/2;
                texture = _spriteList;
            }
            else
            {
                x = WindowWidth - HorizontalWindowPadding - CurrentGameCardWidth - DeckCardsRightOffset + ui.Index * DeckCardOffset;
                y = VerticalWindowPadding + CurrentGameCardHeight + DescTopOffset + ui.Index * DeckCardOffset;
                zIndex = DeckCardsMinZIndex + ui.Index * 0.001f;
                rotation = 0;
                texture = _backSprite;
            }

            position = new Vector2(x, y);
        }

        private void CalculateSpriteState_OutOfDesc(CardUI ui, out Vector2 position, out Texture2D texture, out float rotation, out float zIndex)
        {
            position = new Vector2(-1 * CurrentGameCardWidth - 10, 0);
            texture = _backSprite;
            rotation = 0;
            zIndex = 0;
        }

        #endregion


        #region Converter
        public List<CardUI> ConvertPackageToUI(GamePackage package, ref int countPlayerCards, ref int countEnemyCards)
        {
            countPlayerCards = package.PlayerCards.Count;
            countEnemyCards = package.EnemyCards.Count;

            List<CardUI> cardUIList = new List<CardUI> { };
            int index = 0;
            package.Deck.ForEach((card) =>
            {
                cardUIList.Add(new CardUI
                {
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
#endregion


        public SpriteManager(Game game)
            : base(game)
        {
            _cardUIList = new List<CardUI> { };
            _cardSpriteList = new List<CardSprite> { };
            ShowCards = false;
        }

        public void ResetCardsOnWindow(List<Card> cardList)
        {
            _cardUIList = new List<CardUI> { };
            _cardSpriteList = new List<CardSprite> { };

            cardList.ForEach((card) =>
            {
                var c = new CardSprite(
                    _backSprite,
                    Vector2.Zero,
                    new Point(SpriteFrameWidth, SpriteFrameHeight),
                    Point.Zero,
                    0,
                    CardResizeIndex
                );
                c.Card = new CardUI
                {
                    CardPosition = CardPosition.Deck,
                    CardType = card.CardType,
                    CardSuit = card.CardSuit,
                    Index = 0
                };
                _cardSpriteList.Add(c);
            });

            ShowCards = false;
        }
        public void RenewWindowPackage(GamePackage package)
        {
            _cardUIList = ConvertPackageToUI(package, ref _countPlayerCards, ref _countEnemyCards);

            if (SpritesAreAnimated) return;

            if (_cardSpriteList.Count == 0) return;
        
            for (var i = 0; i < _cardSpriteList.Count; i++)
            {
                for (var j = 0; j < _cardUIList.Count; j++)
                {
                    if (_cardSpriteList[i].Card.CardSuit == _cardUIList[j].CardSuit &&
                        _cardSpriteList[i].Card.CardType == _cardUIList[j].CardType)
                    {
                        Vector2 position;
                        Texture2D texture;
                        float rotation;
                        float zIndex;

                        CalculateNewSpriteState(_cardUIList[j], out position, out texture, out rotation, out zIndex);

                        if (texture == _backSprite)
                        {
                            _cardSpriteList[i].Reset(texture, Point.Zero, rotation, zIndex, CardResizeIndex);
                        }
                        else
                        {
                            _cardSpriteList[i].Reset(texture, new Point((int)_cardUIList[j].CardSuit, (int)_cardUIList[j].CardType), rotation, zIndex, CardResizeIndex);
                        }

                        if (position != _cardSpriteList[i].Position)
                        {
                            _cardSpriteList[i].Animate(position, 500);
                        }
                    }
                }
            }

            ShowCards = true;
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
            _cardSpriteList.ForEach((sprite) => sprite.Update(gameTime));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (ShowCards)
                _cardSpriteList.ForEach((sprite) => sprite.Draw(gameTime, spriteBatch));

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
