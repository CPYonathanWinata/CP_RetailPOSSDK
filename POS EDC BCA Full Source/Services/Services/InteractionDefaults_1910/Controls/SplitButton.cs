/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Microsoft.Dynamics.Retail.Pos.Interaction.Controls
{
    /// <summary>
    /// Exit split button used bt the logon form
    /// </summary>
    internal sealed class SplitButton : Button
    {
        private PushButtonState _state;
        private const int PushButtonWidth = 20;
        private static int BorderSize = SystemInformation.Border3DSize.Width * 2;
        private bool skipNextOpen;
        private Rectangle dropDownRectangle = new Rectangle();
        private bool showSplit = true;
        private VisualStyleRenderer renderer;
        private readonly VisualStyleElement element = VisualStyleElement.Window.CloseButton.Normal;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitButton"/> class.
        /// </summary>
        public SplitButton()
        {
            if (Application.RenderWithVisualStyles && VisualStyleRenderer.IsElementDefined(element))
            {
                renderer = new VisualStyleRenderer(element);
            }
            else
            {
                // Set to Autosize as we will display text instead
                this.AutoSize = true;
            }
        }

        private PushButtonState State
        {
            get { return _state; }
            set
            {
                if (!_state.Equals(value))
                {
                    _state = value;
                    Invalidate();
                }
            }
        }

        #region Overriden Methods and Properties

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);

            if (showSplit && !string.IsNullOrWhiteSpace(Text) && TextRenderer.MeasureText(Text, Font).Width + PushButtonWidth > preferredSize.Width)
            {
                return preferredSize + new Size(PushButtonWidth + BorderSize * 2, 0);
            }
            return preferredSize;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Accept the up arrow to open the menu
            if (keyData.Equals(Keys.Up) && showSplit)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnGotFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Default;
            }
        }

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent != null && showSplit)
            {
                if (kevent.KeyCode.Equals(Keys.Up))
                {
                    ShowContextMenuStrip();
                }
                else if (kevent.KeyCode.Equals(Keys.Space) && kevent.Modifiers == Keys.None)
                {
                    State = PushButtonState.Pressed;
                }
            }

            base.OnKeyDown(kevent);
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (kevent != null && kevent.KeyCode.Equals(Keys.Space))
            {
                if (Control.MouseButtons == MouseButtons.None)
                    State = PushButtonState.Normal;
            }
            base.OnKeyUp(kevent);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnLostFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Normal;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e == null || !showSplit)
            {
                base.OnMouseDown(e);
                return;
            }

            if (dropDownRectangle.Contains(e.Location))
            {
                ShowContextMenuStrip();
            }
            else
            {
                State = PushButtonState.Pressed;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseEnter(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                State = PushButtonState.Hot;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!showSplit)
            {
                base.OnMouseLeave(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                if (Focused)
                {
                    State = PushButtonState.Default;
                }
                else
                {
                    State = PushButtonState.Normal;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent == null || !showSplit)
            {
                base.OnMouseUp(mevent);
                return;
            }

            if (ContextMenuStrip == null || !ContextMenuStrip.Visible)
            {
                SetButtonDrawState();
                if (Bounds.Contains(Parent.PointToClient(Cursor.Position)) && !dropDownRectangle.Contains(mevent.Location))
                    OnClick(new EventArgs());
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (pevent == null || !showSplit)
            {
                return;
            }

            Graphics g = pevent.Graphics;
            Rectangle bounds = this.ClientRectangle;

            // draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsDefault && !Application.RenderWithVisualStyles)
            {
                Rectangle backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);

                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // button renderer doesnt draw the black frame when themes are off =(
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);

            }
            else
            {
                if (renderer != null)
                {
                    renderer.SetParameters(GetVisualStyleElement(State, Focused));

                    if (renderer.IsBackgroundPartiallyTransparent())
                        renderer.DrawParentBackground(g, bounds, this);

                    //
                    // We are using the visual style of a window's close button. In order to render the 'X' offset from center, we render twice:
                    // 1. Render the entire width of the control so the background of the dropdown arrow has the correct style
                    // -------------
                    // |     X     |
                    // -------------
                    // 2. Render the main button portion only which will cover up the originally centered 'X' and render the 'X' offset.
                    // ---------
                    // |   X   |
                    // ---------
                    //
                    Rectangle boundsLessArrowPart = new Rectangle(bounds.Location, new Size(bounds.Width - PushButtonWidth, bounds.Height));
                    if (RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                        boundsLessArrowPart.X += PushButtonWidth + 1;

                    renderer.DrawBackground(g, bounds);
                    renderer.DrawBackground(g, boundsLessArrowPart);
                }
                else
                    ButtonRenderer.DrawButton(g, bounds, State);
            }
            // calculate the current dropdown rectangle.
            dropDownRectangle = new Rectangle(bounds.Right - PushButtonWidth - 1, BorderSize, PushButtonWidth, bounds.Height - BorderSize * 2);

            int internalBorder = BorderSize;
            Rectangle focusRect =
                new Rectangle(internalBorder,
                              internalBorder,
                              bounds.Width - dropDownRectangle.Width - internalBorder,
                              bounds.Height - (internalBorder * 2));

            bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles);

            if (RightToLeft == RightToLeft.Yes)
            {
                dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = dropDownRectangle.Right;
                if (drawSplitLine && renderer == null)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Left + PushButtonWidth, BorderSize, bounds.Left + PushButtonWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Left + PushButtonWidth + 1, BorderSize, bounds.Left + PushButtonWidth + 1, bounds.Bottom - BorderSize);
                }
            }
            else
            {
                if (drawSplitLine && renderer == null)
                {
                    // draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Right - PushButtonWidth, BorderSize, bounds.Right - PushButtonWidth, bounds.Bottom - BorderSize);
                    g.DrawLine(SystemPens.ButtonFace, bounds.Right - PushButtonWidth - 1, BorderSize, bounds.Right - PushButtonWidth - 1, bounds.Bottom - BorderSize);
                }

            }

            // Draw an arrow in the correct location 
            PaintArrow(g, dropDownRectangle, renderer == null ? SystemBrushes.ControlText : SystemBrushes.HighlightText);

            // Paint as normal if we don't have a renderer
            if (renderer == null)
            {
                // Figure out how to draw the text
                TextFormatFlags formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

                // If we dont' use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
                if (!UseMnemonic)
                {
                    formatFlags = formatFlags | TextFormatFlags.NoPrefix;
                }
                else if (!ShowKeyboardCues)
                {
                    formatFlags = formatFlags | TextFormatFlags.HidePrefix;
                }

                if (!string.IsNullOrEmpty(this.Text))
                {
                    TextRenderer.DrawText(g, Text, Font, focusRect, SystemColors.ControlText, formatFlags);
                }

                // Draw the focus rectangle.
                if (State != PushButtonState.Pressed && Focused)
                {
                    ControlPaint.DrawFocusRectangle(g, focusRect);
                }
            }
        }

        #endregion

        private static VisualStyleElement GetVisualStyleElement(PushButtonState state, bool hasFocus)
        {
            switch (state)
            {
                case PushButtonState.Hot:
                    return VisualStyleElement.Window.CloseButton.Hot;
                case PushButtonState.Disabled:
                    return VisualStyleElement.Window.CloseButton.Disabled;
                case PushButtonState.Pressed:
                    return VisualStyleElement.Window.CloseButton.Pressed;
                default:
                    return hasFocus ? VisualStyleElement.Window.CloseButton.Hot : VisualStyleElement.Window.CloseButton.Normal;
            }
        }

        private static void PaintArrow(Graphics g, Rectangle dropDownRect, Brush brush)
        {
            const int Margin = 3;
            Point middle = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            // If the width is odd - favor pushing it over one pixel right.
            middle.X += (dropDownRect.Width % 2);

            Point[] arrow = new Point[] {
                new Point(middle.X - Margin, middle.Y + 1), 
                new Point(middle.X + Margin, middle.Y + 1),
                new Point(middle.X, middle.Y - Margin) };

            g.FillPolygon(brush, arrow);
        }

        private void ShowContextMenuStrip()
        {
            if (skipNextOpen)
            {
                // We were called because we're closing the context menu strip
                // when clicking the dropdown button.
                skipNextOpen = false;
                return;
            }
            State = PushButtonState.Pressed;

            if (ContextMenuStrip != null)
            {
                // Subscribe here. Unsubscribe in handler.
                ContextMenuStrip.Closing += new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
                ContextMenuStrip.Show(this, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
            }
        }

        private void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            if (cms != null)
            {
                cms.Closing -= new ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
            }

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                skipNextOpen = (dropDownRectangle.Contains(this.PointToClient(Cursor.Position)));
            }
        }

        private void SetButtonDrawState()
        {
            if (Bounds.Contains(Parent.PointToClient(Cursor.Position)))
            {
                State = PushButtonState.Hot;
            }
            else if (Focused)
            {
                State = PushButtonState.Default;
            }
            else
            {
                State = PushButtonState.Normal;
            }
        }
    }
}
