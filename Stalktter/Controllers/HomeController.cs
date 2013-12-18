using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;
using System.Configuration;

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

            return View(homeViewModel);

        }

    }
}
