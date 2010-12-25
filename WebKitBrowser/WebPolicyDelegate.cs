/*
 * Copyright (c) 2009, Peter Nelson (charn.opcode@gmail.com)
 * Copyright (c) 2010, Ashod Nakashian ()
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
*/

// Handles events that can be used to modify the policy decisions that the WebView
// makes when handling various data types.  More information can be found at:
// http://developer.apple.com/documentation/Cocoa/Reference/WebKit/Protocols/WebPolicyDelegate_Protocol

using WebKit.Interop;

namespace WebKit
{
    public delegate bool DecidePolicyForNavigationAction(string url, string mainUrl);

    internal class WebPolicyDelegate : IWebPolicyDelegate
    {
        public bool AllowDownloads;
        public bool AllowNewWindows;
        public bool AllowNavigation;

        // so that we can load and display the first page
        public bool AllowInitialNavigation;

        public event DecidePolicyForNavigationAction DecideNavigationAction;

        public WebPolicyDelegate(bool AllowNavigation, bool AllowDownloads, bool AllowNewWindows)
        {
            this.AllowDownloads = AllowDownloads;
            this.AllowNavigation = AllowNavigation;
            this.AllowNewWindows = AllowNewWindows;
            AllowInitialNavigation = true;
        }

        #region IWebPolicyDelegate Members

        public void decidePolicyForMIMEType(WebView WebView, string type, IWebURLRequest request, webFrame frame, IWebPolicyDecisionListener listener)
        {
            // todo: add support for showing custom MIME type documents
            // and for changing which MIME types are handled here
            if (WebView.canShowMIMEType(type) == 0)
            {
                if (AllowDownloads)
                    listener.download();
                else
                    listener.ignore();
            }
            else
            {
                listener.use();
            }
        }

        public void decidePolicyForNavigationAction(WebView WebView, CFDictionaryPropertyBag actionInformation, IWebURLRequest request, webFrame frame, IWebPolicyDecisionListener listener)
        {
            if (InvokeDecideNavigationAction(request.url(), request.mainDocumentURL()) &&
                (AllowNavigation || AllowInitialNavigation))
            {
                listener.use();
            }
            else
            {
                listener.ignore();
            }
        }

        public void decidePolicyForNewWindowAction(WebView WebView, CFDictionaryPropertyBag actionInformation, IWebURLRequest request, string frameName, IWebPolicyDecisionListener listener)
        {
            if (AllowNewWindows)
            {
                listener.use();
            }
            else
            {
                listener.ignore();
            }
        }

        public void unableToImplementPolicyWithError(WebView WebView, WebError error, webFrame frame)
        {
        }

        #endregion

        #region implementation

        /// <summary>
        /// Invoke the DecidePolicyForNavigationAction delegate.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <param name="mainUrl">The main document URL.</param>
        /// <returns>True to navigate, False to cancel.</returns>
        private bool InvokeDecideNavigationAction(string url, string mainUrl)
        {
            DecidePolicyForNavigationAction handler = DecideNavigationAction;
            if (handler != null)
            {
                return handler(url, mainUrl);
            }

            return true;
        }
        #endregion // implementation
    }
}
