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

    using LinqToTwitter;

    using ZerosTwitterClient.Forms;
    using ZerosTwitterClient.Services;
    using ZerosTwitterClient.Services.Interfaces;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        #region Public Properties

        /// <summary>
        /// Gets the moderation form.
        /// </summary>
        public static ModerationForm ModerationForm { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="parameters">
        /// The credentials.
        /// </param>
        [STAThread]
        private static void Main(string[] parameters)
        {
            var container = new WindsorContainer();
            container.Register(
                Component.For<IImageCache>().ImplementedBy<ImageCache>(), 
                Component.For<ITwitterGrabber>().ImplementedBy<L2TGrabber>(), 
                Component.For<DisplayForm>(), 
                Component.For<ModerationForm>());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (parameters.Length != 4)
            {
                return;
            }

            var sua = new SingleUserAuthorizer
                          {
                              CredentialStore =
                                  new SingleUserInMemoryCredentialStore
                                      {
                                          ConsumerKey =
                                              parameters[0],
                                          ConsumerSecret =
                                              parameters[1],
                                          AccessToken =
                                              parameters[2],
                                          AccessTokenSecret =
                                              parameters[3]
                                      }
                          };

            var context = new TwitterContext(sua);

            container.Register(Component.For<TwitterContext>().Instance(context));

            ModerationForm = container.Resolve<ModerationForm>();
            Application.Run(ModerationForm);
        }

        #endregion
    }
}