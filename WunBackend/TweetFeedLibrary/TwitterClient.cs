using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CoreTweet;
using CoreTweet.Streaming;
using HtmlAgilityPack;
using System.Net.Http;

namespace Wun.Backend.TweetFeedHandler
{
    public class TwitterClient
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<IObservable<Tweet>> GetTweetStreamAsync()
        {
            string[] oathFormFieldNames =
            {
                "authenticity_token",
                "redirect_after_login",
                "oauth_token",
                "session[username_or_email]",
                "session[password]"
            };

            var cookieContainer = new CookieContainer();
            OAuth.OAuthSession session = await OAuth.AuthorizeAsync(ConsumerKey, ConsumerSecret);
            WebResponse webResponse = await GetAsync(session.AuthorizeUri, cookieContainer);
            var htmlDocument = new HtmlDocument();
            using (var responseStream = webResponse.GetResponseStream())
            {
                htmlDocument.Load(responseStream);
            }
            (string action, string method, Dictionary<string, string> parameters) =
                    GetFormSubmission("oauth_form", htmlDocument, oathFormFieldNames);
            parameters["session[username_or_email]"] = "dt07715098";
            parameters["session[password]"] = "mbdt2017";
            webResponse = await PostFormAsync(action, method, parameters, cookieContainer);
            htmlDocument = new HtmlDocument();
            using (var responseStream = webResponse.GetResponseStream())
            {
                htmlDocument.Load(responseStream);
            }
            HtmlNode oauthPinElement = htmlDocument.GetElementbyId("oauth_pin");
            string pin = oauthPinElement.SelectSingleNode("//code").InnerText;
            Tokens tokens = await OAuth.GetTokensAsync(session, pin);
            return tokens.Streaming
                .FilterAsObservable(track => "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,x")
                .OfType<StatusMessage>()
                .Select(sm => new Tweet(sm.Status.User.ScreenName, sm.Status.Text,sm.Timestamp.DateTime));
        }

        private (string action, string method, Dictionary<string, string> parameters) GetFormSubmission(
            string formId,
            HtmlDocument htmlDocument,
            IEnumerable<string> fieldNames = null)
        {
            var includeFieldNames = fieldNames?.ToList() ?? new List<string>();
            HtmlNode formElement = htmlDocument.GetElementbyId(formId);
            return
            (
                formElement.Attributes["action"].Value,
                formElement.Attributes["method"].Value,
                formElement.SelectNodes("//input")
                    .Select(n => new { name = n.Attributes["name"]?.Value, value = n.Attributes["value"]?.Value })
                    .Where(p => !string.IsNullOrWhiteSpace(p.name) && !string.IsNullOrWhiteSpace(p.value))
                    .Where(p => includeFieldNames.Count == 0 || includeFieldNames.Contains(p.name))
                    .ToDictionary(p => p.name, p => p.value, StringComparer.OrdinalIgnoreCase)
            );
        }

        private async Task<WebResponse> GetAsync(Uri url, CookieContainer cookieContainer = null)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "GET";
            if (cookieContainer != null && webRequest is HttpWebRequest httpWebRequest)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }
            return await webRequest.GetResponseAsync();
        }

        private async Task<WebResponse> PostFormAsync(string action, string method, IEnumerable<KeyValuePair<string, string>> parameters, CookieContainer cookieContainer = null)
        {
            WebRequest webRequest = WebRequest.Create(action);
            webRequest.Method = method;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            if (cookieContainer != null && webRequest is HttpWebRequest httpWebRequest)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }
            Stream requestStream = await webRequest.GetRequestStreamAsync();
            using (StreamWriter streamWriter = new StreamWriter(requestStream))
            {
                streamWriter.Write(string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}")));
            }
            return await webRequest.GetResponseAsync();
        }
    }
}