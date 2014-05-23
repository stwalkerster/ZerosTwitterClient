// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwitterLogin.cs" company="Simon Walker">
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
//   The twitter login.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ZerosTwitterClient.Forms
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Tweetinvi;
    using Tweetinvi.Core.Interfaces.Credentials;
    using Tweetinvi.Core.Interfaces.oAuth;

    /// <summary>
    /// The twitter login.
    /// </summary>
    public partial class TwitterLogin : Form
    {
        #region Fields

        /// <summary>
        /// The temporary application credentials.
        /// </summary>
        private ITemporaryCredentials applicationCredentials;

        /// <summary>
        /// The credentials.
        /// </summary>
        private IOAuthCredentials credentials;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TwitterLogin"/> class.
        /// </summary>
        public TwitterLogin()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The display login.
        /// </summary>
        /// <returns>
        /// The <see cref="IOAuthCredentials"/>.
        /// </returns>
        public IOAuthCredentials DisplayLogin()
        {
            this.ShowDialog();

            return this.credentials;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The authorise application button click
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AuthAppClick(object sender, EventArgs e)
        {
            this.credentials = CredentialsCreator.GetCredentialsFromVerifierCode(
                this.applicationPinBox.Text, 
                this.applicationCredentials);

            this.Close();
        }

        /// <summary>
        /// The button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button1Click(object sender, EventArgs e)
        {
            this.applicationCredentials = CredentialsCreator.GenerateApplicationCredentials(
                this.consumerKeyBox.Text, 
                this.consumerSecretBox.Text);

            var authorizationUrl = CredentialsCreator.GetAuthorizationURL(this.applicationCredentials);

            Process.Start(authorizationUrl);
        }

        #endregion
    }
}