using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Stalktter.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(StartViewModel startViewModel)
        {

            var auth = new ApplicationOnlyAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"]
                }
            };

            auth.Authorize();

            var twitterCtx = new TwitterContext(auth);

            var srch =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.User &&
                           tweet.ScreenName == startViewModel.twitterUsername
                     select tweet)
                    .ToList();

            var users =
               from tweet in twitterCtx.User
               where tweet.Type == UserType.Show &&
                     tweet.ScreenName == startViewModel.twitterUsername
               select tweet;

            var user = users.SingleOrDefault();

            var homeViewModel = new HomeViewModel()
            {
                tweets = srch,
                twitterProfilePicture = user.ProfileImageUrl
            };

            WebClient client = new WebClient();

            /*UGLY HACK*/
            var userIDFromInstagram = client.DownloadString("http://jelled.com/instagram/user.php?username=" + startViewModel.instagramUsername);

            dynamic userIDDeserialized = JsonConvert.DeserializeObject(userIDFromInstagram);

            string userID = userIDDeserialized.data[0].id;

            var instagramResponse = client.DownloadString("https://api.instagram.com/v1/users/" + userID + "/media/recent/?client_id=d837e1e08fe24802bfe7916b9ed9962c");

            dynamic instagramResponseDeserialized = JsonConvert.DeserializeObject(instagramResponse);

            var urlLowResolutions = new List<string>();

            /*CONS 20*/
            for (var i = 0; i < 20; i++)
                urlLowResolutions.Add(instagramResponseDeserialized.data[i].images.low_resolution.url.ToString());

            homeViewModel.instagramPhotoLinks = urlLowResolutions;

            return View(homeViewModel);

        }

    }
}
