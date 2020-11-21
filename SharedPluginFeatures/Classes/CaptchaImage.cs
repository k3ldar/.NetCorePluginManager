﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by unknown and modified by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: CaptchaImage.cs
 *
 *  Purpose:  Generates a captcha image.
 *
 *  Date        Name                Reason
 *  27/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Generates an image that can be used when confirming that form entry is from a human and not a computer.
    /// </summary>
    public class CaptchaImage : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// The text to be displayed within the image.
        /// </summary>
        /// <value>string</value>
        public string Text { get; private set; }

        /// <summary>
        /// The image generated.
        /// </summary>
        /// <value>Bitmap</value>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// The width of the image.
        /// </summary>
        /// <value>int</value>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the image
        /// </summary>
        /// <value>int</value>
        public int Height { get; private set; }

        #endregion Public Properties

        #region Private Members

        // Internal properties.
        private string _familyName;

        // For generating random numbers.
        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        #endregion Private Members

        #region Constructors / Destructors

        /// <summary>
        /// Constructor
        /// 
        /// Provides specific values for generating an image, using GenericSerif as the font.
        /// </summary>
        /// <param name="s">Text to be displayed on the image.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        public CaptchaImage(string s, int width, int height)
            : this(s, width, height, FontFamily.GenericSerif.Name)
        {

        }

        /// <summary>
        /// Constructor
        /// 
        /// Provides specific values for generating an image, including the font to be used.
        /// </summary>
        /// <param name="s">Text to be displayed on the image.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="familyName">Font name to be used.</param>
        public CaptchaImage(string s, int width, int height, string familyName)
        {
            Text = s;
            SetDimensions(width, height);
            SetFamilyName(familyName);
            GenerateImage();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~CaptchaImage()
        {
            Dispose(false);
        }


        #endregion Constructors / Destructors

        #region Public Methods

        /// <summary>
        /// Dispose, ensures that resources are correctly disposed of.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Disposes of any allocated objects.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Image.Dispose();
        }

        #endregion Protected Methods

        #region Private Methds

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Says what it needs to say in the error")]
        private void SetDimensions(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), width, "Argument out of range, must be greater than zero.");

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height), height, "Argument out of range, must be greater than zero.");

            Width = width;
            Height = height;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                using (Font font = new Font(familyName, 12F))
                {
                    _familyName = familyName;
                }
            }
            catch
            {
                _familyName = FontFamily.GenericSerif.Name;
            }
        }

        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // Fill in the background.
                HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                try
                {
                    g.FillRectangle(hatchBrush, rect);

                    // Set up the text font.
                    SizeF size;
                    float fontSize = rect.Height + 1;
                    Font font = null;
                    try
                    {
                        // Adjust the font size until the text fits within the image.
                        do
                        {
                            fontSize--;

                            if (font != null)
                                font.Dispose();

                            font = new Font(_familyName, fontSize, FontStyle.Bold);
                            size = g.MeasureString(Text, font);
                        } while (size.Width > rect.Width);

                        // Set up the text format.
                        using (StringFormat format = new StringFormat())
                        {
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Center;
                            // Create a path using the text and warp it randomly.
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
                                float v = 4F;
                                PointF[] points =
                                {
                                    new PointF(_random.Next(rect.Width) / v, _random.Next(rect.Height) / v),
                                    new PointF(rect.Width - (_random.Next(rect.Width) / v), _random.Next(rect.Height) / v),
                                    new PointF(_random.Next(rect.Width) / v, rect.Height - (_random.Next(rect.Height) / v)),
                                    new PointF(rect.Width - (_random.Next(rect.Width) / v), rect.Height - (_random.Next(rect.Height) / v))
                                };

                                using (Matrix matrix = new Matrix())
                                {
                                    matrix.Translate(0F, 0F);
                                    path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
                                }

                                // Draw the text.
                                using (HatchBrush hatchBrushText = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray))
                                {
                                    g.FillPath(hatchBrushText, path);
                                }
                            }
                        }

                        // Add some random noise.
                        int m = Math.Max(rect.Width, rect.Height);
                        for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
                        {
                            int x = _random.Next(rect.Width);
                            int y = _random.Next(rect.Height);
                            int w = _random.Next(m / 50);
                            int h = _random.Next(m / 50);
                            g.FillEllipse(hatchBrush, x, y, w, h);
                        }

                        // Set the image.
                        Image = bitmap;
                    }
                    finally
                    {
                        if (font != null)
                            font.Dispose();
                    }
                }
                finally
                {
                    hatchBrush.Dispose();
                }
            }
        }

        #endregion Private Methds
    }
}
