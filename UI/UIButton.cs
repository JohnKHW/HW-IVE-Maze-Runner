using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment.UI
{
    class UIButton : GUI
    {
        public bool isClicked;
        UIText uIText;
        Color defaultColor;
        Color hoverColor;
        Color selectedColor;
        float lerpTime;
        ButtonState lastButtonState;

        public enum ButtonStates
        {
            None,
            Hovered,
            Selected
        }

        public ButtonStates currState;

        public UIButton(
            UIText _uIText,
            Color? _defaultColor = null,
            Color? _hoverColor = null,
            Color? _selectedColor = null,
            float _lerpTime = 1
            ) : base()
        {
            uIText = _uIText;
            defaultColor = _defaultColor ?? _uIText.color;
            hoverColor = _hoverColor ?? _uIText.color;
            selectedColor = _selectedColor ?? _uIText.color;
            lerpTime = _lerpTime;
            pos = uIText.pos;
        }
        public void Update(MouseState _ms, KeyboardState _ks)
        {
            uIText.pos = pos;
            uIText.Update();
            if (currState == ButtonStates.Selected)
            {
                uIText.color = Color.Lerp(uIText.color, selectedColor, lerpTime);
                isClicked = _ms.LeftButton == ButtonState.Released && lastButtonState == ButtonState.Pressed && uIText.rect.Contains(_ms.Position);
            }
            else if (uIText.rect.Contains(_ms.Position))
            {
                currState = ButtonStates.Hovered;
                uIText.color = Color.Lerp(uIText.color, hoverColor, lerpTime);
                if (_ms.LeftButton == ButtonState.Pressed)
                {
                    currState = ButtonStates.Selected;
                }
            }
            else
            {
                uIText.color = Color.Lerp(uIText.color, defaultColor, lerpTime);
            }
            lastButtonState = _ms.LeftButton;
        }
        public override void Draw(SpriteBatch spriteBatch, float depth = 0, float scale = 1, float rotation = 0)
        {
            uIText.Draw(spriteBatch);
        }
        public override void Dispose()
        {

        }
    }
}
