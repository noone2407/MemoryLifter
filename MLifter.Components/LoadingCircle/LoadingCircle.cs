//
// Copyright �2006, 2007, Martin R. Gagn� (martingagne@gmail.com)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//   - Redistributions of source code must retain the above copyright notice, 
//     this list of conditions and the following disclaimer.
//
//   - Redistributions in binary form must reproduce the above copyright notice, 
//     this list of conditions and the following disclaimer in the documentation 
//     and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace MRG.Controls.UI
{
    public partial class LoadingCircle : Control
    {
        // Constants =========================================================
        private const double NumberOfDegreesInCircle = 360;
        private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;
        private const int DefaultInnerCircleRadius = 8;
        private const int DefaultOuterCircleRadius = 10;
        private const int DefaultNumberOfSpoke = 10;
        private const int DefaultSpokeThickness = 4;
        private readonly Color DefaultColor = Color.DarkGray;

        private const int MacOSXInnerCircleRadius = 5;
        private const int MacOSXOuterCircleRadius = 11;
        private const int MacOSXNumberOfSpoke = 12;
        private const int MacOSXSpokeThickness = 2;

        private const int FireFoxInnerCircleRadius = 6;
        private const int FireFoxOuterCircleRadius = 7;
        private const int FireFoxNumberOfSpoke = 9;
        private const int FireFoxSpokeThickness = 4;

        private const int IE7InnerCircleRadius = 8;
        private const int IE7OuterCircleRadius = 9;
        private const int IE7NumberOfSpoke = 24;
        private const int IE7SpokeThickness = 4;

        private const int MLifterInnerCircleRadius = 7;
        private const int MLifterOuterCircleRadius = 8;
        private const int MLifterNumberOfSpoke = 36;
        private const int MLifterSpokeThickness = 4;

        // Enumeration =======================================================
        public enum StylePresets
        {
            MacOSX,
            Firefox,
            IE7,
            MLifter,
            Custom
        }

        // Attributes ========================================================
        private Timer m_Timer;
        private bool m_IsTimerActive;
        private int m_NumberOfSpoke;
        private int m_SpokeThickness;
        private int m_ProgressValue;
        private int m_OuterCircleRadius;
        private int m_InnerCircleRadius;
        private PointF m_CenterPoint;
        private Color m_Color;
        private Color[] m_Colors;
        private double[] m_Angles;
        private StylePresets m_StylePreset;

        // Properties ========================================================
        /// <summary>
        /// Gets or sets the lightest color of the circle.
        /// </summary>
        /// <value>The lightest color of the circle.</value>
        [TypeConverter("System.Drawing.ColorConverter"),
         Category("LoadingCircle"),
         Description("Sets the color of spoke.")]
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;

                GenerateColorsPallet();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the outer circle radius.
        /// </summary>
        /// <value>The outer circle radius.</value>
        [System.ComponentModel.Description("Gets or sets the radius of outer circle."),
         System.ComponentModel.Category("LoadingCircle")]
        public int OuterCircleRadius
        {
            get
            {
                if (m_OuterCircleRadius == 0)
                    m_OuterCircleRadius = DefaultOuterCircleRadius;

                return m_OuterCircleRadius;
            }
            set
            {
                m_OuterCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the inner circle radius.
        /// </summary>
        /// <value>The inner circle radius.</value>
        [System.ComponentModel.Description("Gets or sets the radius of inner circle."),
         System.ComponentModel.Category("LoadingCircle")]
        public int InnerCircleRadius
        {
            get
            {
                if (m_InnerCircleRadius == 0)
                    m_InnerCircleRadius = DefaultInnerCircleRadius;

                return m_InnerCircleRadius;
            }
            set
            {
                m_InnerCircleRadius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the number of spoke.
        /// </summary>
        /// <value>The number of spoke.</value>
        [System.ComponentModel.Description("Gets or sets the number of spoke."),
        System.ComponentModel.Category("LoadingCircle")]
        public int NumberSpoke
        {
            get
            {
                if (m_NumberOfSpoke == 0)
                    m_NumberOfSpoke = DefaultNumberOfSpoke;

                return m_NumberOfSpoke;
            }
            set
            {
                if (m_NumberOfSpoke != value && m_NumberOfSpoke > 0)
                {
                    m_NumberOfSpoke = value;
                    GenerateColorsPallet();
                    GetSpokesAngles();

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:LoadingCircle"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [System.ComponentModel.Description("Gets or sets the number of spoke."),
        System.ComponentModel.Category("LoadingCircle")]
        public bool Active
        {
            get
            {
                return m_IsTimerActive;
            }
            set
            {
                m_IsTimerActive = value;
                ActiveTimer();
            }
        }

        /// <summary>
        /// Gets or sets the spoke thickness.
        /// </summary>
        /// <value>The spoke thickness.</value>
        [System.ComponentModel.Description("Gets or sets the thickness of a spoke."),
        System.ComponentModel.Category("LoadingCircle")]
        public int SpokeThickness
        {
            get
            {
                if (m_SpokeThickness <= 0)
                    m_SpokeThickness = DefaultSpokeThickness;

                return m_SpokeThickness;
            }
            set
            {
                m_SpokeThickness = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rotation speed.
        /// </summary>
        /// <value>The rotation speed.</value>
        [System.ComponentModel.Description("Gets or sets the rotation speed. Higher the slower."),
        System.ComponentModel.Category("LoadingCircle")]
        public int RotationSpeed
        {
            get
            {
                return m_Timer.Interval;
            }
            set
            {
                if (value > 0)
                    m_Timer.Interval = value;
            }
        }

        /// <summary>
        /// Quickly sets the style to one of these presets, or a custom style if desired
        /// </summary>
        /// <value>The style preset.</value>
        [Category("LoadingCircle"),
         Description("Quickly sets the style to one of these presets, or a custom style if desired"),
         DefaultValue(typeof(StylePresets), "Custom")]
        public StylePresets StylePreset
        {
            get { return m_StylePreset; }
            set
            {
                m_StylePreset = value;

                switch (m_StylePreset)
                {
                    case StylePresets.MacOSX:
                        SetCircleAppearance(MacOSXNumberOfSpoke,
                            MacOSXSpokeThickness, MacOSXInnerCircleRadius,
                            MacOSXOuterCircleRadius);
                        break;
                    case StylePresets.Firefox:
                        SetCircleAppearance(FireFoxNumberOfSpoke,
                            FireFoxSpokeThickness, FireFoxInnerCircleRadius,
                            FireFoxOuterCircleRadius);
                        break;
                    case StylePresets.IE7:
                        SetCircleAppearance(IE7NumberOfSpoke,
                            IE7SpokeThickness, IE7InnerCircleRadius,
                            IE7OuterCircleRadius);
                        break;
                    case StylePresets.MLifter:
                        SetCircleAppearance(MLifterNumberOfSpoke,
                            MLifterSpokeThickness, MLifterInnerCircleRadius,
                            MLifterOuterCircleRadius);
                        break;
                    case StylePresets.Custom:
                        SetCircleAppearance(DefaultNumberOfSpoke,
                            DefaultSpokeThickness,
                            DefaultInnerCircleRadius,
                            DefaultOuterCircleRadius);
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when the picture was drawn.
        /// </summary>
        /// <remarks>Documented by Dev05, 2009-03-13</remarks>
        public event EventHandler PictureDrawn;
        /// <summary>
        /// Raises the <see cref="E:PictureDrawn"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks>Documented by Dev05, 2009-03-13</remarks>
        protected virtual void OnPictureDrawn(EventArgs e)
        {
            if (PictureDrawn != null)
                PictureDrawn(this, e);
        }

        /// <summary>
        /// Gets or sets the custom Image to paint the circle on.
        /// </summary>
        /// <value>The custom image.</value>
        /// <remarks>Documented by Dev05, 2009-03-13</remarks>
        public Image CustomImage { get; set; }

        // Construtor ========================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LoadingCircle"/> class.
        /// </summary>
        public LoadingCircle()
        {
            CustomImage = null;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            m_Color = DefaultColor;

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            m_Timer = new Timer();
            m_Timer.Tick += new EventHandler(aTimer_Tick);
            ActiveTimer();

            this.Resize += new EventHandler(LoadingCircle_Resize);
        }

        // Events ============================================================
        /// <summary>
        /// Handles the Resize event of the LoadingCircle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        void LoadingCircle_Resize(object sender, EventArgs e)
        {
            GetControlCenterPoint();
        }

        /// <summary>
        /// Handles the Tick event of the aTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        void aTimer_Tick(object sender, EventArgs e)
        {
            m_ProgressValue = ++m_ProgressValue % m_NumberOfSpoke;
            if (CustomImage == null)
                Invalidate();
            else
                DrawPicture(CustomImage);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawPicture(e.Graphics);

            base.OnPaint(e);
        }

        public void DrawPicture(Image image)
        {
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackColor);
            DrawPicture(g);
        }
        public void DrawPicture(Graphics graphics)
        {
            if (m_NumberOfSpoke > 0)
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                int intPosition = m_ProgressValue;
                for (int intCounter = 0; intCounter < m_NumberOfSpoke; intCounter++)
                {
                    intPosition = intPosition % m_NumberOfSpoke;
                    DrawLine(graphics,
                             GetCoordinate(m_CenterPoint, m_InnerCircleRadius, m_Angles[intPosition]),
                             GetCoordinate(m_CenterPoint, m_OuterCircleRadius, m_Angles[intPosition]),
                             m_Colors[intCounter], m_SpokeThickness);
                    intPosition++;
                }
            }

            OnPictureDrawn(EventArgs.Empty);
        }

        // Overridden Methods ================================================
        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="proposedSize">The custom-sized area for a control.</param>
        /// <returns>
        /// An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.
        /// </returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width =
                (m_OuterCircleRadius + m_SpokeThickness) * 2;

            return proposedSize;
        }

        // Methods ===========================================================
        /// <summary>
        /// Darkens a specified color.
        /// </summary>
        /// <param name="_objColor">Color to darken.</param>
        /// <param name="_intPercent">The percent of darken.</param>
        /// <returns>The new color generated.</returns>
        private Color Darken(Color _objColor, int _intPercent)
        {
            int intRed = _objColor.R;
            int intGreen = _objColor.G;
            int intBlue = _objColor.B;
            return Color.FromArgb(_intPercent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
        }

        /// <summary>
        /// Generates the colors pallet.
        /// </summary>
        private void GenerateColorsPallet()
        {
            m_Colors = GenerateColorsPallet(m_Color, Active, m_NumberOfSpoke);
        }

        /// <summary>
        /// Generates the colors pallet.
        /// </summary>
        /// <param name="_objColor">Color of the lightest spoke.</param>
        /// <param name="_blnShadeColor">if set to <c>true</c> the color will be shaded on X spoke.</param>
        /// <returns>An array of color used to draw the circle.</returns>
        private Color[] GenerateColorsPallet(Color _objColor, bool _blnShadeColor, int _intNbSpoke)
        {
            Color[] objColors = new Color[NumberSpoke];

            // Value is used to simulate a gradient feel... For each spoke, the 
            // color will be darken by value in intIncrement.
            byte bytIncrement = (byte)(byte.MaxValue / NumberSpoke);

            //Reset variable in case of multiple passes
            byte PERCENTAGE_OF_DARKEN = 0;

            for (int intCursor = 0; intCursor < NumberSpoke; intCursor++)
            {
                if (_blnShadeColor)
                {
                    if (intCursor == 0 || intCursor < NumberSpoke - _intNbSpoke)
                        objColors[intCursor] = _objColor;
                    else
                    {
                        // Increment alpha channel color
                        PERCENTAGE_OF_DARKEN += bytIncrement;

                        // Ensure that we don't exceed the maximum alpha
                        // channel value (255)
                        if (PERCENTAGE_OF_DARKEN > byte.MaxValue)
                            PERCENTAGE_OF_DARKEN = byte.MaxValue;

                        // Determine the spoke forecolor
                        objColors[intCursor] = Darken(_objColor, PERCENTAGE_OF_DARKEN);
                    }
                }
                else
                    objColors[intCursor] = _objColor;
            }

            return objColors;
        }

        /// <summary>
        /// Gets the control center point.
        /// </summary>
        private void GetControlCenterPoint()
        {
            m_CenterPoint = GetControlCenterPoint(this);
        }

        /// <summary>
        /// Gets the control center point.
        /// </summary>
        /// <returns>PointF object</returns>
        private PointF GetControlCenterPoint(Control _objControl)
        {
            return new PointF(_objControl.Width / 2, _objControl.Height / 2 - 1);
        }

        /// <summary>
        /// Draws the line with GDI+.
        /// </summary>
        /// <param name="_objGraphics">The Graphics object.</param>
        /// <param name="_objPointOne">The point one.</param>
        /// <param name="_objPointTwo">The point two.</param>
        /// <param name="_objColor">Color of the spoke.</param>
        /// <param name="_intLineThickness">The thickness of spoke.</param>
        private void DrawLine(Graphics _objGraphics, PointF _objPointOne, PointF _objPointTwo,
                              Color _objColor, int _intLineThickness)
        {
            using (Pen objPen = new Pen(new SolidBrush(_objColor), _intLineThickness))
            {
                objPen.StartCap = LineCap.Round;
                objPen.EndCap = LineCap.Round;
                _objGraphics.DrawLine(objPen, _objPointOne, _objPointTwo);
            }
        }

        /// <summary>
        /// Gets the coordinate.
        /// </summary>
        /// <param name="_objCircleCenter">The Circle center.</param>
        /// <param name="_intRadius">The radius.</param>
        /// <param name="_dblAngle">The angle.</param>
        /// <returns></returns>
        private PointF GetCoordinate(PointF _objCircleCenter, int _intRadius, double _dblAngle)
        {
            double dblAngle = Math.PI * _dblAngle / NumberOfDegreesInHalfCircle;

            return new PointF(_objCircleCenter.X + _intRadius * (float)Math.Cos(dblAngle),
                              _objCircleCenter.Y + _intRadius * (float)Math.Sin(dblAngle));
        }

        /// <summary>
        /// Gets the spokes angles.
        /// </summary>
        private void GetSpokesAngles()
        {
            m_Angles = GetSpokesAngles(NumberSpoke);
        }

        /// <summary>
        /// Gets the spoke angles.
        /// </summary>
        /// <param name="_shtNumberSpoke">The number spoke.</param>
        /// <returns>An array of angle.</returns>
        private double[] GetSpokesAngles(int _intNumberSpoke)
        {
            double[] Angles = new double[_intNumberSpoke];
            double dblAngle = (double)NumberOfDegreesInCircle / _intNumberSpoke;

            for (int shtCounter = 0; shtCounter < _intNumberSpoke; shtCounter++)
                Angles[shtCounter] = (shtCounter == 0 ? dblAngle : Angles[shtCounter - 1] + dblAngle);

            return Angles;
        }

        /// <summary>
        /// Actives the timer.
        /// </summary>
        private void ActiveTimer()
        {
            if (m_IsTimerActive)
                m_Timer.Start();
            else
            {
                m_Timer.Stop();
                m_ProgressValue = 0;
            }

            GenerateColorsPallet();
            Invalidate();
        }

        /// <summary>
        /// Sets the circle appearance.
        /// </summary>
        /// <param name="numberSpoke">The number spoke.</param>
        /// <param name="spokeThickness">The spoke thickness.</param>
        /// <param name="innerCircleRadius">The inner circle radius.</param>
        /// <param name="outerCircleRadius">The outer circle radius.</param>
        public void SetCircleAppearance(int numberSpoke, int spokeThickness,
            int innerCircleRadius, int outerCircleRadius)
        {
            NumberSpoke = numberSpoke;
            SpokeThickness = spokeThickness;
            InnerCircleRadius = innerCircleRadius;
            OuterCircleRadius = outerCircleRadius;

            Invalidate();
        }
    }
}