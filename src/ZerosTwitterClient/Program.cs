// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Simon Walker">
//   Copyright (C) 2014 Simon Walker
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
//   to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
//   the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//   THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
//   SOFTWARE.
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ZerosTwitterClient
{
    using System;
    using System.Windows.Forms;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using ZerosTwitterClient.Forms;
    using ZerosTwitterClient.Services;
    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        #region Static Fields

        /// <summary>
        /// The moderation form.
        /// </summary>
        private static ModerationForm moderationForm;

        /// <summary>
        /// Gets the moderation form.
        /// </summary>
        public static ModerationForm ModerationForm
        {
            get
            {
                return moderationForm;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<IImageCache>().ImplementedBy<ImageCache>(),
                Component.For<ITwitterGrabber>().ImplementedBy<TwitterGrabber>(),
                Component.For<DisplayForm>(),
                Component.For<ModerationForm>());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            moderationForm = container.Resolve<ModerationForm>();
            Application.Run(moderationForm);
        }

        #endregion
    }
}