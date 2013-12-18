using System;
using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;

public class HomeViewModel{

    public string twitterProfilePicture {set;get;}

    public List<LinqToTwitter.Status> tweets {set;get;}

    public List<string> instagramPhotoLinks {set;get;}

    public HomeViewModel()
    {
        tweets = new List<LinqToTwitter.Status>();

        instagramPhotoLinks = new List<string>();
    }

}